using MonShop.BackEnd.DAL.Models;
using MonShop.BackEnd.DAL.Data;
using MonShop.BackEnd.DAL.Contracts;


namespace MonShop.BackEnd.DAL.Implementations
{
    public class AccountRepository : Repository<ApplicationUser>, IAccountRepository
    {
        public AccountRepository(MonShopContext context) : base(context)
        {
        }
    }
}
