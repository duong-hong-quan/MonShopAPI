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

namespace MonShopLibrary.Repository
{
    public class AccountRepository : IAccountRepository
    {
        private readonly MonShopContext _db;


        public AccountRepository(MonShopContext db)
        {
            _db = db;
        }

        public async Task<List<Account>> GetAllAccount()
        {
            List<Account> Account = await _db.Account.Include(a => a.Role).ToListAsync();
            return Account;
        }

        public async Task AddAccount(AccountDTO dto)
        {
            Account account = new Account
            {
                Email = dto.Email,
                FirstName = dto.FirstName,
                LastName = dto.LastName,
                Address = dto.Address,
                ImageUrl = dto.ImageUrl,
                IsDeleted = false,
                Password = Utility.HashPassword(dto.Password),
                RoleId = dto.RoleId,
                PhoneNumber = dto.PhoneNumber,
            };
            await _db.Account.AddAsync(account);
            await _db.SaveChangesAsync();
        }
        public async Task SignUp(AccountDTO dto)
        {
            Account account = new Account
            {
                Email = dto.Email,
                FirstName = dto.FirstName,
                LastName = dto.LastName,
                Address = dto.Address,
                ImageUrl = dto.ImageUrl,
                IsDeleted = false,
                Password = Utility.HashPassword(dto.Password),
                RoleId = 3,
                PhoneNumber = dto.PhoneNumber,
            };
            await _db.Account.AddAsync(account);
            await _db.SaveChangesAsync();
        }

        public async Task ChangePassword(ChangePasswordRequest request)
        {
            Account account = await GetAccountByID(request.AccountId);
            if (account != null)
            {

                account.Password = Utility.HashPassword(request.NewPassword);
            }
            await _db.SaveChangesAsync();
        }

        public async Task UpdateAccount(AccountDTO dto)
        {
            Account account = await GetAccountByID(dto.AccountId);

            account.AccountId = dto.AccountId;
            account.Email = dto.Email;
            account.FirstName = dto.FirstName;
            account.LastName = dto.LastName;
            account.Password = account.Password;
            account.Address = dto.Address;
            account.ImageUrl = account.ImageUrl;
            account.IsDeleted = dto.IsDeleted;
            account.RoleId = dto.RoleId;
            account.PhoneNumber = dto.PhoneNumber;
            await _db.SaveChangesAsync();
        }
        public async Task DeleteAccount(AccountDTO dto)
        {
            Account account = await _db.Account.FirstAsync(a => a.AccountId == dto.AccountId);
            account.IsDeleted = true;
            await _db.SaveChangesAsync();

        }

        public async Task<List<Role>> GetAllRole()
        {
            var list = await _db.Role.ToListAsync();
            return list;
        }

        public async Task<Account> GetAccountByID(int id)
        {
            Account account = await _db.Account.FirstAsync(a => a.AccountId == id);
            return account;
        }
        public async Task<Account> GetAccountByEmail(string email)
        {
            Account account = await _db.Account.Where(a => a.Email == email).FirstAsync();
            return account;
        }
        public async Task<Account> Login(LoginRequest loginRequest)
        {
            Account account = await _db.Account.Where(a => a.Email == loginRequest.Email && a.IsDeleted == false).FirstAsync();
            if (account != null && Utility.VerifyPassword(loginRequest.Password, account.Password))
            {
                return account;
            }
            return null;
        }


        public async Task<string> GenerateRefreshToken(int AccountID)
        {
            Token token = await _db.Token.Where(a => a.AccountId == AccountID).FirstOrDefaultAsync();
            string refToken = "";
            if (token == null)
            {
                Token refreshToken = new Token
                {
                    RefreshToken = Guid.NewGuid().ToString(),
                    AccountId = AccountID,
                    ExpiresAt = DateTime.UtcNow.AddMonths(1),
                };
                await _db.Token.AddAsync(refreshToken);
                await _db.SaveChangesAsync();
            }
            else if (token.ExpiresAt <= Utility.getInstance().GetCurrentDateTimeInTimeZone())
            {
                token.RefreshToken = Guid.NewGuid().ToString();
                token.ExpiresAt = DateTime.UtcNow.AddMonths(1);
                await _db.SaveChangesAsync();
            }
            refToken = token.RefreshToken;

            return refToken;


        }

        public async Task<Token> GetToken(string token)
        {
            Token tokenDTO = await _db.Token.FirstAsync(t => t.RefreshToken == token);

            return tokenDTO;

        }



    }
}
