using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using TruckTransport_Web.Helper;

namespace TruckTransport_Web.Controllers
{
    [Audit]
    public class ErrorController : Controller
    {
        // GET: Error
        public ActionResult Error_404()
        {
            Response.StatusCode = 404;
            return View();
        }

        public ActionResult Error_500()
        {
            Response.StatusCode = 500;
            return View();
        }
    }
}