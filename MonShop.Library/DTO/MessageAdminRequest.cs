using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MonShop.Library.DTO
{
    public class MessageAdminRequest
    {
        public int? Sender { get; set; }
        public string? Content { get; set; }
        public int RoomId { get; set; }
    }
}
