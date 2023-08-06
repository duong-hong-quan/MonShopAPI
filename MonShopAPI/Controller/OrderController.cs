using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using MonShopLibrary.DTO;
using MonShopLibrary.Models;
using MonShopLibrary.Repository;

namespace MonShopAPI.Controller
{
    [Route("Order")]
    [ApiController]
    public class OrderController : ControllerBase
    {
        private readonly IOrderRepository _orderRepository;

        public OrderController()
        {
            _orderRepository = new OrderRepository();
        }

        [HttpGet]
        [Route("GetAllOrder")]
        public async Task<IActionResult> GetAllOrder()
        {
            var list = await _orderRepository.GetAllOrder();
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
            int OrderID = await _orderRepository.AddOrderRequest(dto);
            return Ok(OrderID);
        }
        [HttpPut]
        [Route("UpdateStatusForOrder")]

        public async Task<IActionResult> UpdateStatusForOrder(int OrderID, int status)
        {
            await _orderRepository.UpdateStatusForOrder(OrderID, status);
            return Ok();
        }



    }
}
