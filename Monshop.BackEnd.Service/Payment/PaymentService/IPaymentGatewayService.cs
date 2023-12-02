using Microsoft.AspNetCore.Http;
using Monshop.BackEnd.Service.Payment.PaymentRequest;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Monshop.BackEnd.Service.Payment.PaymentService
{
    public interface IPaymentGatewayService
    {
        Task<string> CreatePaymentUrlVnpay(PaymentInformationRequest request, HttpContext httpContext);
        Task<string> CreatePaymentUrlMomo(PaymentInformationRequest request);

    }
}
