using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;
using PickToColor.Models;
using PickToColor.Utility;

namespace PickToColor.Controllers
{
    public class LoginController : Controller
    {
        public TransactionAudit Audit;
        public LoginController()
        {
            Audit = new TransactionAudit();
        }

        /// <summary>
        /// Landing View for Login Page
        /// </summary>
        /// <returns></returns>
        public ActionResult Index()
        {
            //WHEN : If the user tried to access a page that requires authentication, 
            //but the user is yet to login, hence the page got redirected to Login page.
            //Save the redirect url and redirect to that page upon successful login
            if (!string.IsNullOrEmpty(Request.QueryString[KeyConstants.ReturnURL]))
            {
                TempData[KeyConstants.ReturnURL] = Request.QueryString[KeyConstants.ReturnURL];
            }

            return View();
        }

        /// <summary>
        /// Performs Authentication 
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public ActionResult DoAuthentication(string username,string password,string rememberme,string usertype)
        {
            //Get the posted data from Login Form
            string UserName = username;
            string Password = password;
            bool RememberUser = !string.IsNullOrEmpty(rememberme) && rememberme == "on" ? true : false;

            SecurityLayer authenticateUser = new SecurityLayer();

            UserModel CurrentUser = null;
            bool IsAuthenticated = authenticateUser.AuthenticateUser(UserName, Password, usertype, out CurrentUser);

            if (IsAuthenticated)
            {

                TempData.Remove(KeyConstants.LoginError);
                Session[KeyConstants.CurrentUser] = CurrentUser;
                //Set the forms authentication cookie
                FormsAuthentication.SetAuthCookie(UserName, RememberUser);
                Audit.AddHistory(TransactionAudit.AuditType.Login, CurrentUser.UserID, "User logged-in successfully.");

                //Check for redirectURL and redirect to that page if present
                string RedirecURL = TempData[KeyConstants.ReturnURL] != null ? TempData[KeyConstants.ReturnURL].ToString() : string.Empty;
                TempData.Remove(KeyConstants.ReturnURL);

                if (CurrentUser.IsOperator && !CurrentUser.IsManager)
                {
                    return RedirectToAction("SelectStation", "Station");
                }
                else if (string.IsNullOrEmpty(RedirecURL))
                {
                    return RedirectToAction("Index", "Home");
                }
                else
                {
                    return Redirect(RedirecURL);
                }
               
            }
            else
            {
                TempData[KeyConstants.LoginError] = true;
                //return RedirectToAction("Index","Login");
                if (usertype == "Admin")
                    return RedirectToAction("Index", "Login", new
                {                   
                    param = "Admin"               

                });
                else
                    return RedirectToAction("Index", "Login", new
                    {
                        param = "Operator"

                    });
            }
        }


        /// <summary>
        /// Logout action method
        /// </summary>
        /// <returns></returns>
        public ActionResult LogOff()
        {
            if (Session[KeyConstants.CurrentUser] != null)
            {
                Audit.AddHistory(TransactionAudit.AuditType.Login, (Session[KeyConstants.CurrentUser] as UserModel).UserID, "User logged out successfully.");
            }
            Session[KeyConstants.CurrentStation] = Session[KeyConstants.CurrentUser] = null;
            FormsAuthentication.SignOut();
            return RedirectToAction("Index");
        }

        /// <summary>
        /// Http 403 unauthorized access - Refer AuthorizeManager class
        /// </summary>
        /// <returns></returns>
        public ActionResult Unauthorized()
        {
            return View();
        }

        /// <summary>
        /// For any unhandled exception
        /// </summary>
        /// <returns></returns>
        public ActionResult Error()
        {
            return View();
        }
    }
}