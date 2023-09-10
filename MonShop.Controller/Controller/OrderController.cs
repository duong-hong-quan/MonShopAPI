using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using MonShopLibrary.DTO;
using MonShop.Library.Models;
using MonShopLibrary.Repository;
using Microsoft.AspNetCore.Authorization;
using MonShopLibrary.Utils;
using MonShop.Library.DTO;
using MonShop.Controller.Model;
using System.Collections.Generic;

namespace MonShopAPI.Controller
{
    [Route("Order")]
    [ApiController]
    public class OrderController : ControllerBase
    {
        private readonly IOrderRepository _orderRepository;
        private readonly IProductRepository _productRepository;
        private readonly IPaymentRepository _paymentRepository;
        private readonly ResponeDTO _responeDTO;
        public OrderController
            (
            IOrderRepository orderRepository,
            IProductRepository productRepository,
            IPaymentRepository paymentRepository
            )
        {
            _orderRepository = orderRepository;
            _productRepository = productRepository;
            _paymentRepository = paymentRepository;
            _responeDTO = new ResponeDTO();
        }

        [HttpGet]
        [Route("GetAllOrder")]
        public async Task<ResponeDTO> GetAllOrder()
        {
            try
            {
                var list = await _orderRepository.GetAllOrder();
                _responeDTO.Data = list;

            }
            catch (Exception ex)
            {
                _responeDTO.IsSuccess = false;
                _responeDTO.Message = ex.Message;
            }

            return _responeDTO;
        }
        [HttpGet]
        [Route("GetListItemByOrderID")]
        public async Task<ResponeDTO> GetListItemByOrderID(string orderID)
        {
            try
            {
                var list = await _orderRepository.GetListItemByOrderID(orderID);
                _responeDTO.Data = list;
            }
            catch (Exception ex)
            {
                _responeDTO.IsSuccess = false;
                _responeDTO.Message = ex.Message;
            }
            return _responeDTO;
        }

        [HttpGet]
        [Route("GetAllOrderStatus")]
        public async Task<ResponeDTO> GetAllOrderStatus()
        {
            try
            {

                var list = await _orderRepository.GetAllOrderStatus();
                _responeDTO.Data = list;

            }
            catch (Exception ex)
            {
                _responeDTO.IsSuccess = false;
                _responeDTO.Message = ex.Message;
            }
            return _responeDTO;
        }
        [HttpPost]
        [Route("AddOrderStatus")]
        public async Task<ResponeDTO> AddOrderStatus(OrderStatusDTO dto)
        {
            try
            {
                await _orderRepository.AddOrderStatus(dto);
                _responeDTO.Data = dto;

            }
            catch (Exception ex)
            {
                _responeDTO.IsSuccess = false;
                _responeDTO.Message = ex.Message;
            }
            return _responeDTO;

        }
        [HttpPut]
        [Route("UpdateOrderStatus")]
        public async Task<ResponeDTO> UpdateOrderStatus(OrderStatusDTO dto)
        {
            try
            {

                await _orderRepository.UpdateOrderStatus(dto);
                _responeDTO.Data = dto;

            }
            catch (Exception ex)
            {
                _responeDTO.IsSuccess = false;
                _responeDTO.Message = ex.Message;
            }
            return _responeDTO;
        }
        [HttpPost]
        [Route("AddOrderRequest")]
        public async Task<ResponeDTO> AddOrderRequest(OrderRequest dto)
        {
            bool isError = false;

            try
            {
                foreach (var item in dto.Items)
                {
                    if (item.Quantity == 0)
                    {
                        _responeDTO.Message = $"The quantity must greater than 0";
                        isError = true; // Set the error flag
                        break; // Exit the loop
                    }
                    Product product = await _productRepository.GetProductByID(item.ProductId);
                    if (product == null)
                    {
                        _responeDTO.Message = $"No result Product with ID {item.ProductId}";
                        isError = true; // Set the error flag
                        break; // Exit the loop

                    }
                    if (item.Quantity > product?.Quantity)
                    {
                        _responeDTO.Message = "This product doesn't have enough quantity";
                        isError = true; // Set the error flag
                        break; // Exit the loop

                    }

                }
                if (!isError) // Only execute this block if no error occurred in the loop
                {
                    string OrderID = await _orderRepository.AddOrderRequest(dto);
                    _responeDTO.Data = OrderID;
                }



            }
            catch (Exception ex)
            {
                _responeDTO.IsSuccess = false;
                _responeDTO.Message = ex.Message;
            }


            return _responeDTO;
        }
        [HttpPut]
        [Route("UpdateStatusForOrder")]

        public async Task<ResponeDTO> UpdateStatusForOrder(string OrderID, int status)
        {
            try
            {
                if (status == Constant.Order.FAILURE_PAY ||
                    status == Constant.Order.PENDING_PAY ||
                    status == Constant.Order.FAILURE_PAY ||
                    status == Constant.Order.SUCCESS_PAY)
                {
                    _responeDTO.Message = "The system only accept when the order is payed success!";
                }
                else
                {
                    MomoPaymentResponse momo = await _paymentRepository.GetPaymentMomoByOrderID(OrderID);
                    VnpayPaymentResponse vnpay = await _paymentRepository.GetPaymentVNPayByOrderID(OrderID);
                    PayPalPaymentResponse paypal = await _paymentRepository.GetPaymentPaypalByOrderID(OrderID);
                    if (momo != null || vnpay != null || paypal != null)
                    {
                        await _orderRepository.UpdateStatusForOrder(OrderID, status);
                        _responeDTO.Message = "Update successful";
                    }

                }


            }
            catch (Exception ex)
            {
                _responeDTO.IsSuccess = false;
                _responeDTO.Message = ex.Message;
            }
            return _responeDTO;




        }

        //    [Authorize]
        [HttpGet]
        [Route("GetAllOrderByAccountID")]
        public async Task<ResponeDTO> GetAllOrderByAccountID(int AccountID, int OrderStatusID)
        {
            try
            {

                List<Order> list = await _orderRepository.GetAllOrderByAccountID(AccountID, OrderStatusID);

                _responeDTO.Data = list;
            }
            catch (Exception ex)
            {
                _responeDTO.IsSuccess = false;
                _responeDTO.Message = ex.Message;
            }
            return _responeDTO;
        }
        [HttpGet]
        [Route("GetOrderStatistic")]
        public async Task<ResponeDTO> GetOrderStatistic(int AccountID)
        {
            try
            {
                OrderCount order = await _orderRepository.OrderStatistic(AccountID);
                _responeDTO.Data = order;


            }
            catch (Exception ex)
            {
                _responeDTO.IsSuccess = false;
                _responeDTO.Message = ex.Message;
            }
            return _responeDTO;
        }

        [HttpGet]
        [Route("VerifyOrder")]
        public async Task<ResponeDTO> VerifyOrder(string OrderID)
        {
            try
            {

                bool res = await _orderRepository.VerifyOrder(OrderID);
                if (res)
                {
                    _responeDTO.IsSuccess = true;
                    _responeDTO.Message = "Payed successfully";
                }
                else
                {
                    _responeDTO.IsSuccess = false;
                    _responeDTO.Message = "Payed failed";

                }

            }
            catch (Exception ex)
            {

                _responeDTO.IsSuccess = false;
                _responeDTO.Message = ex.Message;
            }

            return _responeDTO;
        }
    }
}
