using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using MonShopLibrary.DTO;
using MonShopLibrary.Repository;

namespace MonShopAPI.Controller
{
    [Route("Product")]
    [ApiController]
    public class ProductController : ControllerBase
    {
        private readonly IProductRepository _productRepository;

        public ProductController()
        {
            _productRepository = new ProductRepository();
        }

        [HttpGet]
        [Route("GetAllProduct")]
        public async Task<IActionResult> GetAllProduct()
        {
         var list =   await _productRepository.GetAllProduct();
            return Ok(list);
        }
        [HttpPost]
        [Route("AddProduct")]
        public async Task<IActionResult> AddProduct(ProductDTO dto)
        {
            await _productRepository.AddProduct(dto);
            return Ok();
        }
        [HttpPut]
        public async Task<IActionResult> UpdateProduct(ProductDTO dto)
        {
            await _productRepository.UpdateProduct(dto);
            return Ok();
        }
        [HttpDelete]
        public async Task<IActionResult> DeleteProduct(ProductDTO dto)
        {
            await _productRepository.DeleteProduct(dto);    
            return Ok();
        }
    }
}
