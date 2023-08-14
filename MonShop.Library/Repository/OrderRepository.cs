using MonShopLibrary.DAO;
using MonShopLibrary.DTO;
using MonShop.Library.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MonShop.Library.DTO;

namespace MonShopLibrary.Repository
{
    public class OrderRepository : IOrderRepository
    {
        OrderDBContext db = new OrderDBContext();
        public async Task<List<OrderStatus>> GetAllOrderStatus() => await db.GetAllOrderStatus();   
       
        public async Task AddOrderStatus(OrderStatusDTO dto)=> await db.AddOrderStatus(dto);
       
        public async Task UpdateOrderStatus(OrderStatusDTO dto)=> await db.UpdateOrderStatus(dto);

        public async Task<string> AddOrderRequest(OrderRequest dto) => await db.AddOrderRequest(dto);


        public async Task UpdateStatusForOrder(string OrderID, int status) => await db.UpdateStatusForOrder(OrderID, status);
        public async Task<Order> GetOrderByID(string OrderID) => await db.GetOrderByID(OrderID);
        public async Task<List<Order>> GetAllOrder() => await db.GetAllOrder();
        public async Task<ListOrder> GetListItemByOrderID(string OrderID) => await db.GetListItemByOrderID(OrderID);
        public async Task UpdateQuantityAfterPay(string OrderID) => await db.UpdateQuantityAfterPay(OrderID);
        public async Task<List<Order>> GetAllOrderByAccountID(int AccountID, int OrderStatusID) => await db.GetAllOrderByAccountID(AccountID, OrderStatusID);


    }
}
