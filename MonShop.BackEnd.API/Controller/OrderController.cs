using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using MonShop.BackEnd.DAL.DTO;
using MonShop.BackEnd.DAL.DTO.Response;
using Monshop.BackEnd.Service.Contracts;
using MonShop.BackEnd.DAL.Models;

namespace MonShop.BackEnd.API.Controller
{
    [Route("Order")]
    [ApiController]
    [Authorize]
    public class OrderController : ControllerBase
    {
     private IOrderService _orderService;

        public OrderController(IOrderService orderService)
        {
            _orderService = orderService;
        }

        [HttpGet]
        [Route("GetAllOrder")]
        public async Task<AppActionResult> GetAllOrder()
        {
           return await _orderService.GetAllOrder();

        }
        [HttpGet]
        [Route("GetListItemByOrderID/{orderID}")]
        public async Task<AppActionResult> GetListItemByOrderID(string orderID)
        {
            return await _orderService.GetListItemByOrderID(orderID);

        }

        [HttpGet]
        [Route("GetAllOrderStatus")]
        public async Task<AppActionResult> GetAllOrderStatus()
        {
            return await _orderService.GetAllOrderStatus();


        }
        [HttpPost]
        [Route("AddOrderStatus")]
        public async Task<AppActionResult> AddOrderStatus(OrderStatusDTO dto)
        {

            return await _orderService.AddOrderStatus(dto);


        }
        [HttpPut]
        [Route("UpdateOrderStatus")]
        public async Task<AppActionResult   > UpdateOrderStatus(OrderStatusDTO dto)
        {
            return await _orderService.UpdateOrderStatus(dto);


        }
        [HttpPost]
        [Route("AddOrderRequest")]
        public async Task<AppActionResult> AddOrderRequest(OrderRequest orderRequest)
        {
            return await _orderService.AddOrderRequest(orderRequest);


        }
        [HttpPut]
        [Route("UpdateStatusForOrder")]

        public async Task<AppActionResult> UpdateStatusForOrder(string OrderID, int status)
        {
            return await _orderService.UpdateStatusForOrder(OrderID,status);


        }

        //    [Authorize]
        [HttpGet]
        [Route("GetAllOrderByAccountID/{AccountID}/{OrderStatusID}")]
        public async Task<AppActionResult> GetAllOrderByAccountID(string AccountID, int OrderStatusID)
        {
            return await _orderService.GetAllOrderByAccountID(AccountID, OrderStatusID);

        }

        [HttpGet]
        [Route("GetAllOrderByAccountID/{AccountID}")]
        public async Task<AppActionResult> GetAllOrderByAccountID(string AccountID)
        {
            return await _orderService.GetAllOrderByAccountID(AccountID);


        }
        [HttpGet]
        [Route("GetOrderStatistic/{AccountID}")]
        public async Task<AppActionResult> GetOrderStatistic(string AccountID)
        {
          //  return await _orderService.GetOrderStatistic(AccountID);
          throw new NotImplementedException();

        }

        [HttpGet]
        [Route("VerifyOrder/{OrderID}")]
        public async Task<AppActionResult> VerifyOrder(string OrderID)
        {
            return await _orderService.VerifyOrder(OrderID);



        }
    }
}
