using MonShop.BackEnd.DAL.Contracts;
using MonShop.BackEnd.DAL.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MonShop.BackEnd.DAL.Repository
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly MonShopContext _db;

        public UnitOfWork(MonShopContext db)
        {
            _db = db;
        }


        public async Task SaveChangeAsync()
        {
            await _db.SaveChangesAsync();
        }
    }
}
