using MonShopLibrary.DTO;
using MonShop.Library.Models;
using MonShop.Library.DTO;
using Microsoft.EntityFrameworkCore;
using MonShop.Library.Repository.IRepository;
using MonShop.Library.Data;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using System.Data;


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
                Address = dto.Address

            };
            await _userManager.CreateAsync(user, dto.Password);
            return await _userManager.FindByEmailAsync(dto.Email);
        }

        public async Task UpdateAccount(ApplicationUser user)
        {
            var existingUser = await _userManager.FindByIdAsync(user.Id);

            if (existingUser != null)
            {
                existingUser.Email = user.Email;
                existingUser.FirstName = user.FirstName;
                existingUser.LastName = user.LastName;
                existingUser.PhoneNumber = user.PhoneNumber;
                existingUser.Address = user.Address;
                existingUser.UserName = user.Email;
                await _userManager.UpdateAsync(existingUser);
            }
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
            if (roleDb == null)
            {
                await _roleManager.CreateAsync(new IdentityRole(roleName));

            }
            if (user != null)
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

        #region address
        public async Task AddAddress(DeliveryAddress addressDto)
        {
            DeliveryAddress address = await _db.DeliveryAddresses.SingleOrDefaultAsync(a => a.Address == addressDto.Address);
            if (address == null)
            {
                await _db.DeliveryAddresses.AddAsync(addressDto);
                await _db.SaveChangesAsync();
            }
        }
        public async Task UpdateAddress(DeliveryAddress addressDto)
        {
            DeliveryAddress address = await _db.DeliveryAddresses.SingleOrDefaultAsync(a => a.Address == addressDto.Address);
            if (address == null)
            {
                 _db.DeliveryAddresses.Update(addressDto);
                await _db.SaveChangesAsync();
            }
        }
        public async Task RemoveAddress(DeliveryAddress addressDto)
        {
            DeliveryAddress address = await _db.DeliveryAddresses.SingleOrDefaultAsync(a => a.Address == addressDto.Address);
            if (address == null)
            {
                _db.DeliveryAddresses.Remove(addressDto);
                await _db.SaveChangesAsync();
            }
        }
        public async Task <List<DeliveryAddress>> GetAllAddressByUserId(string userId)
        {
            List<DeliveryAddress> list = await _db.DeliveryAddresses.Where(a => a.ApplicationUserId == userId).ToListAsync();
            return list;
        }
        #endregion
    }
}
