using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using PickToColor.EntityFramework;
using PickToColor.Models;
using PickToColor.Utility;
using System.IO;
using PagedList;
using System.Configuration;

namespace PickToColor.Controllers
{
    [Authorize]
    public class LocationController : Controller
    {
        public DataContext context;
        public int PageSize;

        public LocationController()
        {
            PageSize = int.Parse(ConfigurationManager.AppSettings["DefaultPageSize"]);
            context = new DataContext();
        }

        /// <summary>
        /// Landing View for "Manage Locations" Link
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
        public ActionResult Create(LocationModel NewLocation)
        {
            try
            {
                if (NewLocation != null)
                {
                    NewLocation.IsActive = true;
                    NewLocation.CreatedOn = DateTime.Now;
                    NewLocation.CreatedBy = HttpContext.User.Identity.Name;
                }
                context.Locations.Add(NewLocation);
                context.SaveChanges();
                TempData[KeyConstants.UserCreatedSuccess] = true;

            }
            //catch (CustomException exc)
            //{
            //    //Show Actual Error Message to User
            //    TempData[KeyConstants.UserCreatedSuccess] = false;
            //    TempData[KeyConstants.UserCreatedFailureError] = exc.DisplayError;
            //}
            catch (Exception ex)
            {
                //Show Generic Error Message to User
                TempData[KeyConstants.UserCreatedSuccess] = false;
                TempData[KeyConstants.UserCreatedFailureError] = "Operation has failed. Please try again after some time.";

            }
            return RedirectToAction("Index");
        }

        /// <summary>
        /// Get All Locations 
        /// </summary>
        /// <returns></returns>

        public PartialViewResult GetAllLocations(int? pageno)
        {
            int pageNumber = (pageno ?? 1);
            PagedList.IPagedList<LocationModel> Locations = null;
            Locations = context.Locations.Where(a => a.IsActive).OrderByDescending(a => a.CreatedOn).ToList().ToPagedList(pageNumber, PageSize);
            return PartialView(Locations);


            //var Locations = context.Locations.ToList();
            //if (Locations != null)
            //    Locations = Locations.Where(a => a.IsActive).OrderByDescending(a => a.CreatedOn).ToList();
            //return PartialView(Locations);
        }

        /// <summary>
        /// Edit Location Deatils
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public ActionResult Edit(int LocationMasterID,string LocationID, string LocationCode, string LocationSoundFile, string ColorSoundFile, string BackgroundColorCode,string FontColorCode,int PickingPriority,int stationid)
        {
            LocationModel EditLocDetail = context.Locations.Single(a => a.LocationMasterID == LocationMasterID);
            EditLocDetail.LocationID = LocationID;
            EditLocDetail.LocationCode = LocationCode;
            EditLocDetail.LocationSoundFile = LocationSoundFile;
            EditLocDetail.BackgroundColorCode = BackgroundColorCode;
            EditLocDetail.FontColorCode = FontColorCode;
            EditLocDetail.ColorSoundFile = ColorSoundFile;
            EditLocDetail.PickingPriority = PickingPriority;
            EditLocDetail.StationID = stationid;
            context.SaveChanges();
            return (new EmptyResult());
        }

        /// <summary>
        /// Delete Location
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public ActionResult Delete(List<string> LocationMasterIDs)
        {
           List<ProductModel> ProductData= context.Products.Where(a => LocationMasterIDs.Contains(a.LocationMasterID.ToString()) && a.IsActive).ToList();
            if (ProductData.Count() <= 0)
            {

                context.Locations.Where(a => LocationMasterIDs.Contains(a.LocationMasterID.ToString())).ToList().ForEach(a =>
                {
                    a.IsActive = false;
                });
                context.SaveChanges();
                return (new EmptyResult());
            }
            else
            {
                return Content("System does not allow to delete this location becuase this Location is being used in Prouct Registration.");
            }
        }

