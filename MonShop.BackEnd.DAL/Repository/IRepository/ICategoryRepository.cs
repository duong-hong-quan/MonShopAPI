using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MonShop.BackEnd.DAL.DTO;
using MonShop.BackEnd.DAL.Models;

namespace MonShop.BackEnd.DAL.Repository.IRepository
{
    public interface ICategoryRepository: IRepository<Category>
    {
        public Task<List<Category>> GetAllCategory();


        public Task AddCategory(CategoryDTO dto);


        public Task UpdateCategory(CategoryDTO dto);


        public Task DeleteCategory(CategoryDTO dto);
    }
}
