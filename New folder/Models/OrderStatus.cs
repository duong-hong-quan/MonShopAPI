using System;
using System.Collections.Generic;

namespace MonShopLibrary.Models;

public partial class OrderStatus
{
    public int OrderStatusId { get; set; }

    public string OrderStatus1 { get; set; } = null!;

    public virtual ICollection<Order> Orders { get; set; } = new List<Order>();
}
