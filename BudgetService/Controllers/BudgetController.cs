using Microsoft.AspNetCore.Mvc;
using NuGet.Protocol;
using BudgetService.Models;
using JwtAuthentication;
using JwtAuthentication.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Components.Authorization;
using System.Security.Claims;
using System.IdentityModel.Tokens.Jwt;
using MongoDB.Driver;
using BudgetService.Services;
using MongoDB.Bson;
using BudgetService.Models.DTOs;
using BudgetService.Clients;

namespace BudgetService.Controllers;

[ApiController]
[Route("/api/[controller]")]
public class BudgetController : ControllerBase {
    private readonly JwtTokenHandler _jwtTokenHandler;
    private readonly ExpenseDbService _expenseDbService;
    private readonly UserServiceClient _userServiceClient;
    private readonly ManagerHostedService _managerHostedService;
    public BudgetController(JwtTokenHandler jwtTokenHandler, ExpenseDbService expenseDbService, UserServiceClient userServiceClient, ManagerHostedService managerHostedService) {
        _jwtTokenHandler = jwtTokenHandler;
        _expenseDbService = expenseDbService;
        _userServiceClient = userServiceClient;
        _managerHostedService = managerHostedService;
    }

    [HttpGet("{userid}")]
    [Authorize]
    public async Task<ActionResult<IEnumerable<Expense>>> GetAllExpensesOfUser(long userid) {
        bool userExists = await _userServiceClient.UserExists(userid);
        if (!userExists) return BadRequest("User does not exists");
        TokenData? tokenData = getUserDetailsFromToken();
        if (tokenData == null) {
            return Unauthorized("User id from token does not match given user id");
        }
        int? idFromToken = tokenData.Id;
        if (idFromToken != userid) {
            return Unauthorized("User id from token does not match given user id");
        }
        return await _expenseDbService.GetExpensesOfUserId(userid);
    }

    [HttpPost]
    [Authorize]
    public async Task<IActionResult> AddExpenseOfUser(AddExpenseRequest addExpenseRequest) {
        long userid = addExpenseRequest.UserId;
        bool userExists = await _userServiceClient.UserExists(userid);
        if (!userExists) return BadRequest("User does not exists");
        TokenData? tokenData = getUserDetailsFromToken();
        if (tokenData == null) {
            return Unauthorized("User id from token does not match given user id");
        }
        int? idFromToken = tokenData.Id;
        if (idFromToken != userid) {
            return Unauthorized("User id from token does not match given user id");
        }
        long maxAmount = _managerHostedService.GetMaxAmountGivenBudgetName(addExpenseRequest.BudgetName);
        if (maxAmount == ManagerHostedService.BUDGET_NOT_FOUND) {
            Console.WriteLine("First time addition of the budget named " + addExpenseRequest.BudgetName);
            maxAmount = addExpenseRequest.MaxAmount;
            _managerHostedService.AddBudget(addExpenseRequest.BudgetName, new KeyValuePair<long, long>(addExpenseRequest.Amount, maxAmount));
        } else if (maxAmount != addExpenseRequest.MaxAmount) {
            Console.WriteLine("maxAmount != addExpenseRequest.MaxAmount.. Using stored max amount value");
        }
        string id = ObjectId.GenerateNewId().ToString();
        Expense expense = new Expense {
            Id = id,
            Username = addExpenseRequest.Username,
            UserId = addExpenseRequest.UserId,
            Description = addExpenseRequest.Description,
            Amount = addExpenseRequest.Amount,
            Budget = new Budget {BudgetName = addExpenseRequest.BudgetName, MaxAmount = maxAmount}
        };
        await _expenseDbService.CreateAsync(expense);
        return Ok(expense);
    }

    [HttpPut]
    [Authorize]
    public async Task<IActionResult> UpdateExpenseOfUser() {
        return Ok();
    }

    [HttpDelete]
    [Authorize]
    public async Task<IActionResult> DeleteExpenseOfUser(DeleteExpenseRequest deleteExpenseRequest) {
        long userid = deleteExpenseRequest.UserId;
        bool userExists = await _userServiceClient.UserExists(userid);
        if (!userExists) return BadRequest("User does not exists");
        TokenData? tokenData = getUserDetailsFromToken();
        if (tokenData == null) {
            return Unauthorized("User id from token does not match given user id");
        }
        int? idFromToken = tokenData.Id;
        if (idFromToken != userid) {
            return Unauthorized("User id from token does not match given user id");
        }
        /*
            If this expense is the last expense of its budget, delete the budget from hosted service
        */
        return Ok();
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