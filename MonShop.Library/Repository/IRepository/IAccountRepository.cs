using MonShopLibrary.DTO;
using MonShop.Library.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MonShop.Library.DTO;

namespace MonShop.Library.Repository.IRepository
{
    public interface IAccountRepository
    {
        public Task<Account> Login(LoginRequest loginRequest);
        public Task<List<Account>> GetAllAccount();

        public Task AddAccount(AccountDTO dto);


        public Task UpdateAccount(AccountDTO dto);

        public Task DeleteAccount(AccountDTO dto);
        public Task<List<Role>> GetAllRole();

        public Task<Account> GetAccountByID(int id);
        public Task<string> GenerateRefreshToken(int AccountID);
        public Task<Token> GetToken(string token);
        public Task SignUp(AccountDTO dto);
        public Task ChangePassword(ChangePasswordRequest request);
    }
}
