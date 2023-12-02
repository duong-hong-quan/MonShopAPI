using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Monshop.BackEnd.Service.Contracts;
using MonShop.BackEnd.Common.Dto.Request;

namespace MonShop.BackEnd.API.Controller
{
    [Route("order")]
    [ApiController]
    public class OrderController : ControllerBase
    {
        private IOrderService _orderService;

        public OrderController(IOrderService orderService)
        {
            _orderService = orderService;
        }

        [HttpGet("get-all-order-by-accountId/{accountId}")]
        public async Task<AppActionResult> GetAllOrderByAccountId(string accountId)
        {
            return await _orderService.GetAllOrderByAccountId(accountId);
        }

        [HttpGet("get-all-order")]
        public async Task<AppActionResult> GetAllOrder()
        {
            return await _orderService.GetAllOrder();
        }
       
        [HttpPost("create-order-with-payment-url")]
        public async Task<AppActionResult> CreateOrderWithPaymentUrl(int cartId, int paymentChoice)
        {
            return await _orderService.CreateOrderWithPaymentUrl(cartId, paymentChoice, HttpContext);
        }

        [HttpPut("update-status")]
        public async Task<AppActionResult> UpdateStatus(string orderId, int status)
        {
            return await _orderService.UpdateStatus(orderId,status);
        }
    }
}
