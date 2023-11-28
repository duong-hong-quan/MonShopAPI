using MonShop.BackEnd.DAL.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MonShop.BackEnd.DAL.DTO
{
    public class CartRequest
    {
        public string ApplicationUserId { get; set; }
        public CartItemDTO item { get; set; }
    }
}
