using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;
using TruckTransport_Web.App_Start;

namespace TruckTransport_Web
{
    public class MvcApplication : HttpApplication
    {
        protected void Application_Start()
        {
            AreaRegistration.RegisterAllAreas();
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            BundleConfig.RegisterBundles(bundles: BundleTable.Bundles);
        }

        protected void Application_Error() 
        {
            var exception = Server.GetLastError();

            try
            {
                HttpException httpException = (HttpException)exception;

                int httpResponseCode = httpException.GetHttpCode();

                switch (httpResponseCode)
                {
                    case 404: Response.Redirect("/Error/Error_404"); break;
                    default: Response.Redirect("/Error/Error_500"); break;
                }
            }
            catch (Exception)
            {
                Response.Redirect("/Error/Error_500");
            }
            

            Server.ClearError();
        }
    }
}
