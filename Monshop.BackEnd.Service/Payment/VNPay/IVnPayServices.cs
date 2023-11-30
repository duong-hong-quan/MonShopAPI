using Microsoft.AspNetCore.Http;

namespace Monshop.BackEnd.Service.Payment.VNPay;
public interface IVnPayServices
{
    string CreatePaymentUrl(PaymentInformationModel model, HttpContext context);
    VNPAYResponseModel PaymentExecute(IQueryCollection collections);
}