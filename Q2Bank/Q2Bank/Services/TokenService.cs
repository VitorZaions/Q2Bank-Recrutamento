using Microsoft.IdentityModel.Tokens;
using Q2Bank.Models;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace Q2Bank.Services
{
    public static class TokenService
    {
        public static string GenerateToken(Usuario User)
        {
            var tokenHandler = new JwtSecurityTokenHandler();
            var tokendescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new Claim[]
                {
                    new Claim(ClaimTypes.Name, User.Id.ToString()),
                    new Claim(ClaimTypes.Role, User.User)
                }),
                Expires = DateTime.Now.AddDays(1),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(Token.Key), SecurityAlgorithms.HmacSha256Signature)
            };

            var token = tokenHandler.CreateToken(tokendescriptor);
            return tokenHandler.WriteToken(token);

        }
    }

}

