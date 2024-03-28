using System.Transactions;
using Microsoft.AspNetCore.Http;
using MonShop.BackEnd.Common.Dto.Request;
using MonShop.BackEnd.DAL.Common;
using MonShop.BackEnd.DAL.Models;
using Monshop.BackEnd.Service.Contracts;
using Monshop.BackEnd.Service.Payment.PaymentRequest;
using Monshop.BackEnd.Service.Payment.PaymentService;
using MonShop.BackEnd.Utility.Utils;
using NetCore.QK.BackEndCore.Application.IRepositories;
using NetCore.QK.BackEndCore.Application.IUnitOfWork;

namespace Monshop.BackEnd.Service.Implementations;

public class OrderService : GenericBackendService, IOrderService
{
    private readonly IRepository<Order> _orderRepository;
    private readonly AppActionResult _result;
    private readonly IUnitOfWork _unitOfWork;

    public OrderService(IUnitOfWork unitOfWork, IRepository<Order> orderRepository, IServiceProvider serviceProvider) :
        base(serviceProvider)
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
                var cartRepository = Resolve<IRepository<Cart>>();
                var cartItemRepository = Resolve<IRepository<CartItem>>();
                var orderItemRepository = Resolve<IRepository<OrderItem>>();
                var productInventoryRepository = Resolve<IRepository<ProductInventory>>();
                var paymentGatewayService = Resolve<IPaymentGatewayService>();
                var cart = await cartRepository.GetByExpression(c => c.CartId == cartId, c => c.ApplicationUser);
                var cartItems =
                    await cartItemRepository.GetAllDataByExpression(c => c.CartId == cart.CartId, 0, 0, c => c.Product);
                var isValid = true;
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

                foreach (var item in cartItems.Items)
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
                    var order = new Order
                    {
                        OrderId = Guid.NewGuid().ToString(),
                        ApplicationUserId = cart.ApplicationUserId,
                        OrderDate = Utility.GetInstance().GetCurrentDateTimeInTimeZone(),
                        OrderStatusId = SD.Order.PENDING_PAY
                    };
                    await _orderRepository.Insert(order);
                    await _unitOfWork.SaveChangesAsync();

                    if (cartItems.Items.Any())
                    {
                        var items = new List<OrderItem>();
                        double total = 0;
                        foreach (var item in cartItems.Items)
                        {
                            var subTotal = (double)((item.Product.Price == null ? 0 : item.Product.Price) *
                                item.Quantity
                                * (100 - item.Product.Discount) / 100);
                            items.Add(new OrderItem
                            {
                                OrderId = order.OrderId,
                                PricePerUnit = item.Product.Price,
                                ProductId = item.Product.ProductId,
                                Quantity = item.Quantity,
                                SizeId = item.SizeId,
                                Subtotal = subTotal
                            });
                            total += subTotal;
                        }

                        order.Total = total;
                        await orderItemRepository.InsertRange(items);
                        await _unitOfWork.SaveChangesAsync();

                        await cartItemRepository.DeleteRange(cartItems.Items);
                        await cartRepository.DeleteById(cartId);
                        await _unitOfWork.SaveChangesAsync();
                    }

                    var payment = new PaymentInformationRequest
                    {
                        AccountID = cart.ApplicationUserId,
                        Amount = (double)order.Total,
                        CustomerName = $"{cart.ApplicationUser?.FirstName} {cart.ApplicationUser?.LastName}",
                        OrderID = order.OrderId
                    };
                    switch (paymentChoice)
                    {
                        case 1:
                            _result.Data = await paymentGatewayService.CreatePaymentUrlVnpay(payment, context);
                            break;
                        case 2:
                            _result.Data = await paymentGatewayService.CreatePaymentUrlMomo(payment);
                            break;
                        case 3:
                            _result.Data = "ok";
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
        var orderItemRepository = Resolve<IRepository<OrderItem>>();
        var paymentResponseRepository = Resolve<IRepository<PaymentResponse>>();

        try
        {
            var orderResponses = new List<OrderResponse>();
            var orders = await _orderRepository.GetAllDataByExpression(null, 0, 0, null);
            foreach (var order in orders.Items)
                orderResponses.Add(new OrderResponse
                {
                    Order = order,
                    Items = orderItemRepository.GetAllDataByExpression(o => o.OrderId == order.OrderId, 0, 0, null)
                        .GetAwaiter().GetResult().Items,
                    Payment = await paymentResponseRepository.GetByExpression(o => o.OrderId == order.OrderId)
                });
            _result.Data = orderResponses;
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
        var orderItemRepository = Resolve<IRepository<OrderItem>>();
        var paymentResponseRepository = Resolve<IRepository<PaymentResponse>>();

        try
        {
            var orderResponses = new List<OrderResponse>();
            var orders =
                await _orderRepository.GetAllDataByExpression(o => o.ApplicationUserId == accountId, 0, 0, null);
            foreach (var order in orders.Items)
                orderResponses.Add(new OrderResponse
                {
                    Order = order,
                    Items = orderItemRepository.GetAllDataByExpression(o => o.OrderId == order.OrderId, 0, 0, null)
                        .GetAwaiter().GetResult().Items,
                    Payment = await paymentResponseRepository.GetByExpression(o => o.OrderId == order.OrderId)
                });
            _result.Data = orderResponses;
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
                var isValid = true;
                var order = await _orderRepository.GetById(orderId);
                var nexStep = GetStatusOrder(order?.OrderStatusId);
                if (order == null)
                {
                    isValid = false;
                    _result.Messages.Add(SD.ResponseMessage.NOTFOUND(orderId, "order"));
                }
                else if (!nexStep.Contains(status))
                {
                    isValid = false;
                    _result.Messages.Add(
                        $"Next step is wrong. Current status is {order.OrderStatusId}. Please check business rule !!");
                }

                if (isValid)
                {
                    order.OrderStatusId = status;
                    await _unitOfWork.SaveChangesAsync();
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
        var result = new List<int>();
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