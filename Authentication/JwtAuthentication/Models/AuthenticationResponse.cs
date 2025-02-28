namespace JwtAuthentication.Models {
    public class AuthenticationResponse {
        public string Username {get;set;}
        public string JwtToken {get;set;}
        public int ExpiresInSec {get;set;}
    }
}