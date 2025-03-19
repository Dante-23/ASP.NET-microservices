namespace BudgetService.Models.DTOs;

public class DeleteExpenseRequest {
    public required string Username {get; set;}
    public required long UserId {get; set;}
    public required string Id {get; set;}
}