using Microsoft.AspNetCore.Mvc;
using Monshop.BackEnd.Service.Contracts;
using MonShop.BackEnd.DAL.DTO;
using MonShop.BackEnd.DAL.DTO.Response;

namespace MonShop.BackEnd.API.Controller
{
    [Route("Category")]
    [ApiController]

    public class CategoryController : ControllerBase
    {
        private readonly ICategoryService _categoryService;
        public CategoryController(ICategoryService categoryService)
        {

            _categoryService = categoryService;
        }

        [HttpGet]
        [Route("GetAllCategory")]
        public async Task<AppActionResult> GetAllCategory()
        {
            return await _categoryService.GetAllCategory();

        }
        [HttpPost]
        [Route("AddCategory")]
        public async Task<AppActionResult> AddCategory(CategoryDTO dto)
        {
            return await _categoryService.AddCategory(dto);

        }
        [HttpPut]
        [Route("UpdateCategory")]
        public async Task<AppActionResult> UpdateCategory(CategoryDTO dto)
        {
            return await _categoryService.UpdateCategory(dto);

        }


        [HttpDelete]
        [Route("DeleteCategory")]

        public async Task<AppActionResult> DeleteCategory(CategoryDTO dto)
        {
            return await _categoryService.DeleteCategory(dto);
        }
    }
}
