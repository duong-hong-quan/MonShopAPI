using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Memory;
using MonShopLibrary.Models;
using MonShopLibrary.Repository;
using MonShopLibrary.Utils;
using Newtonsoft.Json.Linq;
using PaymentGateway.Momo;
using PaymentGateway.Paypal;
using PaymentGateway.VNPay;
using VNPay.Models;
using VNPay.Services;

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
        public async Task<IActionResult> GetPaymentURLMomo(int OrderID)
        {
            Order order = await _orderRepository.GetOrderByID(OrderID);

         
            Account account = await _accountRepository.GetAccountByID(order.BuyerAccountId);
            Momo momo = null;
            if (order != null && account !=null)
            {
                 momo = new Momo { AccountID = order.BuyerAccountId, Amount = (double)order.Total, CustomerName = account.FullName, OrderID = OrderID};
              
                string endpoint = _momoServices.CreatePaymentString(momo);
                return Content(endpoint);
            }
            return BadRequest($"Not found Order with ID :{OrderID} OR Account with ID:{order.BuyerAccountId}");

        }
        [HttpPost]
        [Route("GetPaymentURLVNPay")]
        public async Task<IActionResult> GetPaymentURLVNPay(int OrderID)
        {
            Order order = await _orderRepository.GetOrderByID(OrderID);
            Account account = await _accountRepository.GetAccountByID(order.BuyerAccountId);
            PaymentInformationModel model = null;
            if (order != null && account != null)
            {
                model = new PaymentInformationModel { AccountID = order.BuyerAccountId, Amount = (double)order.Total, CustomerName = account.FullName, OrderID = order.OrderId };

                string endpoint = _vnPayServices.CreatePaymentUrl(model, HttpContext);

                return Content(endpoint);
            }
            return BadRequest($"Not found Order with ID :{OrderID} OR Account with ID:{order.BuyerAccountId}");

        }

        [HttpPost]
        [Route("GetPaymentURLPayPal")]
        public async Task<IActionResult> GetPaymentURLPayPal(int OrderID)
        {
            Order order = await _orderRepository.GetOrderByID(OrderID);
            Account account = await _accountRepository.GetAccountByID(order.BuyerAccountId);
            PaymentInformationModel model = null;
            if (order != null && account != null)
            {
                model = new PaymentInformationModel { AccountID = order.BuyerAccountId, Amount = (double)order.Total, CustomerName = account.FullName, OrderID = order.OrderId };

                string endpoint = await _payPalServices.CreatePaymentUrl(model, HttpContext);

                return Content(endpoint);
            }
            return BadRequest($"Not found Order with ID :{OrderID} OR Account with ID:{order.BuyerAccountId}");

        }
     



        [HttpPost]
        [Route("MomoIPN")]
        public async Task MomoIPN(MomoResponeModel momo)
        {
            
            Order order = await _orderRepository.GetOrderByID(int.Parse(momo.orderId));

            if (momo.resultCode == 0)
            {
                if (order.OrderStatusId == Constant.Order.PENDING_PAY)
                {
                    MomoPaymentResponse dto = new MomoPaymentResponse
                    {
                        PaymentResponseId = (long)momo.transId,
                        OrderId = int.Parse(momo.extraData),
                        Amount = momo.amount.ToString(),
                        OrderInfo = momo.orderInfo,
                        Success = true
                    };
                    await _orderRepository.UpdateStatusForOrder(int.Parse(momo.extraData), Constant.Order.SUCCESS_PAY);
                    await _paymentRepository.AddPaymentMomo(dto);
                }
                else
                {
                    return;
                }
                  
            }
            else
            {
                await _orderRepository.UpdateStatusForOrder(int.Parse(momo.orderId), Constant.Order.FAILURE_PAY);

            }
        }

        [HttpGet]
        [Route("PayPalIPN")]
        public async Task<IActionResult> PayPalIPN()
        {
            IConfiguration config = new ConfigurationBuilder()
           .SetBasePath(Directory.GetCurrentDirectory())
           .AddJsonFile("appsettings.json", true, true)
           .Build();
            string returnURL = config["Paypal:RedirectUrl"];

            PayPalPaymentResponse dto = null;
            var response = _payPalServices.PaymentExecute(Request.Query);
            Order order = await _orderRepository.GetOrderByID(int.Parse(response.OrderId));

            if (response.Success)
            {
                if (order.OrderStatusId == Constant.Order.PENDING_PAY)
                {
                    dto = new PayPalPaymentResponse
                    {
                        PaymentResponseId = response.PaymentId,
                        OrderId = int.Parse(response.OrderId),
                        Amount = response.Amount,
                        OrderInfo = response.OrderDescription,
                        Success = response.Success
                    };
                    await _orderRepository.UpdateStatusForOrder(dto.OrderId, Constant.Order.SUCCESS_PAY);
                    await _paymentRepository.AddPaymentPaypal(dto);
                    return Redirect(returnURL);

                }

                else
                {
                    return Redirect(returnURL);
                }
            }
            else
            {
                await _orderRepository.UpdateStatusForOrder(int.Parse(response.OrderId), Constant.Order.FAILURE_PAY);
            return    Redirect(returnURL);


            }



        }

        [HttpGet]
        [Route("VNPayIPN")]
        public async Task VNPayIPN()
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
            VnpayPaymentResponse dto = null;
            Order order = await _orderRepository.GetOrderByID(int.Parse(response.OrderId));

            if (response.VnPayResponseCode == "00" )
            {
                if (order.OrderStatusId == Constant.Order.PENDING_PAY)
                {
                    dto = new VnpayPaymentResponse
                    {
                        PaymentResponseId = Convert.ToInt32(response.PaymentId),
                        OrderId = int.Parse(response.OrderId),
                        Amount = response.Amount.ToString(),
                        OrderInfo = response.OrderDescription,
                        Success = true

                    };
                    await _orderRepository.UpdateStatusForOrder(dto.OrderId, Constant.Order.SUCCESS_PAY);
                    await _paymentRepository.AddPaymentVNPay(dto);
                }
                else
                {
                    return;
                }
               
            }
            else
            {
                await _orderRepository.UpdateStatusForOrder(int.Parse(response.OrderId), Constant.Order.FAILURE_PAY);

            }

        }

    }
}
