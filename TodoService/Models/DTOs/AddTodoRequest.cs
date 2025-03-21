namespace TodoService.Models.DTOs;

public class AddTodoRequest {
    public required string Username {get; set;}
    public required long UserId {get; set;}
    public required string Description {get; set;}
    public required string Category {get; set;}
}