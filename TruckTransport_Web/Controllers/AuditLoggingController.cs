using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using TruckTransport_Data.DbContext;
using TruckTransport_Data.Entities;
using TruckTransport_Web.Helper;
using TruckTransport_Web.Models;

namespace TruckTransport_Web.Controllers
{    
    [Authorization(isLoggedIn: true)]
    public class AuditLoggingController : Controller
    {       
        [HttpGet]
        public ActionResult Index()
        {
            GetAuditLoggingsVM model = new GetAuditLoggingsVM();

            using (TruckTransportDbContext _db = new TruckTransportDbContext())
            {
                List<logiranidogadjaji> events = _db.logiranidogadjaji.AsNoTracking().ToList();

                model.AuditLoggingsList = events.Select(x => new AuditLoggingVM {
                    AuditLoggingID = x.logirani_dogadjaj_id,
                    Username = x.username,
                    IPAddress = x.ip_adresa,
                    URL = x.url,
                    EventTime = UnixTime.ConvertToDateTimeString(x.vrijeme_dogadjaja)            
                }).ToList();
            }

            return View(viewName: "Index", model: model);
        }
    }
}