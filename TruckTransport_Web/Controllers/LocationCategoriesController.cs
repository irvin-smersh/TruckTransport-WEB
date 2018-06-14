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
    public class LocationCategoriesController : Controller
    {
        [HttpGet]
        public ActionResult Index()
        {
            GetLocationCategoriesVM model = new GetLocationCategoriesVM();

            using (TruckTransportDbContext _db = new TruckTransportDbContext()) 
            {
                model.LocationCategoriesList = _db.kategorije.Include(x => x.poznatelokacije).AsNoTracking().ToList();
            }

            return View(viewName: "Index", model: model);
        }

        [HttpGet]
        public ActionResult Add()
        {
            AddLocationCategoryVM model = new AddLocationCategoryVM();
            return PartialView(viewName: "_Add", model: model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Add(AddLocationCategoryVM model) 
        {
            using (TruckTransportDbContext _db = new TruckTransportDbContext()) 
            {
                if (_db.kategorije.AsNoTracking().Where(x => x.naziv == model.Name).FirstOrDefault() != null) 
                {
                    ModelState.AddModelError("", "Naziv kategorije lokacije već postoji!");
                }

                if (ModelState.IsValid)
                {
                    kategorije newLocationCategoryDB = new kategorije {
                        naziv = model.Name                       
                    };

                    _db.kategorije.Add(newLocationCategoryDB);
                    _db.SaveChanges();

                    return Json(data: new { success = true });
                }
            }

            return PartialView(viewName: "_Add", model: model);
        }

        [HttpGet]
        public ActionResult Edit(int locationCategoryID) 
        {
            EditLocationCategoryVM model = new EditLocationCategoryVM();

            using (TruckTransportDbContext _db = new TruckTransportDbContext()) 
            {
                var locationCategory = _db.kategorije.AsNoTracking().Where(x => x.kategorija_id == locationCategoryID).FirstOrDefault();

                if (locationCategory != null)
                {
                    model.LocationCategoryID = locationCategory.kategorija_id;
                    model.Name = locationCategory.naziv;                   
                }
            }

            return PartialView(viewName: "_Edit", model: model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(EditLocationCategoryVM model) 
        {
            using (TruckTransportDbContext _db = new TruckTransportDbContext())
            {
                if(_db.kategorije.AsNoTracking().Where(x=>x.naziv == model.Name && x.kategorija_id != model.LocationCategoryID).FirstOrDefault() != null)
                {
                    ModelState.AddModelError("", "Naziv kategorije lokacije već postoji!");
                }

                if (ModelState.IsValid)
                {
                    var locationCategoryDB = _db.kategorije.Where(x => x.kategorija_id == model.LocationCategoryID).FirstOrDefault();

                    locationCategoryDB.naziv = model.Name;
                    _db.SaveChanges();

                    return Json(data: new { success = true });
                }
            }

            return PartialView(viewName: "_Edit", model: model);
        }

        public ActionResult Delete(int locationCategoryID)
        {
            using (TruckTransportDbContext _db = new TruckTransportDbContext()) 
            {
                var locationCategoryDB = _db.kategorije.Where(x => x.kategorija_id == locationCategoryID).FirstOrDefault();

                if (locationCategoryDB != null)
                {
                    _db.kategorije.Remove(locationCategoryDB);
                    _db.SaveChanges();

                    return Json(data: new { success = true }, behavior: JsonRequestBehavior.AllowGet);
                }
            }

            return Json(data: new { success = false }, behavior: JsonRequestBehavior.AllowGet);
        }
    }
}