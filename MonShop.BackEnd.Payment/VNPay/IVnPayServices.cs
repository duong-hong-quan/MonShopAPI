using Microsoft.AspNetCore.Http;
using MonShop.BackEnd.Payment;

namespace MonShop.BackEnd.Payment.VNPay;
public interface IVnPayServices
{
    string CreatePaymentUrl(PaymentInformationModel model, HttpContext context);
    VNPAYResponseModel PaymentExecute(IQueryCollection collections);
}