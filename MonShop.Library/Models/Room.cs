using System;
using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace MonShop.Library.Models
{
    public partial class Room
    {
        public Room()
        {
            Messages = new HashSet<Message>();
        }

        public int RoomId { get; set; }
        public string? RoomName { get; set; }
        public string? RoomImg { get; set; }
        [JsonIgnore]

        public virtual ICollection<Message> Messages { get; set; }
    }
}
