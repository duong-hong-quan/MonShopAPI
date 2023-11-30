using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using MonShop.BackEnd.DAL.Models;
using MonShop.BackEnd.DAL.DTO;
using MonShop.BackEnd.Utility.Utils;
using MonShop.BackEnd.DAL.Data;
using MonShop.BackEnd.DAL.IRepository;

namespace MonShop.BackEnd.DAL.Repository
{
    public class ProductRepository : Repository<Product>, IProductRepository
    {
        public ProductRepository(MonShopContext context) : base(context)
        {
        }
    }
}
