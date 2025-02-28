namespace TodoService.Models;

public class AddTodoRequest {
    public required string Username {get; set;}
    public required long UserId {get; set;}
    public required string Description {get; set;}
}