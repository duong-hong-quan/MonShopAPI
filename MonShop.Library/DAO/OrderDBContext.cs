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
                Product product = await this.Products.FirstAsync(i => i.ProductId == itemDTO.ProductId);
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
            Order order = await this.Orders.FirstAsync(o => o.OrderId == OrderID);

            order.OrderStatusId = status;

            await this.SaveChangesAsync();
        }

        public async Task<Order> GetOrderByID(string OrderID)
        {
            Order order = await this.Orders.FirstAsync(o => o.OrderId == OrderID);
            return order;
        }
        public async Task<List<Order>> GetAllOrder()
        {
            List<Order> order = await this.Orders.Include(o => o.BuyerAccount).Include(o => o.OrderStatus).ToListAsync();
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
            Order orderDTO = await this.Orders.Include(o => o.BuyerAccount).Include(o => o.OrderStatus).Where(o => o.OrderId == OrderID).FirstAsync();
            MomoPaymentResponse paymentResponse = await this.MomoPaymentResponses.Where(m => m.OrderId == OrderID).FirstAsync();
            if (paymentResponse != null && paymentResponse.Success == true)
            {
                paymentMethod = "Momo";
            }
            VnpayPaymentResponse vnPaymentResponse = await this.VnpayPaymentResponses.Where(m => m.OrderId == OrderID).FirstAsync();
            if (vnPaymentResponse != null && vnPaymentResponse.Success == true)
            {
                paymentMethod = "VNpay";
            }
            PayPalPaymentResponse paypalPaymentResponse = await this.PayPalPaymentResponses.Where(m => m.OrderId == OrderID).FirstAsync();
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
                Product product = await this.Products.FirstAsync(i => i.ProductId == item.ProductId);
                if (product != null)
                {
                    product.Quantity = product.Quantity - quantity;
                }
            }
            await this.SaveChangesAsync();
        }

        public async Task<OrderCount> OrderStatistic(int AccountID)
        {
            OrderCount order = new OrderCount
            {
                PendingCount = await OrderCountByStatus(AccountID, 1),
                SuccessCount = await OrderCountByStatus(AccountID, 2),
                FailCount = await OrderCountByStatus(AccountID, 3),
                ShipCount = await OrderCountByStatus(AccountID, 4),
                DeliveredCount = await OrderCountByStatus(AccountID, 5),
                CancelCount = await OrderCountByStatus(AccountID, 6),



            };
            return order;
        }

        private async Task<int> OrderCountByStatus(int AccountID, int status)
        {
            int count = 0;
            count = await this.Orders.Where(o => o.BuyerAccountId == AccountID && o.OrderStatusId == status).CountAsync();
            return count;
        }

        public async Task<bool> VerifyOrder(string OrderID)
        {
            MomoPaymentResponse momo = await this.MomoPaymentResponses.Where(m => m.OrderId == OrderID).FirstAsync();
            VnpayPaymentResponse vnpay = await this.VnpayPaymentResponses.Where(m => m.OrderId == OrderID).FirstAsync();
            PayPalPaymentResponse paypal = await this.PayPalPaymentResponses.Where(m => m.OrderId == OrderID).FirstAsync();
            Order order = await this.Orders.FirstAsync(o => o.OrderId == OrderID);
            if (order != null && order.OrderStatusId == Constant.Order.SUCCESS_PAY && (momo != null || vnpay != null || paypal != null))
            {

                return true;
            }
            return false;
        }
    }
}
