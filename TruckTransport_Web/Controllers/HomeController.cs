using System;
using System.Collections.Generic;
using System.Linq;
using System.Data.Entity;
using System.Web;
using System.Web.Mvc;
using TruckTransport_Data.DbContext;
using TruckTransport_Web.Helper;
using TruckTransport_Web.Models;

namespace TruckTransport_Web.Controllers
{
    [Audit]
    [Authorization(isLoggedIn: true)]
    public class HomeController : Controller
    {
        [HttpGet]
        public ActionResult Index()
        {
            IndexVM model = new IndexVM();

            using (TruckTransportDbContext _db = new TruckTransportDbContext())
            {
                model.Statistics = new GetStatisticsVM();
                model.Statistics.NumberOfFinishedOrders = _db.nalozi.Where(s => s.stanje_id == 3).Count();
                model.Statistics.NumberOfFinishedTasks = _db.zadaci.Include(s => s.nalozi).Where(s => s.nalozi.stanje_id == 3).Count();
                model.Statistics.NumberOfDrivers = _db.vozaci.Count();
                model.Statistics.NumberOfVehicles = _db.vozila.Count();

                model.ActiveDrivers = new GetDriversVM();
                model.ActiveDrivers.DriversList = _db.nalozi.Include(x => x.vozaci).Where(x => x.stanje_id == 1).Select(x => new GetDriversVM.DriverVM
                {
                    DriverID = x.vozaci.vozac_id,
                    FirstName = x.vozaci.ime,
                    LastName = x.vozaci.prezime,
                    PersonUniqueID = x.vozaci.jmbg,
                    Username = x.vozaci.user,
                    Status = "Aktivan"
                }).AsNoTracking().ToList();

                model.ActiveDriversLatestPositions = new List<GeoLocationVM>();
                foreach (var driver in _db.vozaci.ToList())
                {
                    var latestPosition = _db.geotacke.Include(x => x.nalozi).Where(x => x.nalozi.stanje_id == 1 && x.nalozi.vozac_id == driver.vozac_id).OrderByDescending(x => x.vrijeme).FirstOrDefault();

                    GeoLocationVM latestPositionVM = new GeoLocationVM();

                    if (latestPosition != null)
                    {
                        latestPositionVM.Info = UnixTime.ConvertToDateTimeString(latestPosition.vrijeme);
                        latestPositionVM.Latitude = latestPosition.sirina;
                        latestPositionVM.Longitude = latestPosition.duzina;
                    }

                    model.ActiveDriversLatestPositions.Add(latestPositionVM);
                }
            }

            return View(viewName: "Index", model: model);
        }

        [HttpGet]
        public ActionResult GetStatistics()
        {
            GetStatisticsVM model = new GetStatisticsVM();

            using (TruckTransportDbContext _db = new TruckTransportDbContext())
            {
                model.NumberOfFinishedOrders = _db.nalozi.Where(s => s.stanje_id == 3).Count();
                model.NumberOfFinishedTasks = _db.zadaci.Include(s => s.nalozi).Where(s => s.nalozi.stanje_id == 3).Count();
                model.NumberOfDrivers = _db.vozaci.Count();
                model.NumberOfVehicles = _db.vozila.Count();
            }

            return PartialView(viewName: "_Statistics", model: model);
        }

        [HttpGet]
        public ActionResult GetActiveDrivers()
        {
            GetDriversVM model = new GetDriversVM();

            using (TruckTransportDbContext _db = new TruckTransportDbContext())
            {
                model.DriversList = _db.nalozi.Include(x => x.vozaci).Where(x => x.stanje_id == 1).Select(x => new GetDriversVM.DriverVM
                {
                    DriverID = x.vozaci.vozac_id,
                    FirstName = x.vozaci.ime,
                    LastName = x.vozaci.prezime,
                    PersonUniqueID = x.vozaci.jmbg,
                    Username = x.vozaci.user,
                    Status = "Aktivan"
                }).AsNoTracking().ToList();
            }

            return PartialView(viewName: "_ActiveDrivers", model: model);
        }

        [HttpGet]
        public ActionResult GetAllActiveDriversLatestPosition()
        {
            List<GeoLocationVM> model = new List<GeoLocationVM>();

            using (TruckTransportDbContext _db = new TruckTransportDbContext())
            {
                foreach (var driver in _db.vozaci.ToList())
                {
                    var latestPosition = _db.geotacke.Include(x => x.nalozi).Where(x => x.nalozi.stanje_id == 1 && x.nalozi.vozac_id == driver.vozac_id).OrderByDescending(x => x.vrijeme).FirstOrDefault();

                    GeoLocationVM latestPositionVM = new GeoLocationVM();

                    if (latestPosition != null)
                    {
                        latestPositionVM.Info = UnixTime.ConvertToDateTimeString(latestPosition.vrijeme);
                        latestPositionVM.Latitude = latestPosition.sirina;
                        latestPositionVM.Longitude = latestPosition.duzina;
                    }

                    model.Add(latestPositionVM);
                }
            }

            return PartialView(viewName: "_ActiveDriversLatestPosition", model: model);
        }

        [HttpGet]
        public ActionResult SetSOS(string latitude, string longitude, string text, string time, int driverID, string driverFullName)
        {
            SetSOSVM model = new SetSOSVM();

            model.DriverID = driverID;
            model.DriverFullName = driverFullName;
            model.SOSMessageTime = time;
            model.SOSMessageText = text;
            model.Latitude = latitude;
            model.Longitude = longitude;

            return PartialView(viewName: "_SOS", model: model);
        }
    }
}