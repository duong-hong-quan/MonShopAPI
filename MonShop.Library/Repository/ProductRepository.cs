using MonShopLibrary.DTO;
using MonShop.Library.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using MonShopLibrary.Utils;
using MonShop.Library.Repository.IRepository;
using MonShop.Library.Data;

namespace MonShopLibrary.Repository
{
    public class ProductRepository: IProductRepository
    {

        private readonly MonShopContext _db;

        public ProductRepository(MonShopContext db)
        {
            _db = db;
        }

        public async Task<List<MonShop.Library.Models.Category>> GetAllCategory()
        {
            List<MonShop.Library.Models.Category> list = await _db.Category.ToListAsync();
            return list;
        }
        public async Task<List<ProductStatus>> GetAllProductStatus()
        {
            List<ProductStatus> list = await _db.ProductStatus.ToListAsync();
            return list;
        }
        public async Task<List<Product>> GetAllProduct()
        {
            List<Product> list = await _db.Product.Include(p => p.Category).Include(p => p.ProductStatus).Where(o => o.ProductStatusId != Constant.Product.IN_ACTIVE && o.IsDeleted == false).ToListAsync();

            return list;
        }
        public async Task<List<Product>> GetAllProductByManager()
        {
            List<Product> list = await _db.Product.Include(p => p.Category).Include(p => p.ProductStatus).ToListAsync();

            return list;
        }
        public async Task AddProduct(ProductDTO dto)
        {
            Product product = new Product
            {
                ProductName = dto.ProductName,
                ImageUrl = dto.ImageUrl,
                Price = dto.Price,
                Description = dto.Description,
                CategoryId = dto.CategoryId,
                ProductStatusId = dto.ProductStatusId,
                Discount = dto.Discount,
                IsDeleted = false

            };
            await _db.Product.AddAsync(product);
            await _db.SaveChangesAsync();
        }

        public async Task UpdateProduct(ProductDTO dto)
        {
            Product product = await GetProductByID(dto.ProductId);
            product.ProductId = dto.ProductId;
            product.ProductName = dto.ProductName;
            product.ImageUrl = dto.ImageUrl;
            product.Price = dto.Price;
            product.Description = dto.Description;
            product.CategoryId = dto.CategoryId;
            product.ProductStatusId = dto.ProductStatusId;
            product.Discount = dto.Discount;
            product.IsDeleted = dto.IsDeleted;
            await _db.SaveChangesAsync();

        }
        public async Task DeleteProduct(ProductDTO dto)
        {
            Product product = await _db.Product.FirstAsync(p => p.ProductId == dto.ProductId);
            product.IsDeleted = true;
            await _db.SaveChangesAsync();
        }

        public async Task<Product> GetProductByID(int id)
        {
            Product product = await _db.Product.Include(p => p.ProductStatus)
               .Include(p => p.Category).FirstAsync(p => p.ProductId == id);
            return product;
        }

        public async Task<List<Product>> GetTopXProduct(int x)
        {
            var newProduct = await _db.Product
               .Where(p => p.IsDeleted == false && p.ProductStatusId != Constant.Product.IN_ACTIVE)
               .OrderByDescending(p => p.ProductId)
               .Take(x)
               .Include(p => p.ProductStatus)
               .Include(p => p.Category)

               .ToListAsync();
            return newProduct;
        }

        public async Task<ProductInventory> GetProductInventory(int ProductId, int SizeId)
        {
           return await _db.ProductInventory.Include(p=> p.Size).Include(p=> p.Product).Include(p=> p.Product.ProductStatus).Include(p => p.Product.Category).FirstOrDefaultAsync(i=> i.ProductId == ProductId && i.SizeId == SizeId);
        }

        public async Task<List<Size>> GetAllSize()
        {
            return await _db.Size.ToListAsync();
        }
    }
}
