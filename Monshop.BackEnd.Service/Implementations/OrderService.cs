using AutoMapper;
using Monshop.BackEnd.Service.Contracts;
using MonShop.BackEnd.DAL.Contracts;
using MonShop.BackEnd.DAL.DTO;
using MonShop.BackEnd.DAL.DTO.Response;
using MonShop.BackEnd.DAL.IRepository;
using MonShop.BackEnd.DAL.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Monshop.BackEnd.Service.Implementations
{
    public class OrderService : GenericBackEndService, IOrderService
    {
        private IOrderRepository _orderRepository;
        private IUnitOfWork _unitOfWork;
        private IMapper _mapper;
        private AppActionResult _result;

        public OrderService(IOrderRepository orderRepository, IUnitOfWork unitOfWork, IMapper mapper, IServiceProvider serviceProvider) : base(serviceProvider)
        {
            _orderRepository = orderRepository;
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _result = new();
        }

        public async Task<AppActionResult> IsOutStock(OrderRequest request)
        {
            var cartRepository = Resolve<ICartRepository>();
            var cartItemRepository = Resolve<ICartItemRepository>();
            var productInventoryRepository = Resolve<IProductInventoryRepository>();
            Cart cart = await cartRepository.GetByExpression(c => c.CartId == request.CartId);
            if (cart != null)
            {
                var cartItems = await cartItemRepository.GetListByExpression(c => c.CartId == request.CartId);
                foreach (var cartItem in cartItems)
                {
                    ProductInventory productInventory = await productInventoryRepository.GetByExpression(p => p.ProductId == cartItem.ProductId && p.SizeId == cartItem.SizeId);
                    if (productInventory != null && productInventory.Quantity < cartItem.Quantity)
                    {

                        _result.Data = true;
                    }
                }

            }
            _result.Data = false;
            return _result;

        }
        public async Task<AppActionResult> AddOrderRequest(OrderRequest orderRequest)
        {
            if ((bool)IsOutStock(orderRequest).Result.Data == false)
            {
                string orderID = null;
                var cartRepository = Resolve<ICartRepository>();
                var cartService = Resolve<ICartService>();
                var productInventoryRepository = Resolve<IProductInventoryRepository>();
                var productRepository = Resolve<IProductRepository>();
                var orderItemRepository = Resolve<IOrderItemRepository>();
                Cart cart = await cartRepository.GetByExpression(c => c.CartId == orderRequest.CartId);
                if (cart != null)
                {
                    double total = 0;
                    Order order = new Order
                    {
                        OrderId = Guid.NewGuid().ToString(),
                        OrderDate = MonShop.BackEnd.Utility.Utils.Utility.GetInstance().GetCurrentDateTimeInTimeZone(),
                        Total = total,
                        OrderStatusId = MonShop.BackEnd.Utility.Utils.Constant.Order.PENDING_PAY,
                        ApplicationUserId = cart.ApplicationUserId,
                        DeliveryAddressId = orderRequest.DeliveryAddressId
                    };
                    await _orderRepository.Insert(order);
                    await _unitOfWork.SaveChangeAsync();
                    orderID = order.OrderId;
                    var items = await cartService.GetItemsByCartId(orderRequest.CartId);
                    foreach (var itemDTO in (IEnumerable<CartItem>)items.Data)
                    {
                        ProductInventory productInventory = await productInventoryRepository.GetByExpression(i => i.ProductId == itemDTO.ProductId && i.SizeId == itemDTO.SizeId);
                        Product product = await productRepository.GetByExpression(i => i.ProductId == itemDTO.ProductId);
                        if (productInventory != null && product != null && productInventory.Quantity >= itemDTO.Quantity)
                        {

                            OrderItem item = new OrderItem
                            {
                                OrderId = orderID,
                                ProductId = (int)itemDTO.ProductId,
                                Quantity = itemDTO.Quantity,
                                PricePerUnit = product.Price,
                                Subtotal = (double)(itemDTO.Quantity * product.Price * (100 - product.Discount) / 100),
                                SizeId = itemDTO.SizeId
                            };
                            total += item.Subtotal;

                            await orderItemRepository.Insert(item);
                        }

                    }
                    order.Total = total;
                    await _unitOfWork.SaveChangeAsync();
                    await cartService.RemoveCart(orderRequest.CartId);

                }
                _result.Data = orderID;
            }
            return _result;
        }

        public async Task<AppActionResult> AddOrderStatus(OrderStatusDTO dto)
        {
            var orderStatusRepository = Resolve<IOrderStatusRepository>();
            await orderStatusRepository.Insert(_mapper.Map<OrderStatus>(dto));
            await _unitOfWork.SaveChangeAsync();
            return _result;
        }

        public async Task<AppActionResult> GetAllOrder()
        {
            _result.Data = await _orderRepository.GetAll();
            return _result;
        }

        public async Task<AppActionResult> GetAllOrderByAccountID(string AccountID, int OrderStatusID)
        {
            _result.Data = await _orderRepository.GetListByExpression(o => o.OrderStatusId == OrderStatusID && o.ApplicationUserId == AccountID);
            return _result;
        }

        public async Task<AppActionResult> GetAllOrderByAccountID(string AccountID)
        {
            _result.Data = await _orderRepository.GetListByExpression(o => o.ApplicationUserId == AccountID);
            return _result;

        }

        public async Task<AppActionResult> GetAllOrderStatus()
        {
            var orderStatusRepository = Resolve<IOrderStatusRepository>();
            _result.Data = await orderStatusRepository.GetAll();
            return _result;
        }

        public async Task<AppActionResult> GetListItemByOrderID(string OrderID)
        {
            var orderItemRepository = Resolve<IOrderItemRepository>();
            _result.Data = await orderItemRepository.GetListByExpression(o => o.OrderId == OrderID);
            return _result;
        }

        public async Task<AppActionResult> GetOrderByID(string OrderID)
        {
            _result.Data = await _orderRepository.GetById(OrderID);
            return _result;
        }



        public Task<AppActionResult> OrderStatistic(string AccountID)
        {
            throw new NotImplementedException();
        }

        public async Task<AppActionResult> UpdateOrderStatus(OrderStatusDTO dto)
        {
            var orderStatusRepository = Resolve<IOrderStatusRepository>();
            await orderStatusRepository.Update(_mapper.Map<OrderStatus>(dto));
            await _unitOfWork.SaveChangeAsync();
            return _result;
        }

        public async Task<AppActionResult> UpdateQuantityAfterPay(string OrderID)
        {
            var orderItemRepository = Resolve<IOrderItemRepository>();
            var productInventoryRepository = Resolve<IProductInventoryRepository>();
            var list = await orderItemRepository.GetListByExpression(o => o.OrderId == OrderID);
            foreach (var item in list)
            {
                int quantity = item.Quantity;
                ProductInventory product = await productInventoryRepository.GetByExpression(i => i.ProductId == item.ProductId);
                if (product != null)
                {
                    product.Quantity = product.Quantity - quantity;
                }
            }
            await _unitOfWork.SaveChangeAsync();
            return _result;
        }

        public async Task<AppActionResult> UpdateStatusForOrder(string OrderID, int status)
        {
            Order order = await _orderRepository.GetByExpression(o => o.OrderId == OrderID);

            order.OrderStatusId = status;

            await _unitOfWork.SaveChangeAsync();
            return _result;
        }

        public async Task<AppActionResult> VerifyOrder(string OrderID)
        {
            var paymentResponseRepository = Resolve<IPaymentResponseRepository>();
            Order order = await  _orderRepository.GetByExpression(o => o.OrderId == OrderID);
            PaymentResponse payment = await paymentResponseRepository.GetByExpression(p => p.OrderId == OrderID);
            if (order != null && order.OrderStatusId == MonShop.BackEnd.Utility.Utils.Constant.Order.SUCCESS_PAY && payment.Success)
            {

                _result.Data= true;
            }
            else
            {
                _result.Data = false;
            }
            return _result;
        }
    }
}
