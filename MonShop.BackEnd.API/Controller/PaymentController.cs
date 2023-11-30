using Microsoft.AspNetCore.Mvc;
using Monshop.BackEnd.Service.Payment.Momo;
using MonShop.BackEnd.DAL.DTO.Response;

namespace MonShop.BackEnd.API.Controller
{

    [Route("Payment")]
    [ApiController]
    public class PaymentController : ControllerBase
    {
        
      




        [HttpGet]
        [Route("GellAllPayment")]
        public async Task<AppActionResult> GellAllPayment()
        {
           throw new NotImplementedException();
        }

        [HttpPost]
        [Route("GetPaymentURLMomo/{OrderID}")]
        public async Task<AppActionResult> GetPaymentURLMomo(string OrderID)
        {

            throw new NotImplementedException();


        }
        [HttpPost]
        [Route("GetPaymentURLVNPay/{OrderID}")]
        public async Task<AppActionResult> GetPaymentURLVNPay(string OrderID)
        {

            throw new NotImplementedException();

        }

        [HttpPost]
        [Route("GetPaymentURLPayPal/{OrderID}")]
        public async Task<AppActionResult> GetPaymentURLPayPal(string OrderID)
        {
           throw new NotImplementedException();


        }

        [HttpPost]
        [Route("MomoIPN")]
        public async Task<IActionResult> MomoIPN(MomoResponeModel momo)
        {

            throw new NotImplementedException();

        }



        [HttpPost]
        [Route("PayPalIPN")]
        public async Task<IActionResult> PayPalIPN()
        {

           throw new NotImplementedException();

        }



        [HttpGet]
        [Route("VNPayIPN")]
        public async Task<IActionResult> VNPayIPN()
        {

            throw new NotImplementedException();


        }


    }
}
