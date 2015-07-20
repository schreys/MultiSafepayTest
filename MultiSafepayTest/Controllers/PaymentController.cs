using MultiSafepayTest.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Web;
using System.Web.Mvc;

namespace MultiSafepayTest.Controllers
{
    public class PaymentController : Controller
    {
        //
        // GET: /Payment/
        public ActionResult Index()
        {
            PaymentModel pm = new PaymentModel();

            pm.SuccessRedirectUrl = Request.Url.ToString().Replace(Request.FilePath, Url.Content("~/Payment/success"));
            pm.NotificationUrl = Request.Url.ToString().Replace(Request.FilePath, Url.Content("~/Notification"));
            pm.CancelRedirectUrl = Request.Url.ToString().Replace(Request.FilePath, Url.Content("~/Payment/Cancel"));

            return View(pm);
        }

       

        [HttpPost]
        public ActionResult Post(PaymentModel pm)
        {
            var ordermodel = CreateRedirectOrder(pm);
            var c = new MultiSafepay.MultiSafepayClient(pm.ApiKey, apiUrl: pm.ApiUrl);

            try
            { 
                
                var order = c.CreateOrder(ordermodel);
                pm.PaymentUrl = order.PaymentUrl;
                
            }
            catch(Exception exc)
            {
                
                while (exc != null)
                {
                    pm.Errors.Add(exc.Message);
                    exc = exc.InnerException;
                }
            }
            
            return Json(pm);
        }

        private MultiSafepay.Model.OrderRequest CreateRedirectOrder(PaymentModel pm)
        {
            pm.Errors = new List<string>();

            MultiSafepay.Model.PaymentOptions paymentOptions = new MultiSafepay.Model.PaymentOptions();
            paymentOptions.CancelRedirectUrl = pm.CancelRedirectUrl;
            paymentOptions.NotificationUrl = pm.NotificationUrl;
            paymentOptions.SuccessRedirectUrl = pm.SuccessRedirectUrl;

            var ordermodel = MultiSafepay.Model.OrderRequest.CreateRedirect(pm.OrderId, pm.Description, pm.Amount, pm.Currency, paymentOptions);
            ordermodel.Customer = new MultiSafepay.Model.Customer() { Locale = pm.Locale, Country = pm.Country };

            return ordermodel;
        }

        [HttpPost]
        public ActionResult Post2(PaymentModel pm)
        {
            var ordermodel = CreateRedirectOrder(pm);

            var param = Newtonsoft.Json.JsonConvert.SerializeObject(ordermodel);
            HttpContent content = new StringContent(param, Encoding.UTF8, "application/json");
            content.Headers.Add("api_key", pm.ApiKey);

            HttpClient client = new HttpClient();
            
            var result = client.PostAsync(pm.ApiUrl + "orders", content).Result;
            string resultContent = result.Content.ReadAsStringAsync().Result;

            var message = Newtonsoft.Json.JsonConvert.DeserializeObject<MultiSafepay.ResponseMessage<MultiSafepay.Model.PaymentLink>>(resultContent);
            
            if (!message.Success)
                pm.Errors.Add(message.ErrorCode.ToString() + "-" + message.ErrorInfo);
            else
            {
                pm.PaymentUrl = message.Data.PaymentUrl;
            }

            return Json(pm);
        }

        public ActionResult Cancel()
        {
            return View();
        }

        public ActionResult Success()
        {
            return View();
        }
	}
}