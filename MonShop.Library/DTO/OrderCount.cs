using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MonShop.Library.DTO
{
    public class OrderCount
    {
        public int PendingCount { get; set; }
        public int SuccessCount { get; set; }
        public int FailCount { get; set; }
        public int ShipCount { get; set; }  
        public int DeliveredCount { get; set; }
        public int CancelCount { get; set; }
    }
}
