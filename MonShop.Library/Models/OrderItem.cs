using System;
using System.Collections.Generic;

namespace MonShopLibrary.Models
{
    public partial class OrderItem
    {
        public int OrderItemId { get; set; }
        public string OrderId { get; set; } = null!;
        public int ProductId { get; set; }
        public int Quantity { get; set; }
        public double PricePerUnit { get; set; }
        public double Subtotal { get; set; }

        public virtual Order Order { get; set; } = null!;
        public virtual Product Product { get; set; } = null!;
    }
}
