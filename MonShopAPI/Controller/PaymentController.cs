using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using MonShopLibrary.Models;
using MonShopLibrary.Repository;
using MonShopLibrary.Utils;
using Newtonsoft.Json.Linq;
using PaymentGateway.Momo;

namespace MonShopAPI.Controller
{
    [Route("Payment")]
    [ApiController]
    public class PaymentController : ControllerBase
    {
        private readonly IMomoServices _momoServices;
        private readonly IOrderRepository _orderRepository;
        private readonly IAccountRepository _accountRepository;
        private readonly IPaymentRepository _paymentRepository;

        public PaymentController()
        {
            _momoServices = new MomoServices();
            _orderRepository = new OrderRepository();
            _accountRepository = new AccountRepository();
            _paymentRepository = new PaymentRepository();
        }

        [HttpPost]
        [Route("Momo")]
        public async Task<IActionResult> GetPaymentURLMomo(Momo momo)
        {
            Order order = await _orderRepository.GetOrderByID(momo.OrderID);
            Account account = await _accountRepository.GetAccountByID(momo.AccountID);
            if (order != null && account !=null)
            {
                momo.CustomerName = account.FullName;
                momo.Amount = (double)order.Total;
                string endpoint = _momoServices.CreatePaymentString(momo);

                return Content(endpoint);
            }
            return BadRequest($"Not found Order with ID :{momo.OrderID} OR Account with ID:{momo.AccountID}");

        }
        [HttpPost]
        [Route("MomoIPN")]
        public async Task MomoIPN(MomoResponeModel momo)
        {
            Order order = await _orderRepository.GetOrderByID(Convert.ToInt32(momo.orderId));

            if (momo.resultCode == 0)
            {
                MomoPaymentResponse dto = new MomoPaymentResponse 
                {
                    PaymentResponseId = Convert.ToInt32(momo.transId) ,
                    OrderId = Convert.ToInt32(momo.orderId),
                    Amount = momo.amount.ToString(), 
                    OrderInfo = momo.orderInfo 
                };
                await _orderRepository.UpdateStatusForOrder(Convert.ToInt32(momo.orderId), Constant.Order.SUCCESS_PAY);
                await _paymentRepository.AddPaymentMomo(dto);
            }
            else
            {
                await _orderRepository.UpdateStatusForOrder(Convert.ToInt32(momo.orderId), Constant.Order.FAILURE_PAY);

            }


        }
    }
}
