using MonShopLibrary.DTO;
using MonShop.Library.Models;
using MonShop.Library.DTO;
using Microsoft.EntityFrameworkCore;
using MonShop.Library.Repository.IRepository;
using MonShop.Library.Data;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;


namespace MonShopLibrary.Repository
{
    public class AccountRepository : IAccountRepository
    {
        private readonly MonShopContext _db;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly IConfiguration _configuration;
        private readonly TokenModel _loginResponse;
        private readonly IJwtGenerator _jwtGenerator;

        public AccountRepository(
            MonShopContext db,
            SignInManager<ApplicationUser> signInManager,
            UserManager<ApplicationUser> userManager,
            IConfiguration configuration,
            RoleManager<IdentityRole> roleManager,
            IJwtGenerator jwtGenerator
            )
        {
            _db = db;
            _signInManager = signInManager;
            _userManager = userManager;
            _configuration = configuration;
            _roleManager = roleManager;
            _loginResponse = new TokenModel();
            _jwtGenerator = jwtGenerator;
        }

        #region Account
        public async Task<TokenModel> Login(LoginRequest loginRequest)
        {
            var result = await _signInManager.PasswordSignInAsync(loginRequest.Email, loginRequest.Password, false, false);


            if (!result.Succeeded)
            {
                return _loginResponse;

            }
            string token = await _jwtGenerator.GenerateAccessToken(loginRequest);
            var user = await _db.Users.FirstOrDefaultAsync(u => u.Email.ToLower() == loginRequest.Email.ToLower());
            string refreshToken = user.RefreshToken;

            if (user?.RefreshToken == null || user.RefreshTokenExpiryTime <= DateTime.Now)
            {
                user.RefreshTokenExpiryTime = DateTime.Now.AddDays(1);
                user.RefreshToken = _jwtGenerator.GenerateRefreshToken();
                await _db.SaveChangesAsync();

            }
            _loginResponse.Token = token;
            _loginResponse.RefreshToken = refreshToken;
            return _loginResponse;
        }


        public async Task<ApplicationUser> SignUp(SignUpRequest dto)
        {
            var user = new ApplicationUser
            {
                Email = dto.Email,
                UserName = dto.Email,
                FirstName = dto.FirstName,
                LastName = dto.LastName,
                PhoneNumber = dto.PhoneNumber,

            };
            await _userManager.CreateAsync(user, dto.Password);
            return await _userManager.FindByEmailAsync(dto.Email);
        }

        public async Task UpdateAccount(ApplicationUser user)
        {
            await _userManager.UpdateAsync(user);
        }
        public async Task DeleteAccount(ApplicationUser user)
        {
            await _userManager.DeleteAsync(user);
        }

        public async Task<ApplicationUser> GetAccountById(string accountId)
        {
            return await _db.Users.FirstOrDefaultAsync(a => a.Id == accountId);
        }

        public async Task<IEnumerable<ApplicationUser>> GetAllAccount()
        {
            return await _db.Users.ToListAsync();

        }

        #endregion

        #region Role
        public async Task AssignRole(string userId, string roleName)
        {
            var user = _db.Users.FirstOrDefault(u => u.Id.ToLower() == userId);
            var roleDb = await _db.Roles.SingleOrDefaultAsync(r => r.Name == roleName);

            if (user != null && roleDb != null)
            {
                await _userManager.AddToRoleAsync(user, roleName);
            }
        }
        public async Task<IEnumerable<IdentityRole>> GetAllRole()
        {
            return await _roleManager.Roles.ToListAsync();
        }
        public async Task<IdentityRole<string>> GetRoleForUserId(string userId)
        {
            IdentityUserRole<string> roleUser = await _db.UserRoles.FindAsync(userId);
            IdentityRole<string> role = await _db.Roles.FindAsync(roleUser.RoleId);
            return role;
        }
        public async Task AddRole(string role)
        {
            var roleDb = await _db.Roles.SingleOrDefaultAsync(r => r.Name == role);
            if (roleDb == null)
            {
                await _roleManager.CreateAsync(new IdentityRole(role));
            }
        }

        public async Task UpdateRole(IdentityRole roleDto)
        {
            await _roleManager.UpdateAsync(roleDto);
        }


        public async Task DeleteRole(IdentityRole roleDto)
        {
            await _roleManager.DeleteAsync(roleDto);
        }
        #endregion

    }
}
