using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using TruckTransport_Data.DbContext;
using TruckTransport_Data.Entities;

namespace TruckTransport_Web.Helper
{
    public class AuditAttribute : ActionFilterAttribute
    {
        public override void OnActionExecuting(ActionExecutingContext filterContext)
        {          
            var request = filterContext.HttpContext.Request;

            logiranidogadjaji audit = new logiranidogadjaji()
            {
                username = (Authentication.GetLoggedInUser() != null) ?
                            Authentication.GetLoggedInUser().username : "Anonimno",
                ip_adresa = request.ServerVariables["HTTP_X_FORWARDED_FOR"] ?? request.UserHostAddress,
                url = request.RawUrl,
                vrijeme_dogadjaja = UnixTime.GetUnixTimeNow()
            };

            using (TruckTransportDbContext _db = new TruckTransportDbContext())
            {
                _db.logiranidogadjaji.Add(audit);
                _db.SaveChanges();
            }

            base.OnActionExecuting(filterContext);
        }
    }
}