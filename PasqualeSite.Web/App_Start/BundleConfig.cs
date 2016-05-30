using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Optimization;

namespace PasqualeSite.Web.App_Start
{
    public class BundleConfig
    {
        public static void RegisterBundles(BundleCollection bundles)
        {
            BundleTable.EnableOptimizations = true;
            bundles.Add(new Bundle("~/bundles/sitejs").Include(
                        "~/js/jquery-1.9.1.min.js",
                        "~/js/plugins/jqueryui/jquery-ui.min.js",
                        "~/js/bootstrap.min.js",
                        "~/js/plugins/owl/owl.carousel.min.js"
                        
            ));


            bundles.Add(new Bundle("~/bundles/workareajs").Include(
                        "~/js/knockout-3.4.0.js",
                        "~/js/knockout-mapping.js",
                        "~/js/knockout.mapping.merge.js",
                        "~/js/plugins/kogrid/koGrid-2.1.1.js",
                        "~/js/knockouthandlers.js",
                        "~/js/plugins/notify/notify.min.js",
                        "~/js/plugins/bootstrap-multiselect/bootstrap-multiselect.js",
                        "~/js/plugins/datejs/date.js",
                        "~/js/plugins/datejs/moment.js"
            ));

            bundles.Add(new StyleBundle("~/Content/sitecss").Include(
                        "~/css/bootstrap.min.css",
                        "~/js/plugins/owl/assets/owl.carousel.css",
                        "~/js/plugins/owl/assets/owl.theme.css",
                        "~/css/app.css"
            ));

            bundles.Add(new StyleBundle("~/Content/workareacss").Include(
                        "~/js/plugins/kogrid/KoGrid.css",
                        "~/js/plugins/bootstrap-multiselect/bootstrap-multiselect.css"
            ));
        }
    }
}