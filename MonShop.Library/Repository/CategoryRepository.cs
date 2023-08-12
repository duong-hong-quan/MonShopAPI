using MonShopLibrary.DAO;
using MonShopLibrary.DTO;
using MonShop.Library.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MonShopLibrary.Repository
{
    public class CategoryRepository : ICategoryRepository
    {
        CategoryDBContext db = new CategoryDBContext();
        public async Task<List<Category>> GetAllCategory() => await db.GetAllCategory();
      

        public async Task AddCategory(CategoryDTO dto) => await db.AddCategory(dto);
      

        public async Task UpdateCategory(CategoryDTO dto) => await db.UpdateCategory(dto);  
      

        public async Task DeleteCategory(CategoryDTO dto)=> await db.DeleteCategory(dto);
      
    }
}
