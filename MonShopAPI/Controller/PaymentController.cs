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
        public async Task<IActionResult> GetPaymentURLMomo(int OrderID)
        {
            Order order = await _orderRepository.GetOrderByID(OrderID);
            Account account = await _accountRepository.GetAccountByID(order.BuyerAccountId);
            Momo momo = null;
            if (order != null && account !=null)
            {
                 momo = new Momo { AccountID = order.BuyerAccountId, Amount = (double)order.Total, CustomerName = account.FullName};
              
                string endpoint = _momoServices.CreatePaymentString(momo);

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
                MomoPaymentResponse dto = new MomoPaymentResponse 
                {
                    PaymentResponseId =(int) momo.transId,
                    OrderId = int.Parse( momo.orderId),
                    Amount = momo.amount.ToString(), 
                    OrderInfo = momo.orderInfo 
                };
                await _orderRepository.UpdateStatusForOrder(int.Parse(momo.orderId), Constant.Order.SUCCESS_PAY);
                await _paymentRepository.AddPaymentMomo(dto);
            }
            else
            {
                await _orderRepository.UpdateStatusForOrder(int.Parse(momo.orderId), Constant.Order.FAILURE_PAY);

            }


        }
    }
}
