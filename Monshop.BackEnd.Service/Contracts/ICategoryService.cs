using MonShop.BackEnd.Common.Dto.Request;

namespace Monshop.BackEnd.Service.Contracts;

public interface ICategoryService
{
    public Task<AppActionResult> AddCategory(CategoryDto categoryDto);
    public Task<AppActionResult> UpdateCategory(CategoryDto categoryDto);
    public Task<AppActionResult> DeleteCategory(int id);
    public Task<AppActionResult> GetAllCategory();
}