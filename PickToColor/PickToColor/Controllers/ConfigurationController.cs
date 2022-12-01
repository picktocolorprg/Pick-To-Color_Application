using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using PickToColor.BO;
using PickToColor.EntityFramework;
using PickToColor.Models;
using PickToColor.Utility;

namespace PickToColor.Controllers
{
    [Authorize]
    public class ConfigurationController : Controller
    {
        DataContext context;
        public ConfigurationController()
        {
            context = new DataContext();
        }

        // GET: Configuration
        public ActionResult Index()
        {
            List<CustomerModel> Customers = context.Customers.Where(a => a.IsActive).OrderByDescending(a => a.CreatedOn).ToList();
            return View(Customers);
        }

        [HttpPost]
        public ActionResult SaveConfiguration(HttpPostedFileBase SingleOrder)
        {
            if (SingleOrder != null)
            {
                SingleOrder.SaveAs(Server.MapPath("~/Images/SingleOrder.png"));
            }

            TempData["SC"] = true;
            return RedirectPermanent("Index");

        }

        [HttpPost]
        public ActionResult SaveOneScanSettings(List<OneScanSettings> CustomerList)
        {
            context.Customers.Where(a=>a.IsActive).ToList().ForEach(a =>
            {
                a.IsEnabledOneScanning = true;
                var val=CustomerList.First(setting => setting.CustomerID == a.CustomerID).ScanEnabled;
            });
            context.SaveChanges();
            return new EmptyResult();
        }

        [CustomException]
        [HttpPost]
        public ActionResult SaveNotificationSounds(HttpPostedFileBase SKUAudio, HttpPostedFileBase SerialNoAudio)
        {
            if(SKUAudio!= null)
            {
                SKUAudio.SaveAs(Server.MapPath("~/Audio/Resources/SKU.WAV"));
            }

            if (SerialNoAudio != null)
            {
                SerialNoAudio.SaveAs(Server.MapPath("~/Audio/Resources/SERIAL.WAV"));
            }
            TempData["NS"] = true;
            return RedirectPermanent("Index");
        }

        /// <summary>
        /// Delete Customer
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public ActionResult DeletePendingOrders()
        {
            var items=context.Orders.Where(a => !a.OrderCompletedStatus).ToList();
            foreach (var item in items)
            {
                context.Orders.Remove(item);
            }       
            context.SaveChanges();

            return (new EmptyResult());
        }

    }
}