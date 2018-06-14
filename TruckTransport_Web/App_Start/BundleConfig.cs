using System.Web.Optimization;

namespace TruckTransport_Web.App_Start {
    public class BundleConfig 
    {
        public static void RegisterBundles(BundleCollection bundles) 
        {
            // Core css
            bundles.Add(new StyleBundle("~/bundles/core/css").Include(
                    "~/Content/AdminLTE-2.4.2/bower_components/bootstrap/dist/css/bootstrap.min.css",                   
                    "~/Content/AdminLTE-2.4.2/dist/css/AdminLTE.min.css",
                    "~/Content/AdminLTE-2.4.2/dist/css/skins/skin-red-light.min.css",                    
                    "~/Content/AdminLTE-2.4.2/bower_components/datatables.net-bs/css/dataTables.bootstrap.css",
                    "~/Content/AdminLTE-2.4.2/bower_components/font-awesome/css/font-awesome.min.css",
                    "~/Content/AdminLTE-2.4.2/bower_components/Ionicons/css/ionicons.min.css",
                    "~/Content/AdminLTE-2.4.2/bower_components/select2/dist/css/select2.css",
                    "~/Content/css/animate.min.css",
                    "~/Content/css/custom.css"
                ));

            // Core js
            bundles.Add(new ScriptBundle("~/bundles/core/js").Include(                    
                    "~/Content/js/jquery-1.9.1.min.js",
                    "~/Content/AdminLTE-2.4.2/bower_components/jquery-ui/jquery-ui.min.js",                   
                    "~/Content/js/jquery.unobtrusive-ajax.min.js",
                    "~/Content/AdminLTE-2.4.2/bower_components/bootstrap/dist/js/bootstrap.min.js",
                    "~/Content/AdminLTE-2.4.2/dist/js/adminlte.min.js",
                    "~/Content/AdminLTE-2.4.2/bower_components/datatables.net/js/jquery.dataTables.min.js",
                    "~/Content/AdminLTE-2.4.2/bower_components/datatables.net-bs/js/dataTables.bootstrap.min.js",
                    "~/Content/AdminLTE-2.4.2/bower_components/fastclick/lib/fastclick.js",
                    "~/Content/AdminLTE-2.4.2/bower_components/jquery-slimscroll/jquery.slimscroll.min.js", 
                    "~/Content/AdminLTE-2.4.2/bower_components/select2/dist/js/select2.min.js",                 
                    "~/Content/js/bootstrap-confirmation.min.js",
                    "~/Content/js/bootstrap-notify.min.js",
                    "~/Content/js/custom.js"
              ));

            // Login only css
            bundles.Add(new StyleBundle("~/bundles/login/css").Include(
                    "~/Content/AdminLTE-2.4.2/bower_components/bootstrap/dist/css/bootstrap.min.css",
                    "~/Content/AdminLTE-2.4.2/bower_components/font-awesome/css/font-awesome.min.css",                    
                    "~/Content/AdminLTE-2.4.2/dist/css/AdminLTE.css",                    
                    "~/Content/AdminLTE-2.4.2/plugins/iCheck/minimal/red.css"
                ));

            // Login only js
            bundles.Add(new ScriptBundle("~/bundles/login/js").Include(
                    "~/Content/js/jquery-1.9.1.min.js",
                    "~/Content/AdminLTE-2.4.2/bower_components/bootstrap/dist/js/bootstrap.min.js",
                    "~/Content/AdminLTE-2.4.2/dist/js/adminlte.min.js",
                    "~/Content/AdminLTE-2.4.2/plugins/iCheck/icheck.min.js",
                    "~/Content/AdminLTE-2.4.2/bower_components/fastclick/lib/fastclick.js",
                    "~/Content/AdminLTE-2.4.2/bower_components/jquery-slimscroll/jquery.slimscroll.min.js"                 
                ));
        }
    }
}