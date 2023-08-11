using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PaymentGateway.Paypal
{
    public class PayPalResponeModel
    {
        public class RedirectUrls
        {
            public string return_url { get; set; }
            public string cancel_url { get; set; }
        }

        public class Link
        {
            public string href { get; set; }
            public string rel { get; set; }
            public string method { get; set; }
        }

        public class ShippingAddress
        {
            public string recipient_name { get; set; }
            public string line1 { get; set; }
            public string city { get; set; }
            public string state { get; set; }
            public string postal_code { get; set; }
            public string country_code { get; set; }
        }

        public class ItemList
        {
            public ShippingAddress shipping_address { get; set; }
        }

        public class Amount
        {
            public string total { get; set; }
            public string currency { get; set; }
        }

        public class Payee
        {
            public string merchant_id { get; set; }
            public string email { get; set; }
        }

        public class Transaction
        {
            public Amount amount { get; set; }
            public Payee payee { get; set; }
            public string description { get; set; }
            public string invoice_number { get; set; }
            public ItemList item_list { get; set; }
            public List<object> related_resources { get; set; }
        }

        public class PayerInfo
        {
            public string email { get; set; }
            public string first_name { get; set; }
            public string last_name { get; set; }
            public string payer_id { get; set; }
            public ShippingAddress shipping_address { get; set; }
            public string country_code { get; set; }
        }

        public class Payer
        {
            public string payment_method { get; set; }
            public string status { get; set; }
            public PayerInfo payer_info { get; set; }
        }

        public class Resource
        {
            public DateTime update_time { get; set; }
            public DateTime create_time { get; set; }
            public RedirectUrls redirect_urls { get; set; }
            public List<Link> links { get; set; }
            public string id { get; set; }
            public string state { get; set; }
            public List<Transaction> transactions { get; set; }
            public string intent { get; set; }
            public Payer payer { get; set; }
        }

        public class PayPalEventData
        {
            public string id { get; set; }
            public string event_version { get; set; }
            public DateTime create_time { get; set; }
            public string resource_type { get; set; }
            public string event_type { get; set; }
            public string summary { get; set; }
            public Resource resource { get; set; }
            public List<Link> links { get; set; }
        }
    }
}
