using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
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
    public class KnownLocationsController : Controller
    {
        private DropdownMaker _dropdownMaker;
        public KnownLocationsController()
        {
            _dropdownMaker = new DropdownMaker();
        }

        [HttpGet]
        public ActionResult Index()
        {
            GetKnownLocationsVM model = new GetKnownLocationsVM();

            using (TruckTransportDbContext _db = new TruckTransportDbContext())
            {
                model.KnownLocationsList = _db.poznatelokacije
                    .Include(x => x.kategorije).Select(x => new GetKnownLocationsVM.KnownLocationVM
                    {
                        KnownLocationID = x.poznatalokacija_id,
                        Name = x.naziv,
                        Description = x.opis,
                        Longitude = x.duzina,
                        Latitude = x.sirina,
                        Category = x.kategorije.naziv
                    }).AsNoTracking().ToList();
            }

            return View(viewName: "Index", model: model);
        }

        [HttpGet]
        public ActionResult Details(int knownLocationID)
        {
            GetKnownLocationDetailsVM model = new GetKnownLocationDetailsVM();

            using (TruckTransportDbContext _db = new TruckTransportDbContext())
            {
                poznatelokacije knownLocation = _db.poznatelokacije.Include(x => x.kategorije).Where(x => x.poznatalokacija_id == knownLocationID).AsNoTracking().FirstOrDefault();

                if(knownLocation != null)
                {
                    model.KnownLocationID = knownLocation.poznatalokacija_id;
                    model.Name = knownLocation.naziv;
                    model.Description = knownLocation.opis;
                    model.LocationCategory = knownLocation.kategorije.naziv;
                    model.Latitude = knownLocation.sirina;
                    model.Longitude = knownLocation.duzina;
                }
            }

            return View(viewName: "Details", model: model);
        }

        [HttpGet]
        public ActionResult Add()
        {
            AddKnownLocationVM model = new AddKnownLocationVM();
            model.LocationCategories = _dropdownMaker.GetLocationCategories();

            return View(viewName: "Add", model: model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Add(AddKnownLocationVM model)
        {
            using (TruckTransportDbContext _db = new TruckTransportDbContext())
            {
                if (_db.poznatelokacije.AsNoTracking().Where(x => x.naziv == model.Name || (x.sirina == model.Latitude && x.duzina == model.Longitude)).FirstOrDefault() != null)
                {
                    ModelState.AddModelError("", "Lokacija sa unesenim nazivom ili pozicijom na karti već postoji!");
                }

                if (ModelState.IsValid)
                {
                    poznatelokacije knownLocationDB = new poznatelokacije();

                    knownLocationDB.naziv = model.Name;
                    knownLocationDB.opis = model.Description;
                    knownLocationDB.kategorija_id = model.LocationCategoryID;
                    knownLocationDB.sirina = model.Latitude;
                    knownLocationDB.duzina = model.Longitude;

                    _db.poznatelokacije.Add(knownLocationDB);
                    _db.SaveChanges();

                    return RedirectToAction(actionName: "Index");
                }
            }

            model.LocationCategories = _dropdownMaker.GetLocationCategories();
            return View(viewName: "Add", model: model);
        }

        [HttpGet]
        public ActionResult Edit(int knownLocationID)
        {
            EditKnownLocationVM model = new EditKnownLocationVM();

            using (TruckTransportDbContext _db = new TruckTransportDbContext())
            {
                poznatelokacije knownLocation = _db.poznatelokacije.AsNoTracking().Where(x => x.poznatalokacija_id == knownLocationID).FirstOrDefault();

                if(knownLocation != null)
                {
                    model.KnownLocationID = knownLocation.poznatalokacija_id;
                    model.Name = knownLocation.naziv;
                    model.Description = knownLocation.opis;
                    model.LocationCategoryID = knownLocation.kategorija_id;
                    model.Latitude = knownLocation.sirina;
                    model.Longitude = knownLocation.duzina;                   
                }
              
                model.LocationCategories = _dropdownMaker.GetLocationCategories();
            }
              
            return View(viewName: "Edit", model: model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(EditKnownLocationVM model)
        {
            using (TruckTransportDbContext _db = new TruckTransportDbContext())
            {
                if (_db.poznatelokacije.AsNoTracking().Where(x => (x.naziv == model.Name || (x.sirina == model.Latitude && x.duzina == model.Longitude)) && x.poznatalokacija_id != model.KnownLocationID).FirstOrDefault() != null)
                {
                    ModelState.AddModelError("", "Lokacija sa unesenim nazivom ili pozicijom na karti već postoji!");
                }

                if (ModelState.IsValid)
                {
                    poznatelokacije knownLocationDB = _db.poznatelokacije.Where(x => x.poznatalokacija_id == model.KnownLocationID).FirstOrDefault();

                    knownLocationDB.naziv = model.Name;
                    knownLocationDB.opis = model.Description;
                    knownLocationDB.kategorija_id = model.LocationCategoryID;
                    knownLocationDB.sirina = model.Latitude;
                    knownLocationDB.duzina = model.Longitude;
                    
                    _db.SaveChanges();

                    return RedirectToAction(actionName: "Index");
                }
            }

            model.LocationCategories = _dropdownMaker.GetLocationCategories();
            return View(viewName: "Edit", model: model);
        }

        [HttpGet]
        public ActionResult GetKnownLocationCoordinatesByID(int knownLocationID)
        {
            using (TruckTransportDbContext _db = new TruckTransportDbContext())
            {
                poznatelokacije knownLocation = _db.poznatelokacije.AsNoTracking().Where(x => x.poznatalokacija_id == knownLocationID).FirstOrDefault();

                if (knownLocation != null)
                {
                    return Json(new { success = true, latitude = knownLocation.sirina, longitude = knownLocation.duzina }, JsonRequestBehavior.AllowGet);
                }
                else
                {
                    return Json(new { success = false }, JsonRequestBehavior.AllowGet);
                }
            }
        }
    }
}