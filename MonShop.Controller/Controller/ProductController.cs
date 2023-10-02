using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using MonShop.Controller.Model;
using MonShop.Library.Models;
using MonShop.Library.Repository.IRepository;
using MonShopLibrary.DTO;
using System.Collections.Generic;

namespace MonShopAPI.Controller
{
    [Route("Product")]
    [ApiController]
    public class ProductController : ControllerBase
    {
        private readonly IProductRepository _productRepository;
        private readonly ResponseDTO _response;
        public ProductController(IProductRepository productRepository)
        {
            _productRepository = productRepository;
            _response = new ResponseDTO();
        }
        [HttpGet]
        [Route("GetAllCategory")]
        public async Task<ResponseDTO> GetAllCategory()
        {
            try
            {
                var list = await _productRepository.GetAllCategory();
                _response.Data = list;

            }
            catch (Exception ex)
            {
                _response.IsSuccess = false;
                _response.Message = ex.Message;
            }
            return _response;
        }
        [HttpGet]
        [Route("GetAllProduct")]
        public async Task<ResponseDTO> GetAllProduct()
        {
            try
            {
                var list = await _productRepository.GetAllProduct();

                _response.Data = list;

            }
            catch (Exception ex)
            {
                _response.IsSuccess = false;
                _response.Message = ex.Message;
            }
            return _response;
        }
        [HttpGet]
        [Route("GetAllProductByManager")]
        public async Task<ResponseDTO> GetAllProductByManager()
        {
            try
            {

                var list = await _productRepository.GetAllProductByManager();
                _response.Data = list;

            }
            catch (Exception ex)
            {
                _response.IsSuccess = false;
                _response.Message = ex.Message;
            }
            return _response;
        }
        [HttpGet]
        [Route("GetAllProductStatus")]
        public async Task<ResponseDTO> GetAllProductStatus()
        {
            try
            {
                var list = await _productRepository.GetAllProductStatus();
                _response.Data = list;


            }
            catch (Exception ex)
            {
                _response.IsSuccess = false;
                _response.Message = ex.Message;
            }
            return _response;
        }
        [HttpGet]
        [Route("GetProductByID/{id}")]
        public async Task<ResponseDTO> GetProductByID(int id)
        {
            try
            {
                var product = await _productRepository.GetProductByID(id);
                _response.Data = product;

            }
            catch (Exception ex)
            {
                _response.IsSuccess = false;
                _response.Message = ex.Message;

            }
            return _response;
        }

        [HttpGet]
        [Route("GetProductInventory/{productId}/{sizeId}")]
        public async Task<ResponseDTO> GetProductInventory(int productId, int sizeId)
        {
            try
            {
                var product = await _productRepository.GetProductInventory(productId,sizeId);
                _response.Data = product;

            }
            catch (Exception ex)
            {
                _response.IsSuccess = false;
                _response.Message = ex.Message;

            }
            return _response;
        }
        [Authorize(Roles = "Admin")]

        [HttpPost]
        [Route("AddProduct")]
        public async Task<ResponseDTO> AddProduct(ProductDTO dto)
        {
            try
            {
                await _productRepository.AddProduct(dto);
                _response.Message = "Add product successfully";
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
        [Route("UpdateProduct")]

        public async Task<ResponseDTO> UpdateProduct(ProductDTO dto)
        {
            try
            {
                await _productRepository.UpdateProduct(dto);
                _response.Message = "Update Product Successfully";
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
        [Route("DeleteProduct")]

        public async Task<ResponseDTO> DeleteProduct(ProductDTO dto)
        {
            try
            {

                await _productRepository.DeleteProduct(dto);
                _response.Data = dto;
                _response.Message = "Delete Product Successfully";
            }
            catch (Exception ex)
            {
                _response.IsSuccess = false;
                _response.Message = ex.Message;
            }
            return _response;
        }

        [HttpGet]
        [Route("GetTopXProduct/{x:int}")]
        public async Task<ResponseDTO> GetTopXProduct(int x)
        {
            try
            {
                List<Product> list = await _productRepository.GetTopXProduct(x);
                _response.Data = list;


            }
            catch (Exception ex)
            {
                _response.IsSuccess = false;
                _response.Message = ex.Message;
            }
            return _response;
        }

        [HttpGet]
        [Route("GetAllSize")]
        public async Task<ResponseDTO> GetAllSize()
        {
            try
            {
                List<Size> list = await _productRepository.GetAllSize();
                _response.Data = list;


            }
            catch (Exception ex)
            {
                _response.IsSuccess = false;
                _response.Message = ex.Message;
            }
            return _response;
        }

        [HttpGet("GetAllProductByCategoryId/{CategoryId:int}")]
        public async Task<ResponseDTO> GetAllProductByCategoryId(int CategoryId)
        {
            try
            {
                List<Product> list = await _productRepository.GetAllProductByCategoryId(CategoryId);
                _response.Data = list;


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
