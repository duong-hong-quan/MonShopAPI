using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MonShop.Library.DTO
{
    public class OrderRequest
    {
        public int CartId {  get; set; }
        public string DeliveryAddressId { get; set; } = null!;
    }
}
