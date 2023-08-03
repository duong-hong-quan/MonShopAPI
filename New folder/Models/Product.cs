using System;
using System.Collections.Generic;

namespace MonShopLibrary.Models;

public partial class Product
{
    public string Sku { get; set; } = null!;

    public string ProductName { get; set; } = null!;

    public string Image { get; set; } = null!;

    public double Price { get; set; }

    public int Quantity { get; set; }

    public string? Description { get; set; }

    public int? CategoryId { get; set; }

    public int? ProductStatusId { get; set; }

    public bool? IsDeleted { get; set; }

    public virtual Category? Category { get; set; }

    public virtual ICollection<OrderItem> OrderItems { get; set; } = new List<OrderItem>();

    public virtual ProductStatus? ProductStatus { get; set; }
}
