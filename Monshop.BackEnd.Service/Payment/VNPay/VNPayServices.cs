using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using MonShop.BackEnd.Utility;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace Monshop.BackEnd.Service.Payment.VNPay
{
    public class VNPayServices : IVnPayServices
    {

        public string CreatePaymentUrl(PaymentInformationModel model, HttpContext context)
        {
            IConfiguration config = new ConfigurationBuilder()
                 .SetBasePath(Directory.GetCurrentDirectory())
                 .AddJsonFile("appsettings.json", true, true)
                 .Build();

            var timeZoneById = TimeZoneInfo.FindSystemTimeZoneById(config["TimeZoneId"]);
            var timeNow = TimeZoneInfo.ConvertTimeFromUtc(DateTime.UtcNow, timeZoneById);
            var tick = MonShop.BackEnd.Utility.Utils.Utility.GetInstance().GetCurrentDateTimeInTimeZone().Ticks.ToString();
            var pay = new VnPayLibrary();
            var urlCallBack = $"{config["Vnpay:ReturnUrl"]}/{model.OrderID}";

            pay.AddRequestData("vnp_Version", config["Vnpay:Version"]);
            pay.AddRequestData("vnp_Command", config["Vnpay:Command"]);
            pay.AddRequestData("vnp_TmnCode", config["Vnpay:TmnCode"]);
            pay.AddRequestData("vnp_Amount", ((int)model.Amount * 100).ToString());
            pay.AddRequestData("vnp_CreateDate", timeNow.ToString("yyyyMMddHHmmss"));

            pay.AddRequestData("vnp_CurrCode", config["Vnpay:CurrCode"]);
            pay.AddRequestData("vnp_IpAddr", pay.GetIpAddress(context));
            pay.AddRequestData("vnp_Locale", config["Vnpay:Locale"]);
            pay.AddRequestData("vnp_OrderInfo", $"Khach hang: {model.CustomerName} thanh toan hoa don {model.OrderID}");
            pay.AddRequestData("vnp_OrderType", "other");

            pay.AddRequestData("vnp_ReturnUrl", urlCallBack);
            pay.AddRequestData("vnp_TxnRef", model.OrderID.ToString());

            var paymentUrl = pay.CreateRequestUrl(config["Vnpay:BaseUrl"], config["Vnpay:HashSecret"]);

            return paymentUrl;
        }
        public VNPAYResponseModel PaymentExecute(IQueryCollection collections)
        {
            IConfiguration config = new ConfigurationBuilder()
                 .SetBasePath(Directory.GetCurrentDirectory())
                 .AddJsonFile("appsettings.json", true, true)
                 .Build();
            var pay = new VnPayLibrary();
            var response = pay.GetFullResponseData(collections, config["Vnpay:HashSecret"]);

            return response;
        }
    }
}
