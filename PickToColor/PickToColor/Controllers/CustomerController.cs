using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using PagedList;
using PickToColor.EntityFramework;
using PickToColor.Models;
using PickToColor.Utility;

namespace PickToColor.Controllers
{
    [Authorize]
    public class CustomerController : Controller
    {

        public DataContext context;
        public int PageSize;

        public CustomerController()
        {
            PageSize = int.Parse(ConfigurationManager.AppSettings["DefaultPageSize"]);
            context = new DataContext();
        }


        /// <summary>
        /// Landing View for "Manage Customers" Link
        /// </summary>
        /// <returns></returns>
        [AuthorizeManager]
        [HttpGet]
        public ActionResult Index()
        {
            return View();
        }

        [AuthorizeManager]
        [HttpPost]
        [CustomException]
        public ActionResult Create(CustomerModel NewCustomer)
        {

            if (NewCustomer != null)
            {
                if (context.Customers.Any(a => a.CustomerCode == NewCustomer.CustomerCode && a.IsActive))
                {
                    throw new Exception("The Customer Code " + NewCustomer.CustomerCode + " already exists for another customer. Please check and try again.");
                }
                NewCustomer.IsActive = true;
                NewCustomer.CreatedOn = DateTime.Now;
                NewCustomer.CreatedBy = HttpContext.User.Identity.Name;
            }
            context.Customers.Add(NewCustomer);
            context.SaveChanges();
            return new EmptyResult();
        }

        /// <summary>
        /// Get All Customers 
        /// </summary>
        /// <returns></returns>

        public PartialViewResult GetAllCustomers(int? pageno)
        {
            int pageNumber = (pageno ?? 1);
            PagedList.IPagedList<CustomerModel> Customers = null;
            Customers = context.Customers.Where(a => a.IsActive).OrderByDescending(a => a.CreatedOn).ToList().ToPagedList(pageNumber, PageSize);
            return PartialView(Customers);
        }
        /// <summary>
        /// Edit Customer Deatils
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        [CustomException]
        public ActionResult Edit(int CustomerID, string CustomerCode, string CustomerName, string Description)
        {
            if (context.Customers.Any(existingCustomer => existingCustomer.CustomerCode == CustomerCode && existingCustomer.CustomerID != CustomerID && existingCustomer.IsActive))
            {
                throw new Exception("The entered Customer code already exists for another customer. Please change it and try again.");
            }

            CustomerModel EditCusDetail = context.Customers.Single(a => a.CustomerID == CustomerID);
            EditCusDetail.CustomerCode = CustomerCode;
            EditCusDetail.CustomerName = CustomerName;
            EditCusDetail.Description = Description;
            context.SaveChanges();
            return (new EmptyResult());
        }
        /// <summary>
        /// Delete Customer
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public ActionResult Delete(List<string> CustomerIDs)
        {
            context.Customers.Where(a => CustomerIDs.Contains(a.CustomerID.ToString())).ToList().ForEach(a =>
            {
                a.IsActive = false;
            });
            context.SaveChanges();
            return (new EmptyResult());
        }
    }
}