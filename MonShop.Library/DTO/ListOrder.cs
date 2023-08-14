using MonShop.Library.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MonShop.Library.DTO
{
    public class ListOrder
    {
        public Order order { get; set; }
        public List<OrderItem> orderItem { get; set; }
        public string paymentMethod { get; set; }

    }

  
}
