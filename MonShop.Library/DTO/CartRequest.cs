using MonShop.Library.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MonShop.Library.DTO
{
    public class CartRequest
    {
        public int AccountId {  get; set; }
        public CartItemDTO item { get; set; }  
    }
}
