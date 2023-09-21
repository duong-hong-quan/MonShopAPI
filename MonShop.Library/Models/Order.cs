using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace MonShop.Library.Models
{
    public partial class Order
    {
        [Key]
        public string OrderId { get; set; } = null!;
        public DateTime? OrderDate { get; set; }
        public double? Total { get; set; }
        public int OrderStatusId { get; set; }
        [ForeignKey("OrderStatusId")]

        public OrderStatus OrderStatus { get; set; } = null!;

        public string ApplicationUserId { get; set; }
        [ForeignKey("ApplicationUserId")]
        public ApplicationUser ApplicationUser { get; set; }


    }
}
