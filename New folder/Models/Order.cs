using System;
using System.Collections.Generic;

namespace MonShopLibrary.Models;

public partial class Order
{
    public int OrderId { get; set; }

    public string? Email { get; set; }

    public string? OrderDate { get; set; }

    public double? Total { get; set; }

    public int OrderStatusId { get; set; }

    public virtual Account? EmailNavigation { get; set; }

    public virtual ICollection<OrderItem> OrderItems { get; set; } = new List<OrderItem>();

    public virtual OrderStatus OrderStatus { get; set; } = null!;
}
