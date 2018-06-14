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
    [Audit]
    [Authorization(isLoggedIn: true)]
    public class VehiclesController : Controller
    {
        [HttpGet]
        public ActionResult Index()
        {
            GetVehiclesVM model = new GetVehiclesVM();

            using (TruckTransportDbContext _db = new TruckTransportDbContext())
            {
                model.VehiclesList = _db.vozila.AsNoTracking().ToList();
            }

            return View(viewName: "Index", model: model);
        }

        [HttpGet]
        public ActionResult Add()
        {
            AddVehicleVM model = new AddVehicleVM();
            return PartialView(viewName: "_Add", model: model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Add(AddVehicleVM model)
        {
            using (TruckTransportDbContext _db = new TruckTransportDbContext())
            {
                if (_db.vozila.AsNoTracking().Where(x => x.naziv == model.Name).FirstOrDefault() != null)
                {
                    ModelState.AddModelError("", "Vozilo sa unesenim nazivom već postoji u sistemu!");
                }               

                if (ModelState.IsValid)
                {
                    vozila newVehicleDB = new vozila
                    {
                       naziv = model.Name,
                       marka = model.Manufacturer,
                       tip = model.Type,
                       godiste = model.YearMade,
                       nosivost = model.LoadCapacity
                    };

                    _db.vozila.Add(newVehicleDB);
                    _db.SaveChanges();

                    return Json(data: new { success = true });
                }
            }


            return PartialView(viewName: "_Add", model: model);
        }

        [HttpGet]
        public ActionResult Edit(int vehicleID)
        {
            EditVehicleVM model = new EditVehicleVM();

            using (TruckTransportDbContext _db = new TruckTransportDbContext())
            {
                var vehicle = _db.vozila.AsNoTracking().Where(x => x.vozilo_id == vehicleID).FirstOrDefault();

                if (vehicle != null)
                {
                    model.VehicleID = vehicle.vozilo_id;
                    model.Name = vehicle.naziv;
                    model.Manufacturer = vehicle.marka;
                    model.Type = vehicle.tip;
                    model.YearMade = vehicle.godiste;
                    model.LoadCapacity = vehicle.nosivost;
                }
            }

            return PartialView(viewName: "_Edit", model: model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(EditVehicleVM model)
        {
            using (TruckTransportDbContext _db = new TruckTransportDbContext())
            {
                if (_db.vozila.AsNoTracking().Where(x => x.naziv == model.Name && x.vozilo_id != model.VehicleID).FirstOrDefault() != null)
                {
                    ModelState.AddModelError("", "Vozilo sa unesenim nazivom već postoji u sistemu!");
                }              

                if (ModelState.IsValid)
                {
                    var vehicleDB = _db.vozila.Where(x => x.vozilo_id == model.VehicleID).FirstOrDefault();
                  
                    vehicleDB.naziv = model.Name;
                    vehicleDB.marka = model.Manufacturer;
                    vehicleDB.tip = model.Type;
                    vehicleDB.godiste = model.YearMade;
                    vehicleDB.nosivost = model.LoadCapacity;

                    _db.SaveChanges();

                    return Json(data: new { success = true });
                }
            }

            return PartialView(viewName: "_Edit", model: model);
        }
    }
}