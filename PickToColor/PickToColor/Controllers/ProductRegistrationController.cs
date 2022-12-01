using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using OfficeOpenXml;
using PagedList;
using PickToColor.BO;
using PickToColor.EntityFramework;
using PickToColor.Models;
using PickToColor.Utility;

namespace PickToColor.Controllers
{
    [Authorize]
    [AuthorizeManager]
    public class ProductRegistrationController : Controller
    {
        public DataContext context;
        public int PageSize;

        public ProductRegistrationController()
        {
            context = new DataContext();
            PageSize = int.Parse(ConfigurationManager.AppSettings["DefaultPageSize"]);
        }

        [Authorize]
        [AuthorizeManager]
        public ActionResult Index()
        {
            return View();
        }

        public PartialViewResult GetAllProducts(ProductRegistrationSearch searchCriteria, int? pageno)
        {
            int pageNumber = (pageno ?? 1);
            PagedList.IPagedList<ProductModel> Products = null;

            if (!searchCriteria.CustomerID.HasValue)
                Products = (new List<ProductModel>()).ToPagedList<ProductModel>(pageNumber, PageSize);
            else
                Products = context.Products.Where(product => (product.CustomerID == (searchCriteria.CustomerID.HasValue ? searchCriteria.CustomerID.Value : 0)) &&
                                                 (string.IsNullOrEmpty(searchCriteria.SKU) || product.SKU.Contains(searchCriteria.SKU)) &&
                                                 (string.IsNullOrEmpty(searchCriteria.Description) || product.Description.Contains(searchCriteria.Description.Replace("\r\n", string.Empty))) &&
                                                 (string.IsNullOrEmpty(searchCriteria.Barcode) || product.Barcode.Contains(searchCriteria.Barcode)) && product.IsActive).ToList().ToPagedList(pageNumber, PageSize);

            return PartialView(Products);
        }

        public ActionResult AddProduct(ProductModel product)
        {
            if (product != null)
            {
                product.IsActive = true;
                product.CreatedBy = HttpContext.User.Identity.Name;
                product.CreatedOn = DateTime.Now;

                context.Products.Add(product);
                context.SaveChanges();
            }
            return (new EmptyResult());
        }

        [HttpPost]
        public ActionResult DeleteProducts(List<string> ProductIDs)
        {
            context.Products.Where(a => ProductIDs.Contains(a.ProductID.ToString())).ToList().ForEach(a =>
            {
                a.IsActive = false;
            });
            context.SaveChanges();
            return (new EmptyResult());
        }

 

       


    }
}