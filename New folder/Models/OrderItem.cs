using System;
using System.Collections.Generic;

namespace MonShopLibrary.Models;

public partial class OrderItem
{
    public int OrderItemId { get; set; }

    public int OrderId { get; set; }

    public string Sku { get; set; } = null!;

    public int Quantity { get; set; }

    public double? Price { get; set; }

    public double? Subtotal { get; set; }

    public virtual Order Order { get; set; } = null!;

    public virtual Product SkuNavigation { get; set; } = null!;
}
