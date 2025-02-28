using System.ComponentModel.DataAnnotations;

namespace UserService.Models;

public class User {
    [Key]
    public long Id {get; set; }
    [Required]
    [MinLength(1)]
    [MaxLength(50)]
    public string Username {get; set;}
    [Required]
    [MinLength(6)]
    [MaxLength(100)]
    public string Email {get; set;}
    [Required]
    [MinLength(1)]
    [MaxLength(50)]
    public string Name {get; set;}
    [Required]
    [MinLength(5)]
    [MaxLength(50)]
    public string Password {get; set;}

}