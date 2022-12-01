using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using PickToColor.EntityFramework;
using PickToColor.Models;

namespace PickToColor.Utility
{
    public class AuthorizeManager : AuthorizeAttribute
    {
        protected override bool AuthorizeCore(HttpContextBase httpContext)
        {
            if (httpContext.User != null && httpContext.User.Identity != null && httpContext.User.Identity.IsAuthenticated)
            {
                UserModel CurrentUser = (new DataContext()).Users.SingleOrDefault(a => a.UserName == httpContext.User.Identity.Name);
                if (CurrentUser != null)
                {
                    return CurrentUser.IsManager;
                }
            }
            return false;
        }

        protected override void HandleUnauthorizedRequest(AuthorizationContext filterContext)
        {
            filterContext.Result = new RedirectResult("~/Login/Unauthorized");
        }
    }
}