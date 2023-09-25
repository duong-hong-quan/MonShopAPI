using System;
using System.Collections.Generic;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.Extensions.Configuration;
using MonShop.Library.Models;

namespace MonShop.Library.Data
{
    public partial class MonShopContext : IdentityDbContext<ApplicationUser>
    {


        public MonShopContext(DbContextOptions<MonShopContext> options)
            : base(options)
        {
        }

        public DbSet<Category> Category { get; set; } = null!;
        public DbSet<Message> Message { get; set; } = null!;
        public DbSet<Order> Order { get; set; } = null!;
        public DbSet<OrderItem> OrderItem { get; set; } = null!;
        public DbSet<OrderStatus> OrderStatus { get; set; } = null!;
        public DbSet<PaymentResponse> PaymentResponse { get; set; } = null!;
        public DbSet<Product> Product { get; set; } = null!;
        public DbSet<ProductStatus> ProductStatus { get; set; } = null!;
        public DbSet<Room> Room { get; set; } = null!;
        public DbSet<PaymentType> PaymentType { get; set; } = null!;
        public DbSet<Cart> Cart { get; set; } = null!;
        public DbSet<CartItem> CartItem { get; set; } = null!;
        public DbSet<Size> Size { get; set; } = null!;
        public DbSet<ProductInventory> ProductInventory { get; set; } = null!;





        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.Entity<ProductInventory>()
               .HasKey(pi => new { pi.ProductId, pi.SizeId });

            modelBuilder.Entity<Size>().HasData(new Size
            {
                SizeId = 1,
                SizeName = "S"
            });
            modelBuilder.Entity<Size>().HasData(new Size
            {
                SizeId = 2,
                SizeName = "M"
            });
            modelBuilder.Entity<Size>().HasData(new Size
            {
                SizeId = 3,
                SizeName = "L"
            });
            modelBuilder.Entity<Size>().HasData(new Size
            {
                SizeId = 4,
                SizeName = "XL"
            });
            modelBuilder.Entity<PaymentType>().HasData(new PaymentType
            {
                PaymentTypeId = 1,
                Type = "Momo"

            });

            modelBuilder.Entity<PaymentType>().HasData(new PaymentType
            {
                PaymentTypeId = 2,
                Type = "VNPay"

            });
            modelBuilder.Entity<PaymentType>().HasData(new PaymentType
            {
                PaymentTypeId = 3,
                Type = "PayPal"

            });

            modelBuilder.Entity<ProductStatus>().HasData(new ProductStatus
            {
                ProductStatusId = 1,
                Status = "Active"

            });
            modelBuilder.Entity<ProductStatus>().HasData(new ProductStatus
            {
                ProductStatusId = 2,
                Status = "In Active"

            });

            modelBuilder.Entity<OrderStatus>().HasData(new OrderStatus
            {
                OrderStatusId = 1,
                Status = "Pending Pay"
            });

            modelBuilder.Entity<OrderStatus>().HasData(new OrderStatus
            {
                OrderStatusId = 2,
                Status = "Success Pay"
            });
            modelBuilder.Entity<OrderStatus>().HasData(new OrderStatus
            {
                OrderStatusId = 3,
                Status = "Failure Pay"
            });
            modelBuilder.Entity<OrderStatus>().HasData(new OrderStatus
            {
                OrderStatusId = 4,
                Status = "Shipped"
            });
            modelBuilder.Entity<OrderStatus>().HasData(new OrderStatus
            {
                OrderStatusId = 5,
                Status = "Delivered"
            });
            modelBuilder.Entity<OrderStatus>().HasData(new OrderStatus
            {
                OrderStatusId = 6,
                Status = "Cancelled"
            });


            modelBuilder.Entity<Category>().HasData(new Category
            {
                CategoryId = 1,
                CategoryName = "Pants"
            });

            modelBuilder.Entity<Category>().HasData(new Category
            {
                CategoryId = 2,
                CategoryName = "Shirt"
            });
            modelBuilder.Entity<Category>().HasData(new Category
            {
                CategoryId = 3,
                CategoryName = "Shoes"
            });

            modelBuilder.Entity<Category>().HasData(new Category
            {
                CategoryId = 4,
                CategoryName = "Accessories"
            });
            modelBuilder.Entity<Product>().HasData(
    new Product
    {
        ProductId = 1,
        ProductName = "Product 1",
        ImageUrl = "image1.jpg",
        Price = 19.99,
        Discount = 5.00,
        Description = "This is the description for Product 1.",
        CategoryId = 1,
        ProductStatusId = 1,
        IsDeleted = false
    },
    new Product
    {
        ProductId = 2,
        ProductName = "Product 2",
        ImageUrl = "image2.jpg",
        Price = 29.99,
        Discount = null,
        Description = "This is the description for Product 2.",
        CategoryId = 2,
        ProductStatusId = 1,
        IsDeleted = false
    },

    new Product
    {
        ProductId = 3,
        ProductName = "Product 3",
        ImageUrl = null,
        Price = 9.99,
        Discount = null,
        Description = null,
        CategoryId = 1,
        ProductStatusId = 2,
        IsDeleted = true
    }

);


        }
    }
}
