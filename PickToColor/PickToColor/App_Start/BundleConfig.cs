using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Optimization;

namespace PickToColor
{
    public class BundleConfig
    {
        public static void RegisterBundles(BundleCollection bundles)
        {
            bundles.Add(new StyleBundle("~/bundles/commoncss").Include(
                                                        "~/Content/bootstrap.min.css",
                                                        "~/Content/fa-svg-with-js.css"
                                                    ));
            bundles.Add(new StyleBundle("~/bundles/sitecss").Include(
                                                        "~/Content/Site.css"
                                                    ));

            bundles.Add(new StyleBundle("~/bundles/logincss").Include(
                                                       "~/Content/Login.css"
                                                   ));

            bundles.Add(new ScriptBundle("~/bundles/fontawesome").Include(
                                                    "~/Scripts/fontawesome-all.js"
                                                    ));

            bundles.Add(new ScriptBundle("~/bundles/commonjs").Include(
                "~/Scripts/jquery-3.3.1.min.js",
                "~/Scripts/popper.min.js",
                "~/Scripts/bootstrap.min.js",
               "~/Scripts/CommonUtil.js"));

            bundles.Add(new ScriptBundle("~/bundles/modernizer").Include(
                "~/Scripts/modernizr-2.6.2.js"));

            bundles.Add(new ScriptBundle("~/bundles/bootbox").Include(
                "~/Scripts/bootbox.min.js"));

            //Color Picker Css and JS files 
            bundles.Add(new ScriptBundle("~/bundles/minicolorJS").Include(
                "~/Scripts/jquery.minicolors.js"
                ));

            bundles.Add(new StyleBundle("~/bundles/minicolorCSS").Include(
               "~/Content/jquery.minicolors.css"
               ));

            //Progressbar JS
            bundles.Add(new ScriptBundle("~/bundles/progbarJS").Include(
                "~/Scripts/bootstrap-progressbar.min.js"
                ));

            bundles.Add(new ScriptBundle("~/bundles/datetimejs").Include(
                "~/Scripts/moment.min.js",
                "~/Scripts/daterangepicker.min.js"
                ));

            bundles.Add(new StyleBundle("~/bundles/datetimecss").Include(
               "~/Content/daterangepicker.css"
               ));
        }
    }
}