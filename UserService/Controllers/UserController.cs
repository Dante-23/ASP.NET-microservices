using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using NuGet.Protocol;
using UserService.Models;
using JwtAuthentication;
using JwtAuthentication.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Components.Authorization;
using System.Security.Claims;
using System.IdentityModel.Tokens.Jwt;
using System.Net.Http.Headers;
using Microsoft.Extensions.Primitives;

namespace UserService.Controllers;

[ApiController]
[Route("/api/[controller]")]
public class UserController : ControllerBase {
    private readonly JwtTokenHandler _jwtTokenHandler;
    private readonly UserDbContext _context;
    private readonly SignUpValidator _signupValidator;
    private static readonly string authKey = "CustomBearer";
    private static readonly string authValue = "Custom";
    public UserController(UserDbContext context, JwtTokenHandler jwtTokenHandler, SignUpValidator signUpValidator) {
        _context = context;
        _jwtTokenHandler = jwtTokenHandler;
        _signupValidator = signUpValidator;
        // List<UserAccount> tempList = new List<UserAccount> {
        //     new UserAccount { Id = 1, Username = "test", Password = "Test123", Role = "Admin" },
        //     new UserAccount { Id = 2, Username = "test1", Password = "Test1123", Role = "User" },
        // };
        // var users = _context.Users.ToList();
        // users.ForEach(user => 
        //     tempList.Add(new UserAccount { Id = user.Id, Username = user.Username, Password = user.Password, Role = "User" } )
        // );
        // _jwtTokenHandler.AddUsers(tempList);
    }

    [HttpGet]
    // [Authorize (Roles = ("Admin"))]
    [Authorize]
    public async Task<ActionResult<IEnumerable<User>>> GetAllUsers() {
        var users = await _context.Users.ToListAsync();
        return Ok(users);
    }

    [HttpGet("{id}")]
    [Authorize]
    public async Task<ActionResult<IEnumerable<User>>> GetUser(long id) {
        TokenData tokenData = getUserDetailsFromToken();
        if (tokenData == null) {
            return Unauthorized();
        }
        int? idFromToken = tokenData.Id;
        if (idFromToken != id) {
            Console.WriteLine("Id from token does not match id");
            return Unauthorized();
        }
        var user = await _context.Users.FindAsync(id);
        if (user == null) {
            return NotFound();
        }
        return Ok(user);
    }

    [HttpPost]
    public async Task<ActionResult<IEnumerable<User>>> addUser(AddUserRequest addUserRequest) {
        // Validate first
        SignUpValidationResponse signUpValidationResponse = _signupValidator.SetUserName(addUserRequest.Username)
        .SetEmail(addUserRequest.Email)
        .SetName(addUserRequest.Name)
        .SetPassword(addUserRequest.Password)
        .Validate();
        if (!signUpValidationResponse.status) return BadRequest(new AddUserResponse {Status = false, Message = signUpValidationResponse.message});
        User user = new User() {
            Username = addUserRequest.Username,
            Email = addUserRequest.Email,
            Name = addUserRequest.Name,
            Password = addUserRequest.Password
        };
        bool exists = await userExists(user);
        if (exists) {
            return BadRequest(new AddUserResponse {Status = true, Message = "User already exists"});
        }
        _context.Users.Add(user);
        await _context.SaveChangesAsync();
        // Add user to authentication lib
        _jwtTokenHandler.AddUser(new UserAccount {Id = user.Id, Username = user.Username, Password = user.Password, Role = "User"});
        return Ok(new AddUserResponse {Status = true, Message = "User added successfully"});
    }

    [HttpPost("/api/[controller]/login")]
    public async Task<ActionResult<AuthenticationResponse>> LoginUser(AuthenticationRequest authenticationRequest) {
        var authenticationResponse = _jwtTokenHandler.GenerateJwtToken(authenticationRequest);
        if (authenticationResponse == null) return Unauthorized();
        return authenticationResponse;
    }

    [HttpPut("{id}")]
    public async Task<ActionResult<User>> updateUser(long id, string newName) {
        Console.WriteLine(newName);
        TokenData? tokenData = getUserDetailsFromToken();
        if (tokenData == null) {
            return Unauthorized();
        }
        int? idFromToken = tokenData.Id;
        if (idFromToken != id) {
            Console.WriteLine("Id from token does not match id");
            return Unauthorized();
        }
        User? user = await _context.Users.FindAsync(id);
        if (user == null) return BadRequest();
        user.Name = newName;
        _context.Entry(user).State = EntityState.Modified;
        try {
            await _context.SaveChangesAsync();
        } catch(DbUpdateConcurrencyException) {
            bool exists = await userExists(user);
            if (!exists) return NotFound();
        }
        return Ok(user);
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteUser(long id) {
        TokenData? tokenData = getUserDetailsFromToken();
        if (tokenData == null) {
            return Unauthorized();
        }
        int? idFromToken = tokenData.Id;
        if (idFromToken != id) {
            Console.WriteLine("Id from token does not match id");
            return Unauthorized();
        }
        User? user = await _context.Users.FindAsync(id);
        _context.Users.Remove(user);
        await _context.SaveChangesAsync();
        return Ok("Delete successfully");
    }

    // [HttpGet("/intercomm/microcomm/{id}")]
    [HttpGet("/api/[controller]/microcomm/exists/{id}")]
    public async Task<ActionResult<IEnumerable<User>>> MicroCommUserExists(long id) {
        const string HeaderKeyName = "Authorization";
        bool isHeaderPresent = Request.Headers.TryGetValue(HeaderKeyName, out StringValues headerValue);
        if (!isHeaderPresent) return BadRequest();
        string[] values = headerValue[0]!.Split(' ');
        if (!values[0].Equals(authKey) || !values[1].Equals(authValue)) {
            Console.WriteLine("Service communication auth failed");
            return Unauthorized();
        }
        // Console.WriteLine("Header value: " + id + values[0] + "-" + values[1]);
        var user = await _context.Users.FindAsync(id);
        if (user == null) return NotFound();
        return Ok();
    }

    private Task<bool> userExists(User user) {
        bool result = _context.Users.Any(e => 
            user.Username == e.Username || user.Email == e.Email
        );
        return Task.FromResult(result);
    }

    private TokenData? getUserDetailsFromToken() {
        var tokenData = User.FindFirst(JwtRegisteredClaimNames.Name);
        if (tokenData == null) {
            Console.WriteLine("tokenData is null");
            return null;
        }
        Console.WriteLine(tokenData);
        Console.WriteLine(tokenData.ToString().Split('-').ElementAt(0).Split(": ")[1]);
        Console.WriteLine(tokenData.ToString().Split('-').ElementAt(1));
        int idFromToken = int.Parse(tokenData.ToString().Split('-').ElementAt(0).Split(": ")[1]);
        string usernameFromToken = tokenData.ToString().Split('-').ElementAt(1);
        return new TokenData {Id = idFromToken, Username = usernameFromToken};
    }
}

class TokenData {
    public int? Id {get; set;}
    public string? Username {get; set;}
}