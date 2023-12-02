using Microsoft.AspNetCore.Http;
using Monshop.BackEnd.Service.Contracts;
using Monshop.BackEnd.Service.Payment.PaymentRequest;
using Monshop.BackEnd.Service.Payment.PaymentService;
using MonShop.BackEnd.Common.Dto.Request;
using MonShop.BackEnd.DAL.Common;
using MonShop.BackEnd.DAL.Contracts;
using MonShop.BackEnd.DAL.Implementations;
using MonShop.BackEnd.DAL.Models;
using MonShop.BackEnd.Utility.Utils;
using System.Transactions;

namespace Monshop.BackEnd.Service.Implementations
{
    public class OrderService : GenericBackendService, IOrderService
    {
        private IUnitOfWork _unitOfWork;
        private AppActionResult _result;
        private IOrderRepository _orderRepository;

        public OrderService(IUnitOfWork unitOfWork, IOrderRepository orderRepository, IServiceProvider serviceProvider) : base(serviceProvider)
        {
            _unitOfWork = unitOfWork;
            _orderRepository = orderRepository;
            _result = new AppActionResult();
        }

        public async Task<AppActionResult> CreateOrderWithPaymentUrl(int cartId, int paymentChoice, HttpContext context)
        {
            using (var scope = new TransactionScope(TransactionScopeAsyncFlowOption.Enabled))
            {

                try
                {
                    var cartRepository = Resolve<ICartRepository>();
                    var cartItemRepository = Resolve<ICartItemRepository>();
                    var orderItemRepository = Resolve<IOrderItemRepository>();
                    var productInventoryRepository = Resolve<IProductInventoryRepository>();
                    var paymentGatewayService = Resolve<IPaymentGatewayService>();
                    var cart = await cartRepository.GetByExpression(c => c.CartId == cartId, c => c.ApplicationUser);
                    var cartItems = await cartItemRepository.GetListByExpression(c => c.CartId == cart.CartId, c => c.Product);
                    bool isValid = true;
                    if (paymentChoice != 1 && paymentChoice != 2 && paymentChoice != 3)
                    {

                        isValid = false;
                        _result.Messages.Add("The system only accept pay with VNPAY (1) Momo (2) ShipCOD (3)");
                    }
                    if (cart == null)
                    {
                        isValid = false;
                        _result.Messages.Add(SD.ResponseMessage.NOTFOUND(cart.CartId, "cart"));
                    }
                    foreach (var item in cartItems)
                    {
                        var productInventory = await productInventoryRepository
                            .GetByExpression(i => i.ProductId == item.ProductId && i.SizeId == item.SizeId);

                        if (productInventory.Quantity < item.Quantity)
                        {
                            isValid = false;
                            _result.Messages.Add($"The product id {item.ProductId} and size {item.SizeId} is out of stock");
                        }
                    }

                    if (isValid)
                    {
                        Order order = new Order
                        {
                            OrderId = Guid.NewGuid().ToString(),
                            ApplicationUserId = cart.ApplicationUserId,
                            OrderDate = Utility.GetInstance().GetCurrentDateTimeInTimeZone(),
                            OrderStatusId = SD.Order.PENDING_PAY
                        };
                        await _orderRepository.Insert(order);
                        await _unitOfWork.SaveChangeAsync();

                        if (cartItems.Any())
                        {
                            List<OrderItem> items = new List<OrderItem>();
                            double total = 0;
                            foreach (var item in cartItems)
                            {
                                var subTotal = (double)((item.Product.Price == null ? 0 : item.Product.Price) * (item.Quantity)
                                    * (100 - item.Product.Discount) / 100);
                                items.Add(new OrderItem
                                {
                                    OrderId = order.OrderId,
                                    PricePerUnit = item.Product.Price,
                                    ProductId = item.Product.ProductId,
                                    Quantity = item.Quantity,
                                    SizeId = item.SizeId,
                                    Subtotal = subTotal,
                                });
                                total += subTotal;
                            }
                            order.Total = total;
                            await orderItemRepository.InsertRange(items);
                            await _unitOfWork.SaveChangeAsync();

                            await cartItemRepository.DeleteRange(cartItems);
                            await cartRepository.DeleteById(cartId);
                            await _unitOfWork.SaveChangeAsync();
                        }
                        PaymentInformationRequest payment = new PaymentInformationRequest
                        {
                            AccountID = cart.ApplicationUserId,
                            Amount = (double)order.Total,
                            CustomerName = $"{cart.ApplicationUser?.FirstName} {cart.ApplicationUser?.LastName}",
                            OrderID = order.OrderId

                        };
                        switch (paymentChoice)
                        {
                            case 1:
                                _result.Result.Data = await paymentGatewayService.CreatePaymentUrlVnpay(payment, context);
                                break;
                            case 2:
                                _result.Result.Data = await paymentGatewayService.CreatePaymentUrlMomo(payment);
                                break;
                            case 3:
                                _result.Result.Data = "ok";
                                break;
                        }
                    }

                    scope.Complete();
                }

                catch (Exception ex)
                {
                    _result.IsSuccess = false;
                    _result.Messages.Add(ex.Message);
                }
                return _result;
            }

        }

