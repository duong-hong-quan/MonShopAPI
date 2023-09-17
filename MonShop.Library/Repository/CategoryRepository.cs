using MonShopLibrary.DTO;
using MonShop.Library.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace MonShopLibrary.Repository
{
    public class CategoryRepository : ICategoryRepository
    {

        private readonly MonShopContext _db;

        public CategoryRepository(MonShopContext db)
        {
            _db = db;
        }

        public async Task<List<Category>> GetAllCategory()
        {
            List<Category> list = await _db.Categories.ToListAsync();
            return list;
        }

        public async Task AddCategory(CategoryDTO dto)
        {
            Category category = new Category { CategoryId = dto.CategoryId, CategoryName = dto.CategoryName };
            await _db.Categories.AddAsync(category);
            await _db.SaveChangesAsync();
        }

        public async Task UpdateCategory(CategoryDTO dto)
        {
            Category category = new Category { CategoryId = dto.CategoryId, CategoryName = dto.CategoryName };
            _db.Categories.Update(category);
            await _db.SaveChangesAsync();
        }

        public async Task DeleteCategory(CategoryDTO dto)
        {
            Category category = await _db.Categories.FirstAsync(c => c.CategoryId == dto.CategoryId);
            _db.Categories.Remove(category);
            await _db.SaveChangesAsync();
        }

    }
}
