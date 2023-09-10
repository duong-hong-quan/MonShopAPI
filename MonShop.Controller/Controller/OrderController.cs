using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using MonShopLibrary.DTO;
using MonShop.Library.Models;
using MonShopLibrary.Repository;
using Microsoft.AspNetCore.Authorization;
using MonShopLibrary.Utils;
using MonShop.Library.DTO;

namespace MonShopAPI.Controller
{
    [Route("Order")]
    [ApiController]
    public class OrderController : ControllerBase
    {
        private readonly IOrderRepository _orderRepository;
        private readonly IProductRepository _productRepository;
        private readonly IPaymentRepository _paymentRepository;
        public OrderController(IOrderRepository orderRepository, IProductRepository productRepository, IPaymentRepository paymentRepository)
        {
            _orderRepository = orderRepository;
            _productRepository = productRepository;
            _paymentRepository = paymentRepository;
        }

        [HttpGet]
        [Route("GetAllOrder")]
        public async Task<IActionResult> GetAllOrder()
        {
            var list = await _orderRepository.GetAllOrder();
            return Ok(list);
        }
        [HttpGet]
        [Route("GetListItemByOrderID")]
        public async Task<IActionResult> GetListItemByOrderID(string orderID)
        {
            var list = await _orderRepository.GetListItemByOrderID(orderID);
            return Ok(list);
        }

        [HttpGet]
        [Route("GetAllOrderStatus")]
        public async Task<IActionResult> GetAllOrderStatus()
        {
            var list = await _orderRepository.GetAllOrderStatus();
            return Ok(list);
        }
        [HttpPost]
        [Route("AddOrderStatus")]
        public async Task<IActionResult> AddOrderStatus(OrderStatusDTO dto)
        {
            await _orderRepository.AddOrderStatus(dto);
            return Ok(dto);

        }
        [HttpPut]
        [Route("UpdateOrderStatus")]
        public async Task<IActionResult> UpdateOrderStatus(OrderStatusDTO dto)
        {
            await _orderRepository.UpdateOrderStatus(dto);
            return Ok(dto);
        }
        [HttpPost]
        [Route("AddOrderRequest")]
        public async Task<IActionResult> AddOrderRequest(OrderRequest dto)
        {
            foreach (var item in dto.Items)
            {
                if (item.Quantity == 0)
                {
                    return BadRequest($"The quantity must greater than 0");
                }
                Product product = await _productRepository.GetProductByID(item.ProductId);
                if (product == null)
                {
                    return BadRequest($"No result Product with ID {item.ProductId}");
                }
                if (item.Quantity > product.Quantity)
                {
                    return BadRequest("This product doesn't have enough quantity");
                } 

            }
          
            string OrderID = await _orderRepository.AddOrderRequest(dto);
            return Ok(OrderID);
        }
        [HttpPut]
        [Route("UpdateStatusForOrder")]

        public async Task<IActionResult> UpdateStatusForOrder(string OrderID, int status)
        {
            Order order = await _orderRepository.GetOrderByID(OrderID);
            if (order == null)
            {
                return BadRequest();
            }

            if (status == Constant.Order.FAILURE_PAY || status == Constant.Order.PENDING_PAY || status == Constant.Order.FAILURE_PAY || status == Constant.Order.SUCCESS_PAY)
            {
                return BadRequest("The system only accept when the order is payed success!");
            }
            else
            {
                MomoPaymentResponse momo = await _paymentRepository.GetPaymentMomoByOrderID(OrderID);
                VnpayPaymentResponse vnpay = await _paymentRepository.GetPaymentVNPayByOrderID(OrderID);
                PayPalPaymentResponse paypal = await _paymentRepository.GetPaymentPaypalByOrderID(OrderID);
                if (momo != null || vnpay != null || paypal != null)
                {
                    await _orderRepository.UpdateStatusForOrder(OrderID, status);
                    return Ok("Update successful");
                }

            }
            return BadRequest("The system only accept when the order is payed success!");



        }

        //    [Authorize]
        [HttpGet]
        [Route("GetAllOrderByAccountID")]
        public async Task<IActionResult> GetAllOrderByAccountID(int AccountID, int OrderStatusID)
        {
            List<Order> list = await _orderRepository.GetAllOrderByAccountID(AccountID, OrderStatusID);
            return Ok(list);
        }
        [HttpGet]
        [Route("GetOrderStatistic")]
        public async Task<IActionResult> GetOrderStatistic(int AccountID)
        {
            OrderCount order = await _orderRepository.OrderStatistic(AccountID);
            return Ok(order);
        }

        [HttpGet]
        [Route("VerifyOrder")]
        public async Task<IActionResult> VerifyOrder(string OrderID)
        {
            bool res = await _orderRepository.VerifyOrder(OrderID);
            if (res)
            {
                return Ok();

            }
            else
            {
                return BadRequest();
            }
        }
    }
}
