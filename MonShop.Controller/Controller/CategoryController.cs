using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using MonShopLibrary.DTO;
using MonShopLibrary.Repository;

namespace MonShopAPI.Controller
{
    [Route("Category")]
    [ApiController]
    public class CategoryController : ControllerBase
    {
        private readonly ICategoryRepository _categoryRepository;
        public CategoryController(ICategoryRepository categoryRepository)
        {
            _categoryRepository = categoryRepository;
        }

        [HttpGet]
        [Route("GetAllCategory")]
        public async Task<IActionResult> GetAllCategory()
        {
            var list  = await _categoryRepository.GetAllCategory();
            return Ok(list);
        }
        [HttpPost]
        [Route("AddCategory")]
        public async Task<IActionResult> AddCategory(CategoryDTO dto)
        {
            await _categoryRepository.AddCategory(dto);
            return Ok();
        }
        [HttpPut]
        [Route("UpdateCategory")]
        public async Task<IActionResult> UpdateCategory(CategoryDTO dto)
        {
            await _categoryRepository.UpdateCategory(dto);
            return Ok();
        }
        [HttpDelete]
        [Route("DeleteCategory")]

        public async Task<IActionResult> DeleteCategory(CategoryDTO dto)
        {
            await _categoryRepository.DeleteCategory(dto);
            return Ok();
        }
    }
}
