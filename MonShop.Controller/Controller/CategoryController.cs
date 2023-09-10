using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using MonShop.Controller.Model;
using MonShopLibrary.DTO;
using MonShopLibrary.Repository;

namespace MonShopAPI.Controller
{
    [Route("Category")]
    [ApiController]
    public class CategoryController : ControllerBase
    {
        private readonly ICategoryRepository _categoryRepository;
        private readonly ResponeDTO _responeDTO;
        public CategoryController(ICategoryRepository categoryRepository)
        {

            _categoryRepository = categoryRepository;
            _responeDTO = new ResponeDTO();
        }

        [HttpGet]
        [Route("GetAllCategory")]
        public async Task<ResponeDTO> GetAllCategory()
        {
            try
            {
                _responeDTO.Data = await _categoryRepository.GetAllCategory();

            }
            catch (Exception ex)
            {
                _responeDTO.IsSuccess = false;
                _responeDTO.Message = ex.Message;

            }

            return _responeDTO;
        }
        [HttpPost]
        [Route("AddCategory")]
        public async Task<ResponeDTO> AddCategory(CategoryDTO dto)
        {
            try
            {

                await _categoryRepository.AddCategory(dto);
                _responeDTO.Data = dto;

            }
            catch (Exception ex)
            {
                _responeDTO.IsSuccess = false;
                _responeDTO.Message = ex.Message;

            }

            return _responeDTO;
        }
        [HttpPut]
        [Route("UpdateCategory")]
        public async Task<ResponeDTO> UpdateCategory(CategoryDTO dto)
        {
            try
            {
                await _categoryRepository.UpdateCategory(dto);

                _responeDTO.Data = dto;

            }
            catch (Exception ex)
            {
                _responeDTO.IsSuccess = false;
                _responeDTO.Message = ex.Message;
            }

            return _responeDTO;
        }
        [HttpDelete]
        [Route("DeleteCategory")]

        public async Task<ResponeDTO> DeleteCategory(CategoryDTO dto)
        {
            try { 
            await _categoryRepository.DeleteCategory(dto);
                _responeDTO.Data = dto;


            }
            catch (Exception ex)
            {
                _responeDTO.IsSuccess = false;
                _responeDTO.Message = ex.Message;
            }

            return _responeDTO;
        }
    }
}
