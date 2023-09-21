using MonShopLibrary.DTO;
using MonShop.Library.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MonShop.Library.DTO;

namespace MonShop.Library.Repository.IRepository
{
    public interface IOrderRepository
    {
        public Task<List<OrderStatus>> GetAllOrderStatus();
        public Task AddOrderStatus(OrderStatusDTO dto);
        public Task UpdateOrderStatus(OrderStatusDTO dto);
        public Task<string> AddOrderRequest(int cartId);
        public Task UpdateStatusForOrder(string OrderID, int status);
        public Task<Order> GetOrderByID(string OrderID);
        public Task<List<Order>> GetAllOrder();
        public Task<ListOrder> GetListItemByOrderID(string OrderID);
        public Task UpdateQuantityAfterPay(string OrderID);
        public Task<List<Order>> GetAllOrderByAccountID(int AccountID, int OrderStatusID);
        public Task<OrderCount> OrderStatistic(int AccountID);
        public Task<bool> VerifyOrder(string OrderID);
    }
}
