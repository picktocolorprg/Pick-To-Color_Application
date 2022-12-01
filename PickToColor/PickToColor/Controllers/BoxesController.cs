using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Remoting.Contexts;
using System.Web;
using System.Web.Mvc;

using PickToColor.EntityFramework;
using PickToColor.Models;
using PickToColor.Utility;
using PickToColor.BO;
using System.Configuration;
using PagedList;

namespace PickToColor.Controllers
{
    public class BoxesController : Controller
    {
        public DataContext context;
        public int PageSize;
        // GET: Boxes
        public ActionResult Index()
        {
            return View();
        }
        public BoxesController()
        {
            PageSize = int.Parse(ConfigurationManager.AppSettings["DefaultPageSize"]);
            context = new DataContext();
        }

        /// <summary>
        /// Get All Boxes 
        /// </summary>
        /// <returns></returns>

        public PartialViewResult GetAllBoxes(int? pageno)
        {       
            int pageNumber = (pageno ?? 1);
            PagedList.IPagedList<BoxesModel> Boxes = null;
            Boxes = context.Boxes.Where(a => a.IsActive).OrderByDescending(a => a.CreatedOn).ToList().ToPagedList(pageNumber, PageSize);
            return PartialView(Boxes);
        }

        /// <summary>
        /// Create BoxType Deatils
        /// </summary>
        /// <returns></returns>
        [AuthorizeManager]
        [HttpPost]
        [CustomException]
        public ActionResult Create(BoxesModel NewBox)
        {
            try
            {
                if (NewBox != null)
                {
                    if (context.Boxes.Any(a => a.BoxTypeCode == NewBox.BoxTypeCode && a.CustomerID == NewBox.CustomerID && a.IsActive))
                    {
                        throw new Exception("The Box  " + NewBox.BoxTypeCode + " already exists. Please check and try again.");
                    }
                    NewBox.IsActive = true;
                    NewBox.CreatedOn = DateTime.Now;
                    NewBox.CreatedBy = HttpContext.User.Identity.Name;
                }
                context.Boxes.Add(NewBox);
                context.SaveChanges();
                return new EmptyResult();
            }
            catch (System.Data.Entity.Validation.DbEntityValidationException dbEx)
            {
                Exception raise = dbEx;
                foreach (var validationErrors in dbEx.EntityValidationErrors)
                {
                    foreach (var validationError in validationErrors.ValidationErrors)
                    {
                        string message = string.Format("{0}:{1}",
                            validationErrors.Entry.Entity.ToString(),
                            validationError.ErrorMessage);
                        // raise a new exception nesting
                        // the current instance as InnerException
                        raise = new InvalidOperationException(message, raise);
                    }
                }
                throw raise;
            }
        }
        /// <summary>
        /// Edit BoxType Details
        /// </summary>
        /// <returns></returns>
     
        [HttpPost]
        [CustomException]
        public ActionResult Edit(int BoxTypeID, string BoxTypeCode, string BoxTypeDescription, decimal TotalCBM, int CustomerID)
        {
            if (context.Boxes.Any(existingBox => existingBox.BoxTypeCode == BoxTypeCode && existingBox.CustomerID == CustomerID && existingBox.IsActive && existingBox.BoxTypeID != BoxTypeID))
            {
                throw new Exception("The entered Box code already exists for customer. Please change it and try again.");
            }

            BoxesModel EditBoxType = context.Boxes.Single(a => a.BoxTypeID == BoxTypeID);
            EditBoxType.BoxTypeCode = BoxTypeCode;
            EditBoxType.BoxTypeDescription = BoxTypeDescription;
            EditBoxType.TotalCBM = TotalCBM;
            EditBoxType.CustomerID = CustomerID;
            context.SaveChanges();
            return (new EmptyResult());
        }
        /// <summary>
        /// Delete Boxes
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        [CustomException]
        public ActionResult Delete(List<string> BoxIDs)
        {
            try
            {
                context.Boxes.Where(a => BoxIDs.Contains(a.BoxTypeID.ToString())).ToList().ForEach(a =>
                {
                    a.IsActive = false;
                });
                context.SaveChanges();
                return (new EmptyResult());
            }
            catch (System.Data.Entity.Validation.DbEntityValidationException dbEx)
            {
                Exception raise = dbEx;
                foreach (var validationErrors in dbEx.EntityValidationErrors)
                {
                    foreach (var validationError in validationErrors.ValidationErrors)
                    {
                        string message = string.Format("{0}:{1}",
                            validationErrors.Entry.Entity.ToString(),
                            validationError.ErrorMessage);
                        // raise a new exception nesting
                        // the current instance as InnerException
                        raise = new InvalidOperationException(message, raise);
                    }
                }
                throw raise;
            }
        }
    }
}