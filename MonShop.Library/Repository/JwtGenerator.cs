using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using MonShop.Library.Data;
using MonShop.Library.Models;
using MonShop.Library.Repository.IRepository;
using MonShopLibrary.DTO;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace MonShop.Library.Repository
{
    public class JwtGenerator : IJwtGenerator
    {
        private readonly MonShopContext _db;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IConfiguration _configuration;
        private readonly TokenModel _loginResponse;

        public JwtGenerator(MonShopContext db, UserManager<ApplicationUser> userManager, IConfiguration configuration)
        {
            _db = db;
            _userManager = userManager;
            _configuration = configuration;
            _loginResponse = new TokenModel();
        }

        public string GenerateRefreshToken()
        {
            var randomNumber = new byte[64];
            using var rng = RandomNumberGenerator.Create();
            rng.GetBytes(randomNumber);
            return Convert.ToBase64String(randomNumber);
        }
        public async Task<string> GenerateAccessToken(LoginRequest loginRequest)
        {
            var user = _db.Users.FirstOrDefault(u => u.Email.ToLower() == loginRequest.Email.ToLower());

            var roles = await _userManager.GetRolesAsync(user);

            var claims = new List<Claim>
            {
           new Claim (ClaimTypes.Email, loginRequest.Email),
           new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
           new Claim("AccountId", user.Id)

           };
            claims.AddRange(roles.Select(role => new Claim(ClaimTypes.Role, role)));


            var authenKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["JWT:Key"]));
            var token = new JwtSecurityToken(
                issuer: _configuration["JWT:Issuer"],
                audience: _configuration["JWT:Audience"],
                expires: DateTime.Now.AddDays(1),
                claims: claims,
                signingCredentials: new SigningCredentials(authenKey, SecurityAlgorithms.HmacSha512Signature)
                );
            return new JwtSecurityTokenHandler().WriteToken(token);

        }

        public async Task<TokenModel> GetNewToken(string refreshToken, string accountId)
        {
            var user = await _db.Users.FirstOrDefaultAsync(u => u.Id.ToLower() == accountId);
            if (user?.RefreshToken == refreshToken)
            {
                var roles = await _userManager.GetRolesAsync(user);
                var claims = new List<Claim>
            {
           new Claim (ClaimTypes.Email, user.Email),
           new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
           new Claim("AccountId", user.Id)
           };
                claims.AddRange(roles.Select(role => new Claim(ClaimTypes.Role, role)));
                var authenKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["JWT:Key"]));
                var token = new JwtSecurityToken
                    (
                    issuer: _configuration["JWT:Issuer"],
                    audience: _configuration["JWT:Audience"],
                    expires: DateTime.Now.AddDays(1),
                    claims: claims,
                    signingCredentials: new SigningCredentials(authenKey, SecurityAlgorithms.HmacSha512Signature)
                    );

                _loginResponse.Token = new JwtSecurityTokenHandler().WriteToken(token);
                if (user.RefreshTokenExpiryTime <= DateTime.Now)
                {
                    user.RefreshToken = GenerateRefreshToken();
                    user.RefreshTokenExpiryTime = DateTime.Now.AddDays(1);
                    await _db.SaveChangesAsync();
                    _loginResponse.RefreshToken = user.RefreshToken;
                }
                else
                {
                    _loginResponse.RefreshToken = refreshToken;

                }

            }

            return _loginResponse;

        }
    }
}
