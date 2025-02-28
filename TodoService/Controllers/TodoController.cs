using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using NuGet.Protocol;
using TodoService.Models;
using JwtAuthentication;
using JwtAuthentication.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Components.Authorization;
using System.Security.Claims;
using System.IdentityModel.Tokens.Jwt;

namespace TodoService.Controllers;

[ApiController]
[Route("/api/[controller]")]
public class TodoController : ControllerBase {
    private readonly JwtTokenHandler _jwtTokenHandler;
    private readonly TodoDbContext _context;
    public TodoController(TodoDbContext context, JwtTokenHandler jwtTokenHandler) {
        _context = context;
        _jwtTokenHandler = jwtTokenHandler;
    }

    // [HttpGet]
    // [Authorize (Roles = ("Admin"))]
    // public async Task<ActionResult<IEnumerable<User>>> GetAllTodos() {
    //     var todos = await _context.Todos.ToListAsync();
    //     return Ok(todos);
    // }

    // [HttpGet("{id}")]
    // [Authorize (Roles = ("Admin"))]
    // public async Task<ActionResult<IEnumerable<User>>> GetUser(long id) {
    //     var user = await _context.Todos.FindAsync(id);
    //     if (user == null) {
    //         return NotFound();
    //     }
    //     return Ok(user);
    // }

    [HttpGet("/api/[controller]/test")]
    [Authorize]
    public string Test() {
        // var u = User.FindFirst(JwtRegisteredClaimNames.Name);
        // if (u == null) {
            
        // } 
        // Console.WriteLine(u);
        return "You are authenticated";
    }

    // [HttpPost]
    // public async Task<ActionResult<IEnumerable<User>>> addUser(AddUserRequest addUserRequest) {
    //     User user = new User() {
    //         Username = addUserRequest.Username,
    //         Email = addUserRequest.Email,
    //         Name = addUserRequest.Name,
    //         Password = addUserRequest.Password
    //     };
    //     if (userExists(user)) {
    //         return BadRequest();
    //     }
    //     _context.Users.Add(user);
    //     await _context.SaveChangesAsync();
    //     return Ok(user);
    // }

    // [HttpPost("/api/[controller]/login")]
    // public async Task<ActionResult<AuthenticationResponse>> LoginUser(AuthenticationRequest authenticationRequest) {
    //     var authenticationResponse = _jwtTokenHandler.GenerateJwtToken(authenticationRequest);
    //     if (authenticationResponse == null) return Unauthorized();
    //     return authenticationResponse;
    // }

    // [HttpPut("{id}")]
    // public async Task<ActionResult<User>> updateUser(long id, string newName) {
    //     User user = await _context.Users.FindAsync(id);
    //     if (user == null) return BadRequest();
    //     user.Name = newName;
    //     _context.Entry(user).State = EntityState.Modified;
    //     try {
    //         await _context.SaveChangesAsync();
    //     } catch(DbUpdateConcurrencyException) {
    //         if (!userExists(user)) return NotFound();
    //     }
    //     return Ok(user);
    // }

    // private bool userExists(User user) {
    //     return _context.Users.Any(e => 
    //         user.Username == e.Username || user.Email == e.Email
    //     );
    // }
}