using Microsoft.EntityFrameworkCore;
using MonShop.BackEnd.DAL.Data;
using MonShop.BackEnd.DAL.DTO;
using MonShop.BackEnd.DAL.IRepository;
using MonShop.BackEnd.DAL.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MonShop.BackEnd.DAL.Repository
{
    public class CartRepository : Repository<Cart>, ICartRepository
    {
        public CartRepository(MonShopContext context) : base(context)
        {
        }


    }

}
