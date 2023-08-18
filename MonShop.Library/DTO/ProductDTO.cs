using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MonShopLibrary.DTO
{
    public class ProductDTO
    {
        public int ProductId { get; set; }
        public string ProductName { get; set; } = null!;
        public string ImageUrl { get; set; } = null!;
        public double Price { get; set; }
        public int Quantity { get; set; }
        public string? Description { get; set; }
        public int? CategoryId { get; set; }
        public int? ProductStatusId { get; set; }
        public double? Discount { get; set; }
        public bool? IsDeleted { get; set; }

    }
}
