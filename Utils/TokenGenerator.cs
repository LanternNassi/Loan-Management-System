using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Text;
using Microsoft.AspNetCore.Mvc;
using Loan_Management_System.Models.UserX;
using System.Security.Claims;

namespace Loan_Management_System.Utils
{
    public class TokenGenerator
    {
        public static string GenerateToken(IConfiguration _config , User user)
        {
            var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_config["Jwt:Key"]));
            var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);

            var claims = new[]
            {
                new Claim(JwtRegisteredClaimNames.Sub , user.Username),
                new Claim(JwtRegisteredClaimNames.Jti, user.Id.ToString()),
                new Claim("role" , user.Role),
            };


            var Sectoken = new JwtSecurityToken(_config["Jwt:Issuer"],
              _config["Jwt:Issuer"],
              claims : claims,
              expires: DateTime.Now.AddMinutes(120),
              signingCredentials: credentials);

            var token = new JwtSecurityTokenHandler().WriteToken(Sectoken);

            return token;
        }
    }
}