        public async Task<AppActionResult> GetAllOrder()
        {
            var orderItemRepository = Resolve<IOrderItemRepository>();
            var paymentResponseRepository = Resolve<IPaymentResponseRepository>();

            try
            {
                List<OrderResponse> orderResponses = new List<OrderResponse>();
                var orders = await _orderRepository.GetAll();
                foreach (var order in orders)
                {
                    orderResponses.Add(new OrderResponse
                    {
                        Order = order,
                        Items = await orderItemRepository.GetListByExpression(o => o.OrderId == order.OrderId),
                        Payment = await paymentResponseRepository.GetByExpression(o => o.OrderId == order.OrderId)
                    });
                }
                _result.Result.Data = orderResponses;
            }

            catch (Exception ex)
            {
                _result.IsSuccess = false;
                _result.Messages.Add(ex.Message);
            }
            return _result;
        }

        public async Task<AppActionResult> GetAllOrderByAccountId(string accountId)
        {
            var orderItemRepository = Resolve<IOrderItemRepository>();
            var paymentResponseRepository = Resolve<IPaymentResponseRepository>();

            try
            {
                List<OrderResponse> orderResponses = new List<OrderResponse>();
                var orders = await _orderRepository.GetListByExpression(o => o.ApplicationUserId == accountId);
                foreach (var order in orders)
                {
                    orderResponses.Add(new OrderResponse
                    {
                        Order = order,
                        Items = await orderItemRepository.GetListByExpression(o => o.OrderId == order.OrderId),
                        Payment = await paymentResponseRepository.GetByExpression(o => o.OrderId == order.OrderId)
                    });
                }
                _result.Result.Data = orderResponses;
            }

            catch (Exception ex)
            {
                _result.IsSuccess = false;
                _result.Messages.Add(ex.Message);
            }
            return _result;
        }

        public async Task<AppActionResult> UpdateStatus(string orderId, int status)
        {
            using (var scope = new TransactionScope(TransactionScopeAsyncFlowOption.Enabled))
            {
                try
                {
                    bool isValid = true;
                    var order = await _orderRepository.GetById(orderId);
                    List<int> nexStep = GetStatusOrder(order?.OrderStatusId);
                    if (order == null)
                    {
                        isValid = false;
                        _result.Messages.Add(SD.ResponseMessage.NOTFOUND(orderId, "order"));
                    }
                    else if (!nexStep.Contains(status))
                    {
                        isValid = false;
                        _result.Messages.Add($"Next step is wrong. Current status is {order.OrderStatusId}. Please check business rule !!");

                    }
                    if (isValid)
                    {
                        order.OrderStatusId = status;
                        await _unitOfWork.SaveChangeAsync();
                        scope.Complete();
                    }
                }
                catch (Exception ex)
                {
                    _result.IsSuccess = false;
                    _result.Messages.Add(ex.Message);

                }
            }

            return _result;
        }

        private List<int> GetStatusOrder(int? status)
        {
            List<int> result = new List<int>();
            result.Add(SD.Order.CANCELLED);

            switch (status)
            {
                case 1:
                    result.Add(SD.Order.SUCCESS_PAY);
                    result.Add(SD.Order.FAILURE_PAY);
                    break;
                case 2:
                    result.Add(SD.Order.SHIPPED);
                    break;
                case 4:
                    result.Add(SD.Order.DELIVERED);
                    break;
            }
            return result;

        }
    }
}
