namespace TodoService.Models.DTOs;

public class UpdateTodoRequest {
    public required string Id {get; set; }
    public required long UserId {get; set;}
    public required string Description {get; set;}
}