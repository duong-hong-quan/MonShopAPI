using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Monshop.BackEnd.Service.Contracts;
using MonShop.BackEnd.DAL.DTO;
using MonShop.BackEnd.DAL.DTO.Response;

namespace MonShop.BackEnd.API.Controller
{
    [Route("Product")]
    [ApiController]
    public class ProductController : ControllerBase
    {
        private readonly IProductService _productService;
        public ProductController(IProductService productService)
        {
            _productService = productService;
        }
        [HttpGet]
        [Route("GetAllCategory")]
        public async Task<AppActionResult> GetAllCategory()
        {
            return await _productService.GetAllCategory();

        }
        [HttpGet]
        [Route("GetAllProduct")]
        public async Task<AppActionResult> GetAllProduct()
        {
            return await _productService.GetAllProduct();

        }
        [HttpGet]
        [Route("GetAllProductByManager")]
        public async Task<AppActionResult> GetAllProductByManager()
        {
            return await _productService.GetAllProductByManager();

        }
        [HttpGet]
        [Route("GetAllProductStatus")]
        public async Task<AppActionResult> GetAllProductStatus()
        {
            return await _productService.GetAllProductStatus();
        }
        [HttpGet]
        [Route("GetProductByID/{id}")]
        public async Task<AppActionResult> GetProductByID(int id)
        {
            return await _productService.GetProductByID(id);

        }

        [HttpGet]
        [Route("GetProductInventory/{productId}/{sizeId}")]
        public async Task<AppActionResult> GetProductInventory(int productId, int sizeId)
        {
            return await _productService.GetProductInventory(productId, sizeId);

        }
        [Authorize(Roles = "Admin")]

        [HttpPost]
        [Route("AddProduct")]
        public async Task<AppActionResult> AddProduct(ProductDTO dto)
        {
            return await _productService.AddProduct(dto);


        }
        [Authorize(Roles = "Admin")]

        [HttpPut]
        [Route("UpdateProduct")]

        public async Task<AppActionResult> UpdateProduct(ProductDTO dto)
        {
            return await _productService.UpdateProduct(dto);

        }
        [Authorize(Roles = "Admin")]

        [HttpDelete]
        [Route("DeleteProduct")]

        public async Task<AppActionResult> DeleteProduct(ProductDTO dto)
        {
            return await _productService.DeleteProduct(dto);

        }

        [HttpGet]
        [Route("GetTopXProduct/{x:int}")]
        public async Task<AppActionResult> GetTopXProduct(int x)
        {
            return await _productService.GetTopXProduct(x);

        }

        [HttpGet]
        [Route("GetAllSize")]
        public async Task<AppActionResult> GetAllSize()
        {
            return await _productService.GetAllSize();

        }

        [HttpGet("GetAllProductByCategoryId/{CategoryId:int}")]
        public async Task<AppActionResult> GetAllProductByCategoryId(int CategoryId)
        {
            return await _productService.GetAllProductByCategoryId(CategoryId);

        }
    }
}
