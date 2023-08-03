using MonShopLibrary.DTO;
using MonShopLibrary.Models;
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
    }
}
