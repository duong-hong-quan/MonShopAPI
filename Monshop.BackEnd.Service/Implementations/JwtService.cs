using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using Monshop.BackEnd.Service.Contracts;
using MonShop.BackEnd.Common.Dto.Request;
using MonShop.BackEnd.Common.Dto.Response;
using MonShop.BackEnd.DAL.Contracts;
using MonShop.BackEnd.DAL.Models;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;

namespace Monshop.BackEnd.Service.Implementations
{
    public class JwtService : GenericBackendService, IJwtService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IConfiguration _configuration;


        public JwtService(
            IUnitOfWork unitOfWork,
            UserManager<ApplicationUser> userManager,
            IConfiguration configuration,
            IServiceProvider serviceProvider)
            : base(serviceProvider)
        {
            _unitOfWork = unitOfWork;
            _userManager = userManager;
            _configuration = configuration;
        }

        public string GenerateRefreshToken()
        {
            var randomNumber = new byte[64];
            using var rng = RandomNumberGenerator.Create();
            rng.GetBytes(randomNumber);
            return Convert.ToBase64String(randomNumber);
        }
        public async Task<string> GenerateAccessToken(LoginRequestDto loginRequest)
        {
            var accountRepository = Resolve<IAccountRepository>();
            var utility = Resolve<MonShop.BackEnd.Utility.Utils.Utility>();
            var user = await accountRepository.GetByExpression(u => u.Email.ToLower() == loginRequest.Email.ToLower());

            if (user != null)
            {
                var roles = await _userManager.GetRolesAsync(user);
                if (roles != null)
                {
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
                        expires: utility.GetCurrentDateInTimeZone().AddDays(1),
                        claims: claims,
                        signingCredentials: new SigningCredentials(authenKey, SecurityAlgorithms.HmacSha512Signature)
                        );
                    return new JwtSecurityTokenHandler().WriteToken(token);
                }
            }
            return string.Empty;
        }
        public async Task<TokenDto> GetNewToken(string refreshToken, string accountId)
        {
            string accessTokenNew = "";
            string refreshTokenNew = "";
            var accountRepository = Resolve<IAccountRepository>();
            var utility = Resolve<MonShop.BackEnd.Utility.Utils.Utility>();

            var user = await accountRepository.GetByExpression(u => u.Id.ToLower() == accountId);

            if (user != null && user.RefreshToken == refreshToken)
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
                    expires: utility.GetCurrentDateInTimeZone().AddDays(1),
                    claims: claims,
                    signingCredentials: new SigningCredentials(authenKey, SecurityAlgorithms.HmacSha512Signature)
                );

                accessTokenNew = new JwtSecurityTokenHandler().WriteToken(token);
                if (user.RefreshTokenExpiryTime <= utility.GetCurrentDateInTimeZone())
                {
                    user.RefreshToken = GenerateRefreshToken();
                    user.RefreshTokenExpiryTime = utility.GetCurrentDateInTimeZone().AddDays(1);
                    await _unitOfWork.SaveChangeAsync();
                    refreshTokenNew = user.RefreshToken;
                }
                else
                {
                    refreshTokenNew = refreshToken;
                }

            }
            return new TokenDto { Token = accessTokenNew, RefreshToken = refreshTokenNew };

        }
    }

}
