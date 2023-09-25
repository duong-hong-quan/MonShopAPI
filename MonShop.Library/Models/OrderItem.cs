using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MonShop.Library.Models
{
    public  class OrderItem
    {
        [Key]
        public int OrderItemId { get; set; }
        public string OrderId { get; set; } = null!;
        [ForeignKey("OrderId")]
        public Order Order { get; set; } = null!;

        public int ProductId { get; set; }
        [ForeignKey("ProductId")]

        public Product Product { get; set; } = null!;

        public int SizeId { get; set; }
        [ForeignKey("SizeId")]
        public Size Size { get; set; }  

        public int Quantity { get; set; }
        public double PricePerUnit { get; set; }
        public double Subtotal { get; set; }

    }
}
