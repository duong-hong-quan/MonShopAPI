using MonShopLibrary.DTO;
using MonShop.Library.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MonShop.Library.DTO;
using Microsoft.EntityFrameworkCore;
using MonShopLibrary.Utils;
using MonShop.Library.Repository.IRepository;
using MonShop.Library.Data;
using Microsoft.AspNetCore.Identity;
using System.Security.Claims;
using System.IdentityModel.Tokens.Jwt;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using Utility = MonShopLibrary.Utils.Utility;

namespace MonShopLibrary.Repository
{
    public class AccountRepository : IAccountRepository
    {
        private readonly MonShopContext _db;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly IConfiguration _configuration;


        public AccountRepository(MonShopContext db, SignInManager<ApplicationUser> signInManager, UserManager<ApplicationUser> userManager, IConfiguration configuration, RoleManager<IdentityRole> roleManager)
        {
            _db = db;
            _signInManager = signInManager;
            _userManager = userManager;
            _configuration = configuration;
            _roleManager = roleManager;
        }

        public  async Task AssignRole(SignUpRequest request)
        {
            var user = _db.Users.FirstOrDefault(u => u.Email.ToLower() == request.Email.ToLower());
            if (user != null)
            {
                if (!await _roleManager.RoleExistsAsync(request.Role))
                {
                    _roleManager.CreateAsync(new IdentityRole(request.Role)).GetAwaiter().GetResult();
                }
                await _userManager.AddToRoleAsync(user, request.Role);
            }
        }

        public async Task SignUp(SignUpRequest dto)
        {
            var user = new ApplicationUser { Email = dto.Email, UserName = dto.Email, FirstName = dto.FirstName, LastName = dto.LastName };
            await _userManager.CreateAsync(user, dto.Password);
            await AssignRole(dto);
        }

        public async Task<string?> Login(LoginRequest loginRequest)
        {
            var result = await _signInManager.PasswordSignInAsync(loginRequest.Email, loginRequest.Password, false, false);
            var user = _db.Users.FirstOrDefault(u => u.Email.ToLower() == loginRequest.Email.ToLower());

            var roles = await _userManager.GetRolesAsync(user);

            if (!result.Succeeded)
            {
                return string.Empty;

            }
            var claims = new List<Claim>
            {
           new Claim (ClaimTypes.Email, loginRequest.Email),
           new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
           };
            claims.AddRange(roles.Select(role => new Claim(ClaimTypes.Role, role)));


            var authenKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["JWT:Key"]));
            var token = new JwtSecurityToken(
                issuer: _configuration["JWT:Issuer"],
                audience: _configuration["JWT:Audience"],
                expires: DateTime.Now.AddDays(1),
                claims: claims,
                signingCredentials: new Microsoft.IdentityModel.Tokens.SigningCredentials(authenKey, SecurityAlgorithms.HmacSha512Signature)
                );

            return new JwtSecurityTokenHandler().WriteToken(token);
        }

        public async Task<ApplicationUser> GetAccountById(string accountId)
        {
            return await _db.Users.FirstOrDefaultAsync(a => a.Id == accountId);
        }
    }
}
