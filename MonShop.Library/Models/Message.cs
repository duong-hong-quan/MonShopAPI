using System;
using System.Collections.Generic;

namespace MonShopLibrary.Models
{
    public partial class Message
    {
        public int MessageId { get; set; }
        public int? Sender { get; set; }
        public int? Receiver { get; set; }
        public DateTime? SendTime { get; set; }

        public virtual Account? ReceiverNavigation { get; set; }
        public virtual Account? SenderNavigation { get; set; }
    }
}
