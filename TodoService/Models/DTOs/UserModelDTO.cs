namespace TodoService.Models.DTOs;

public class UserModelDTO {
    public long Id {get; set; }
    public string Username {get; set;}
    public string Email {get; set;}
    public string Name {get; set;}
    public string Password {get; set;}

    public override string ToString(){
        return "User: " + Id + " " + Username + " " + Email + " " + Password;
    }
}