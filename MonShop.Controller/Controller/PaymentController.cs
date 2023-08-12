using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Memory;
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

        public PaymentController()
        {
            _momoServices = new MomoServices();
            _payPalServices = new PayPalServices();
            _orderRepository = new OrderRepository();
            _accountRepository = new AccountRepository();
            _paymentRepository = new PaymentRepository();
            _vnPayServices = new VNPayServices();

        }



        [HttpGet]
        [Route("GellAllPaymentMomo")]
        public async Task<IActionResult> GetAllPaymentMomo()
        {
            var list = await _paymentRepository.GetAllPaymentMomo();
            return Ok(list);
        }

        [HttpGet]
        [Route("GellAllPaymentVNPay")]
        public async Task<IActionResult> GellAllPaymentVNPay()
        {
            var list = await _paymentRepository.GetAllPaymenVNPay();
            return Ok(list);
        }
        [HttpGet]
        [Route("GellAllPaymentPayPal")]
        public async Task<IActionResult> GellAllPaymentPayPal()
        {
            var list = await _paymentRepository.GetAllPaymentPayPal();
            return Ok(list);
        }

        [HttpPost]
        [Route("GetPaymentURLMomo")]
        public async Task<IActionResult> GetPaymentURLMomo(string OrderID)
        {
            Order order = await _orderRepository.GetOrderByID(OrderID);
            Account account = await _accountRepository.GetAccountByID(order.BuyerAccountId);
            if (order == null)
            {
                return BadRequest($"No result order with ID {OrderID}");
            }

            if (order.OrderStatusId != Constant.Order.PENDING_PAY)
            {
                return BadRequest($"No result for order {OrderID} with status pending");
            }

            if (order.OrderStatusId == Constant.Order.PENDING_PAY)
            {
                Momo momo = new Momo
                {
                    AccountID = order.BuyerAccountId,
                    Amount = (double)order.Total,
                    CustomerName = $"{account.FirstName} {account.LastName}",
                    OrderID = OrderID
                };

                string endpoint = _momoServices.CreatePaymentString(momo);
                return Content(endpoint);
            }
            return BadRequest($"Not found Order with ID :{OrderID} OR Account with ID:{order.BuyerAccountId}");

        }
        [HttpPost]
        [Route("GetPaymentURLVNPay")]
        public async Task<IActionResult> GetPaymentURLVNPay(string OrderID)
        {
            Order order = await _orderRepository.GetOrderByID(OrderID);
            Account account = await _accountRepository.GetAccountByID(order.BuyerAccountId);
            if (order == null)
            {
                return BadRequest($"No result order with ID {OrderID}");
            }

            if (order.OrderStatusId != Constant.Order.PENDING_PAY)
            {
                return BadRequest($"No result for order {OrderID} with status pending");
            }

            if (order.OrderStatusId == Constant.Order.PENDING_PAY)
            {
                PaymentInformationModel model = new PaymentInformationModel { AccountID = order.BuyerAccountId, Amount = (double)order.Total, CustomerName = $"{account.FirstName} {account.LastName}", OrderID = order.OrderId };

                string endpoint = _vnPayServices.CreatePaymentUrl(model, HttpContext);

                return Content(endpoint);
            }
            return BadRequest($"Not found Order with ID :{OrderID} OR Account with ID:{order.BuyerAccountId}");

        }

        [HttpPost]
        [Route("GetPaymentURLPayPal")]
        public async Task<IActionResult> GetPaymentURLPayPal(string OrderID)
        {
            Order order = await _orderRepository.GetOrderByID(OrderID);
            Account account = await _accountRepository.GetAccountByID(order.BuyerAccountId);
            if (order == null)
            {
                return BadRequest($"No result order with ID {OrderID}");
            }

            if (order.OrderStatusId != Constant.Order.PENDING_PAY)
            {
                return BadRequest($"No result for order {OrderID} with status pending");
            }

            if (order.OrderStatusId == Constant.Order.PENDING_PAY)
            {
                PaymentInformationModel model = new PaymentInformationModel { AccountID = order.BuyerAccountId, Amount = (double)order.Total, CustomerName = $"{account.FirstName} {account.LastName}", OrderID = order.OrderId };

                string endpoint = await _payPalServices.CreatePaymentUrl(model, HttpContext);

                return Content(endpoint);
            }
            return BadRequest($"Not found Order with ID :{OrderID} OR Account with ID:{order.BuyerAccountId}");

        }

        [HttpPost]
        [Route("MomoIPN")]
        public async Task<IActionResult> MomoIPN(MomoResponeModel momo)
        {
            try {
                Order order = await _orderRepository.GetOrderByID(momo.extraData);

                MomoPaymentResponse dto = new MomoPaymentResponse
                {
                    PaymentResponseId = (long)momo.transId,
                    OrderId = momo.extraData,
                    Amount = momo.amount.ToString(),
                    OrderInfo = momo.orderInfo,
                    Success = true
                };
                await _paymentRepository.AddPaymentMomo(dto);

                if (momo.resultCode == 0 && order.OrderStatusId == Constant.Order.PENDING_PAY)
                {

                    await _orderRepository.UpdateStatusForOrder(momo.extraData, Constant.Order.SUCCESS_PAY);
                    await _orderRepository.UpdateQuantityAfterPay(momo.extraData);

                }
                else
                {
                    await _paymentRepository.UpdateStatusPaymentMomo(int.Parse(momo.extraData), false);
                    await _orderRepository.UpdateStatusForOrder(momo.orderId, Constant.Order.PENDING_PAY);

                }

                return Ok();
            }
            catch(Exception ex) {
            
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
                    await _paymentRepository.AddPaymentPaypal(dto);

                    if (eventData.resource.payer.status == "VERIFIED" && order.OrderStatusId == Constant.Order.PENDING_PAY)
                    {


                        await _orderRepository.UpdateStatusForOrder(dto.OrderId, Constant.Order.SUCCESS_PAY);
                        await _orderRepository.UpdateQuantityAfterPay(eventData.resource.transactions[0].invoice_number);

                    }
                    else
                    {
                        await _paymentRepository.UpdateStatusPaymentPayPal(dto.OrderId.ToString(), false);
                        await _orderRepository.UpdateStatusForOrder(dto.OrderId, Constant.Order.PENDING_PAY);

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

                VnpayPaymentResponse dto = new VnpayPaymentResponse
                {
                    PaymentResponseId = Convert.ToInt32(response.PaymentId),
                    OrderId = response.OrderId,
                    Amount = response.Amount.ToString(),
                    OrderInfo = response.OrderDescription,
                    Success = true

                }; ;
                Order order = await _orderRepository.GetOrderByID(response.OrderId);
                await _paymentRepository.AddPaymentVNPay(dto);

                if (response.VnPayResponseCode == "00" && order.OrderStatusId == Constant.Order.PENDING_PAY)
                {
                    await _orderRepository.UpdateStatusForOrder(dto.OrderId, Constant.Order.SUCCESS_PAY);
                    await _orderRepository.UpdateQuantityAfterPay(dto.OrderId);

                }
                else
                {
                    await _orderRepository.UpdateStatusForOrder(response.OrderId, Constant.Order.PENDING_PAY);
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
