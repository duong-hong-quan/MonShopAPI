using System;
using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace MonShopLibrary.Models
{
    public partial class MomoPaymentResponse
    {
        public int PaymentResponseId { get; set; }
        public int OrderId { get; set; }
        public string? Amount { get; set; }
        public string? OrderInfo { get; set; }
        [JsonIgnore]

        public virtual Order Order { get; set; } = null!;
    }
}
