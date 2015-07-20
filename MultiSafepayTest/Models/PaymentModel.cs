using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MultiSafepayTest.Models
{
    public class PaymentModel
    {
        public PaymentModel()
        {
            Currency = "EUR";
            ApiKey = "8286257f9d78e2dc0de06080e256e4233b807e62";
            ApiUrls = new List<string>();
            ApiUrls.Add("https://testapi.multisafepay.com/v1/json/");
            ApiUrls.Add("https://api.multisafepay.com/v1/json/");
            Errors = new List<string>();
            Locale = "nl";
            Country = "BE";
        }
        public string OrderId { get; set; }
        public string Description { get; set; }
        public int Amount { get; set; }
        public string Currency { get; set; }
        public string PaymentUrl { get; set; }
        public string ApiKey { get; set; }
        public string ApiUrl { get; set; }
        public List<string> ApiUrls { get; set; }
        public List<string> Errors { get; set; }
        public string NotificationUrl { get; set; }
        public string SuccessRedirectUrl { get; set; }
        public string CancelRedirectUrl { get; set; }
        public string Locale { get; set; }
        public string Country { get; set; }
    }
}