using MonShop.BackEnd.DAL.Contracts;
using MonShop.BackEnd.DAL.Data;
using MonShop.BackEnd.DAL.Models;

namespace MonShop.BackEnd.DAL.Implementations
{
    public class CartRepository : Repository<Cart>, ICartRepository
    {
        public CartRepository(MonShopContext context) : base(context)
        {
        }


    }

}
