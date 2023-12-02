using System;
using System.Collections.Generic;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.Extensions.Configuration;
using MonShop.BackEnd.DAL.Models;
using MonShop.BackEnd.Utility.Utils;

namespace MonShop.BackEnd.DAL.Data
{
    public partial class MonShopContext : IdentityDbContext<ApplicationUser>
    {


        public MonShopContext(DbContextOptions<MonShopContext> options)
            : base(options)
        {
        }

        public DbSet<ApplicationUser> ApplicationUsers { get; set; } = null!;
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

        public DbSet<DeliveryAddress> DeliveryAddresses { get; set; } = null!;

        public DbSet<RoomMemberChat> RoomMemberChats { get; set; } = null!; 

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.Entity<ProductInventory>()
               .HasKey(pi => new { pi.ProductId, pi.SizeId });

            modelBuilder.Entity<IdentityRole>().HasData(
            new IdentityRole { Id = Guid.NewGuid().ToString(), Name = Permission.ADMIN, NormalizedName = Permission.ADMIN.ToLower() });

            modelBuilder.Entity<IdentityRole>().HasData(
               new IdentityRole { Id = Guid.NewGuid().ToString(), Name = Permission.STAFF, NormalizedName = Permission.STAFF.ToLower() });

            modelBuilder.Entity<IdentityRole>().HasData(
                         new IdentityRole { Id = Guid.NewGuid().ToString(), Name = Permission.USER, NormalizedName = Permission.USER.ToLower() });

           


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
                ImageUrl = "https://media.coolmate.me/cdn-cgi/image/width=672,height=990,quality=85,format=auto/uploads/December2021/1-copy-2_99.jpg",
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
                ImageUrl = "https://media.coolmate.me/cdn-cgi/image/quality=80,format=auto/uploads/April2023/somicfxam4.jpg",
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
                ImageUrl = "https://media.coolmate.me/cdn-cgi/image/width=672,height=990,quality=85,format=auto/uploads/December2021/1-copy-2_99.jpg",
                Price = 9.99,
                Discount = null,
                Description = null,
                CategoryId = 1,
                ProductStatusId = 2,
                IsDeleted = true
            },
             new Product
             {
                 ProductId = 4,
                 ProductName = "Product 4",
                 ImageUrl = "https://media.coolmate.me/cdn-cgi/image/quality=80,format=auto/uploads/April2023/somicfxam4.jpg",
                 Price = 24.99,
                 Discount = 2.50,
                 Description = "This is the description for Product 4.",
                 CategoryId = 2,
                 ProductStatusId = 1,
                 IsDeleted = false
             },
            new Product
            {
                ProductId = 5,
                ProductName = "Product 5",
                ImageUrl = "https://images.lululemon.com/is/image/lululemon/LW9EPYS_061712_1",
                Price = 14.99,
                Discount = null,
                Description = "This is the description for Product 5.",
                CategoryId = 3,
                ProductStatusId = 1,
                IsDeleted = false
            },
            new Product
            {
                ProductId = 6,
                ProductName = "Product 6",
                ImageUrl = "https://images.lululemon.com/is/image/lululemon/LW9EPYS_061712_1",
                Price = 17.99,
                Discount = null,
                Description = "This is the description for Product 6.",
                CategoryId = 3,
                ProductStatusId = 1,
                IsDeleted = false
            },
            new Product
            {
                ProductId = 7,
                ProductName = "Product 7",
                ImageUrl = "https://media.coolmate.me/cdn-cgi/image/width=672,height=990,quality=85,format=auto/uploads/December2021/1-copy-2_99.jpg",
                Price = 22.99,
                Discount = 3.00,
                Description = "This is the description for Product 7.",
                CategoryId = 1,
                ProductStatusId = 1,
                IsDeleted = false
            },
            new Product
            {
                ProductId = 8,
                ProductName = "Product 8",
                ImageUrl = "https://media.coolmate.me/cdn-cgi/image/quality=80,format=auto/uploads/April2023/somicfxam4.jpg",
                Price = 10.99,
                Discount = null,
                Description = "This is the description for Product 8.",
                CategoryId = 2,
                ProductStatusId = 2,
                IsDeleted = false
            },
            new Product
            {
                ProductId = 9,
                ProductName = "Product 9",
                ImageUrl = "https://aldo.com.sa/cdn/shop/collections/s22_q1_w_vday_accessories_cluster_4467-547936.jpg?v=1644355846",
                Price = 27.99,
                Discount = 4.50,
                Description = "This is the description for Product 9.",
                CategoryId = 4,
                ProductStatusId = 1,
                IsDeleted = false
            },
            new Product
            {
                ProductId = 10,
                ProductName = "Product 10",
                ImageUrl = "https://images.lululemon.com/is/image/lululemon/LW9EPYS_061712_1",
                Price = 14.99,
                Discount = null,
                Description = "This is the description for Product 10.",
                CategoryId = 3,
                ProductStatusId = 1,
                IsDeleted = false
            }, new Product
            {
                ProductId = 11,
                ProductName = "Product 11",
                ImageUrl = "https://media.coolmate.me/cdn-cgi/image/quality=80,format=auto/uploads/April2023/somicfxam4.jpg",
                Price = 32.99,
                Discount = 6.00,
                Description = "This is the description for Product 11.",
                CategoryId = 2,
                ProductStatusId = 1,
                IsDeleted = false
            },
            new Product
            {
                ProductId = 12,
                ProductName = "Product 12",
                ImageUrl = "https://media.coolmate.me/cdn-cgi/image/width=672,height=990,quality=85,format=auto/uploads/December2021/1-copy-2_99.jpg",
                Price = 19.99,
                Discount = null,
                Description = "This is the description for Product 12.",
                CategoryId = 1,
                ProductStatusId = 1,
                IsDeleted = false
            },
            new Product
            {
                ProductId = 13,
                ProductName = "Product 13",
                ImageUrl = "https://images.lululemon.com/is/image/lululemon/LW9EPYS_061712_1",
                Price = 14.99,
                Discount = 2.00,
                Description = "This is the description for Product 13.",
                CategoryId = 3,
                ProductStatusId = 1,
                IsDeleted = false
            },
            new Product
            {
                ProductId = 14,
                ProductName = "Product 14",
                ImageUrl = "https://aldo.com.sa/cdn/shop/collections/s22_q1_w_vday_accessories_cluster_4467-547936.jpg?v=1644355846",
                Price = 23.99,
                Discount = null,
                Description = "This is the description for Product 14.",
                CategoryId = 4,
                ProductStatusId = 2,
                IsDeleted = false
            },
            new Product
            {
                ProductId = 15,
                ProductName = "Product 15",
                ImageUrl = "https://media.coolmate.me/cdn-cgi/image/quality=80,format=auto/uploads/April2023/somicfxam4.jpg",
                Price = 11.99,
                Discount = 1.50,
                Description = "This is the description for Product 15.",
                CategoryId = 2,
                ProductStatusId = 1,
                IsDeleted = false
            }, new Product
            {
                ProductId = 16,
                ProductName = "Product 16",
                ImageUrl = "https://images.lululemon.com/is/image/lululemon/LW9EPYS_061712_1",
                Price = 29.99,
                Discount = null,
                Description = "This is the description for Product 16.",
                CategoryId = 3,
                ProductStatusId = 1,
                IsDeleted = false
            },
            new Product
            {
                ProductId = 17,
                ProductName = "Product 17",
                ImageUrl = "https://media.coolmate.me/cdn-cgi/image/width=672,height=990,quality=85,format=auto/uploads/December2021/1-copy-2_99.jpg",
                Price = 18.99,
                Discount = 3.00,
                Description = "This is the description for Product 17.",
                CategoryId = 1,
                ProductStatusId = 1,
                IsDeleted = false
            },
            new Product
            {
                ProductId = 18,
                ProductName = "Product 18",
                ImageUrl = "https://media.coolmate.me/cdn-cgi/image/quality=80,format=auto/uploads/April2023/somicfxam4.jpg",
                Price = 13.99,
                Discount = null,
                Description = "This is the description for Product 18.",
                CategoryId = 2,
                ProductStatusId = 2,
                IsDeleted = false
            },
            new Product
            {
                ProductId = 19,
                ProductName = "Product 19",
                ImageUrl = "https://aldo.com.sa/cdn/shop/collections/s22_q1_w_vday_accessories_cluster_4467-547936.jpg?v=1644355846",
                Price = 26.99,
                Discount = 4.50,
                Description = "This is the description for Product 19.",
                CategoryId = 4,
                ProductStatusId = 1,
                IsDeleted = false
            },
            new Product
            {
                ProductId = 20,
                ProductName = "Product 20",
                ImageUrl = "https://images.lululemon.com/is/image/lululemon/LW9EPYS_061712_1",
                Price = 12.99,
                Discount = null,
                Description = "This is the description for Product 20.",
                CategoryId = 3,
                ProductStatusId = 1,
                IsDeleted = false
            },
            new Product
            {
                ProductId = 21,
                ProductName = "Product 21",
                ImageUrl = "https://media.coolmate.me/cdn-cgi/image/quality=80,format=auto/uploads/April2023/somicfxam4.jpg",
                Price = 31.99,
                Discount = 5.00,
                Description = "This is the description for Product 21.",
                CategoryId = 2,
                ProductStatusId = 1,
                IsDeleted = false
            },
            new Product
            {
                ProductId = 22,
                ProductName = "Product 22",
                ImageUrl = "https://media.coolmate.me/cdn-cgi/image/width=672,height=990,quality=85,format=auto/uploads/December2021/1-copy-2_99.jpg",
                Price = 16.99,
                Discount = null,
                Description = "This is the description for Product 22.",
                CategoryId = 1,
                ProductStatusId = 1,
                IsDeleted = false
            },
            new Product
            {
                ProductId = 23,
                ProductName = "Product 23",
                ImageUrl = "https://images.lululemon.com/is/image/lululemon/LW9EPYS_061712_1",
                Price = 15.99,
                Discount = 2.00,
                Description = "This is the description for Product 23.",
                CategoryId = 3,
                ProductStatusId = 1,
                IsDeleted = false
            },
            new Product
            {
                ProductId = 24,
                ProductName = "Product 24",
                ImageUrl = "https://aldo.com.sa/cdn/shop/collections/s22_q1_w_vday_accessories_cluster_4467-547936.jpg?v=1644355846",
                Price = 28.99,
                Discount = null,
                Description = "This is the description for Product 24.",
                CategoryId = 4,
                ProductStatusId = 2,
                IsDeleted = false
            },
            new Product
            {
                ProductId = 25,
                ProductName = "Product 25",
                ImageUrl = "https://media.coolmate.me/cdn-cgi/image/quality=80,format=auto/uploads/April2023/somicfxam4.jpg",
                Price = 10.99,
                Discount = 1.50,
                Description = "This is the description for Product 25.",
                CategoryId = 2,
                ProductStatusId = 1,
                IsDeleted = false
            },
            new Product
            {
                ProductId = 26,
                ProductName = "Product 26",
                ImageUrl = "https://media.coolmate.me/cdn-cgi/image/width=672,height=990,quality=85,format=auto/uploads/December2021/1-copy-2_99.jpg",
                Price = 19.99,
                Discount = null,
                Description = "This is the description for Product 26.",
                CategoryId = 1,
                ProductStatusId = 1,
                IsDeleted = false
            },
            new Product
            {
                ProductId = 27,
                ProductName = "Product 27",
                ImageUrl = "https://images.lululemon.com/is/image/lululemon/LW9EPYS_061712_1",
                Price = 14.99,
                Discount = 2.00,
                Description = "This is the description for Product 27.",
                CategoryId = 3,
                ProductStatusId = 1,
                IsDeleted = false
            },
            new Product
            {
                ProductId = 28,
                ProductName = "Product 28",
                ImageUrl = "https://aldo.com.sa/cdn/shop/collections/s22_q1_w_vday_accessories_cluster_4467-547936.jpg?v=1644355846",
                Price = 23.99,
                Discount = null,
                Description = "This is the description for Product 28.",
                CategoryId = 4,
                ProductStatusId = 2,
                IsDeleted = false
            },
            new Product
            {
                ProductId = 29,
                ProductName = "Product 29",
                ImageUrl = "https://media.coolmate.me/cdn-cgi/image/quality=80,format=auto/uploads/April2023/somicfxam4.jpg",
                Price = 11.99,
                Discount = 1.50,
                Description = "This is the description for Product 29.",
                CategoryId = 2,
                ProductStatusId = 1,
                IsDeleted = false
            },
            new Product
            {
                ProductId = 30,
                ProductName = "Product 30",
                ImageUrl = "https://media.coolmate.me/cdn-cgi/image/width=672,height=990,quality=85,format=auto/uploads/December2021/1-copy-2_99.jpg",
                Price = 24.99,
                Discount = null,
                Description = "This is the description for Product 30.",
                CategoryId = 1,
                ProductStatusId = 1,
                IsDeleted = false
            }

);


        }
    }
}
