using Microsoft.IdentityModel.Tokens;
using MonShop.Library.Models;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace MonShopAPI.Util
{
    public class JWTHelper
    {

        public JWTHelper()
        {
        }

        public static string GenerateToken(Account user)
        {
            IConfiguration config = new ConfigurationBuilder()
           .SetBasePath(Directory.GetCurrentDirectory())
           .AddJsonFile("appsettings.json", true, true)
           .Build();
            string role = "";
            if (user.RoleId == 1)
            {
                role = "admin";
            }
            else if (user.RoleId == 2)
            {
                role = "user";

            }

            var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(config["Jwt:Key"]));
            var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);
            var claims = new[]
            {
                new Claim("AccountID",user.AccountId.ToString()),
                new Claim(ClaimTypes.Role,$"{role}")
            };
            var token = new JwtSecurityToken(
                config["Jwt:Issuer"],
                config["Jwt:Audience"],
                claims,
                expires: DateTime.Now.AddHours(1),
                signingCredentials: credentials);


            return new JwtSecurityTokenHandler().WriteToken(token);

        }
    }
}
