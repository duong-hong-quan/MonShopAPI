using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MonShopLibrary.DTO
{
    public class OrderStatusDTO
    {
        public int OrderStatusId { get; set; }
        public string Status { get; set; } = null!;
    }
}
