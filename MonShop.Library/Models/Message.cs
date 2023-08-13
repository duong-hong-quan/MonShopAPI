using System;
using System.Collections.Generic;

namespace MonShop.Library.Models
{
    public partial class Message
    {
        public int MessageId { get; set; }
        public int? Sender { get; set; }
        public string? Content { get; set; }
        public DateTime? SendTime { get; set; }
        public int RoomId { get; set; }

        public virtual Room Room { get; set; } = null!;
        public virtual Account? SenderNavigation { get; set; }
    }
}
