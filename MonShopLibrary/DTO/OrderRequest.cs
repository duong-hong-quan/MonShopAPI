using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MonShopLibrary.DTO
{
    public class OrderRequest
    {
        public List<OrderItemDTO> Items { get; set; }
        public OrderDTO Order { get; set; }
    }
}
