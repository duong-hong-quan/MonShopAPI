using MonShop.BackEnd.DAL.Contracts;
using MonShop.BackEnd.DAL.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MonShop.BackEnd.DAL.Implementations
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly MonShopContext _db;

        public UnitOfWork(MonShopContext db)
        {
            _db = db;
        }


        public async Task BeginTransaction()
        {
            await _db.Database.BeginTransactionAsync();
        }

        public async Task SaveChangeAndCommitAsync()
        {
            await SaveChangeAsync();
            await CommitAsync();
        }

        public async Task CommitAsync()
        {
            await _db.Database.CommitTransactionAsync();
        }

        public async Task RollBack()
        {
            await _db.Database.RollbackTransactionAsync();
        }

        public async Task SaveChangeAsync()
        {
            await _db.SaveChangesAsync();
        }
    }
}
