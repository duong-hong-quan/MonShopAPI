using MonShop.BackEnd.DAL.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MonShop.BackEnd.DAL.DTO
{
    public class ListOrder
    {
        public Order order { get; set; }
        public List<OrderItem> orderItem { get; set; }
        public PaymentType? paymentMethod { get; set; }

    }


}
