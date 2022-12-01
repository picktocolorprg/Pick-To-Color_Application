using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Script.Serialization;
using PagedList;
using PickToColor.EntityFramework;
using PickToColor.Models;
using PickToColor.Utility;

namespace PickToColor.Controllers
{
    [Authorize]
    public class UserController : Controller
    {
        #region Public variable and constuctors
        DataContext context;
        public int PageSize;
        public UserController()
        {
            PageSize = int.Parse(ConfigurationManager.AppSettings["DefaultPageSize"]);
            context = new DataContext();
        }
        #endregion

        /// <summary>
        /// Landing View for "Manage Users" Link
        /// </summary>
        /// <returns></returns>
        public ActionResult Index()
        {
            return View();
        }


        /// <summary>
        /// Creates new User Account
        /// </summary>
        /// <param name="NewUser"></param>
        /// <returns></returns>
        [HttpPost]
        [CustomException]
        public ActionResult Create(UserModel NewUser)
        {
            if (NewUser != null)
            {
                if (context.Users.Any(user => user.UserName == NewUser.UserName && user.IsActive))
                    throw new Exception("Username already exists. Please use another username and try again.");
                NewUser.IsOperator = true; // By default all users will have Operator functions
                NewUser.IsActive = true;
                NewUser.CreatedOn = DateTime.Now;

                SecurityLayer sl = new SecurityLayer();
                NewUser.Password = sl.SaltedPassword(NewUser.Password);

                context.Users.Add(NewUser);
                context.SaveChanges();
            }
            return new EmptyResult();
        }


        [HttpPost]
        [CustomException]
        public ActionResult Edit(int UserID, string Username, string Name, bool IsManager)
        {
            if (context.Users.Any(existinguser => existinguser.UserName == Username && existinguser.UserID != UserID && existinguser.IsActive))
            {
                throw new Exception("The Username already exists for another user. Please enter a differnet username and try again");
            }
            UserModel EditUserDetail = context.Users.Single(a => a.UserID == UserID);
            EditUserDetail.UserName = Username;
            EditUserDetail.IsManager = IsManager;
            EditUserDetail.Name = Name;
            context.SaveChanges();
            return (new EmptyResult());
        }


        [HttpPost]
        public ActionResult Delete(List<string> UserIDs)
        {
            context.Users.Where(a => UserIDs.Contains(a.UserID.ToString())).ToList().ForEach(a =>
            {
                a.IsActive = false;
            });
            context.SaveChanges();
            return (new EmptyResult());
        }

        /// <summary>
        /// Get All Users 
        /// </summary>
        /// <returns></returns>
        public PartialViewResult GetAllUsers(int? pageno)
        {
            int pageNumber = (pageno ?? 1);
            PagedList.IPagedList<UserModel> Users = null;
            Users = context.Users.Where(a => a.IsActive).OrderByDescending(a => a.CreatedOn).ToList().ToPagedList(pageNumber, PageSize);
            return PartialView(Users);
        }

        public ActionResult DeleteImage(string ImagePath, bool ForCurrentUser)
        {
            ImagePath = "~" + ImagePath;
            if (System.IO.File.Exists(Server.MapPath(ImagePath)))
            {
                System.IO.File.Delete(Server.MapPath(ImagePath));
            }
            if (ForCurrentUser)
            {
                UserModel CurrentUser = (new PickToColor.Utility.SecurityLayer()).ReturnCurrentUser();
                context.Users.FirstOrDefault(user => user.UserID == CurrentUser.UserID).ImagePath = string.Empty;
                context.SaveChanges();
                Session[PickToColor.Utility.KeyConstants.CurrentUser] = (new PickToColor.Utility.SecurityLayer()).ReturnCurrentUser();
            }
            return (new EmptyResult());
        }


        public string UploadImage(HttpPostedFileBase UserImage, bool ForCurrentUser)
        {
            if (UserImage != null)
            {
                FileInfo fi = null;
                string DirectoryPath = "~/Images/Users/";
                string FilePath = Server.MapPath(DirectoryPath + UserImage.FileName); //~/App_Data/test.xlsx
                string FullPath = "";
                if (System.IO.File.Exists(FilePath))
                {
                    fi = new FileInfo(FilePath);
                    int counter = 1;
                    FullPath = Server.MapPath(string.Format("{0}{1}_{2}{3}", DirectoryPath, fi.Name.Replace(fi.Extension, ""), counter, fi.Extension)); //~/App_Data/test_1.xlsx
                    while (System.IO.File.Exists(FullPath))
                    {
                        counter++;
                        FullPath = Server.MapPath(string.Format("{0}{1}_{2}{3}", DirectoryPath, fi.Name.Replace(fi.Extension, ""), counter, fi.Extension)); //~/App_Data/test_1.xlsx
                    }
                }
                else
                {
                    FullPath = FilePath;
                }
                UserImage.SaveAs(FullPath);

                JavaScriptSerializer jss = new JavaScriptSerializer();
                FullPath = FullPath.Substring(FullPath.IndexOf(DirectoryPath.Substring(1).Replace('/', '\\'))).Replace('\\', '/'); //Converting Abs path to rel path

                if (ForCurrentUser)
                {
                    UserModel CurrentUser = (new PickToColor.Utility.SecurityLayer()).ReturnCurrentUser();
                    context.Users.FirstOrDefault(user => user.UserID == CurrentUser.UserID).ImagePath = FullPath;
                    context.SaveChanges();
                    Session[PickToColor.Utility.KeyConstants.CurrentUser] = (new PickToColor.Utility.SecurityLayer()).ReturnCurrentUser();

                }

                return jss.Serialize(FullPath);
            }
            return null;
        }

        #region Change Password 

        /// <summary>
        /// Landing View for "Change Password" Link
        /// </summary>
        /// <returns></returns>
        public ActionResult ChangePassword()
        {
            UserModel CurrentUser = (new SecurityLayer()).ReturnCurrentUser();
            return View(CurrentUser);
        }


        /// <summary>
        /// Change Password Form Post - Where the new password will be sent to be updated to DB
        /// </summary>
        /// <param name="UserID"></param>
        /// <param name="NewPassword"></param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult ChangePassword(int UserID, string NewPassword)
        {
            SecurityLayer sl = new SecurityLayer();
            context.Users.SingleOrDefault(a => a.UserID == UserID).Password = sl.SaltedPassword(NewPassword);
            context.SaveChanges();
            return (new EmptyResult());
        }

        #endregion

    }
}