using System;
using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace MonShopLibrary.Models
{
    public partial class ProductStatus
    {
        public ProductStatus()
        {
            Products = new HashSet<Product>();
        }

        public int ProductStatusId { get; set; }
        public string Status { get; set; } = null!;
        [JsonIgnore]

        public virtual ICollection<Product> Products { get; set; }
    }
}
