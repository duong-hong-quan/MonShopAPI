using MonShop.BackEnd.DAL.Contracts;
using MonShop.BackEnd.DAL.Data;
using MonShop.BackEnd.DAL.Models;
using MonShop.BackEnd.DAL.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MonShop.BackEnd.DAL.Implementations
{
    public class ProductStatusRepository : Repository<ProductStatus>, IProductStatusRepository
    {
        public ProductStatusRepository(MonShopContext context) : base(context)
        {
        }
    }
}
