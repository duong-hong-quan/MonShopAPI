using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace MonShop.Library.Models
{
    public class OrderStatus
    {
        [Key]
        public int OrderStatusId { get; set; }
        public string Status { get; set; } = null!;
        
    }
}
