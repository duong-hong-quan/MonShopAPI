using MonShopLibrary.DTO;
using MonShopLibrary.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MonShopLibrary.Repository
{
    public interface IOrderRepository
    {
        public Task<List<OrderStatus>> GetAllOrderStatus();
        public Task AddOrderStatus(OrderStatusDTO dto);
        public Task UpdateOrderStatus(OrderStatusDTO dto);
        public Task<string> AddOrderRequest(OrderRequest dto);
        public Task UpdateStatusForOrder(string OrderID, int status);
        public Task<Order> GetOrderByID(string OrderID);
        public Task<List<Order>> GetAllOrder();
        public  Task<List<OrderItem>> GetListItemByOrderID(string OrderID);
        public Task UpdateQuantityAfterPay(string OrderID);
    }
}
