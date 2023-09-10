﻿using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using MonShop.Library.Models;
using MonShopLibrary.DTO;
using MonShopLibrary.Repository;

namespace MonShopAPI.Controller
{
    [Route("Product")]
    [ApiController]
    public class ProductController : ControllerBase
    {
        private readonly IProductRepository _productRepository;

        public ProductController(IProductRepository productRepository)
        {
            _productRepository = productRepository;
        }
        [HttpGet]
        [Route("GetAllCategory")]
        public async Task<IActionResult> GetAllCategory()
        {
            var list = await _productRepository.GetAllCategory();
            return Ok(list);
        }
        [HttpGet]
        [Route("GetAllProduct")]
        public async Task<IActionResult> GetAllProduct()
        {
            var list = await _productRepository.GetAllProduct();
            return Ok(list);
        }
        [HttpGet]
        [Route("GetAllProductByManager")]
        public async Task<IActionResult> GetAllProductByManager()
        {
            var list = await _productRepository.GetAllProductByManager();
            return Ok(list);
        }
        [HttpGet]
        [Route("GetAllProductStatus")]
        public async Task<IActionResult> GetAllProductStatus()
        {
            var list = await _productRepository.GetAllProductStatus();
            return Ok(list);
        }
        [HttpGet]
        [Route("GetProductByID")]
        public async Task<IActionResult> GetProductByID(int id)
        {
            var product = await _productRepository.GetProductByID(id);
            return Ok(product);
        }
        [HttpPost]
        [Route("AddProduct")]
        public async Task<IActionResult> AddProduct(ProductDTO dto)
        {
            await _productRepository.AddProduct(dto);
            return Ok("Add product successfully");
        }
        [HttpPut]
        [Route("UpdateProduct")]

        public async Task<IActionResult> UpdateProduct(ProductDTO dto)
        {
            await _productRepository.UpdateProduct(dto);
            return Ok("Update Product Successfully");
        }
        [HttpDelete]
        [Route("DeleteProduct")]

        public async Task<IActionResult> DeleteProduct(ProductDTO dto)
        {
            await _productRepository.DeleteProduct(dto);
            return Ok("Delete Product Successfully");
        }
        [HttpGet]
        [Route("GetTop4")]
        public async Task<IActionResult> GetTop4()
        {
            List<Product> list = await _productRepository.GetTop4();
            return Ok(list);
        }
    }
}
