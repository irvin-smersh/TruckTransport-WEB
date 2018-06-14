using System;
using System.Data.Entity;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using TruckTransport_Data.DbContext;
using TruckTransport_Web.Helper;
using TruckTransport_Web.Models;
using TruckTransport_Data.Entities;

namespace TruckTransport_Web.Controllers
{
    [Audit]
    [Authorization(isLoggedIn: true)]   
    public class TasksController : Controller
    {
        private DropdownMaker _dropdownMaker;

        public TasksController()
        {
            _dropdownMaker = new DropdownMaker();                
        }

        [HttpGet]
        public ActionResult Index()
        {
            GetTasksVM model = new GetTasksVM();

            using (TruckTransportDbContext _db = new TruckTransportDbContext())
            {
                model.TasksList = _db.zadaci
                    .Include(x => x.poznatelokacije)
                    .Select(x => new GetTasksVM.TaskVM
                    {
                        TaskID = x.zadatak_id,
                        TaskNumber = x.broj_zadatka,
                        Name = x.naziv,
                        Description = x.opis,
                        CheckIn = x.checkin,
                        CheckOut = x.checkout,
                        KnownLocation = x.poznatelokacije.naziv,
                        OrderID = x.nalog_id
                    }).AsNoTracking().ToList();              
            }

            return View(viewName: "Index", model: model);
        }

        [HttpGet]
        public ActionResult Details(int taskID)
        {
            GetTaskDetailsVM model = new GetTaskDetailsVM();

            using (TruckTransportDbContext _db = new TruckTransportDbContext())
            {
                zadaci task = _db.zadaci.Include(x=>x.poznatelokacije)
                                        .Include(x=>x.nalozi.stanja)
                                        .Include(x=>x.nalozi.vozaci)
                                        .Include(x=>x.nalozi.vozila).Where(x => x.zadatak_id == taskID).AsNoTracking().FirstOrDefault();

                if(task != null)
                {
                    model.TaskID = task.zadatak_id;
                    model.TaskNumber = task.broj_zadatka;
                    model.Name = task.naziv;
                    model.Description = task.opis;

                    model.CheckIn = task.checkin != null && task.checkin != 0 ? UnixTime.ConvertToDateTimeString((long)task.checkin) : "-";
                    model.CheckOut = task.checkout != null && task.checkout != 0 ? UnixTime.ConvertToDateTimeString((long)task.checkout) : "-";

                    model.KnownLocation = task.poznatelokacije.naziv;
                    model.Latitude = task.poznatelokacije.sirina;
                    model.Longitude = task.poznatelokacije.duzina;

                    if(task.nalog_id != null && task.nalozi != null)
                    {
                        model.OrderID = task.nalog_id;
                        model.OrderCreationTime = UnixTime.ConvertToDateTimeString(task.nalozi.vrijeme_kreiranja);
                        model.OrderCondition = task.nalozi.stanja.opis;
                        model.Driver = task.nalozi.vozaci.ime + " " + task.nalozi.vozaci.prezime;
                        model.Vehicle = task.nalozi.vozila.naziv + ", " + task.nalozi.vozila.marka;
                    }                  
                }
            }

            return View(viewName: "Details", model: model);
        }

