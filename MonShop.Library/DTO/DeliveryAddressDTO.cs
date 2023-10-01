using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MonShop.Library.DTO
{
    public class DeliveryAddressDTO
    {
        public string? DeliveryAddressId { get; set; }
        public string? Address { get; set; }

        public string? ApplicationUserId { get; set; }
    }
}
