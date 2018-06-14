using System;
using System.Collections.Generic;
using System.Linq;
using System.Data.Entity;
using System.Web;
using System.Web.Mvc;
using TruckTransport_Data.DbContext;
using TruckTransport_Data.Entities;
using TruckTransport_Web.Helper;
using TruckTransport_Web.Models;

namespace TruckTransport_Web.Controllers
{
    [Audit]
    [Authorization(isLoggedIn: true)]
    public class DriverStopsController : Controller
    {
        [HttpGet]
        public ActionResult Index()
        {
            GetDriverStopsVM model = new GetDriverStopsVM();

            using (TruckTransportDbContext _db = new TruckTransportDbContext())
            {
                model.DriverStopsList = _db.stajalista.AsNoTracking().ToList();
            }

            return View(viewName: "Index", model: model);
        }

        [HttpGet]
        public ActionResult Details(int driverStopID)
        {
            GetDriverStopDetailsVM model = new GetDriverStopDetailsVM();

            using (TruckTransportDbContext _db = new TruckTransportDbContext())
            {
                stajalista driverStop = _db.stajalista.AsNoTracking().Where(x => x.stajaliste_id == driverStopID).FirstOrDefault();

                if (driverStop != null)
                {
                    model.DriverStopID = driverStop.stajaliste_id;
                    model.Name = driverStop.naziv;
                    model.Description = driverStop.opis;                   
                    model.Latitude = driverStop.sirina;
                    model.Longitude = driverStop.duzina;
                }
            }

            return View(viewName: "Details", model: model);
        }

        [HttpGet]
        public ActionResult Add()
        {
            AddDriverStopVM model = new AddDriverStopVM();          
            return View(viewName: "Add", model: model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Add(AddDriverStopVM model)
        {
            using (TruckTransportDbContext _db = new TruckTransportDbContext())
            {
                if (_db.stajalista.AsNoTracking().Where(x => x.naziv == model.Name || (x.sirina == model.Latitude && x.duzina == model.Longitude)).FirstOrDefault() != null)
                {
                    ModelState.AddModelError("", "Stajalište sa unesenim nazivom ili pozicijom na karti već postoji!");
                }

                if (ModelState.IsValid)
                {
                    stajalista driverStopDB = new stajalista();

                    driverStopDB.naziv = model.Name;
                    driverStopDB.opis = model.Description;
                    driverStopDB.sirina = model.Latitude;
                    driverStopDB.duzina = model.Longitude;

                    _db.stajalista.Add(driverStopDB);
                    _db.SaveChanges();

                    return RedirectToAction(actionName: "Index");
                }
            }

            return View(viewName: "Add", model: model);
        }

        [HttpGet]
        public ActionResult Edit(int driverStopID)
        {
            EditDriverStopVM model = new EditDriverStopVM();

            using (TruckTransportDbContext _db = new TruckTransportDbContext())
            {
                stajalista driverStop = _db.stajalista.AsNoTracking().Where(x => x.stajaliste_id == driverStopID).FirstOrDefault();

                if (driverStop != null)
                {
                    model.DriverStopID = driverStop.stajaliste_id;
                    model.Name = driverStop.naziv;
                    model.Description = driverStop.opis;
                    model.Latitude = driverStop.sirina;
                    model.Longitude = driverStop.duzina;
                }
            }

            return View(viewName: "Edit", model: model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(EditDriverStopVM model)
        {
            using (TruckTransportDbContext _db = new TruckTransportDbContext())
            {
                if (_db.stajalista.AsNoTracking().Where(x => (x.naziv == model.Name || (x.sirina == model.Latitude && x.duzina == model.Longitude)) && x.stajaliste_id != model.DriverStopID).FirstOrDefault() != null)
                {
                    ModelState.AddModelError("", "Stajalište sa unesenim nazivom ili pozicijom na karti već postoji!");
                }

                if (ModelState.IsValid)
                {
                    stajalista driverStopDB = _db.stajalista.Where(x => x.stajaliste_id == model.DriverStopID).FirstOrDefault();

                    driverStopDB.naziv = model.Name;
                    driverStopDB.opis = model.Description;
                    driverStopDB.sirina = model.Latitude;
                    driverStopDB.duzina = model.Longitude;

                    _db.SaveChanges();

                    return RedirectToAction(actionName: "Index");
                }
            }

            return View(viewName: "Edit", model: model);
        }

        [HttpGet]
        public ActionResult GetDriverStopLocationsCoordinatesByIDs(int[] driverStopIDs)
        {
            using (TruckTransportDbContext _db = new TruckTransportDbContext())
            {
                List<GeoLocationVM> model = new List<GeoLocationVM>();

                for (int i = 0; i < driverStopIDs.Length; i++)
                {
                    int driverStopID = driverStopIDs[i];

                    GeoLocationVM singleLocation = _db.stajalista.Where(x => x.stajaliste_id == driverStopID).Select(x => new GeoLocationVM
                    {
                        ID = x.stajaliste_id,
                        Info = x.naziv,
                        Latitude = x.sirina,
                        Longitude = x.duzina
                    }).AsNoTracking().FirstOrDefault();

                    model.Add(singleLocation);
                }

                if (model != null && model.Count != 0)
                {
                    return Json(new { success = true, driverStopLocations = model }, JsonRequestBehavior.AllowGet);
                }
                else
                {
                    return Json(new { success = false }, JsonRequestBehavior.AllowGet);
                }
            }
        }
    }
}