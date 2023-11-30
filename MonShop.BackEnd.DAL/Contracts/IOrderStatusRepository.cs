using MonShop.BackEnd.DAL.IRepository;
using MonShop.BackEnd.DAL.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MonShop.BackEnd.DAL.Contracts
{
    public interface IOrderStatusRepository :IRepository<OrderStatus>
    {
    }
}
