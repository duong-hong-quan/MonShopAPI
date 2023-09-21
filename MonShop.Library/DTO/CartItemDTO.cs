using MonShop.Library.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MonShop.Library.DTO
{
    public class CartItemDTO
    {
        public int CartItemId { get; set; }
        public int? ProductId { get; set; }
        public int Quantity { get; set; }

    }
}
