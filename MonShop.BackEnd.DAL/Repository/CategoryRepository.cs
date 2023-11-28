using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using MonShop.BackEnd.DAL.DTO;
using MonShop.BackEnd.DAL.Repository.IRepository;
using MonShop.BackEnd.DAL.Models;
using MonShop.BackEnd.DAL.Data;

namespace MonShop.BackEnd.DAL.Repository
{
    public class CategoryRepository : Repository<Category>,ICategoryRepository
    {
        public CategoryRepository(MonShopContext context) : base(context)
        {
        }

        public async Task<List<Category>> GetAllCategory()
        {
            List<Category> list = await context.Category.ToListAsync();
            return list;
        }

        public async Task AddCategory(CategoryDTO dto)
        {
            Category category = new Category { CategoryId = dto.CategoryId, CategoryName = dto.CategoryName };
            await context.Category.AddAsync(category);
            await context.SaveChangesAsync();
        }

        public async Task UpdateCategory(CategoryDTO dto)
        {
            Category category = new Category { CategoryId = dto.CategoryId, CategoryName = dto.CategoryName };
            context.Category.Update(category);
            await context.SaveChangesAsync();
        }

        public async Task DeleteCategory(CategoryDTO dto)
        {
            Category category = await context.Category.FirstAsync(c => c.CategoryId == dto.CategoryId);
            context.Category.Remove(category);
            await context.SaveChangesAsync();
        }

    }
}
