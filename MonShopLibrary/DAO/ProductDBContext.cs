using Microsoft.EntityFrameworkCore;
using MonShopLibrary.DTO;
using MonShopLibrary.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MonShopLibrary.DAO
{
    public class ProductDBContext : MonShopContext
    {
        public ProductDBContext() { }


        public async Task<List<Category>> GetAllCategory()
        {
            List<Category> list = await this.Categories.ToListAsync();
            return list;
        }
        public async Task<List<Product>> GetAllProduct()
        {
            List<Product> list = await this.Products.ToListAsync();
            return list;
        }
        public async Task AddProduct(ProductDTO dto)
        {
            Product product = new Product
            {
                ProductName = dto.ProductName,
                ImageUrl = dto.ImageUrl,
                Price = dto.Price,
                Quantity = dto.Quantity,
                Description = dto.Description,
                CategoryId = dto.CategoryId,
                ProductStatusId = dto.ProductStatusId,
                IsDeleted = false

            };
            await this.Products.AddAsync(product);
            await this.SaveChangesAsync();
        }

        public async Task UpdateProduct(ProductDTO dto)
        {
            Product product = new Product
            {
                ProductId = dto.ProductId,
                ProductName = dto.ProductName,
                ImageUrl = dto.ImageUrl,
                Price = dto.Price,
                Quantity = dto.Quantity,
                Description = dto.Description,
                CategoryId = dto.CategoryId,
                ProductStatusId = dto.ProductStatusId,
                IsDeleted = false

            };
            this.Products.Update(product);
            await this.SaveChangesAsync();

        }
        public async Task DeleteProduct(ProductDTO dto)
        {
            Product product = await this.Products.FindAsync(dto.ProductId);
            product.IsDeleted = true;
            await this.SaveChangesAsync();
        }

    }
}
