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
        public Task<string> Login(LoginRequest loginRequest);
        public Task SignUp(SignUpRequest dto);

        public Task<ApplicationUser> GetAccountById(string accountId);
    }
}
