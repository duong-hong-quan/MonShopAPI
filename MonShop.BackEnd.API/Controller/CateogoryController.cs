using Microsoft.AspNetCore.Mvc;
using MonShop.BackEnd.Common.Dto.Request;
using Monshop.BackEnd.Service.Contracts;

namespace MonShop.BackEnd.API.Controller;

[Route("category")]
[ApiController]
public class CateogoryController : ControllerBase
{
    private readonly ICategoryService _categoryService;

    public CateogoryController(ICategoryService categoryService)
    {
        _categoryService = categoryService;
    }

    [HttpPost("add-category")]
    public async Task<AppActionResult> AddCategory(CategoryDto categoryDto)
    {
        return await _categoryService.AddCategory(categoryDto);
    }

    [HttpGet("get-all-category")]
    public async Task<AppActionResult> GetAllCategory()
    {
        return await _categoryService.GetAllCategory();
    }

    [HttpPut("update-category")]
    public async Task<AppActionResult> UpdateCategory(CategoryDto categoryDto)
    {
        return await _categoryService.UpdateCategory(categoryDto);
    }

    [HttpDelete("delete-category-by-id/{id:int}")]
    public async Task<AppActionResult> UpdateCategory(int id)
    {
        return await _categoryService.DeleteCategory(id);
    }
}