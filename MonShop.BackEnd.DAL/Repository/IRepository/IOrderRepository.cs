using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MonShop.BackEnd.DAL.DTO;
using MonShop.BackEnd.DAL.Models;

namespace MonShop.BackEnd.DAL.Repository.IRepository
{
    public interface IOrderRepository
    {
        public Task<List<OrderStatus>> GetAllOrderStatus();
        public Task AddOrderStatus(OrderStatusDTO dto);
        public Task UpdateOrderStatus(OrderStatusDTO dto);
        public Task<string> AddOrderRequest(OrderRequest orderRequest);
        public Task UpdateStatusForOrder(string OrderID, int status);
        public Task<Order> GetOrderByID(string OrderID);
        public Task<List<Order>> GetAllOrder();
        public Task<ListOrder> GetListItemByOrderID(string OrderID);
        public Task UpdateQuantityAfterPay(string OrderID);
        public Task<List<Order>> GetAllOrderByAccountID(string AccountID, int OrderStatusID);
        public Task<List<Order>> GetAllOrderByAccountID(string AccountID);

        public Task<OrderCount> OrderStatistic(string AccountID);
        public Task<bool> VerifyOrder(string OrderID);
        public Task<bool> IsOutStock(OrderRequest request);

    }
}
