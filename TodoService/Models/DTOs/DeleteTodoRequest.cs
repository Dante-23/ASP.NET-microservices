namespace TodoService.Models.DTOs;

public class DeleteTodoRequest {
    public required string Id {get; set;}
    public required long UserId {get; set;}
}