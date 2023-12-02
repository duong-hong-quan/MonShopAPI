using MonShop.BackEnd.DAL.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MonShop.BackEnd.DAL.Common
{
    public class OrderResponse
    {
        public Order Order { get; set; }
        public IEnumerable<OrderItem> Items { get; set; }
        public PaymentResponse Payment { get; set; }
    }
}
