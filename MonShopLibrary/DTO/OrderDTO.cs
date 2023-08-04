using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MonShopLibrary.DTO
{
    public class OrderDTO
    {
        public int OrderId { get; set; }
        public string? Email { get; set; }
        public DateTime? OrderDate { get; set; }
        public double? Total { get; set; }
        public int OrderStatusId { get; set; }
        public int BuyerAccountId { get; set; }
    }
}
