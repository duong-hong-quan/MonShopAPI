using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using MonShop.Controller.Model;
using MonShop.Library.Models;
using MonShopLibrary.DTO;
using MonShopLibrary.Repository;
using System.Collections.Generic;

namespace MonShopAPI.Controller
{
    [Route("Product")]
    [ApiController]
    public class ProductController : ControllerBase
    {
        private readonly IProductRepository _productRepository;
        private readonly ResponeDTO _responeDTO;
        public ProductController(IProductRepository productRepository)
        {
            _productRepository = productRepository;
            _responeDTO = new ResponeDTO();
        }
        [HttpGet]
        [Route("GetAllCategory")]
        public async Task<ResponeDTO> GetAllCategory()
        {
            try
            {
                var list = await _productRepository.GetAllCategory();
                _responeDTO.Data = list;

            }
            catch (Exception ex)
            {
                _responeDTO.IsSuccess = false;
                _responeDTO.Message = ex.Message;
            }
            return _responeDTO;
        }
        [HttpGet]
        [Route("GetAllProduct")]
        public async Task<ResponeDTO> GetAllProduct()
        {
            try
            {
                var list = await _productRepository.GetAllProduct();

                _responeDTO.Data = list;

            }
            catch (Exception ex)
            {
                _responeDTO.IsSuccess = false;
                _responeDTO.Message = ex.Message;
            }
            return _responeDTO;
        }
        [HttpGet]
        [Route("GetAllProductByManager")]
        public async Task<ResponeDTO> GetAllProductByManager()
        {
            try
            {

                var list = await _productRepository.GetAllProductByManager();
                _responeDTO.Data = list;

            }
            catch (Exception ex)
            {
                _responeDTO.IsSuccess = false;
                _responeDTO.Message = ex.Message;
            }
            return _responeDTO;
        }
        [HttpGet]
        [Route("GetAllProductStatus")]
        public async Task<ResponeDTO> GetAllProductStatus()
        {
            try
            {
                var list = await _productRepository.GetAllProductStatus();
                _responeDTO.Data = list;


            }
            catch (Exception ex)
            {
                _responeDTO.IsSuccess = false;
                _responeDTO.Message = ex.Message;
            }
            return _responeDTO;
        }
        [HttpGet]
        [Route("GetProductByID")]
        public async Task<ResponeDTO> GetProductByID(int id)
        {
            try
            {
                var product = await _productRepository.GetProductByID(id);
                _responeDTO.Data = product;

            }
            catch (Exception ex)
            {
                _responeDTO.IsSuccess = false;
                _responeDTO.Message = ex.Message;

            }
            return _responeDTO;
        }
        [HttpPost]
        [Route("AddProduct")]
        public async Task<ResponeDTO> AddProduct(ProductDTO dto)
        {
            try
            {
                await _productRepository.AddProduct(dto);
                _responeDTO.Message = "Add product successfully";
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
        [Route("UpdateProduct")]

        public async Task<ResponeDTO> UpdateProduct(ProductDTO dto)
        {
            try
            {
                await _productRepository.UpdateProduct(dto);
                _responeDTO.Message = "Update Product Successfully";
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
        [Route("DeleteProduct")]

        public async Task<ResponeDTO> DeleteProduct(ProductDTO dto)
        {
            try
            {

                await _productRepository.DeleteProduct(dto);
                _responeDTO.Data = dto;
                _responeDTO.Message = "Delete Product Successfully";
            }
            catch (Exception ex)
            {
                _responeDTO.IsSuccess = false;
                _responeDTO.Message = ex.Message;
            }
            return _responeDTO;
        }
        [HttpGet]
        [Route("GetTop4")]
        public async Task<ResponeDTO> GetTop4()
        {
            try
            {
                List<Product> list = await _productRepository.GetTop4();
                _responeDTO.Data = list;


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
