using Microsoft.EntityFrameworkCore;
using MonShopLibrary.DTO;
using MonShop.Library.Models;
using MonShopLibrary.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MonShopLibrary.DAO
{
    public class OrderDBContext : MonShopContext
    {

        public OrderDBContext() { }

        public async Task<List<OrderStatus>> GetAllOrderStatus()
        {
            List<OrderStatus> list = await this.OrderStatuses.ToListAsync();
            return list;
        }
        public async Task AddOrderStatus(OrderStatusDTO dto)
        {
            OrderStatus status = new OrderStatus { OrderStatusId = dto.OrderStatusId, Status = dto.Status };
            await this.OrderStatuses.AddAsync(status);
            await this.SaveChangesAsync();
        }
        public async Task UpdateOrderStatus(OrderStatusDTO dto)
        {
            OrderStatus status = new OrderStatus { OrderStatusId = dto.OrderStatusId, Status = dto.Status };
            this.OrderStatuses.Update(status);
            await this.SaveChangesAsync();
        }

        public async Task<string> AddOrderRequest(OrderRequest dto)
        {
            double total = 0;
            Order order = new Order
            {
                OrderId = Guid.NewGuid().ToString(),
                OrderDate = Utility.getInstance().GetCurrentDateTimeInTimeZone(),
                Total = total,
                OrderStatusId = Constant.Order.PENDING_PAY,
                BuyerAccountId = dto.Order.BuyerAccountId
            };
            await this.Orders.AddAsync(order);
            await this.SaveChangesAsync();
            string orderID = order.OrderId;
            foreach (OrderItemDTO itemDTO in dto.Items)
            {
                Product product = await this.Products.FindAsync(itemDTO.ProductId);
                double price = product.Price;
                OrderItem item = new OrderItem
                {
                    OrderId = orderID,
                    ProductId = itemDTO.ProductId,
                    Quantity = itemDTO.Quantity,
                    PricePerUnit = price,
                    Subtotal = itemDTO.Quantity * price,
                };
                total += item.Subtotal;

                await this.OrderItems.AddAsync(item);
            }
            order.Total = total;

            await this.SaveChangesAsync();
            return orderID;
        }

        public async Task UpdateStatusForOrder(string OrderID, int status)
        {
            Order order = await this.Orders.FindAsync(OrderID);
            order.OrderStatusId = status;
            await this.SaveChangesAsync();
        }

        public async Task<Order> GetOrderByID(string OrderID)
        {
            Order order = await this.Orders.FindAsync(OrderID);
            return order;
        }
        public async Task<List<Order>> GetAllOrder()
        {
            List<Order> order = await this.Orders.ToListAsync();
            return order;
        }

        public async Task<List<OrderItem>> GetListItemByOrderID(string OrderID)
        {
            List<OrderItem> orderItems = await this.OrderItems.Include(o=> o.Product).Where(o=> o.OrderId == OrderID).ToListAsync();
            return orderItems;
        }

        public async Task UpdateQuantityAfterPay(string OrderID)
        {
            List<OrderItem> list = await this.OrderItems.Where(o => o.OrderId == OrderID).ToListAsync();
            foreach(OrderItem item in list)
            {
                int quantity = item.Quantity;
                Product product = await this.Products.FindAsync(item.ProductId);
                if(product != null)
                {
                    product.Quantity = product.Quantity - quantity;
                }
            }
            await this.SaveChangesAsync();  
        }

    }
}