        [HttpPost]
        public async System.Threading.Tasks.Task<JsonResult> SoundFileUpload(string id)
        {
            string fileName="";
            try
            {
                foreach (string file in Request.Files)
                {
                    var fileContent = Request.Files[file];
                    if (fileContent != null && fileContent.ContentLength > 0)
                    {
                        // get a stream
                        int counter = 0;
                        var stream = fileContent.InputStream;
                        // and optionally write the file to disk
                        fileName = fileContent.FileName;
                        var path = Path.Combine(Server.MapPath("~/Audio"), fileName);
                        FileInfo fi = new FileInfo(path);

                        while (System.IO.File.Exists(path))
                        {
                            counter++;
                            fileName = fi.Name.Replace(fi.Extension, "") + '_'+counter+fi.Extension;
                            path = Path.Combine(Server.MapPath("~/Audio"), fileName);
                        }
                       
                        using (var fileStream =System.IO.File.Create(path))
                        {
                            stream.CopyTo(fileStream);
                        }
                    }
                }
            }
            catch (Exception)
            {                
                return Json("File Upload failed. Please try again!");
            }

            return Json(fileName);
        }

        //[HttpPost]
        //public ActionResult SoundFileUpload(HttpPostedFileBase SoundFile)
        //{
        //    if (SoundFile != null)
        //    {
        //        FileInfo fi = null;
        //        string DirectoryPath = "~/Audio/";
        //        string FilePath = Server.MapPath(DirectoryPath + SoundFile.FileName); //~/App_Data/sound/test.wav
        //        string FullPath = "";
        //        if (System.IO.File.Exists(FilePath))
        //        {
        //            fi = new FileInfo(FilePath);
        //            int counter = 1;
        //            FullPath = Server.MapPath(string.Format("{0}{1}_{2}{3}", DirectoryPath, fi.Name.Replace(fi.Extension, ""), counter, fi.Extension)); //~/App_Data/test_1.xlsx
        //            while (System.IO.File.Exists(FullPath))
        //            {
        //                counter++;
        //                FullPath = Server.MapPath(string.Format("{0}{1}_{2}{3}", DirectoryPath, fi.Name.Replace(fi.Extension, ""), counter, fi.Extension)); //~/App_Data/test_1.xlsx
        //            }
        //        }
        //        else
        //        {
        //            FullPath = FilePath;
        //        }

        //        SoundFile.SaveAs(FullPath);
        //        //Get data from Excel
        //        //Check if all data is in line
        //        ExcelHelper excelHelper = new ExcelHelper();
        //        string ErrorMessage;
        //        List<ProductModel> UploadedProducts = excelHelper.GetProductsFromExcel(FullPath, out ErrorMessage);
        //        //If there is any problem, Throw Exception
        //        if (!string.IsNullOrEmpty(ErrorMessage))
        //        {
        //            TempData[KeyConstants.ProductUploadFailureError] = ErrorMessage;
        //            TempData[KeyConstants.ProductUploadSuccess] = false;
        //        }
        //        else
        //        {
        //            TempData.Remove(KeyConstants.ProductUploadFailureError);
        //            UploadedProducts.ForEach(up =>
        //            {
        //                up.IsActive = true;
        //                up.CreatedBy = HttpContext.User.Identity.Name;
        //                up.CreatedOn = DateTime.Now;
        //            });

        //            List<int> Customers = UploadedProducts.Select(a => a.CustomerID).Distinct().ToList();
        //            context.Products.Where(product => Customers.Contains(product.CustomerID)).ToList().ForEach(existingProduct =>
        //            {
        //                existingProduct.IsActive = false;
        //            });

        //            //If they are correct,Upload them to database.
        //            context.Products.AddRange(UploadedProducts);
        //            context.SaveChanges();
        //            TempData[KeyConstants.ProductUploadSuccess] = true;
        //        }

        //        System.IO.File.Delete(FullPath);
        //    }
        //    return RedirectToAction("Index", "ProductRegistration");

        //}
    }


}