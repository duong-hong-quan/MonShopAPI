using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PaymentGateway.Paypal
{
    public class PaymentResponePayPal
    {
        public string Amount { get; set; }  
        public string OrderDescription { get; set; }
        public string TransactionId { get; set; }
        public string OrderId { get; set; }
        public string PaymentMethod { get; set; }
        public string PaymentId { get; set; }
        public string PayerId { get; set; }
        public bool Success { get; set; }
    }
}
