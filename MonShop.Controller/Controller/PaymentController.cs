using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Memory;
using MonShop.Controller.Model;
using MonShop.Library.Models;
using MonShopLibrary.Repository;
using MonShopLibrary.Utils;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using PaymentGateway.Momo;
using PaymentGateway.Paypal;
using PaymentGateway.VNPay;
using VNPay.Models;
using VNPay.Services;
using static PaymentGateway.Paypal.PayPalResponeModel;

namespace MonShopAPI.Controller
{
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
        private readonly ResponeDTO _responeDTO;
        public PaymentController(IMomoServices momoServices, IVnPayServices vnPayServices, IPayPalServices payPalServices, IOrderRepository orderRepository, IAccountRepository accountRepository, IPaymentRepository paymentRepository)
        {
            _momoServices = momoServices;
            _payPalServices = payPalServices;
            _orderRepository = orderRepository;
            _accountRepository = accountRepository;
            _paymentRepository = paymentRepository;
            _vnPayServices = vnPayServices;
            _responeDTO = new ResponeDTO();

        }



        [HttpGet]
        [Route("GellAllPaymentMomo")]
        public async Task<ResponeDTO> GetAllPaymentMomo()
        {
            try
            {
                var list = await _paymentRepository.GetAllPaymentMomo();
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
        [Route("GellAllPaymentVNPay")]
        public async Task<ResponeDTO> GellAllPaymentVNPay()
        {
            try
            {
                var list = await _paymentRepository.GetAllPaymenVNPay();
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
        [Route("GellAllPaymentPayPal")]
        public async Task<ResponeDTO> GellAllPaymentPayPal()
        {
            try
            {

                var list = await _paymentRepository.GetAllPaymentPayPal();
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
        [Route("GetPaymentURLMomo")]
        public async Task<ResponeDTO> GetPaymentURLMomo(string OrderID)
        {
            try
            {
                Order order = await _orderRepository.GetOrderByID(OrderID);
                Account account = await _accountRepository.GetAccountByID(order.BuyerAccountId);
                if (order == null)
                {
                    _responeDTO.Message = $"No result order with ID {OrderID}";
                }

                if (order?.OrderStatusId != Constant.Order.PENDING_PAY)
                {
                    _responeDTO.Message = $"No result for order {OrderID} with status pending";
                }

                if (order?.OrderStatusId == Constant.Order.PENDING_PAY)
                {
                    Momo momo = new Momo
                    {
                        AccountID = order.BuyerAccountId,
                        Amount = (double)order.Total,
                        CustomerName = $"{account.FirstName} {account.LastName}",
                        OrderID = OrderID
                    };

                    string endpoint = _momoServices.CreatePaymentString(momo);
                    _responeDTO.Data = endpoint;
                }

            }
            catch (Exception ex)
            {
                _responeDTO.IsSuccess = false;
                _responeDTO.Message = ex.Message;

            }
            return _responeDTO;


        }
        [HttpPost]
        [Route("GetPaymentURLVNPay")]
        public async Task<ResponeDTO> GetPaymentURLVNPay(string OrderID)
        {
            try
            {
                Order order = await _orderRepository.GetOrderByID(OrderID);
                Account account = await _accountRepository.GetAccountByID(order.BuyerAccountId);
                if (order == null)
                {
                    _responeDTO.Message = $"No result order with ID {OrderID}";
                }

                if (order?.OrderStatusId != Constant.Order.PENDING_PAY)
                {
                    _responeDTO.Message = $"No result for order {OrderID} with status pending";
                }

                if (order?.OrderStatusId == Constant.Order.PENDING_PAY)
                {
                    PaymentInformationModel model = new PaymentInformationModel
                    {
                        AccountID = order.BuyerAccountId,
                        Amount = (double)order.Total,
                        CustomerName = $"{account.FirstName} {account.LastName}",
                        OrderID = order.OrderId
                    };

                    string endpoint = _vnPayServices.CreatePaymentUrl(model, HttpContext);
                    _responeDTO.Data = endpoint;
                }

            }
            catch (Exception ex)
            {
                _responeDTO.IsSuccess = false;
                _responeDTO.Message = ex.Message;

            }
            return _responeDTO;

        }

        [HttpPost]
        [Route("GetPaymentURLPayPal")]
        public async Task<ResponeDTO> GetPaymentURLPayPal(string OrderID)
        {
            try
            {
                Order order = await _orderRepository.GetOrderByID(OrderID);
                Account account = await _accountRepository.GetAccountByID(order.BuyerAccountId);
                if (order == null)
                {
                    _responeDTO.Message = $"No result order with ID {OrderID}";
                }

                if (order.OrderStatusId != Constant.Order.PENDING_PAY)
                {
                    _responeDTO.Message = $"No result for order {OrderID} with status pending";
                }

                if (order.OrderStatusId == Constant.Order.PENDING_PAY)
                {
                    PaymentInformationModel model = new PaymentInformationModel 
                    {
                        AccountID = order.BuyerAccountId,
                        Amount = (double)order.Total, 
                        CustomerName = $"{account.FirstName} {account.LastName}", 
                        OrderID = order.OrderId 
                    };

                    string endpoint = await _payPalServices.CreatePaymentUrl(model, HttpContext);
                    _responeDTO.Data = endpoint;
                }

            }
            catch (Exception ex)
            {
                _responeDTO.IsSuccess = false;
                _responeDTO.Message = ex.Message;

            }

            return _responeDTO;

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
                    MomoPaymentResponse dto = new MomoPaymentResponse
                    {
                        PaymentResponseId = (long)momo.transId,
                        OrderId = momo.extraData,
                        Amount = momo.amount.ToString(),
                        OrderInfo = momo.orderInfo,
                        Success = true
                    };
                    await _paymentRepository.AddPaymentMomo(dto);

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

                    PayPalPaymentResponse dto = new PayPalPaymentResponse
                    {
                        PaymentResponseId = eventData.resource.id,
                        OrderId = eventData.resource.transactions[0].invoice_number,
                        Amount = eventData.resource.transactions[0].amount.total,
                        OrderInfo = eventData.resource.transactions[0].description,
                        Success = true
                    };

                    if (eventData.resource.payer.status == "VERIFIED" && order.OrderStatusId == Constant.Order.PENDING_PAY)
                    {

                        await _paymentRepository.AddPaymentPaypal(dto);

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

                var response = new PaymentResponseModel
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
                    VnpayPaymentResponse dto = new VnpayPaymentResponse
                    {
                        PaymentResponseId = Convert.ToInt32(response.PaymentId),
                        OrderId = response.OrderId,
                        Amount = response.Amount.ToString(),
                        OrderInfo = response.OrderDescription,
                        Success = true

                    };
                    await _paymentRepository.AddPaymentVNPay(dto);

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
