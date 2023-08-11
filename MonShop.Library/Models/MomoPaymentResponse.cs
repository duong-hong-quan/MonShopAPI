using System;
using System.Collections.Generic;

namespace MonShopLibrary.Models
{
    public partial class MomoPaymentResponse
    {
        public long PaymentResponseId { get; set; }
        public string OrderId { get; set; } = null!;
        public string? Amount { get; set; }
        public string? OrderInfo { get; set; }
        public bool Success { get; set; }

        public virtual Order Order { get; set; } = null!;
    }
}
