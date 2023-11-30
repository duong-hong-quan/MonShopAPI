using MonShop.BackEnd.DAL.DTO;
using MonShop.BackEnd.DAL.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Monshop.BackEnd.Service.Contracts
{
    public interface ICategoryService
    {
        public Task<List<Category>> GetAllCategory();


        public Task AddCategory(CategoryDTO dto);


        public Task UpdateCategory(CategoryDTO dto);


        public Task DeleteCategory(CategoryDTO dto);
    }
}
