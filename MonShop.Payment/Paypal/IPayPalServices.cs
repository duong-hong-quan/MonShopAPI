using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VNPay.Models;

namespace PaymentGateway.Paypal
{
    public interface IPayPalServices
    {
        public Task<string> CreatePaymentUrl(PaymentInformationModel model, HttpContext context);
    }
}
