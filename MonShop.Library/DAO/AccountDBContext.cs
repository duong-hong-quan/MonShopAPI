using Microsoft.EntityFrameworkCore;
using MonShopLibrary.DTO;
using MonShop.Library.Models;
using MonShopLibrary.Utils;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MonShopLibrary.DAO
{
    public class AccountDBContext : MonShopContext
    {
        public AccountDBContext() { }

        public async Task<List<Account>> GetAllAccount()
        {
            List<Account> accounts = await this.Accounts.ToListAsync();
            return accounts;
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
                RoleId = dto.RoleId
            };
            await this.Accounts.AddAsync(account);
            await this.SaveChangesAsync();
        }

        public async Task UpdateAccount(AccountDTO dto)
        {

            Account account = new Account
            {
                AccountId = dto.AccountId,
                Email = dto.Email,
                FirstName = dto.FirstName,
                LastName = dto.LastName,

                Address = dto.Address,
                ImageUrl = dto.ImageUrl,
                IsDeleted = false,
                Password = dto.Password,
                RoleId = dto.RoleId
            };
            this.Accounts.Update(account);
            await this.SaveChangesAsync();
        }
        public async Task DeleteAccount(AccountDTO dto)
        {
            Account account = this.Accounts.Find(dto.AccountId);
            account.IsDeleted = true;
            await this.SaveChangesAsync();

        }

        public async Task<List<Role>> GetAllRole()
        {
            var list = await this.Roles.ToListAsync();
            return list;
        }

        public async Task<Account> GetAccountByID(int id)
        {
            Account account = await this.Accounts.FindAsync(id);
            return account;
        }

        public async Task<Account> Login(LoginRequest loginRequest)
        {
            Account account = await this.Accounts.Where(a => a.Email == loginRequest.Email && a.IsDeleted == false).FirstOrDefaultAsync();
            if (account != null && Utility.VerifyPassword(loginRequest.Password, account.Password))
            {
                return account;
            }
            return null;
        }

        public async Task<string> GenerateRefreshToken(int AccountID)
        {
            Token token = await this.Tokens.Where(a => a.AccountId == AccountID).FirstOrDefaultAsync();
            if (token == null)
            {
                Token refreshToken = new Token
                {
                    RefreshToken = Guid.NewGuid().ToString(),
                    AccountId = AccountID,
                    ExpiresAt = DateTime.UtcNow.AddMonths(1),
                };
                await this.Tokens.AddAsync(refreshToken);
                await this.SaveChangesAsync();
                return refreshToken.RefreshToken;

            }
            else if (token.ExpiresAt <= Utility.getInstance().GetCurrentDateTimeInTimeZone())
            {
                token.RefreshToken = Guid.NewGuid().ToString();
                token.ExpiresAt = DateTime.UtcNow.AddMonths(1);
                await this.SaveChangesAsync();
            }
            return token.RefreshToken;


        }

        public async Task<Token> GetToken(string token)
        {
            Token tokenDTO = await this.Tokens.FindAsync(token);
            if (tokenDTO != null)
            {
                return tokenDTO;
            }
            return null;
        }
    }
}