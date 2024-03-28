using Microsoft.AspNetCore.Mvc;
using MonShop.BackEnd.Common.Dto.Request;
using Monshop.BackEnd.Service.Contracts;

namespace MonShop.BackEnd.API.Controller;

[Route("product")]
[ApiController]
public class ProductController : ControllerBase
{
    private readonly IProductService _productService;

    public ProductController(IProductService productService)
    {
        _productService = productService;
    }

    [HttpPost("add-product")]
    public async Task<AppActionResult> AddProduct([FromForm] ProductDto productDto)
    {
        return await _productService.AddProduct(productDto);
    }

    [HttpPut("update-product")]
    public async Task<AppActionResult> UpdateProduct([FromForm] ProductDto productDto)
    {
        return await _productService.UpdateProduct(productDto);
    }

    [HttpDelete("delete-product-by-id/{id:int}")]
    public async Task<AppActionResult> DeleteProduct(int id)
    {
        return await _productService.DeleteProduct(id);
    }

    [HttpGet("get-product-by-id/{id:int}")]
    public async Task<AppActionResult> GetProductById(int id)
    {
        return await _productService.GetProductById(id);
    }

    [HttpGet("get-product-status")]
    public async Task<AppActionResult> GetAllProductStatus()
    {
        return await _productService.GetAllProductStatus();
    }

    [HttpGet("get-product-inventory")]
    public async Task<AppActionResult> GetAllProductInventory()
    {
        return await _productService.GetAllProductInventory();
    }

    [HttpGet("get-product-by-manager")]
    public async Task<AppActionResult> GetProductByManager()
    {
        return await _productService.GetProductByManager();
    }
}