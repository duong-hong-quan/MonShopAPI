using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Memory;
using MonShop.BackEnd.API.Model;
using MonShop.BackEnd.DAL.Models;
using MonShop.BackEnd.DAL.Repository.IRepository;
using MonShop.BackEnd.Utility.Utils;
using MonShop.BackEnd.Payment;
using MonShop.BackEnd.Payment.Momo;
using MonShop.BackEnd.Payment.Paypal;
using MonShop.BackEnd.Payment.VNPay;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

using static MonShop.BackEnd.Payment.Paypal.PayPalResponeModel;

namespace MonShop.BackEnd.API.Controller
{
    [Authorize]

    [Route("Payment")]
    [ApiController]
    public class PaymentController : ControllerBase
    {
        private readonly IMomoServices _momoServices;
        private readonly IVnPayServices _vnPayServices;
        private readonly IPayPalServices _payPalServices;
        private readonly IOrderRepository _orderRepository;
        private readonly IAccountRepository _accountRepository;
        private readonly IPaymentRepository _paymentRepository;
        private readonly ResponseDTO _response;
        public PaymentController
            (
            IMomoServices momoServices,
            IVnPayServices vnPayServices,
            IPayPalServices payPalServices,
            IOrderRepository orderRepository,
            IAccountRepository accountRepository,
            IPaymentRepository paymentRepository
            )
        {
            _momoServices = momoServices;
            _payPalServices = payPalServices;
            _orderRepository = orderRepository;
            _accountRepository = accountRepository;
            _paymentRepository = paymentRepository;
            _vnPayServices = vnPayServices;
            _response = new ResponseDTO();

        }




        [HttpGet]
        [Route("GellAllPayment")]
        public async Task<ResponseDTO> GellAllPayment()
        {
            try
            {

                var list = await _paymentRepository.GetAllPayment();
                _response.Data = list;

            }
            catch (Exception ex)
            {

                _response.IsSuccess = false;
                _response.Message = ex.Message;
            }

            return _response;
        }

        [HttpPost]
        [Route("GetPaymentURLMomo/{OrderID}")]
        public async Task<ResponseDTO> GetPaymentURLMomo(string OrderID)
        {
            try
            {
                Order order = await _orderRepository.GetOrderByID(OrderID);
                ApplicationUser account = await _accountRepository.GetAccountById(order.ApplicationUserId);
                if (order == null)
                {
                    _response.Message = $"No result order with ID {OrderID}";
                }

                if (order?.OrderStatusId != Constant.Order.PENDING_PAY)
                {
                    _response.Message = $"No result for order {OrderID} with status pending";
                }

                if (order?.OrderStatusId == Constant.Order.PENDING_PAY)
                {
                    Momo momo = new Momo
                    {
                        AccountID = order.ApplicationUserId,
                        Amount = (double)order.Total,
                        CustomerName = $"{account.FirstName} {account.LastName}",
                        OrderID = OrderID
                    };

                    string endpoint = _momoServices.CreatePaymentString(momo);
                    _response.Data = endpoint;
                }

            }
            catch (Exception ex)
            {
                _response.IsSuccess = false;
                _response.Message = ex.Message;

            }
            return _response;


        }
        [HttpPost]
        [Route("GetPaymentURLVNPay/{OrderID}")]
        public async Task<ResponseDTO> GetPaymentURLVNPay(string OrderID)
        {
            try
            {
                Order order = await _orderRepository.GetOrderByID(OrderID);
                ApplicationUser account = await _accountRepository.GetAccountById(order.ApplicationUserId);
                if (order == null)
                {
                    _response.Message = $"No result order with ID {OrderID}";
                }

                if (order?.OrderStatusId != Constant.Order.PENDING_PAY)
                {
                    _response.Message = $"No result for order {OrderID} with status pending";
                }

                if (order?.OrderStatusId == Constant.Order.PENDING_PAY)
                {
                    PaymentInformationModel model = new PaymentInformationModel
                    {
                        AccountID = order.ApplicationUserId,
                        Amount = (double)order.Total,
                        CustomerName = $"{account.FirstName} {account.LastName}",
                        OrderID = order.OrderId
                    };

                    string endpoint = _vnPayServices.CreatePaymentUrl(model, HttpContext);
                    _response.Data = endpoint;
                }

            }
            catch (Exception ex)
            {
                _response.IsSuccess = false;
                _response.Message = ex.Message;

            }
            return _response;

        }

