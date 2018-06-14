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
    public class DriversController : Controller
    {
        [HttpGet]
        public ActionResult Index() 
        {
            GetDriversVM model = new GetDriversVM(); 

            using (TruckTransportDbContext _db = new TruckTransportDbContext())
            {
                model.DriversList = _db.vozaci.Select(x => new GetDriversVM.DriverVM {
                    DriverID = x.vozac_id,
                    FirstName = x.ime,
                    LastName = x.prezime,
                    PersonUniqueID = x.jmbg,
                    Username = x.user,
                    Status = _db.nalozi.Where(s => s.stanje_id == 1 && s.vozac_id == x.vozac_id).FirstOrDefault() != null ? "Aktivan" : "Na čekanju"
                }).AsNoTracking().ToList();
            }

            return View(viewName: "Index", model: model);
        }
       
        [HttpGet]
        public ActionResult Details(int driverID)
        {
            GetDriverDetailsVM model = new GetDriverDetailsVM();

            using (TruckTransportDbContext _db = new TruckTransportDbContext())
            {
                model = _db.vozaci.Where(x => x.vozac_id == driverID).Select(x => new GetDriverDetailsVM
                {
                    DriverID = x.vozac_id,
                    FirstName = x.ime,
                    LastName = x.prezime,
                    PersonUniqueID = x.jmbg,
                    Username = x.user,
                    Status = _db.nalozi.Where(s => s.stanje_id == 1 && s.vozac_id == x.vozac_id).FirstOrDefault() != null ? "Aktivan" : "Na čekanju"           
                }).FirstOrDefault();

                model.NumberOfFinishedOrders = _db.nalozi.Where(s => s.stanje_id == 3 && s.vozac_id == model.DriverID).Count();
                model.NumberOfFinishedTasks = _db.zadaci.Include(s => s.nalozi).Where(s => s.nalozi.stanje_id == 3 && s.nalozi.vozac_id == model.DriverID).Count();
                model.NumberOfGivenOrders = _db.nalozi.Where(s => s.vozac_id == model.DriverID).Count();
                model.NumberOfGivenTasks = _db.zadaci.Include(s => s.nalozi).Where(s => s.nalozi.vozac_id == model.DriverID).Count();

                var vehicleID = _db.nalozi.Where(x => x.vozac_id == model.DriverID && x.stanje_id == 3).ToList() != null && _db.nalozi.Where(x => x.vozac_id == model.DriverID && x.stanje_id == 3).ToList().Count != 0 ? _db.nalozi.Where(x => x.vozac_id == model.DriverID && x.stanje_id == 3).Max(x => x.vozilo_id) : 0;

                if (vehicleID != 0)
                {
                    var vehicle = _db.vozila.Where(x => x.vozilo_id == vehicleID).FirstOrDefault();
                    model.MostUsedVehicle = vehicle.naziv + ", " + vehicle.marka;
                }
                else
                {
                    model.MostUsedVehicle = "";
                }
            }

            return PartialView(viewName: "_Details", model: model);
        }

        [HttpGet]
        public ActionResult GetLatestPosition(int driverID)
        {
            GeoLocationVM model = new GeoLocationVM();

            using (TruckTransportDbContext _db = new TruckTransportDbContext())
            {
                var latestPosition = _db.geotacke.Include(x => x.nalozi).Where(x => x.nalozi.stanje_id == 1 && x.nalozi.vozac_id == driverID).OrderByDescending(x => x.vrijeme).FirstOrDefault();

                if(latestPosition != null)
                {
                    model.ID = driverID;
                    model.Info = UnixTime.ConvertToDateTimeString(latestPosition.vrijeme);
                    model.Latitude = latestPosition.sirina;
                    model.Longitude = latestPosition.duzina;
                }
            }

            return PartialView(viewName: "_LatestMapPosition", model: model);
        }

        [HttpGet]
        public JsonResult GetLatestCoordinatePosition(int driverID)
        {
            GeoLocationVM model = new GeoLocationVM();

            using (TruckTransportDbContext _db = new TruckTransportDbContext())
            {
                var latestPosition = _db.geotacke.Include(x => x.nalozi).Where(x => x.nalozi.stanje_id == 1 && x.nalozi.vozac_id == driverID).OrderByDescending(x => x.vrijeme).FirstOrDefault();

                if (latestPosition != null)
                {
                    model.Info = UnixTime.ConvertToDateTimeString(latestPosition.vrijeme);
                    model.Latitude = latestPosition.sirina;
                    model.Longitude = latestPosition.duzina;
                }
            }

            return Json(new { info = model.Info, latitude = model.Latitude, longitude = model.Longitude }, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public ActionResult Add()
        {
            AddDriverVM model = new AddDriverVM();
            return PartialView(viewName: "_Add", model: model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Add(AddDriverVM model)
        {
            using (TruckTransportDbContext _db = new TruckTransportDbContext())
            {
                if (_db.vozaci.AsNoTracking().Where(x => x.jmbg == model.PersonUniqueID).FirstOrDefault() != null)
                {
                    ModelState.AddModelError("", "Vozač sa unesenim JMBG-om je već registrovan u sistemu!");
                }

                if (_db.vozaci.AsNoTracking().Where(x => x.user == model.Username).FirstOrDefault() != null)
                {
                    ModelState.AddModelError("", "Korisničko ime se već koristi!");
                }

                if (ModelState.IsValid)
                {
                    vozaci newDriverDB = new vozaci
                    {
                        ime = model.FirstName,
                        prezime = model.LastName,
                        jmbg = model.PersonUniqueID,
                        user = model.Username,
                        pass = model.Password
                    };

                    _db.vozaci.Add(newDriverDB);
                    _db.SaveChanges();

                    return Json(data: new { success = true });
                }
            }

            return PartialView(viewName: "_Add", model: model);
        }

        [HttpGet]
        public ActionResult Edit(int driverID)
        {
            EditDriverVM model = new EditDriverVM();

            using (TruckTransportDbContext _db = new TruckTransportDbContext())
            {
                var driver = _db.vozaci.AsNoTracking().Where(x => x.vozac_id == driverID).FirstOrDefault();

                if (driver != null)
                {
                    model.DriverID = driver.vozac_id;
                    model.FirstName = driver.ime;
                    model.LastName = driver.prezime;
                    model.PersonUniqueID = driver.jmbg;
                    model.Username = driver.user;                  
                }
            }

            return PartialView(viewName: "_Edit", model: model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(EditDriverVM model)
        {
            using (TruckTransportDbContext _db = new TruckTransportDbContext())
            {
                if (_db.vozaci.AsNoTracking().Where(x => x.jmbg == model.PersonUniqueID && x.vozac_id != model.DriverID).FirstOrDefault() != null)
                {
                    ModelState.AddModelError("", "Vozač sa unesenim JMBG-om je već registrovan u sistemu!");
                }

                if (_db.vozaci.AsNoTracking().Where(x => x.user == model.Username && x.vozac_id != model.DriverID).FirstOrDefault() != null)
                {
                    ModelState.AddModelError("", "Korisničko ime se već koristi!");
                }

                if (ModelState.IsValid)
                {
                    var driverDB = _db.vozaci.Where(x => x.vozac_id == model.DriverID).FirstOrDefault();

                    driverDB.ime = model.FirstName;
                    driverDB.prezime = model.LastName;
                    driverDB.jmbg = model.PersonUniqueID;
                    driverDB.user = model.Username;
                    
                    _db.SaveChanges();

                    return Json(data: new { success = true });
                }
            }

            return PartialView(viewName: "_Edit", model: model);
        }

        [HttpGet]
        public ActionResult ChangePassword(int driverID)
        {
            ChangeDriverPasswordVM model = new ChangeDriverPasswordVM();

            using (TruckTransportDbContext _db = new TruckTransportDbContext())
            {
                var driver = _db.vozaci.AsNoTracking().Where(x => x.vozac_id == driverID).SingleOrDefault();

                if(driver != null)
                {
                    model.DriverID = driver.vozac_id;
                }
            }

            return PartialView(viewName: "_ChangePassword", model: model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult ChangePassword(ChangeDriverPasswordVM model)
        {
            using (TruckTransportDbContext _db = new TruckTransportDbContext())
            {
                if (ModelState.IsValid)
                {
                    var driver = _db.vozaci.Where(x => x.vozac_id == model.DriverID).SingleOrDefault();

                    if (driver != null)
                    {
                        driver.pass = model.NewPassword;
                        _db.SaveChanges();

                        return Json(new { success = true });
                    }
                    else
                    {
                        return Json(new { success = false });
                    }                                    
                }
            }

            return PartialView(viewName: "_ChangePassword", model: model);
        }
    }
}