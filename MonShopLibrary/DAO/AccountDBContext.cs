using Microsoft.EntityFrameworkCore;
using MonShopLibrary.DTO;
using MonShopLibrary.Models;
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
                FullName = dto.FullName,
                Address = dto.Address, 
                ImageUrl =dto.ImageUrl, 
                IsDeleted = false,
                Password = dto.Password ,
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
                FullName = dto.FullName,
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
    }
}