        [HttpPost]
        [Route("GetPaymentURLPayPal/{OrderID}")]
        public async Task<ResponseDTO> GetPaymentURLPayPal(string OrderID)
        {
            try
            {
                Order order = await _orderRepository.GetOrderByID(OrderID);
                ApplicationUser account = await _accountRepository.GetAccountById(order.ApplicationUserId);
                if (order == null)
                {
                    _response.Message = $"No result order with ID {OrderID}";
                }

                if (order.OrderStatusId != Constant.Order.PENDING_PAY)
                {
                    _response.Message = $"No result for order {OrderID} with status pending";
                }

                if (order.OrderStatusId == Constant.Order.PENDING_PAY)
                {
                    PaymentInformationModel model = new PaymentInformationModel
                    {
                        AccountID = order.ApplicationUserId,
                        Amount = (double)order.Total,
                        CustomerName = $"{account.FirstName} {account.LastName}",
                        OrderID = order.OrderId
                    };

                    string endpoint = await _payPalServices.CreatePaymentUrl(model, HttpContext);
                    _response.Data = endpoint;
                }

            }
            catch (Exception ex)
            {
                _response.IsSuccess = false;
                _response.Message = ex.Message;

            }

            return _response;

        }

        [HttpPost]
        [Route("MomoIPN")]
        public async Task<IActionResult> MomoIPN(MomoResponeModel momo)
        {
            try
            {
                Order order = await _orderRepository.GetOrderByID(momo.extraData);



                if (momo.resultCode == 0 && order.OrderStatusId == Constant.Order.PENDING_PAY)
                {
                    PaymentResponse dto = new PaymentResponse
                    {
                        PaymentResponseId = Convert.ToString(momo.transId),
                        OrderId = momo.extraData,
                        Amount = momo.amount.ToString(),
                        OrderInfo = momo.orderInfo,
                        Success = true,
                        PaymentTypeId = Constant.PaymentType.PAYMENT_MOMO
                    };
                    await _paymentRepository.AddPaymentRespone(dto);

                    await _orderRepository.UpdateStatusForOrder(momo.extraData, Constant.Order.SUCCESS_PAY);
                    await _orderRepository.UpdateQuantityAfterPay(momo.extraData);

                }


                return Ok();
            }
            catch (Exception ex)
            {

                return BadRequest(ex.Message);
            }

        }



        [HttpPost]
        [Route("PayPalIPN")]
        public async Task<IActionResult> PayPalIPN()
        {

            try
            {
                using (StreamReader reader = new StreamReader(Request.Body))
                {
                    string requestBody = await reader.ReadToEndAsync();
                    PayPalEventData eventData = JsonConvert.DeserializeObject<PayPalEventData>(requestBody);
                    Order order = await _orderRepository.GetOrderByID(eventData.resource.transactions[0].invoice_number);

                    PaymentResponse dto = new PaymentResponse
                    {
                        PaymentResponseId = eventData.resource.id,
                        OrderId = eventData.resource.transactions[0].invoice_number,
                        Amount = eventData.resource.transactions[0].amount.total,
                        OrderInfo = eventData.resource.transactions[0].description,
                        Success = true,
                        PaymentTypeId = Constant.PaymentType.PAYMENT_PAYPAL

                    };

                    if (eventData.resource.payer.status == "VERIFIED" && order.OrderStatusId == Constant.Order.PENDING_PAY)
                    {

                        await _paymentRepository.AddPaymentRespone(dto);

                        await _orderRepository.UpdateStatusForOrder(dto.OrderId, Constant.Order.SUCCESS_PAY);
                        await _orderRepository.UpdateQuantityAfterPay(eventData.resource.transactions[0].invoice_number);

                    }



                    return Ok();
                }
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }



        [HttpGet]
        [Route("VNPayIPN")]
        public async Task<IActionResult> VNPayIPN()
        {
            try
            {

                var response = new VNPAYResponseModel
                {
                    PaymentMethod = Request.Query["vnp_BankCode"],
                    OrderDescription = Request.Query["vnp_OrderInfo"],
                    OrderId = Request.Query["vnp_TxnRef"],
                    PaymentId = Request.Query["vnp_TransactionNo"],
                    TransactionId = Request.Query["vnp_TransactionNo"],
                    Token = Request.Query["vnp_SecureHash"],
                    VnPayResponseCode = Request.Query["vnp_ResponseCode"],
                    PayDate = Request.Query["vnp_PayDate"],
                    Amount = Request.Query["vnp_Amount"],
                    Success = true
                };


                Order order = await _orderRepository.GetOrderByID(response.OrderId);

                if (response.VnPayResponseCode == "00" && order.OrderStatusId == Constant.Order.PENDING_PAY)
                {
                    PaymentResponse dto = new PaymentResponse
                    {
                        PaymentResponseId = Convert.ToString(response.PaymentId),
                        OrderId = response.OrderId,
                        Amount = response.Amount.ToString(),
                        OrderInfo = response.OrderDescription,
                        Success = true,
                        PaymentTypeId = Constant.PaymentType.PAYMENT_VNPAY



                    };
                    await _paymentRepository.AddPaymentRespone(dto);

                    await _orderRepository.UpdateStatusForOrder(dto.OrderId, Constant.Order.SUCCESS_PAY);
                    await _orderRepository.UpdateQuantityAfterPay(dto.OrderId);

                }

                return Ok();
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);

            }


        }


    }
}
