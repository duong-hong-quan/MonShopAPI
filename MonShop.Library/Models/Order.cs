using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace MonShop.Library.Models
{
    public class Order
    {
        [Key]
        public string OrderId { get; set; } = null!;
        public DateTime? OrderDate { get; set; }
        public double? Total { get; set; }
        public int? OrderStatusId { get; set; }
        [ForeignKey("OrderStatusId"), Column(Order = 1)]

        public OrderStatus? OrderStatus { get; set; } = null!;

        public string? ApplicationUserId { get; set; }
        [ForeignKey("ApplicationUserId"), Column(Order = 2)]
        public ApplicationUser? ApplicationUser { get; set; }


        public string? DeliveryAddressId { get; set; }
        [ForeignKey("DeliveryAddressId"), Column(Order = 3)]
        public  DeliveryAddress? DeliveryAddress { get; set; }



    }
}
