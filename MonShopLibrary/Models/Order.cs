﻿using System;
using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace MonShopLibrary.Models
{
    public partial class Order
    {
        public Order()
        {
            MomoPaymentResponses = new HashSet<MomoPaymentResponse>();
            PayPalPaymentResponses = new HashSet<PayPalPaymentResponse>();
            VnpayPaymentResponses = new HashSet<VnpayPaymentResponse>();
        }

        public int OrderId { get; set; }
        public string? Email { get; set; }
        public DateTime? OrderDate { get; set; }
        public double? Total { get; set; }
        public int OrderStatusId { get; set; }
        public int BuyerAccountId { get; set; }
        [JsonIgnore]

        public virtual Account BuyerAccount { get; set; } = null!;
        [JsonIgnore]

        
        public virtual OrderStatus OrderStatus { get; set; } = null!;
        [JsonIgnore]

        public virtual ICollection<MomoPaymentResponse> MomoPaymentResponses { get; set; }
        [JsonIgnore]

        public virtual ICollection<PayPalPaymentResponse> PayPalPaymentResponses { get; set; }
        [JsonIgnore]

        public virtual ICollection<VnpayPaymentResponse> VnpayPaymentResponses { get; set; }
    }
}
