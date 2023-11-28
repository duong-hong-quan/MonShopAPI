using Microsoft.EntityFrameworkCore;
using MonShop.BackEnd.DAL.Data;
using MonShop.BackEnd.DAL.Repository.IRepository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace MonShop.BackEnd.DAL.Repository
{
    public class Repository<T> : IRepository<T> where T : class
    {
        private readonly MonShopContext _context;
        private readonly DbSet<T> _dbSet;

        public Repository(MonShopContext context)
        {
            _context = context;
            _dbSet = context.Set<T>();
        }

        public async Task<IOrderedQueryable<T>> GetAll()
        {
            return (IOrderedQueryable<T>)_dbSet.AsNoTracking();
        }

        public async Task<T> GetById(object id)
        {
            return await _dbSet.FindAsync(id);
        }

        public async Task<T> Insert(T entity)
        {
            await _dbSet.AddAsync(entity);
            return entity;
        }

        public async Task Update(T entity)
        {
            _dbSet.Attach(entity);
            _context.Entry(entity).State = EntityState.Modified;

        }

        public async Task DeleteById(object id)
        {
            T entityToDelete = await _dbSet.FindAsync(id);
            if (entityToDelete != null)
            {
                _dbSet.Remove(entityToDelete);
            }

        }

        public async Task<IOrderedQueryable<T>> GetListByExpression(Expression<Func<T, bool>> filter, params Expression<Func<T, object>>[] includeProperties)
        {
            var query = _dbSet.AsQueryable();

            // Apply eager loading
            if (includeProperties != null)
            {
                foreach (var includeProperty in includeProperties)
                {
                    query = query.Include(includeProperty);
                }

            }

            if (filter == null && includeProperties.Length > 0)
            {
                return (IOrderedQueryable<T>)await query.ToListAsync();
            }

            return (IOrderedQueryable<T>)query.Where(filter);
        }

        public async Task<T> GetByExpression(Expression<Func<T, bool>> filter, params Expression<Func<T, object>>[] includeProperties)
        {
            if (includeProperties != null)
            {
                foreach (var includeProperty in includeProperties)
                {
                    _dbSet.Include(includeProperty);
                }
            }
            return await _dbSet.SingleOrDefaultAsync(filter);
        }

        public async Task<IEnumerable<T>> InsertRange(IEnumerable<T> entities)
        {
            _dbSet.AddRange(entities);
            return entities;
        }
    }
}
