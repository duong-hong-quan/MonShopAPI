using System;
using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace MonShop.Library.Models
{
    public partial class Account
    {
        public Account()
        {
            Orders = new HashSet<Order>();
            Tokens = new HashSet<Token>();
        }

        public int AccountId { get; set; }
        public string Email { get; set; } = null!;
        public string Password { get; set; } = null!;
        public string? ImageUrl { get; set; }
        public string? FirstName { get; set; }
        public string? LastName { get; set; }
        public string? Address { get; set; }
        public string? PhoneNumber { get; set; }
        public bool? IsDeleted { get; set; }
        public int RoleId { get; set; }

        public virtual Role Role { get; set; } = null!;
        [JsonIgnore]
        public virtual ICollection<Order> Orders { get; set; }
        [JsonIgnore]

        public virtual ICollection<Token> Tokens { get; set; }
    }
}
