using System.Net;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Monshop.BackEnd.Service.Payment.PaymentLibrary;
using Monshop.BackEnd.Service.Payment.PaymentRequest;
using Newtonsoft.Json.Linq;
using RestSharp;

namespace Monshop.BackEnd.Service.Payment.PaymentService;

public class PaymentGatewayService : IPaymentGatewayService
{
    private readonly IConfiguration _configuration;

    public PaymentGatewayService(IConfiguration configuration)
    {
        _configuration = configuration;
    }

    public async Task<string> CreatePaymentUrlMomo(PaymentInformationRequest requestDto)
    {
        var connection = "";

        var momo = new PaymentInformationRequest
        {
            AccountID = requestDto.AccountID,
            Amount = requestDto.Amount,
            CustomerName = requestDto.CustomerName,
            OrderID = requestDto.OrderID
        };


        var endpoint = "https://test-payment.momo.vn/v2/gateway/api/create";
        var partnerCode = "MOMO";
        var accessKey = _configuration["Momo:accessKey"];
        var secretkey = _configuration["Momo:secretkey"];
        var orderInfo = $"Khach hang: {momo.CustomerName} thanh toan hoa don {momo.OrderID}";
        var redirectUrl = $"{_configuration["Momo:RedirectUrl"]}/{momo.OrderID}";
        var ipnUrl = _configuration["Momo:IPNUrl"];
        //  string ipnUrl = "https://webhook.site/3399b42a-eee3-4e2d-8925-c2f893737de9";

        var requestType = "captureWallet";

        var amount = momo.Amount.ToString();
        var orderId = Guid.NewGuid().ToString();
        var requestId = Guid.NewGuid().ToString();
        var extraData = momo.OrderID;

        //Before sign HMAC SHA256 signature
        var rawHash = "accessKey=" + accessKey +
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

        var crypto = new MomoSecurity();
        //sign signature SHA256
        var signature = crypto.signSHA256(rawHash, secretkey);

        //build body json request
        var message = new JObject
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
        var response = client.Execute(request);
        if (response.StatusCode == HttpStatusCode.OK)
        {
            var jmessage = JObject.Parse(response.Content);
            connection = jmessage.GetValue("payUrl").ToString();
        }

        return connection;
    }

    public async Task<string> CreatePaymentUrlVnpay(PaymentInformationRequest requestDto, HttpContext httpContext)
    {
        var paymentUrl = "";
        var momo = new PaymentInformationRequest
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
        pay.AddRequestData("vnp_OrderInfo",
            $"Khach hang: {requestDto.CustomerName} thanh toan hoa don {requestDto.OrderID}");
        pay.AddRequestData("vnp_OrderType", "other");
        pay.AddRequestData("vnp_ReturnUrl", urlCallBack);
        pay.AddRequestData("vnp_TxnRef", requestDto.OrderID);
        paymentUrl = pay.CreateRequestUrl(_configuration["Vnpay:BaseUrl"], _configuration["Vnpay:HashSecret"]);


        return paymentUrl;
    }
}