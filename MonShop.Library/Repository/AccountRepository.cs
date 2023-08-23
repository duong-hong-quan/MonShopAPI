using MonShopLibrary.DAO;
using MonShopLibrary.DTO;
using MonShop.Library.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MonShop.Library.DTO;

namespace MonShopLibrary.Repository
{
    public class AccountRepository : IAccountRepository
    {
        AccountDBContext db = new AccountDBContext();

        public async Task AddAccount(AccountDTO dto) => await db.AddAccount(dto);
        public async Task DeleteAccount(AccountDTO dto) => await db.DeleteAccount(dto);
        public async Task<List<Account>> GetAllAccount() => await db.GetAllAccount();
        public async Task UpdateAccount(AccountDTO dto)=> await db.UpdateAccount(dto);
        public async Task<List<Role>> GetAllRole()=> await db.GetAllRole();
        public async Task<Account> GetAccountByID(int id) => await db.GetAccountByID(id);
        public async Task<Account> Login(LoginRequest loginRequest) => await db.Login(loginRequest);
        public async Task<string> GenerateRefreshToken(int AccountID)=> await db.GenerateRefreshToken(AccountID);
        public async Task<Token> GetToken(string token) => await db.GetToken(token);
        public async Task SignUp(AccountDTO dto) => await db.SignUp(dto);
        public async Task ChangePassword(ChangePasswordRequest request)=> await db.ChangePassword(request);



    }
}
