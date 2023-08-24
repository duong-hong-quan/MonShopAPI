using Microsoft.EntityFrameworkCore;
using MonShopLibrary.DTO;
using MonShop.Library.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Diagnostics;
using MonShopLibrary.Utils;

namespace MonShopLibrary.DAO
{
    public class ProductDBContext : MonShopContext
    {
        public ProductDBContext() { }


        public async Task<List<MonShop.Library.Models.Category>> GetAllCategory()
        {
            List<MonShop.Library.Models.Category> list = await this.Categories.ToListAsync();
            return list;
        }
        public async Task<List<ProductStatus>> GetAllProductStatus()
        {
            List<ProductStatus> list = await this.ProductStatuses.ToListAsync();
            return list;
        }
        public async Task<List<Product>> GetAllProduct()
        {
            List<Product> list = await this.Products.Include(p=> p.Category).Include(p => p.ProductStatus).Where(o=> o.ProductStatusId != Constant.Product.IN_ACTIVE && o.IsDeleted == false).ToListAsync();

            return list;
        }
        public async Task<List<Product>> GetAllProductByManager()
        {
            List<Product> list = await this.Products.Include(p => p.Category).Include(p => p.ProductStatus).ToListAsync();

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
                Discount = dto.Discount,
                IsDeleted = false

            };
            await this.Products.AddAsync(product);
            await this.SaveChangesAsync();
        }

        public async Task UpdateProduct(ProductDTO dto)
        {
            Product product = await GetProductByID(dto.ProductId);
            product.ProductId = dto.ProductId;
            product.ProductName = dto.ProductName;
            product.ImageUrl = dto.ImageUrl;
            product.Price = dto.Price;
            product.Quantity = dto.Quantity;
            product.Description = dto.Description;
            product.CategoryId = dto.CategoryId;
            product.ProductStatusId = dto.ProductStatusId;
            product.Discount = dto.Discount;
            product.IsDeleted = dto.IsDeleted;
            await this.SaveChangesAsync();

        }
        public async Task DeleteProduct(ProductDTO dto)
        {
            Product product = await this.Products.FindAsync(dto.ProductId);
            product.IsDeleted = true;
            await this.SaveChangesAsync();
        }

        public async Task<Product> GetProductByID(int id)
        {
            Product product = await this.Products.FindAsync(id);
            return product;
        }

        public async Task<List<Product>> GetTop4()
        {
            var newProducts = await this.Products
               .Where(p => p.IsDeleted == false && p.ProductStatusId != Constant.Product.IN_ACTIVE) 
               .OrderByDescending(p => p.ProductId)
               .Take(4) 
               .ToListAsync();
            return newProducts;
        }

    }
}
