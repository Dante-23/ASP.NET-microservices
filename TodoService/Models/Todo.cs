using System.ComponentModel.DataAnnotations;

namespace TodoService.Models;

public class Todo {
    [Key]
    public long Id {get; set; }
    [Required]
    [MinLength(1)]
    [MaxLength(50)]
    public string Username {get; set;}
    [Required]
    public long UserId {get; set;}
    [Required]
    [MinLength(1)]
    [MaxLength(50)]
    public string Description {get; set;}
}