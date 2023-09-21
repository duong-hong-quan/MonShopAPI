using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace MonShop.Library.Models
{
    public partial class Room
    {
        [Key]

        public int RoomId { get; set; }
        public string? RoomName { get; set; }
        public string? RoomImg { get; set; }

    }
}
