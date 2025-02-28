using System;
using System.Text.RegularExpressions;
using JwtAuthentication.Models;

namespace JwtAuthentication {
    public class SignUpValidator
    {
        private string UserName;
        private string Email;
        private string Name;
        private string Password;
        private static short UsernameMinLength = 3;
        private static short UsernameMaxLength = 20;
        private static short PasswordMinLength = 5;

        public SignUpValidator() {}
        // public SignUpValidator(string username, string email, string name, string password) {
        //     UserName = username;
        //     Email = email;
        //     Name = name;
        //     Password = password;
        // }

        public SignUpValidator SetUserName(string username) {
            UserName = username;
            return this;
        }

        public SignUpValidator SetEmail(string email) {
            Email = email;
            return this;
        }

        public SignUpValidator SetName(string name) {
            Name = name;
            return this;
        }

        public SignUpValidator SetPassword(string password) {
            Password = password;
            return this;
        }

        public SignUpValidationResponse Validate() {
            string msg = ValidateUsername();
            if (msg != null) return new SignUpValidationResponse {status = false, message = msg};
            msg = ValidateEmail();
            if (msg != null) return new SignUpValidationResponse {status = false, message = msg};
            msg = ValidatePassword();
            if (msg != null) return new SignUpValidationResponse {status = false, message = msg};
            else return new SignUpValidationResponse {status = true, message = ""};
        }
        // Validates that the username is not empty and within a specified length
        public string? ValidateUsername() {
            if (string.IsNullOrEmpty(UserName)) return "Username is required.";
            if (UserName.Length < UsernameMinLength || UserName.Length > UsernameMaxLength) {
                return $"Username must be between {UsernameMinLength} and {UsernameMaxLength} characters.";
            }
            return null;
        }

        // Validates that the email is in a correct format
        public string? ValidateEmail() {
            if (string.IsNullOrEmpty(Email)) {
                return "Email is required.";
            }
            string emailPattern = @"^[^@\s]+@[^@\s]+\.[^@\s]+$";
            if (!Regex.IsMatch(Email, emailPattern)) {
                return "Invalid email format.";
            }
            return null;
        }

        // Validates that the password meets specified strength requirements
        public string? ValidatePassword() {
            if (string.IsNullOrEmpty(Password)) {
                return "Password is required.";
            }
            if (Password.Length < PasswordMinLength) {
                return $"Password must be at least {PasswordMinLength} characters long.";
            }
            return null;
            /* Development purpose. Make it easy
            if (!Regex.IsMatch(Password, @"[A-Z]")) {
                Console.WriteLine("Password must contain at least one uppercase letter.");
                return false;
            }
            if (!Regex.IsMatch(Password, @"[a-z]"))
            {
                Console.WriteLine("Password must contain at least one lowercase letter.");
                return false;
            }
            if (!Regex.IsMatch(Password, @"[0-9]"))
            {
                Console.WriteLine("Password must contain at least one digit.");
                return false;
            }
            if (!Regex.IsMatch(Password, @"[\W_]"))
            {
                Console.WriteLine("Password must contain at least one special character.");
                return false;
            }
            return true;
            */
        }

    /* Confirm password validation not required as of now. 
        // Validates that the password and confirm password fields match
        public bool ValidateConfirmPassword(string password, string confirmPassword)
        {
            if (password != confirmPassword)
            {
                Console.WriteLine("Passwords do not match.");
                return false;
            }
            return true;
        }
    */
    }

}