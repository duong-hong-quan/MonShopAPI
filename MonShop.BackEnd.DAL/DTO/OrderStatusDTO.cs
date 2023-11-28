using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MonShop.BackEnd.DAL.DTO
{
    public class OrderStatusDTO
    {
        public int OrderStatusId { get; set; }
        public string Status { get; set; } = null!;
    }
}
