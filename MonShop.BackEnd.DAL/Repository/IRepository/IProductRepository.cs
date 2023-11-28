using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MonShop.BackEnd.DAL.Models;
using MonShop.BackEnd.DAL.DTO;

namespace MonShop.BackEnd.DAL.Repository.IRepository
{
    public interface IProductRepository : IRepository <Product>
    {
        public Task<List<Product>> GetAllProduct();
        public Task AddProduct(ProductDTO dto);
        public Task UpdateProduct(ProductDTO dto);
        public Task DeleteProduct(ProductDTO dto);
        public Task<List<Category>> GetAllCategory();
        public Task<Product> GetProductByID(int id);
        public Task<List<ProductStatus>> GetAllProductStatus();
        public Task<List<Product>> GetAllProductByManager();
        public Task<List<Product>> GetTopXProduct(int x);

        public Task<ProductInventory> GetProductInventory(int ProductId, int SizeId);
        public Task<List<Size>> GetAllSize();
        public Task<List<Product>> GetAllProductByCategoryId(int CategoryId);


    }
}
