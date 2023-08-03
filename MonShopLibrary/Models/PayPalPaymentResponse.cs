using System;
using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace MonShopLibrary.Models
{
    public partial class PayPalPaymentResponse
    {
        public int PaymentResponseId { get; set; }
        public int OrderId { get; set; }
        public string? OrderDescription { get; set; }
        public string? TransactionId { get; set; }
        public string? PaymentId { get; set; }
        public bool? Success { get; set; }
        [JsonIgnore]

        public virtual Order Order { get; set; } = null!;
    }
}
