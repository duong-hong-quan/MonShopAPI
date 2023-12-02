using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Monshop.BackEnd.Service.Payment.PaymentLibrary;
using Monshop.BackEnd.Service.Payment.PaymentRequest;
using MonShop.BackEnd.DAL.Contracts;
using Newtonsoft.Json.Linq;
using RestSharp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace Monshop.BackEnd.Service.Payment.PaymentService
{
    public class PaymentGatewayService : IPaymentGatewayService
    {
        private IConfiguration _configuration;

        public PaymentGatewayService(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public async Task<string> CreatePaymentUrlMomo(PaymentInformationRequest requestDto)
        {
            string connection = "";

                PaymentInformationRequest momo = new PaymentInformationRequest
                {
                    AccountID = requestDto.AccountID,
                    Amount = requestDto.Amount,
                    CustomerName = requestDto.CustomerName,
                    OrderID = requestDto.OrderID
                };


                string endpoint = "https://test-payment.momo.vn/v2/gateway/api/create";
                string partnerCode = "MOMO";
                string accessKey = _configuration["Momo:accessKey"];
                string secretkey = _configuration["Momo:secretkey"];
                string orderInfo = $"Khach hang: {momo.CustomerName} thanh toan hoa don {momo.OrderID}";
                string redirectUrl = $"{_configuration["Momo:RedirectUrl"]}/{momo.OrderID}";
                string ipnUrl = _configuration["Momo:IPNUrl"];
                //  string ipnUrl = "https://webhook.site/3399b42a-eee3-4e2d-8925-c2f893737de9";

                string requestType = "captureWallet";

                string amount = momo.Amount.ToString();
                string orderId = Guid.NewGuid().ToString();
                string requestId = Guid.NewGuid().ToString();
                string extraData = momo.OrderID.ToString();

                //Before sign HMAC SHA256 signature
                string rawHash = "accessKey=" + accessKey +
                    "&amount=" + amount +
                    "&extraData=" + extraData +
                    "&ipnUrl=" + ipnUrl +
                    "&orderId=" + orderId +
                    "&orderInfo=" + orderInfo +
                    "&partnerCode=" + partnerCode +
                    "&redirectUrl=" + redirectUrl +
                    "&requestId=" + requestId +
                    "&requestType=" + requestType
                    ;

                MomoSecurity crypto = new MomoSecurity();
                //sign signature SHA256
                string signature = crypto.signSHA256(rawHash, secretkey);

                //build body json request
                JObject message = new JObject
            {
                { "partnerCode", partnerCode },
                { "partnerName", "Test" },
                { "storeId", "MomoTestStore" },
                { "requestId", requestId },
                { "amount", amount },
                { "orderId", orderId },
                { "orderInfo", orderInfo },
                { "redirectUrl", redirectUrl },
                { "ipnUrl", ipnUrl },
                { "lang", "en" },
                { "extraData", extraData },
                { "requestType", requestType },
                { "signature", signature }
                };

                var client = new RestClient();

                var request = new RestRequest(endpoint, Method.Post);
                request.AddJsonBody(message.ToString());
                RestResponse response = client.Execute(request);
                if (response.StatusCode == HttpStatusCode.OK)
                {
                    JObject jmessage = JObject.Parse(response.Content);
                    connection = jmessage.GetValue("payUrl").ToString();
                }
            
            return connection;
        }

        public async Task<string> CreatePaymentUrlVnpay(PaymentInformationRequest requestDto, HttpContext httpContext)
        {
            var paymentUrl = "";
            PaymentInformationRequest momo = new PaymentInformationRequest
            {
                AccountID = requestDto.AccountID,
                Amount = requestDto.Amount,
                CustomerName = requestDto.CustomerName,
                OrderID = requestDto.OrderID
            };
            var timeZoneById = TimeZoneInfo.FindSystemTimeZoneById(_configuration["TimeZoneId"]);
                var timeNow = TimeZoneInfo.ConvertTimeFromUtc(DateTime.UtcNow, timeZoneById);
                var pay = new VNPayLibrary();
                var urlCallBack = $"{_configuration["Vnpay:ReturnUrl"]}/{requestDto.OrderID}";

                pay.AddRequestData("vnp_Version", _configuration["Vnpay:Version"]);
                pay.AddRequestData("vnp_Command", _configuration["Vnpay:Command"]);
                pay.AddRequestData("vnp_TmnCode", _configuration["Vnpay:TmnCode"]);
                pay.AddRequestData("vnp_Amount", ((int)requestDto.Amount * 100).ToString());
                pay.AddRequestData("vnp_CreateDate", timeNow.ToString("yyyyMMddHHmmss"));
                pay.AddRequestData("vnp_CurrCode", _configuration["Vnpay:CurrCode"]);
                pay.AddRequestData("vnp_IpAddr", pay.GetIpAddress(httpContext));
                pay.AddRequestData("vnp_Locale", _configuration["Vnpay:Locale"]);
                pay.AddRequestData("vnp_OrderInfo", $"Khach hang: {requestDto.CustomerName} thanh toan hoa don {requestDto.OrderID}");
                pay.AddRequestData("vnp_OrderType", "other");
                pay.AddRequestData("vnp_ReturnUrl", urlCallBack);
                pay.AddRequestData("vnp_TxnRef", requestDto.OrderID.ToString());
                paymentUrl = pay.CreateRequestUrl(_configuration["Vnpay:BaseUrl"], _configuration["Vnpay:HashSecret"]);
            

            return paymentUrl;
        }
    }
}
