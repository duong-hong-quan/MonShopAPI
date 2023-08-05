using MonShopLibrary.DAO;
using MonShopLibrary.DTO;
using MonShopLibrary.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MonShopLibrary.Repository
{
    public class ProductRepository: IProductRepository
    {
        ProductDBContext db = new ProductDBContext();
        public async Task<List<Product>> GetAllProduct() => await db.GetAllProduct();
        public async Task AddProduct(ProductDTO dto) => await db.AddProduct(dto);
        public async Task UpdateProduct(ProductDTO dto)=> await db.UpdateProduct(dto);
        public async Task DeleteProduct(ProductDTO dto)=> await db.DeleteProduct(dto);
        public async Task<List<Category>> GetAllCategory() => await db.GetAllCategory();

        public async Task<Product> GetProductByID(int id) => await db.GetProductByID(id);

    }
}
