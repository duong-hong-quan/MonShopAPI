using MonShopLibrary.DTO;
using MonShop.Library.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MonShopLibrary.Repository
{
    public interface IProductRepository
    {
        public Task<List<Product>> GetAllProduct();
        public Task AddProduct(ProductDTO dto);
        public Task UpdateProduct(ProductDTO dto);
        public Task DeleteProduct(ProductDTO dto);
        public Task<List<Category>> GetAllCategory();
        public Task<Product> GetProductByID(int id);
        public  Task<List<ProductStatus>> GetAllProductStatus();
    }
}
