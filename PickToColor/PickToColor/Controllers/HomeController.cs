using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using PickToColor.Utility;

namespace PickToColor.Controllers
{
    [Authorize]
    //[AuthorizeManager]
    public class HomeController : Controller
    {
        [Authorize]
        //[AuthorizeManager]
        public ActionResult Index()
        {
            return View();
        }
    }
}