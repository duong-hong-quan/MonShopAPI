using MonShop.BackEnd.DAL.DTO;
using MonShop.BackEnd.DAL.DTO.Response;
using MonShop.BackEnd.DAL.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Monshop.BackEnd.Service.Contracts
{
    public interface IOrderService
    {
        public Task<AppActionResult> GetAllOrderStatus();
        public Task<AppActionResult> AddOrderStatus(OrderStatusDTO dto);
        public Task<AppActionResult> UpdateOrderStatus(OrderStatusDTO dto);
        public Task<AppActionResult> AddOrderRequest(OrderRequest orderRequest);
        public Task<AppActionResult> UpdateStatusForOrder(string OrderID, int status);
        public Task<AppActionResult> GetOrderByID(string OrderID);
        public Task<AppActionResult> GetAllOrder();
        public Task<AppActionResult> GetListItemByOrderID(string OrderID);
        public Task<AppActionResult> UpdateQuantityAfterPay(string OrderID);
        public Task<AppActionResult> GetAllOrderByAccountID(string AccountID, int OrderStatusID);
        public Task<AppActionResult> GetAllOrderByAccountID(string AccountID);

        public Task<AppActionResult> OrderStatistic(string AccountID);
        public Task<AppActionResult> VerifyOrder(string OrderID);
        public Task<AppActionResult> IsOutStock(OrderRequest request);
    }
}
