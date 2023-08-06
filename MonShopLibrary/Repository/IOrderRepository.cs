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

        public Task<int> AddOrderRequest(OrderRequest dto);


        public Task UpdateStatusForOrder(int OrderID, int status);
        public Task<Order> GetOrderByID(int OrderID);
        public Task<List<Order>> GetAllOrder();
    }
}
