using MonShopLibrary.DTO;
using MonShopLibrary.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MonShopLibrary.Repository
{
    public interface ICategoryRepository
    {
        public Task<List<Category>> GetAllCategory();


        public  Task AddCategory(CategoryDTO dto);


        public  Task UpdateCategory(CategoryDTO dto);


        public  Task DeleteCategory(CategoryDTO dto);
    }
}
