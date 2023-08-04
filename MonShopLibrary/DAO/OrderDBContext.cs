using Microsoft.EntityFrameworkCore;
using MonShopLibrary.DTO;
using MonShopLibrary.Models;
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
            OrderStatus status = new OrderStatus { OrderStatusId = dto.OrderStatusId, OrderStatus1 = dto.OrderStatus1 };
            await this.OrderStatuses.AddAsync(status);
            await this.SaveChangesAsync();
        }
        public async Task UpdateOrderStatus(OrderStatusDTO dto)
        {
            OrderStatus status = new OrderStatus { OrderStatusId = dto.OrderStatusId, OrderStatus1 = dto.OrderStatus1 };
            this.OrderStatuses.Update(status);
            await this.SaveChangesAsync();
        }

        public async Task AddOrderRequest(OrderRequest dto)
        {
            double total = 0;
            Order order = new Order
            {
                Email = dto.Order.Email,
                OrderDate = dto.Order.OrderDate,
                Total = total,
                OrderStatusId = Constant.Order.PENDING_PAY,
                BuyerAccountId = dto.Order.BuyerAccountId
            };
            await this.Orders.AddAsync(order);
            await this.SaveChangesAsync();
            int orderID = order.OrderId;
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
        }

        public async Task UpdateStatusForOrder(int OrderID, int status)
        {
            Order order = await this.Orders.FindAsync(OrderID);
            order.OrderStatusId = status;
            await this.SaveChangesAsync();
        }

        public async Task<Order> GetOrderByID(int OrderID)
        {
            Order order = await this.Orders.FindAsync(OrderID);
            return order;
        }
        public async Task<List<Order>> GetAllOrder()
        {
            List<Order> order = await this.Orders.ToListAsync();
            return order;
        }

    }
}
