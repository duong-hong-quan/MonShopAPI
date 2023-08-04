using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.Extensions.Configuration;

namespace MonShopLibrary.Models
{
    public partial class MonShopContext : DbContext
    {
        public MonShopContext()
        {
        }

        public MonShopContext(DbContextOptions<MonShopContext> options)
            : base(options)
        {
        }

        public virtual DbSet<Account> Accounts { get; set; } = null!;
        public virtual DbSet<Category> Categories { get; set; } = null!;
        public virtual DbSet<MomoPaymentResponse> MomoPaymentResponses { get; set; } = null!;
        public virtual DbSet<Order> Orders { get; set; } = null!;
        public virtual DbSet<OrderItem> OrderItems { get; set; } = null!;
        public virtual DbSet<OrderStatus> OrderStatuses { get; set; } = null!;
        public virtual DbSet<PayPalPaymentResponse> PayPalPaymentResponses { get; set; } = null!;
        public virtual DbSet<Product> Products { get; set; } = null!;
        public virtual DbSet<Role> Roles { get; set; } = null!;
        public virtual DbSet<VnpayPaymentResponse> VnpayPaymentResponses { get; set; } = null!;

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            IConfiguration config = new ConfigurationBuilder()
                       .SetBasePath(Directory.GetCurrentDirectory())
                       .AddJsonFile("appsettings.json", true, true)
                       .Build();
            string cs = config["ConnectionStrings:Host"];
            if (!optionsBuilder.IsConfigured)
            {
                optionsBuilder.UseSqlServer(cs);
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Account>(entity =>
            {
                entity.ToTable("Account");

                entity.Property(e => e.Address).HasMaxLength(255);

                entity.Property(e => e.Email).HasMaxLength(255);

                entity.Property(e => e.FullName).HasMaxLength(255);

                entity.Property(e => e.ImageUrl).HasMaxLength(1000);

                entity.Property(e => e.Password).HasMaxLength(255);

                entity.Property(e => e.PhoneNumber).HasMaxLength(20);

                entity.HasOne(d => d.Role)
                    .WithMany(p => p.Accounts)
                    .HasForeignKey(d => d.RoleId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__Account__RoleId__47DBAE45");
            });

            modelBuilder.Entity<Category>(entity =>
            {
                entity.Property(e => e.CategoryName).HasMaxLength(255);
            });

            modelBuilder.Entity<MomoPaymentResponse>(entity =>
            {
                entity.HasKey(e => e.PaymentResponseId)
                    .HasName("PK__MomoPaym__766E687ACB54DB4C");

                entity.Property(e => e.PaymentResponseId).ValueGeneratedNever();

                entity.Property(e => e.Amount).HasMaxLength(50);

                entity.Property(e => e.OrderInfo).HasMaxLength(255);

                entity.HasOne(d => d.Order)
                    .WithMany(p => p.MomoPaymentResponses)
                    .HasForeignKey(d => d.OrderId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__MomoPayme__Order__48CFD27E");
            });

            modelBuilder.Entity<Order>(entity =>
            {
                entity.Property(e => e.OrderId).ValueGeneratedNever();

                entity.Property(e => e.Email).HasMaxLength(255);

                entity.Property(e => e.OrderDate).HasColumnType("date");

                entity.HasOne(d => d.BuyerAccount)
                    .WithMany(p => p.Orders)
                    .HasForeignKey(d => d.BuyerAccountId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__Orders__BuyerAcc__4BAC3F29");

                entity.HasOne(d => d.OrderStatus)
                    .WithMany(p => p.Orders)
                    .HasForeignKey(d => d.OrderStatusId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__Orders__OrderSta__4CA06362");
            });

            modelBuilder.Entity<OrderItem>(entity =>
            {
                entity.HasOne(d => d.Product)
                    .WithMany(p => p.OrderItems)
                    .HasForeignKey(d => d.ProductId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__OrderItem__Produ__4AB81AF0");
            });

            modelBuilder.Entity<OrderStatus>(entity =>
            {
                entity.Property(e => e.OrderStatusId).ValueGeneratedNever();

                entity.Property(e => e.OrderStatus1)
                    .HasMaxLength(255)
                    .HasColumnName("OrderStatus");
            });

            modelBuilder.Entity<PayPalPaymentResponse>(entity =>
            {
                entity.HasKey(e => e.PaymentResponseId)
                    .HasName("PK__PayPalPa__766E687AF3668493");

                entity.Property(e => e.PaymentResponseId).ValueGeneratedNever();

                entity.Property(e => e.OrderDescription).HasMaxLength(255);

                entity.Property(e => e.PaymentId).HasMaxLength(50);

                entity.Property(e => e.TransactionId).HasMaxLength(50);

                entity.HasOne(d => d.Order)
                    .WithMany(p => p.PayPalPaymentResponses)
                    .HasForeignKey(d => d.OrderId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__PayPalPay__Order__4D94879B");
            });

            modelBuilder.Entity<Product>(entity =>
            {
                entity.Property(e => e.Description).HasColumnType("text");

                entity.Property(e => e.ImageUrl).HasMaxLength(1000);

                entity.Property(e => e.ProductName).HasMaxLength(255);

                entity.HasOne(d => d.Category)
                    .WithMany(p => p.Products)
                    .HasForeignKey(d => d.CategoryId)
                    .HasConstraintName("FK__Products__Catego__4E88ABD4");
            });

            modelBuilder.Entity<Role>(entity =>
            {
                entity.Property(e => e.RoleId).ValueGeneratedNever();

                entity.Property(e => e.RoleName).HasMaxLength(255);
            });

            modelBuilder.Entity<VnpayPaymentResponse>(entity =>
            {
                entity.HasKey(e => e.PaymentResponseId)
                    .HasName("PK__VNPayPay__766E687A8951372B");

                entity.ToTable("VNPayPaymentResponses");

                entity.Property(e => e.PaymentResponseId).ValueGeneratedNever();

                entity.Property(e => e.OrderDescription).HasMaxLength(255);

                entity.Property(e => e.PaymentId).HasMaxLength(50);

                entity.Property(e => e.Token).HasMaxLength(255);

                entity.Property(e => e.TransactionId).HasMaxLength(50);

                entity.Property(e => e.VnPayResponseCode).HasMaxLength(50);

                entity.HasOne(d => d.Order)
                    .WithMany(p => p.VnpayPaymentResponses)
                    .HasForeignKey(d => d.OrderId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__VNPayPaym__Order__4F7CD00D");
            });

            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}
