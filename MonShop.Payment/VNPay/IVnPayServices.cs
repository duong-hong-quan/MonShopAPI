using Microsoft.AspNetCore.Http;
using VNPay.Models;
namespace VNPay.Services;
public interface IVnPayServices
{
    string CreatePaymentUrl(PaymentInformationModel model, HttpContext context);
    PaymentResponseModel PaymentExecute(IQueryCollection collections);
}