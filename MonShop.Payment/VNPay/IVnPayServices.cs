using Microsoft.AspNetCore.Http;
using MonShop.Payment;
using VNPay.Models;
namespace VNPay.Services;
public interface IVnPayServices
{
    string CreatePaymentUrl(PaymentInformationModel model, HttpContext context);
    VNPAYResponseModel PaymentExecute(IQueryCollection collections);
}