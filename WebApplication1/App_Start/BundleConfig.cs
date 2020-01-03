using System.Web;
using System.Web.Optimization;

namespace WebApplication1
{
    public class BundleConfig
    {
        // For more information on bundling, visit https://go.microsoft.com/fwlink/?LinkId=301862
        public static void RegisterBundles(BundleCollection bundles)
        {
            bundles.Add(new ScriptBundle("~/bundles/jquery").Include(
                        "~/Scripts/jquery-{version}.js",
                        "~/Scripts/jquery.qrcode.js",
                        "~/Scripts/qrcode.js",
                        "~/Scripts/FileSaver.js",
                        "~/Scripts/xlsx.full.min.js",
                        "~/Scripts/select2.full.min.js"
                        //"~/Scripts/datatables.bootstrap.js"
                        ));

            bundles.Add(new ScriptBundle("~/bundles/jqueryval").Include(
                        "~/Scripts/jquery.validate*"));

            // Use the development version of Modernizr to develop with and learn from. Then, when you're
            // ready for production, use the build tool at https://modernizr.com to pick only the tests you need.
            bundles.Add(new ScriptBundle("~/bundles/modernizr").Include(
                        "~/Scripts/modernizr-*"));

            bundles.Add(new ScriptBundle("~/bundles/bootstrap").Include(
                      "~/Scripts/bootstrap.js",
                      "~/Scripts/respond.js"));
            
            bundles.Add(new StyleBundle("~/assets/css").Include(
                        "~/assets/css/bootstrap.min.css",
                        "~/assets/css/custom.css",
                        "~/assets/css/icons.css",
                        "~/assets/css/style.css",
                        "~/assets/css/typicons.css"
                ));
            bundles.Add(new ScriptBundle("~/assets/js").Include(
                      "~/assets/js/app.js",
                      "~/assets/js/bootstrap.min.js",
                      "~/assets/js/custom.js",
                      "~/assets/js/detect.js",
                      "~/assets/js/fastclick.js",
                      "~/assets/js/jquery.blockUI.js",
                      "~/assets/js/jquery.min.js",
                      "~/assets/js/jquery.nicescroll.js",
                      "~/assets/js/jquery.scrollTo.min.js",
                      "~/assets/js/jquery.slimscroll.js",
                      "~/assets/js/modernizr.min.js",
                      "~/assets/js/proper.min.js",
                      "~/assets/js/waves.js"
               ));
            System.Threading.Thread.CurrentThread.CurrentCulture = new System.Globalization.CultureInfo("en-US");
        }
    }
}
