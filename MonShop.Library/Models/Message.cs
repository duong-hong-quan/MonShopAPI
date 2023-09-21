using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MonShop.Library.Models
{
    public partial class Message
    {
        [Key]
        public int MessageId { get; set; }
        public int? Sender { get; set; }
        [ForeignKey("Sender")]
        public virtual Account? SenderNavigation { get; set; }

        public string? Content { get; set; }
        public DateTime? SendTime { get; set; }
        public int RoomId { get; set; }
        [ForeignKey("RoomId")]

        public  Room Room { get; set; } = null!;
    }
}
