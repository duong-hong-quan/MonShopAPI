using Microsoft.AspNetCore.Http;
using MonShop.BackEnd.Payment;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MonShop.BackEnd.Payment.Paypal
{
    public interface IPayPalServices
    {
        public Task<string> CreatePaymentUrl(PaymentInformationModel model, HttpContext context);
    }
}
