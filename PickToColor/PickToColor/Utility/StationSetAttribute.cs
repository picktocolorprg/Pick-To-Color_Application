using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using PickToColor.Models;

namespace PickToColor.Utility
{
    /// <summary>
    /// Attribute to verify if the operator has selected a station 
    /// after login and before performing any other operation.
    /// </summary>
    public class StationSetAttribute : AuthorizeAttribute
    {
        protected override bool AuthorizeCore(HttpContextBase httpContext)
        {
            if (httpContext.Session[KeyConstants.CurrentUser] != null)
            {
                UserModel currentUser = httpContext.Session[KeyConstants.CurrentUser] as UserModel;
                if (currentUser != null && httpContext.Session[KeyConstants.CurrentStation] != null)
                {
                    return true;
                }
            }
            return false;
        }

        protected override void HandleUnauthorizedRequest(AuthorizationContext filterContext)
        {
            filterContext.Result = new RedirectResult(string.Format("~/Station/SelectStation?sel={0}&returl={1}","none", HttpUtility.UrlEncode(filterContext.RequestContext.HttpContext.Request.Url.AbsolutePath)));
        }
    }
}