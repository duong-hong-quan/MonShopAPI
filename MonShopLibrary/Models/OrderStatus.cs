using System;
using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace MonShopLibrary.Models
{
    public partial class OrderStatus
    {
        public OrderStatus()
        {
            Orders = new HashSet<Order>();
        }

        public int OrderStatusId { get; set; }
        public string OrderStatus1 { get; set; } = null!;
        [JsonIgnore]

        public virtual ICollection<Order> Orders { get; set; }
    }
}
