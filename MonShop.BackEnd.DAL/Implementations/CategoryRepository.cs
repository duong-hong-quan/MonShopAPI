using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using MonShop.BackEnd.DAL.DTO;
using MonShop.BackEnd.DAL.Models;
using MonShop.BackEnd.DAL.Data;
using MonShop.BackEnd.DAL.IRepository;

namespace MonShop.BackEnd.DAL.Repository
{
    public class CategoryRepository : Repository<Category>,ICategoryRepository
    {
        private MonShopContext _context;

        public CategoryRepository(MonShopContext context) : base(context)
        {
            _context = context;
        }

        public async Task<List<Category>> GetAllCategory()
        {
            List<Category> list = await _context.Category.ToListAsync();
            return list;
        }

        public async Task AddCategory(CategoryDTO dto)
        {
            Category category = new Category { CategoryId = dto.CategoryId, CategoryName = dto.CategoryName };
            await _context.Category.AddAsync(category);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateCategory(CategoryDTO dto)
        {
            Category category = new Category { CategoryId = dto.CategoryId, CategoryName = dto.CategoryName };
            _context.Category.Update(category);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteCategory(CategoryDTO dto)
        {
            Category category = await _context.Category.FirstAsync(c => c.CategoryId == dto.CategoryId);
            _context.Category.Remove(category);
            await _context.SaveChangesAsync();
        }

    }
}
