using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MonShop.Library.Models
{
    public partial class Token
    {
        [Key]
        public string RefreshToken { get; set; } = null!;
        public int AccountId { get; set; }
        [ForeignKey("AccountId")]
        public Account Account { get; set; } = null!;

        public DateTime ExpiresAt { get; set; }

    }
}
