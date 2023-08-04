using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PaymentGateway.Momo
{
    public class MomoResponeModel
    {
        public string partnerCode { get; set; }
        public string requestId { get; set; }
        public string transId { get; set; }

        public string orderId { get; set; }
        public string amount { get; set; }
        public string orderInfo { get; set; }   
        public string resultCode { get; set; }
        public string message { get; set; }
        public string responseTime { get; set; }
        public string extraData { get; set; }
        public string signature { get; set; }
    }
}
