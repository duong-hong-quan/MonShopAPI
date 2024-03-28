using Microsoft.AspNetCore.Http;
using MonShop.BackEnd.Common.Dto.Request;

namespace Monshop.BackEnd.Service.Contracts;

public interface IOrderService
{
    public Task<AppActionResult> CreateOrderWithPaymentUrl(int cartId, int paymentChoice, HttpContext context);
    public Task<AppActionResult> UpdateStatus(string orderId, int status);
    public Task<AppActionResult> GetAllOrder();
    public Task<AppActionResult> GetAllOrderByAccountId(string accountId);
}