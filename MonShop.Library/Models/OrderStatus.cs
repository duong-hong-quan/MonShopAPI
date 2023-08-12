using System;
using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace MonShop.Library.Models
{
    public partial class OrderStatus
    {
        public OrderStatus()
        {
            Orders = new HashSet<Order>();
        }

        public int OrderStatusId { get; set; }
        public string Status { get; set; } = null!;
        [JsonIgnore]

        public virtual ICollection<Order> Orders { get; set; }
    }
}
