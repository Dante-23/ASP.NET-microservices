

using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using JwtAuthentication.Models;
using Microsoft.IdentityModel.Tokens;

namespace JwtAuthentication {
    public class JwtTokenHandler {
        public const string JWT_SECURITY_KEY = "541d912e52aa3ce05bdc37b5e9f82434534b3a3ddc86d34e9a26164fef3bf7ae";
        private const int JWT_TOKEN_VALIDITY = 2;
        private readonly List<UserAccount> _userAccountList;
        public JwtTokenHandler() {
            _userAccountList = new List<UserAccount> {
                // new UserAccount { Id = 1, Username = "test", Password = "Test123", Role = "Admin" },
                // new UserAccount { Id = 2, Username = "test1", Password = "Test1123", Role = "User" },
            };
        }

        public void AddUsers(List<UserAccount> usersFromDb) {
            _userAccountList.AddRange(usersFromDb);
        }

        public void AddUser(UserAccount userAccount) {
            _userAccountList.Add(userAccount);
        }

        public AuthenticationResponse? GenerateJwtToken(AuthenticationRequest authenticationRequest) {
            if (string.IsNullOrWhiteSpace(authenticationRequest.Username) || string.IsNullOrWhiteSpace(authenticationRequest.Password)) return null;
            var userAccount = _userAccountList.Find(x => x.Username == authenticationRequest.Username && x.Password == authenticationRequest.Password);
            if (userAccount == null) return null;
            var tokenExpiryTimeStamp = DateTime.Now.AddMinutes(JWT_TOKEN_VALIDITY);
            var tokenKey = Encoding.ASCII.GetBytes(JWT_SECURITY_KEY);
            var claimsIdentity = new ClaimsIdentity(new List<Claim> {
                new Claim(JwtRegisteredClaimNames.Name as string, userAccount.Id + "-" + userAccount.Username as string),
                new Claim(ClaimTypes.Role, userAccount.Role)
            });

            var signingCredentials = new SigningCredentials(
                new SymmetricSecurityKey(tokenKey),
                SecurityAlgorithms.HmacSha256Signature
            );

            var securityTokenDescriptor = new SecurityTokenDescriptor {
                Subject = claimsIdentity,
                Expires = tokenExpiryTimeStamp,
                SigningCredentials = signingCredentials
            };

            var jwtSecurityTokenHandler = new JwtSecurityTokenHandler();
            var securityToken = jwtSecurityTokenHandler.CreateToken(securityTokenDescriptor);
            var token = jwtSecurityTokenHandler.WriteToken(securityToken);

            return new AuthenticationResponse {
                Username = userAccount.Username,
                ExpiresInSec = (int)tokenExpiryTimeStamp.Subtract(DateTime.Now).TotalSeconds,
                JwtToken = token,
                Id = userAccount.Id
            };
        }
    }
}
