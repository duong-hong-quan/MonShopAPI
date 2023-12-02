using MonShop.BackEnd.DAL.Models;
using MonShop.BackEnd.DAL.Data;
using MonShop.BackEnd.DAL.Contracts;

namespace MonShop.BackEnd.DAL.Implementations
{
    public class ProductRepository : Repository<Product>, IProductRepository
    {
        public ProductRepository(MonShopContext context) : base(context)
        {
        }
    }
}
