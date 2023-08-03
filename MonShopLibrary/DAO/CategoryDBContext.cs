using Microsoft.EntityFrameworkCore;
using MonShopLibrary.DTO;
using MonShopLibrary.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MonShopLibrary.DAO
{
    public class CategoryDBContext: MonShopContext
    {
        public CategoryDBContext() { }  

        public async Task<List<Category>> GetAllCategory()
        {
            List<Category> list = await this.Categories.ToListAsync();
            return list;
        }

        public async Task AddCategory(CategoryDTO dto)
        {
            Category category = new Category { CategoryId = dto.CategoryId , CategoryName = dto.CategoryName};
            await this.Categories.AddAsync(category);
            await this.SaveChangesAsync();  
        }

        public async Task UpdateCategory(CategoryDTO dto)
        {
            Category category = await this.Categories.FindAsync(dto.CategoryId);
             category = new Category { CategoryId = dto.CategoryId, CategoryName = dto.CategoryName };
             this.Categories.Update(category);
            await this.SaveChangesAsync();
        }

        public async Task DeleteCategory(CategoryDTO dto)
        {
            Category category = await this.Categories.FindAsync(dto.CategoryId);
            this.Categories.Remove(category);   
            await this.SaveChangesAsync();  
        }

    }
}
