using MonShopLibrary.DTO;
using MonShop.Library.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MonShop.Library.DTO;
using Microsoft.EntityFrameworkCore;
using MonShopLibrary.Utils;
using MonShop.Library.Repository.IRepository;
using MonShop.Library.Data;

namespace MonShopLibrary.Repository
{
    public class OrderRepository : IOrderRepository
    {
        private readonly MonShopContext _db;
        private readonly ICartRepository _cartRepository;   

        public OrderRepository(MonShopContext db, ICartRepository cartRepository)
        {
            _db = db;
            _cartRepository = cartRepository;
        }

        public async Task<List<OrderStatus>> GetAllOrderStatus()
        {
            List<OrderStatus> list = await _db.OrderStatus.ToListAsync();
            return list;
        }
        public async Task AddOrderStatus(OrderStatusDTO dto)
        {
            OrderStatus status = new OrderStatus { OrderStatusId = dto.OrderStatusId, Status = dto.Status };
            await _db.OrderStatus.AddAsync(status);
            await _db.SaveChangesAsync();
        }
        public async Task UpdateOrderStatus(OrderStatusDTO dto)
        {
            OrderStatus status = new OrderStatus { OrderStatusId = dto.OrderStatusId, Status = dto.Status };
            _db.OrderStatus.Update(status);
            await _db.SaveChangesAsync();
        }

        public async Task<string> AddOrderRequest(int cartId)
        {
            string orderID = null;
            Cart cart = await _db.Cart.FirstOrDefaultAsync(c => c.CartId == cartId);
            if (cart != null)
            {
                double total = 0;
                Order order = new Order
                {
                    OrderId = Guid.NewGuid().ToString(),
                    OrderDate = Utility.getInstance().GetCurrentDateTimeInTimeZone(),
                    Total = total,
                    OrderStatusId = Constant.Order.PENDING_PAY,
                    BuyerAccountId = cart.AccountId
                };
                await _db.Order.AddAsync(order);
                await _db.SaveChangesAsync();
                 orderID = order.OrderId;

                IEnumerable<CartItem> items = await _cartRepository.GetItemsByCartId(cartId);
                foreach (CartItem itemDTO in items)
                {
                    Product product = await _db.Product.FirstAsync(i => i.ProductId == itemDTO.ProductId);
                    if (product.Quantity >= itemDTO.Quantity) {

                        OrderItem item = new OrderItem
                        {
                            OrderId = orderID,
                            ProductId = (int)itemDTO.ProductId,
                            Quantity = itemDTO.Quantity,
                            PricePerUnit = product.Price,
                            Subtotal = itemDTO.Quantity * product.Price,
                        };
                        total += item.Subtotal;

                        await _db.OrderItem.AddAsync(item);
                    }
                  
                }
                order.Total = total;
                await _db.SaveChangesAsync();
                await _cartRepository.RemoveCart(cartId);

            }

            return orderID;
        }

        public async Task UpdateStatusForOrder(string OrderID, int status)
        {
            Order order = await _db.Order.FirstAsync(o => o.OrderId == OrderID);

            order.OrderStatusId = status;

            await _db.SaveChangesAsync();
        }

        public async Task<Order> GetOrderByID(string OrderID)
        {
            Order order = await _db.Order.FirstAsync(o => o.OrderId == OrderID);
            return order;
        }
        public async Task<List<Order>> GetAllOrder()
        {
            List<Order> order = await _db.Order.Include(o => o.BuyerAccount).Include(a => a.BuyerAccount.Role).Include(o => o.OrderStatus).ToListAsync();
            return order;
        }
        public async Task<List<Order>> GetAllOrderByAccountID(int AccountID, int OrderStatusID)
        {
            List<Order> order = await _db.Order.Where(a => a.BuyerAccountId == AccountID && a.OrderStatusId == OrderStatusID).ToListAsync();
            return order;
        }
        public async Task<ListOrder> GetListItemByOrderID(string OrderID)
        {
            Order orderDTO = await _db.Order.Include(o => o.BuyerAccount).
                Include(o => o.OrderStatus).
                Where(o => o.OrderId == OrderID).
                FirstAsync();
            List<OrderItem> OrderItem = await _db.OrderItem.
                Include(o => o.Product).
                Where(o => o.OrderId == OrderID).
                ToListAsync();

            PaymentResponse paymentResponse = await _db.PaymentResponse.FirstOrDefaultAsync(p => p.OrderId == OrderID);
            PaymentType paymentMethod = await _db.PaymentType.FirstOrDefaultAsync(p => p.PaymentTypeId == paymentResponse.PaymentTypeId);

            ListOrder listOrder = new ListOrder { order = orderDTO, orderItem = OrderItem, paymentMethod = paymentMethod };
            return listOrder;
        }

        public async Task UpdateQuantityAfterPay(string OrderID)
        {
            List<OrderItem> list = await _db.OrderItem.Where(o => o.OrderId == OrderID).ToListAsync();
            foreach (OrderItem item in list)
            {
                int quantity = item.Quantity;
                Product product = await _db.Product.FirstAsync(i => i.ProductId == item.ProductId);
                if (product != null)
                {
                    product.Quantity = product.Quantity - quantity;
                }
            }
            await _db.SaveChangesAsync();
        }

        public async Task<OrderCount> OrderStatistic(int AccountID)
        {
            OrderCount order = new OrderCount
            {
                PendingCount = await OrderCountByStatus(AccountID, Constant.Order.PENDING_PAY),
                SuccessCount = await OrderCountByStatus(AccountID, Constant.Order.SUCCESS_PAY),
                FailCount = await OrderCountByStatus(AccountID, Constant.Order.FAILURE_PAY),
                ShipCount = await OrderCountByStatus(AccountID, Constant.Order.SHIPPED),
                DeliveredCount = await OrderCountByStatus(AccountID, Constant.Order.DELIVERED),
                CancelCount = await OrderCountByStatus(AccountID, Constant.Order.CANCELLED),



            };
            return order;
        }

        private async Task<int> OrderCountByStatus(int AccountID, int status)
        {
            int count = 0;
            count = await _db.Order.Where(o => o.BuyerAccountId == AccountID && o.OrderStatusId == status).CountAsync();
            return count;
        }

        public async Task<bool> VerifyOrder(string OrderID)
        {

            Order order = await _db.Order.FirstAsync(o => o.OrderId == OrderID);
            PaymentResponse payment = await _db.PaymentResponse.FirstOrDefaultAsync(p => p.OrderId == OrderID);
            if (order != null && order.OrderStatusId == Constant.Order.SUCCESS_PAY && payment.Success)
            {

                return true;
            }
            return false;
        }

     
    }
}
