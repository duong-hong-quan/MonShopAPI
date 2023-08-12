using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.Extensions.Configuration;

namespace MonShop.Library.Models
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
        public virtual DbSet<Message> Messages { get; set; } = null!;
        public virtual DbSet<MomoPaymentResponse> MomoPaymentResponses { get; set; } = null!;
        public virtual DbSet<Order> Orders { get; set; } = null!;
        public virtual DbSet<OrderItem> OrderItems { get; set; } = null!;
        public virtual DbSet<OrderStatus> OrderStatuses { get; set; } = null!;
        public virtual DbSet<PayPalPaymentResponse> PayPalPaymentResponses { get; set; } = null!;
        public virtual DbSet<Product> Products { get; set; } = null!;
        public virtual DbSet<ProductStatus> ProductStatuses { get; set; } = null!;
        public virtual DbSet<Role> Roles { get; set; } = null!;
        public virtual DbSet<Token> Tokens { get; set; } = null!;
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

                entity.Property(e => e.FirstName).HasMaxLength(255);

                entity.Property(e => e.ImageUrl).HasMaxLength(1000);

                entity.Property(e => e.LastName).HasMaxLength(255);

                entity.Property(e => e.Password).HasMaxLength(255);

                entity.Property(e => e.PhoneNumber).HasMaxLength(20);

                entity.HasOne(d => d.Role)
                    .WithMany(p => p.Accounts)
                    .HasForeignKey(d => d.RoleId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__Account__RoleId__4CA06362");
            });

            modelBuilder.Entity<Category>(entity =>
            {
                entity.Property(e => e.CategoryName).HasMaxLength(255);
            });

            modelBuilder.Entity<Message>(entity =>
            {
                entity.HasNoKey();

                entity.Property(e => e.Content).HasMaxLength(255);

                entity.Property(e => e.MessageId)
                    .ValueGeneratedOnAdd()
                    .HasColumnName("MessageID");

                entity.Property(e => e.SendTime).HasColumnType("datetime");

                entity.HasOne(d => d.ReceiverNavigation)
                    .WithMany()
                    .HasForeignKey(d => d.Receiver)
                    .HasConstraintName("FK__Messages__Receiv__5812160E");

                entity.HasOne(d => d.SenderNavigation)
                    .WithMany()
                    .HasForeignKey(d => d.Sender)
                    .HasConstraintName("FK__Messages__Sender__571DF1D5");
            });

            modelBuilder.Entity<MomoPaymentResponse>(entity =>
            {
                entity.HasKey(e => e.PaymentResponseId)
                    .HasName("PK__MomoPaym__766E687A18396922");

                entity.Property(e => e.PaymentResponseId).ValueGeneratedNever();

                entity.Property(e => e.Amount).HasMaxLength(50);

                entity.Property(e => e.OrderId)
                    .HasMaxLength(255)
                    .IsUnicode(false);

                entity.Property(e => e.OrderInfo).HasMaxLength(255);

                entity.HasOne(d => d.Order)
                    .WithMany(p => p.MomoPaymentResponses)
                    .HasForeignKey(d => d.OrderId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__MomoPayme__Order__4D94879B");
            });

            modelBuilder.Entity<Order>(entity =>
            {
                entity.Property(e => e.OrderId)
                    .HasMaxLength(255)
                    .IsUnicode(false);

                entity.Property(e => e.OrderDate).HasColumnType("datetime");

                entity.HasOne(d => d.BuyerAccount)
                    .WithMany(p => p.Orders)
                    .HasForeignKey(d => d.BuyerAccountId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__Orders__BuyerAcc__5070F446");

                entity.HasOne(d => d.OrderStatus)
                    .WithMany(p => p.Orders)
                    .HasForeignKey(d => d.OrderStatusId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__Orders__OrderSta__5165187F");
            });

            modelBuilder.Entity<OrderItem>(entity =>
            {
                entity.Property(e => e.OrderId)
                    .HasMaxLength(255)
                    .IsUnicode(false);

                entity.HasOne(d => d.Order)
                    .WithMany(p => p.OrderItems)
                    .HasForeignKey(d => d.OrderId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__OrderItem__Order__4E88ABD4");

                entity.HasOne(d => d.Product)
                    .WithMany(p => p.OrderItems)
                    .HasForeignKey(d => d.ProductId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__OrderItem__Produ__4F7CD00D");
            });

            modelBuilder.Entity<OrderStatus>(entity =>
            {
                entity.Property(e => e.OrderStatusId).ValueGeneratedNever();

                entity.Property(e => e.Status).HasMaxLength(255);
            });

            modelBuilder.Entity<PayPalPaymentResponse>(entity =>
            {
                entity.HasKey(e => e.PaymentResponseId)
                    .HasName("PK__PayPalPa__766E687A9FF01B47");

                entity.Property(e => e.PaymentResponseId).HasMaxLength(255);

                entity.Property(e => e.Amount).HasMaxLength(50);

                entity.Property(e => e.OrderId)
                    .HasMaxLength(255)
                    .IsUnicode(false);

                entity.Property(e => e.OrderInfo).HasMaxLength(255);

                entity.HasOne(d => d.Order)
                    .WithMany(p => p.PayPalPaymentResponses)
                    .HasForeignKey(d => d.OrderId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__PayPalPay__Order__52593CB8");
            });

            modelBuilder.Entity<Product>(entity =>
            {
                entity.Property(e => e.Description).HasColumnType("text");

                entity.Property(e => e.ImageUrl).HasMaxLength(1000);

                entity.Property(e => e.ProductName).HasMaxLength(255);

                entity.HasOne(d => d.Category)
                    .WithMany(p => p.Products)
                    .HasForeignKey(d => d.CategoryId)
                    .HasConstraintName("FK__Products__Catego__5441852A");

                entity.HasOne(d => d.ProductStatus)
                    .WithMany(p => p.Products)
                    .HasForeignKey(d => d.ProductStatusId)
                    .HasConstraintName("FK__Products__Produc__534D60F1");
            });

            modelBuilder.Entity<ProductStatus>(entity =>
            {
                entity.ToTable("ProductStatus");

                entity.Property(e => e.ProductStatusId).ValueGeneratedNever();

                entity.Property(e => e.Status)
                    .HasMaxLength(255)
                    .IsUnicode(false);
            });

            modelBuilder.Entity<Role>(entity =>
            {
                entity.Property(e => e.RoleId).ValueGeneratedNever();

                entity.Property(e => e.RoleName).HasMaxLength(255);
            });

            modelBuilder.Entity<Token>(entity =>
            {
                entity.HasKey(e => e.RefreshToken)
                    .HasName("PK__Tokens__DEA298DB0C03CCBE");

                entity.Property(e => e.RefreshToken)
                    .HasMaxLength(255)
                    .IsUnicode(false);

                entity.Property(e => e.AccountId).HasColumnName("AccountID");

                entity.Property(e => e.ExpiresAt).HasColumnType("datetime");

                entity.HasOne(d => d.Account)
                    .WithMany(p => p.Tokens)
                    .HasForeignKey(d => d.AccountId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__Tokens__AccountI__5629CD9C");
            });

            modelBuilder.Entity<VnpayPaymentResponse>(entity =>
            {
                entity.HasKey(e => e.PaymentResponseId)
                    .HasName("PK__VNPayPay__766E687A85AA1B91");

                entity.ToTable("VNPayPaymentResponses");

                entity.Property(e => e.PaymentResponseId).ValueGeneratedNever();

                entity.Property(e => e.Amount).HasMaxLength(50);

                entity.Property(e => e.OrderId)
                    .HasMaxLength(255)
                    .IsUnicode(false);

                entity.Property(e => e.OrderInfo).HasMaxLength(255);

                entity.HasOne(d => d.Order)
                    .WithMany(p => p.VnpayPaymentResponses)
                    .HasForeignKey(d => d.OrderId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__VNPayPaym__Order__5535A963");
            });

            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}
