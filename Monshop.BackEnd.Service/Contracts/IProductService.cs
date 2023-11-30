using MonShop.BackEnd.DAL.DTO;
using MonShop.BackEnd.DAL.DTO.Response;
using MonShop.BackEnd.DAL.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Monshop.BackEnd.Service.Contracts
{
    public interface IProductService
    {
        public Task<AppActionResult> GetAllProduct();
        public Task<AppActionResult> AddProduct(ProductDTO dto);
        public Task<AppActionResult> UpdateProduct(ProductDTO dto);
        public Task<AppActionResult> DeleteProduct(ProductDTO dto);
        public Task<AppActionResult> GetAllCategory();
        public Task<AppActionResult> GetProductByID(int id);
        public Task<AppActionResult> GetAllProductStatus();
        public Task<AppActionResult> GetAllProductByManager();
        public Task<AppActionResult> GetTopXProduct(int x);

        public Task<AppActionResult> GetProductInventory(int ProductId, int SizeId);
        public Task<AppActionResult> GetAllSize();
        public Task<AppActionResult> GetAllProductByCategoryId(int CategoryId);
    }
}
