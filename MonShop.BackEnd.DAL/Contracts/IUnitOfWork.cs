using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MonShop.BackEnd.DAL.Contracts
{
    public interface IUnitOfWork
    {
        Task SaveChangeAsync();

    }
}
