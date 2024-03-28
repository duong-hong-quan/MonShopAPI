using MonShop.BackEnd.Common.Dto.Request;

namespace Monshop.BackEnd.Service.Contracts;

public interface IProductService
{
    public Task<AppActionResult> GetProductById(int id);
    public Task<AppActionResult> GetProductByManager();
    public Task<AppActionResult> GetTopXProduct(int x);
    public Task<AppActionResult> AddProduct(ProductDto product);
    public Task<AppActionResult> UpdateProduct(ProductDto product);
    public Task<AppActionResult> DeleteProduct(int productId);
    public Task<AppActionResult> GetAllProductStatus();
    public Task<AppActionResult> GetAllProductInventory();
    public Task<AppActionResult> UpdateProductInventory(ProductInventoryDto inventoryDto);
}