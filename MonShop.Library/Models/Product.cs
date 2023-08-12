using System;
using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace MonShop.Library.Models
{
    public partial class Product
    {
        public Product()
        {
            OrderItems = new HashSet<OrderItem>();
        }

        public int ProductId { get; set; }
        public string ProductName { get; set; } = null!;
        public string? ImageUrl { get; set; }
        public double Price { get; set; }
        public double? Discount { get; set; }
        public int Quantity { get; set; }
        public string? Description { get; set; }
        public int? CategoryId { get; set; }
        public int? ProductStatusId { get; set; }
        public bool? IsDeleted { get; set; }

        public virtual Category? Category { get; set; }
        public virtual ProductStatus? ProductStatus { get; set; }
        [JsonIgnore]

        public virtual ICollection<OrderItem> OrderItems { get; set; }
    }
}
