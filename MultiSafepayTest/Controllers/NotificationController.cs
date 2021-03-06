﻿using MultiSafepayTest.Business.Domain;
using MultiSafepayTest.Business.Facade;
using MultiSafepayTest.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace MultiSafepayTest.Controllers
{
    public class NotificationController : Controller
    {
        //
        // GET: /Notification/
        public ActionResult List()
        {
            var logFileFolder = Server.MapPath(string.Format("~/"));
            var files = Directory.GetFiles(logFileFolder, "*.log");
            List<string> lines = new List<string>();

            if(files.Count()>0)
            {
                //order by name
                var orderedFiles = files.OrderBy(f => f);
                //get the last, which is the most recent
                string last = orderedFiles.Last();
           
                StreamReader sr = new StreamReader(last);

                
                string line;
                while ((line = sr.ReadLine()) != null)
                {
                    lines.Add(line);
                }

                sr.Close();
                sr.Dispose();
            }

            return View(lines);
        }


        /// <summary>
        /// Log all incoming text in a text file
        /// </summary>
        public string Index()
        { 

            var path = Server.MapPath(string.Format("~/notification{0}.log", DateTime.Now.ToString("yyyy-MM-dd")));

            //check post values
            string notification = new StreamReader(Request.InputStream).ReadToEnd();

            //check get values
            string query = Request.Url.Query;

            string line = string.Format("[{0}] POST: {1} GET: {2}", DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"), notification, query);
            
            if (System.IO.File.Exists(path))
            {
                string oldText = System.IO.File.ReadAllText(path);
                using (var sw = new StreamWriter(path, false))
                {
                    sw.WriteLine(line);
                    sw.Write(oldText);
                }
            }
            else System.IO.File.WriteAllText(path, line);


            //now stars the order processing
            //step 1: get the order from the MultiSafepay client
            var transactionid = Request["transactionid"];
            var pm = new PaymentModel();
            var c = new MultiSafepay.MultiSafepayClient(pm.ApiKey, apiUrl: pm.ApiUrls[0]);
            bool ordercompleted = false;
           
            var order = c.GetOrder(transactionid);
            if(order != null)
                ordercompleted = order.Status == "completed";
           

            if(ordercompleted)
            {
                //get our order
                var facade = FacadeFactory.GetInstance().GetFacade<MSPFacade>();
                var orderdb = facade.GetById<MSPOrder>(transactionid);

                if (orderdb != null)
                {
                    orderdb.Status = MSPOrderStatus.Completed;
                    facade.Save();
                }

            }
            return "OK";
        }
	}
}