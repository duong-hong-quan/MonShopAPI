using System;
using System.Collections.Generic;

namespace MonShopLibrary.Models
{
    public partial class Token
    {
        public string RefreshToken { get; set; } = null!;
        public int AccountId { get; set; }
        public DateTime ExpiresAt { get; set; }

        public virtual Account Account { get; set; } = null!;
    }
}
