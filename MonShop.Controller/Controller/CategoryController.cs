using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MonShop.Controller.Model;
using MonShop.Library.Repository.IRepository;
using MonShopLibrary.DTO;

namespace MonShopAPI.Controller
{
    [Route("Category")]
    [ApiController]

    public class CategoryController : ControllerBase
    {
        private readonly ICategoryRepository _categoryRepository;
        private readonly ResponseDTO _response;
        public CategoryController(ICategoryRepository categoryRepository)
        {

            _categoryRepository = categoryRepository;
            _response = new ResponseDTO();
        }

        [HttpGet]
        [Route("GetAllCategory")]
        public async Task<ResponseDTO> GetAllCategory()
        {
            try
            {
                _response.Data = await _categoryRepository.GetAllCategory();

            }
            catch (Exception ex)
            {
                _response.IsSuccess = false;
                _response.Message = ex.Message;

            }

            return _response;
        }
        [Authorize(Roles ="Admin")]
        [HttpPost]
        [Route("AddCategory")]
        public async Task<ResponseDTO> AddCategory(CategoryDTO dto)
        {
            try
            {

                await _categoryRepository.AddCategory(dto);
                _response.Data = dto;

            }
            catch (Exception ex)
            {
                _response.IsSuccess = false;
                _response.Message = ex.Message;

            }

            return _response;
        }
        [Authorize(Roles = "Admin")]
        [HttpPut]
        [Route("UpdateCategory")]
        public async Task<ResponseDTO> UpdateCategory(CategoryDTO dto)
        {
            try
            {
                await _categoryRepository.UpdateCategory(dto);

                _response.Data = dto;

            }
            catch (Exception ex)
            {
                _response.IsSuccess = false;
                _response.Message = ex.Message;
            }

            return _response;
        }


        [Authorize(Roles = "Admin")]
        [HttpDelete]
        [Route("DeleteCategory")]

        public async Task<ResponseDTO> DeleteCategory(CategoryDTO dto)
        {
            try { 
            await _categoryRepository.DeleteCategory(dto);
                _response.Data = dto;


            }
            catch (Exception ex)
            {
                _response.IsSuccess = false;
                _response.Message = ex.Message;
            }

            return _response;
        }
    }
}
