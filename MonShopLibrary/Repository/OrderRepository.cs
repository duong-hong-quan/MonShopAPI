using MonShopLibrary.DAO;
using MonShopLibrary.DTO;
using MonShopLibrary.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MonShopLibrary.Repository
{
    public class OrderRepository : IOrderRepository
    {
        OrderDBContext db = new OrderDBContext();
        public async Task<List<OrderStatus>> GetAllOrderStatus() => await db.GetAllOrderStatus();   
       
        public async Task AddOrderStatus(OrderStatusDTO dto)=> await db.AddOrderStatus(dto);
       
        public async Task UpdateOrderStatus(OrderStatusDTO dto)=> await db.UpdateOrderStatus(dto);

        public async Task<int> AddOrderRequest(OrderRequest dto) => await db.AddOrderRequest(dto);


        public async Task UpdateStatusForOrder(int OrderID, int status) => await db.UpdateStatusForOrder(OrderID, status);
        public async Task<Order> GetOrderByID(int OrderID) => await db.GetOrderByID(OrderID);
        public async Task<List<Order>> GetAllOrder() => await db.GetAllOrder();
        public async Task<List<OrderItem>> GetListItemByOrderID(int OrderID) => await db.GetListItemByOrderID(OrderID);
        public async Task UpdateQuantityAfterPay(int OrderID) => await db.UpdateQuantityAfterPay(OrderID);


    }
}
