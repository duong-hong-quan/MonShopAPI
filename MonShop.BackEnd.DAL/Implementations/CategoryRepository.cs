using MonShop.BackEnd.DAL.Models;
using MonShop.BackEnd.DAL.Data;
using MonShop.BackEnd.DAL.Contracts;

namespace MonShop.BackEnd.DAL.Implementations
{
    public class CategoryRepository : Repository<Category>, ICategoryRepository
    {

        public CategoryRepository(MonShopContext context) : base(context)
        {
        }

      

    }
}
