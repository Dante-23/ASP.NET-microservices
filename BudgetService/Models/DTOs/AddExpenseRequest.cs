namespace BudgetService.Models.DTOs;

public class AddExpenseRequest {
    public required string Username {get; set;}
    public required long UserId {get; set;}
    public required string Description {get; set;}
    public required long Amount {get; set;}
    public required string BudgetName {get; set;}
}