        [HttpGet]
        public ActionResult Add()
        {
            AddTaskVM model = new AddTaskVM();
            model.KnownLocations = _dropdownMaker.GetKnownLocations();

            using (TruckTransportDbContext _db = new TruckTransportDbContext())
            {
                var knownLocationID = Convert.ToInt32(model.KnownLocations.First().Value);
                var selectedKnownLocation = _db.poznatelokacije.Where(x => x.poznatalokacija_id == knownLocationID).FirstOrDefault();

                if(selectedKnownLocation != null)
                {
                    model.Latitude = selectedKnownLocation.sirina;
                    model.Longitude = selectedKnownLocation.duzina;
                }
            }

            return View(viewName: "Add", model: model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Add(AddTaskVM model)
        {
            using (TruckTransportDbContext _db = new TruckTransportDbContext())
            {
                if(model.TaskNumber == 0)
                {
                    ModelState.AddModelError("", "Redni broj zadatka ne može biti 0!");
                }

                if (_db.zadaci.AsNoTracking().Where(x => x.naziv == model.Name && x.nalog_id == null && x.broj_zadatka == model.TaskNumber).FirstOrDefault() != null)
                {
                    ModelState.AddModelError("", "Nedodijeljen zadatak sa unesenim nazivom ili rednim brojem već postoji!");
                }

                if (ModelState.IsValid)
                {
                    zadaci taskDB = new zadaci();
                    taskDB.broj_zadatka = model.TaskNumber;
                    taskDB.naziv = model.Name;
                    taskDB.opis = model.Description;
                    taskDB.checkin = null;
                    taskDB.checkout = null;
                    taskDB.poznatalokacija_id = model.KnownLocationID;
                    taskDB.nalog_id = null;

                    _db.zadaci.Add(taskDB);
                    _db.SaveChanges();

                    return RedirectToAction(actionName: "Index");
                }              
            }

            model.KnownLocations = _dropdownMaker.GetKnownLocations();
            return View(viewName: "Add", model: model);
        }

        [HttpGet]
        public ActionResult Edit(int taskID)
        {
            EditTaskVM model = new EditTaskVM();

            using (TruckTransportDbContext _db = new TruckTransportDbContext())
            {
                zadaci task = _db.zadaci.AsNoTracking().Where(x => x.zadatak_id == taskID).FirstOrDefault();

                if (task != null)
                {
                    model.TaskID = task.zadatak_id;
                    model.Name = task.naziv;
                    model.Description = task.opis;
                    model.TaskNumber = task.broj_zadatka;
                    model.KnownLocationID = task.poznatalokacija_id;

                    var selectedKnownLocation = _db.poznatelokacije.Where(x => x.poznatalokacija_id == model.KnownLocationID).FirstOrDefault();

                    if (selectedKnownLocation != null)
                    {
                        model.Latitude = selectedKnownLocation.sirina;
                        model.Longitude = selectedKnownLocation.duzina;
                    }
                }

                model.KnownLocations = _dropdownMaker.GetKnownLocations();
            }

            return View(viewName: "Edit", model: model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(EditTaskVM model)
        {
            using (TruckTransportDbContext _db = new TruckTransportDbContext())
            {
                if (model.TaskNumber == 0)
                {
                    ModelState.AddModelError("", "Redni broj zadatka ne može biti 0!");
                }

                if (_db.zadaci.AsNoTracking().Where(x => x.naziv == model.Name && x.nalog_id == null && x.broj_zadatka == model.TaskNumber && x.zadatak_id != model.TaskID).FirstOrDefault() != null)
                {
                    ModelState.AddModelError("", "Nedodijeljen zadatak sa unesenim nazivom ili brojem već postoji!");
                }

                if (ModelState.IsValid)
                {
                    zadaci taskDB = _db.zadaci.Where(x => x.zadatak_id == model.TaskID).FirstOrDefault();

                    taskDB.broj_zadatka = model.TaskNumber;
                    taskDB.naziv = model.Name;
                    taskDB.opis = model.Description;
                    taskDB.poznatalokacija_id = model.KnownLocationID;

                    _db.Entry(taskDB).State = EntityState.Modified;
                    _db.SaveChanges();

                    return RedirectToAction(actionName: "Index");
                }               
            }

            model.KnownLocations = _dropdownMaker.GetKnownLocations();
            return View(viewName: "Edit", model: model);
        }

        [HttpGet]
        public ActionResult Delete(int taskID)
        {
            using (TruckTransportDbContext _db = new TruckTransportDbContext())
            {
                zadaci task = _db.zadaci.Where(x => x.zadatak_id == taskID).FirstOrDefault();

                if(task != null)
                {
                    _db.zadaci.Remove(task);
                    _db.SaveChanges();
                    return Json(new { success = true }, JsonRequestBehavior.AllowGet);
                }
                else
                {
                    return Json(new { success = false }, JsonRequestBehavior.AllowGet);
                }
            }
        }

        [HttpGet]
        public ActionResult GetTaskLocationsCoordinatesByIDs(int[] taskIDs)
        {
            using (TruckTransportDbContext _db = new TruckTransportDbContext())
            {
                List<GeoLocationVM> model = new List<GeoLocationVM>();

                for (int i = 0; i < taskIDs.Length; i++)
                {
                    int taskID = taskIDs[i];

                    GeoLocationVM singleLocation = _db.zadaci.Where(x => x.zadatak_id == taskID).Include(x => x.poznatelokacije).Select(x => new GeoLocationVM
                    {
                        ID = x.zadatak_id,
                        Info = x.naziv,
                        Latitude = x.poznatelokacije.sirina,
                        Longitude = x.poznatelokacije.duzina
                    }).AsNoTracking().FirstOrDefault();

                    model.Add(singleLocation);
                }

                if (model != null && model.Count != 0)
                {
                    return Json(new { success = true, taskLocations = model }, JsonRequestBehavior.AllowGet);
                }
                else
                {
                    return Json(new { success = false }, JsonRequestBehavior.AllowGet);
                }
            }
        }
    }
}