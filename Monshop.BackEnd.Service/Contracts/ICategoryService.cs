using MonShop.BackEnd.DAL.DTO;
using MonShop.BackEnd.DAL.DTO.Response;
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
        public Task<AppActionResult> GetAllCategory();


        public Task<AppActionResult> AddCategory(CategoryDTO dto);


        public Task<AppActionResult> UpdateCategory(CategoryDTO dto);


        public Task<AppActionResult> DeleteCategory(CategoryDTO dto);
    }
}
