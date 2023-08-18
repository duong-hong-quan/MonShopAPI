using Microsoft.EntityFrameworkCore;
using MonShopLibrary.DTO;
using MonShop.Library.Models;
using MonShopLibrary.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MonShop.Library.DTO;

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
            if (order != null) {
                if(order.OrderStatusId != Utils.Constant.Order.PENDING_PAY ||
                    order.OrderStatusId != Utils.Constant.Order.FAILURE_PAY ||
                    order.OrderStatusId != Utils.Constant.Order.SUCCESS_PAY)
                {
                    order.OrderStatusId = status;

                }

            }
            await this.SaveChangesAsync();
        }

        public async Task<Order> GetOrderByID(string OrderID)
        {
            Order order = await this.Orders.FindAsync(OrderID);
            return order;
        }
        public async Task<List<Order>> GetAllOrder()
        {
            List<Order> order = await this.Orders.Include(o => o.BuyerAccount).Include(o=> o.OrderStatus).ToListAsync();
            return order;
        }
        public async Task<List<Order>> GetAllOrderByAccountID(int AccountID, int OrderStatusID)
        {
            List<Order> order = await this.Orders.Where(a => a.BuyerAccountId == AccountID && a.OrderStatusId == OrderStatusID).ToListAsync();
            return order;
        }
        public async Task<ListOrder> GetListItemByOrderID(string OrderID)
        {
            string paymentMethod = "Pending Pay";
            Order orderDTO = await this.Orders.Include(o=> o.BuyerAccount).Include(o => o.OrderStatus).Where(o=>o.OrderId== OrderID).SingleOrDefaultAsync();
            MomoPaymentResponse paymentResponse = await this.MomoPaymentResponses.Where(m => m.OrderId == OrderID).FirstOrDefaultAsync();
            if (paymentResponse != null && paymentResponse.Success == true)
            {
                paymentMethod = "Momo";
            }
            VnpayPaymentResponse vnPaymentResponse = await this.VnpayPaymentResponses.Where(m => m.OrderId == OrderID).FirstOrDefaultAsync();
            if (vnPaymentResponse != null && vnPaymentResponse.Success == true)
            {
                paymentMethod = "VNpay";
            }
            PayPalPaymentResponse paypalPaymentResponse = await this.PayPalPaymentResponses.Where(m => m.OrderId == OrderID).FirstOrDefaultAsync();
            if (paypalPaymentResponse != null && paypalPaymentResponse.Success == true)
            {
                paymentMethod = "PayPal";
            }
            List<OrderItem> orderItems = await this.OrderItems.Include(o => o.Product).Where(o => o.OrderId == OrderID).ToListAsync();
            ListOrder listOrder = new ListOrder { order = orderDTO, orderItem = orderItems, paymentMethod = paymentMethod };

            return listOrder;
        }

        public async Task UpdateQuantityAfterPay(string OrderID)
        {
            List<OrderItem> list = await this.OrderItems.Where(o => o.OrderId == OrderID).ToListAsync();
            foreach (OrderItem item in list)
            {
                int quantity = item.Quantity;
                Product product = await this.Products.FindAsync(item.ProductId);
                if (product != null)
                {
                    product.Quantity = product.Quantity - quantity;
                }
            }
            await this.SaveChangesAsync();
        }


        
    }
}
