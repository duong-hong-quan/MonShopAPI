using Microsoft.AspNetCore.Http;
using Monshop.BackEnd.Service.Payment.PaymentRequest;

namespace Monshop.BackEnd.Service.Payment.PaymentService;

public interface IPaymentGatewayService
{
    Task<string> CreatePaymentUrlVnpay(PaymentInformationRequest request, HttpContext httpContext);
    Task<string> CreatePaymentUrlMomo(PaymentInformationRequest request);
}