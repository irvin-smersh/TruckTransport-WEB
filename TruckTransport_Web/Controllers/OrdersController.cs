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
    public class OrdersController : Controller
    {
        private DropdownMaker _dropdownMaker;

        public OrdersController()
        {
            _dropdownMaker = new DropdownMaker();
        }

        [HttpGet]
        public ActionResult Index()
        {
            GetOrdersVM model = new GetOrdersVM();

            using (TruckTransportDbContext _db = new TruckTransportDbContext())
            {
                model.OrdersList = _db.nalozi
                    .Include(x => x.stanja)
                    .Include(x => x.vozila)
                    .Include(x => x.vozaci).Select(x => new GetOrdersVM.OrderVM
                    {
                        OrderID = x.nalog_id,
                        UnixCreationTime = x.vrijeme_kreiranja,
                        OrderCondition = x.stanja.opis,
                        Vehicle = x.vozila.naziv,
                        Driver = x.vozaci.ime + " " + x.vozaci.prezime
                    }).AsNoTracking().ToList();

                foreach (var order in model.OrdersList)
                {
                    order.CreationTime = UnixTime.ConvertToDateTimeString(order.UnixCreationTime);
                }
            }

            return View(viewName: "Index", model: model);
        }

        [HttpGet]
        public ActionResult Details(int orderID)
        {
            GetOrderDetailsVM model = new GetOrderDetailsVM();

            using (TruckTransportDbContext _db = new TruckTransportDbContext())
            {
                var order = _db.nalozi.Where(x => x.nalog_id == orderID)
                                      .Include(x => x.stanja)
                                      .Include(x => x.vozila)
                                      .Include(x => x.vozaci).AsNoTracking().FirstOrDefault();

                if (order != null)
                {
                    model.OrderID = order.nalog_id;
                    model.OrderCreationTime = UnixTime.ConvertToDateTimeString(order.vrijeme_kreiranja);
                    model.OrderCondition = order.stanja.opis;
                    model.Vehicle = order.vozila.naziv + ", " + order.vozila.marka;
                    model.Driver = order.vozaci.ime + " " + order.vozaci.prezime;

                    var tasks = _db.zadaci.Where(x => x.nalog_id == order.nalog_id).Include(x => x.poznatelokacije).AsNoTracking().ToList();
                    var driverStops = _db.stajalista_nalozi.Where(x => x.nalog_id == order.nalog_id).Include(x => x.stajalista).AsNoTracking().ToList();

                    if (tasks != null && tasks.Count != 0)
                    {
                        model.Tasks = new List<GetOrderDetailsVM.TaskDetails>();

                        foreach (var task in tasks)
                        {
                            GetOrderDetailsVM.TaskDetails taskModel = new GetOrderDetailsVM.TaskDetails();

                            taskModel.TaskID = task.zadatak_id;
                            taskModel.TaskNumber = task.broj_zadatka;
                            taskModel.Name = task.naziv;
                            taskModel.Description = task.opis;
                            taskModel.CheckIn = task.checkin != null ? UnixTime.ConvertToDateTimeString((long)task.checkin) : "";
                            taskModel.CheckOut = task.checkout != null ? UnixTime.ConvertToDateTimeString((long)task.checkout) : "";
                            taskModel.KnownLocation = task.poznatelokacije.naziv;
                            taskModel.Latitude = task.poznatelokacije.sirina;
                            taskModel.Longitude = task.poznatelokacije.duzina;

                            model.Tasks.Add(taskModel);
                        }
                    }

                    if (driverStops != null && driverStops.Count != 0)
                    {
                        model.DriverStops = new List<GetOrderDetailsVM.DriverStopDetails>();

                        foreach (var driverStop in driverStops)
                        {
                            GetOrderDetailsVM.DriverStopDetails driverStopModel = new GetOrderDetailsVM.DriverStopDetails();

                            driverStopModel.DriverStopID = driverStop.stajaliste_id;
                            driverStopModel.DriverStopNumber = driverStop?.broj_stajalista ?? 0;
                            driverStopModel.DriverStop = driverStop.stajalista.naziv;
                            driverStopModel.CheckIn = driverStop.checkin != null ? UnixTime.ConvertToDateTimeString((long)driverStop.checkin) : "";
                            driverStopModel.CheckOut = driverStop.checkout != null ? UnixTime.ConvertToDateTimeString((long)driverStop.checkout) : "";
                            driverStopModel.Latitude = driverStop.stajalista.sirina;
                            driverStopModel.Longitude = driverStop.stajalista.duzina;

                            model.DriverStops.Add(driverStopModel);
                        }
                    }
                }
            }

            return View(viewName: "Details", model: model);
        }

        [HttpGet]
        public ActionResult Add()
        {
            AddOrderVM model = new AddOrderVM();

            model.OrderConditionID = 2;

            model.Drivers = _dropdownMaker.GetDrivers();
            model.Vehicles = _dropdownMaker.GetVehicles();

            model.Tasks = _dropdownMaker.GetUnattachedTasks();
            model.DriverStops = _dropdownMaker.GetDriverStops();

            model.TaskAndDriverStopNumbers = new List<AssignTaskAndDriverStopNumbersInOrderVM>();

            return View(viewName: "Add", model: model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Add(AddOrderVM model)
        {
            if (model.TaskIDs != null && model.DriverStopIDs != null)
            {
                if (model.TaskIDs.Length > 10 || model.DriverStopIDs.Length > 10)
                {
                    ModelState.AddModelError("", "Broj zadataka i broj stajališta ne može biti veći od 10!");
                }
                else if(model.TaskAndDriverStopNumbers == null)
                {
                    ModelState.AddModelError("", "Redni brojevi za sve odabrane zadatke i stajališta nisu dodijeljeni!");
                }
                else if (model.TaskAndDriverStopNumbers != null && model.TaskIDs.Length + model.DriverStopIDs.Length > model.TaskAndDriverStopNumbers.Count)
                {                    
                    ModelState.AddModelError("", "Redni brojevi za sve odabrane zadatke i stajališta nisu dodijeljeni!");                    
                }
            }

            if (model.TaskIDs != null && model.DriverStopIDs == null)
            {
                if (model.TaskIDs.Length > 2)
                {
                    ModelState.AddModelError("", "Stajalište je obavezno ako je broj zadataka veći od 2!");
                }

                if(model.TaskAndDriverStopNumbers == null)
                {
                    ModelState.AddModelError("", "Redni brojevi za sve odabrane zadatke nisu dodijeljeni!");                    
                } 
                else if (model.TaskIDs.Length > model.TaskAndDriverStopNumbers.Count)
                {
                    ModelState.AddModelError("", "Redni brojevi za sve odabrane zadatke nisu dodijeljeni!");
                }
            }

            //check difference between numbers in list
            if (model.TaskAndDriverStopNumbers != null)
            {
                if (model.TaskAndDriverStopNumbers.Count != 0)
                {
                    var descOrderedNumbers = model.TaskAndDriverStopNumbers.OrderByDescending(x => x.Number);
                   
                    if(descOrderedNumbers.Last().Number != 1)
                    {
                        ModelState.AddModelError("", "Najmanji redni broj mora biti 1!");
                    }

                    int orderNumberErrorCounter = 0;

                    for (int i = 0; i < descOrderedNumbers.Count(); i++)
                    {
                        if (descOrderedNumbers.Count() > 1 && (i + 1) < descOrderedNumbers.Count())
                        {
                            int difference = descOrderedNumbers.ElementAt(i).Number - descOrderedNumbers.ElementAt(i + 1).Number;

                            if (difference != 1 && orderNumberErrorCounter == 0)
                            {
                                orderNumberErrorCounter = 1;
                                ModelState.AddModelError("", "Redni brojevi nisu ispravno raspoređeni. Razlika između dva broja mora biti 1!");
                            }
                        }

                    }
                }
            }

            if (ModelState.IsValid)
            {
                try
                {
                    using (TruckTransportDbContext _db = new TruckTransportDbContext())
                    {
                        nalozi nalogDB = new nalozi();
                        nalogDB.vrijeme_kreiranja = UnixTime.GetUnixTimeNow();
                        nalogDB.stanje_id = model.OrderConditionID;
                        nalogDB.vozilo_id = model.VehicleID;
                        nalogDB.vozac_id = model.DriverID;
                        _db.nalozi.Add(nalogDB);
                        _db.SaveChanges();

                        foreach (var taskID in model.TaskIDs)
                        {
                            zadaci taskDB = _db.zadaci.Where(x => x.zadatak_id == taskID).FirstOrDefault();

                            if (taskDB != null)
                            {
                                taskDB.nalog_id = nalogDB.nalog_id;
                                taskDB.broj_zadatka = model.TaskAndDriverStopNumbers.Where(x => x.ID == taskDB.zadatak_id && x.IsTask).FirstOrDefault().Number;
                                _db.Entry(taskDB).State = EntityState.Modified;
                            }
                        }

                        if (model.DriverStopIDs != null)
                        {
                            List<stajalista_nalozi> driverStopsOrders = new List<stajalista_nalozi>();

                            foreach (var driverStopID in model.DriverStopIDs)
                            {
                                stajalista_nalozi driverStopOrderDB = new stajalista_nalozi();
                                driverStopOrderDB.stajaliste_id = (int)driverStopID;
                                driverStopOrderDB.nalog_id = nalogDB.nalog_id;
                                driverStopOrderDB.broj_stajalista = model.TaskAndDriverStopNumbers.Where(x => x.ID == driverStopOrderDB.stajaliste_id && !x.IsTask).FirstOrDefault().Number;
                                driverStopsOrders.Add(driverStopOrderDB);
                            }

                            _db.stajalista_nalozi.AddRange(driverStopsOrders);
                        }

                        _db.SaveChanges();

                        return RedirectToAction(actionName: "Index");
                    }
                }
                catch (Exception)
                {

                }
            }

            //delete selected values because of map init
            model.TaskIDs = null;
            model.DriverStopIDs = null;
            model.TaskAndDriverStopNumbers = new List<AssignTaskAndDriverStopNumbersInOrderVM>();
           
            model.Drivers = _dropdownMaker.GetDrivers();
            model.Vehicles = _dropdownMaker.GetVehicles();

            model.Tasks = _dropdownMaker.GetUnattachedTasks();
            model.DriverStops = _dropdownMaker.GetDriverStops();

            return View(viewName: "Add", model: model);
        }

        [HttpGet]
        public ActionResult Edit(int orderID)
        {
            EditOrderVM model = new EditOrderVM();

            using (TruckTransportDbContext _db = new TruckTransportDbContext())
            {
                var order = _db.nalozi.Where(x => x.nalog_id == orderID).AsNoTracking().FirstOrDefault();

                if (order != null)
                {
                    var taskList = _db.zadaci.Where(x => x.nalog_id == order.nalog_id).AsNoTracking().ToList();
                    var orderStopsList = _db.stajalista_nalozi.Where(x => x.nalog_id == order.nalog_id).ToList();

                    model.OrderID = order.nalog_id;
                    model.DriverID = order.vozac_id;
                    model.VehicleID = order.vozilo_id;

                    if (taskList != null)
                    {
                        model.TaskIDs = new int[taskList.Count];

                        for (int i = 0; i < taskList.Count; i++)
                        {
                            model.TaskIDs.SetValue(taskList[i].zadatak_id, i);
                        }
                    }

                    if (orderStopsList != null)
                    {
                        model.DriverStopIDs = new int?[orderStopsList.Count];

                        for (int i = 0; i < orderStopsList.Count; i++)
                        {
                            model.DriverStopIDs.SetValue(orderStopsList[i].stajaliste_id, i);
                        }
                    }
                }

                model.Drivers = _dropdownMaker.GetDrivers();
                model.Vehicles = _dropdownMaker.GetVehicles();

                model.Tasks = _dropdownMaker.GetUnattachedAndSpecificOrderAttachedTasks(orderID);
                model.DriverStops = _dropdownMaker.GetDriverStops();

                model.TaskAndDriverStopNumbers = new List<AssignTaskAndDriverStopNumbersInOrderVM>();

                foreach (var taskID in model.TaskIDs)
                {
                    AssignTaskAndDriverStopNumbersInOrderVM taskAndDriverStopNumberModel = new AssignTaskAndDriverStopNumbersInOrderVM();
                    var task = _db.zadaci.Where(x => x.zadatak_id == taskID).AsNoTracking().FirstOrDefault();

                    taskAndDriverStopNumberModel.ID = task.zadatak_id;
                    taskAndDriverStopNumberModel.Name = task.naziv;
                    taskAndDriverStopNumberModel.IsTask = true;
                    taskAndDriverStopNumberModel.Number = task.broj_zadatka;

                    model.TaskAndDriverStopNumbers.Add(taskAndDriverStopNumberModel);
                }

                if (model.DriverStopIDs != null)
                {
                    foreach (var driverStopID in model.DriverStopIDs)
                    {
                        if (driverStopID != null)
                        {
                            AssignTaskAndDriverStopNumbersInOrderVM taskAndDriverStopNumberModel = new AssignTaskAndDriverStopNumbersInOrderVM();
                            var driverStop = _db.stajalista_nalozi.Include(x => x.stajalista).Where(x => x.stajaliste_id == driverStopID).AsNoTracking().FirstOrDefault();

                            taskAndDriverStopNumberModel.ID = driverStop.stajaliste_id;
                            taskAndDriverStopNumberModel.Name = driverStop.stajalista.naziv;
                            taskAndDriverStopNumberModel.IsTask = false;
                            taskAndDriverStopNumberModel.Number = driverStop?.broj_stajalista ?? 0;

                            model.TaskAndDriverStopNumbers.Add(taskAndDriverStopNumberModel);
                        }
                    }
                }
            }

            return View(viewName: "Edit", model: model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(EditOrderVM model)
        {
            if (model.TaskIDs != null && model.DriverStopIDs != null)
            {
                if (model.TaskIDs.Length > 10 || model.DriverStopIDs.Length > 10)
                {
                    ModelState.AddModelError("", "Broj zadataka i broj stajališta ne može biti veći od 10!");
                }
                else if (model.TaskAndDriverStopNumbers == null)
                {
                    ModelState.AddModelError("", "Redni brojevi za sve odabrane zadatke i stajališta nisu dodijeljeni!");
                }
                else if (model.TaskAndDriverStopNumbers != null && model.TaskIDs.Length + model.DriverStopIDs.Length > model.TaskAndDriverStopNumbers.Count)
                {
                    ModelState.AddModelError("", "Redni brojevi za sve odabrane zadatke i stajališta nisu dodijeljeni!");
                }
            }

            if (model.TaskIDs != null && model.DriverStopIDs == null)
            {
                if (model.TaskIDs.Length > 2)
                {
                    ModelState.AddModelError("", "Stajalište je obavezno ako je broj zadataka veći od 2!");
                }

                if (model.TaskAndDriverStopNumbers == null)
                {
                    ModelState.AddModelError("", "Redni brojevi za sve odabrane zadatke nisu dodijeljeni!");
                }
                else if (model.TaskIDs.Length > model.TaskAndDriverStopNumbers.Count)
                {
                    ModelState.AddModelError("", "Redni brojevi za sve odabrane zadatke nisu dodijeljeni!");
                }
            }

            //check difference between numbers in list
            if (model.TaskAndDriverStopNumbers != null)
            {
                if (model.TaskAndDriverStopNumbers.Count != 0)
                {
                    var descOrderedNumbers = model.TaskAndDriverStopNumbers.OrderByDescending(x => x.Number);

                    if (descOrderedNumbers.Last().Number != 1)
                    {
                        ModelState.AddModelError("", "Najmanji redni broj mora biti 1!");
                    }

                    int orderNumberErrorCounter = 0;

                    for (int i = 0; i < descOrderedNumbers.Count(); i++)
                    {
                        if (descOrderedNumbers.Count() > 1 && (i + 1) < descOrderedNumbers.Count())
                        {
                            int difference = descOrderedNumbers.ElementAt(i).Number - descOrderedNumbers.ElementAt(i + 1).Number;

                            if (difference != 1 && orderNumberErrorCounter == 0)
                            {
                                orderNumberErrorCounter = 1;
                                ModelState.AddModelError("", "Redni brojevi nisu ispravno raspoređeni. Razlika između dva broja mora biti 1!");
                            }
                        }

                    }
                }
            }

            if (ModelState.IsValid)
            {
                try
                {
                    using (TruckTransportDbContext _db = new TruckTransportDbContext())
                    {
                        nalozi nalogDB = _db.nalozi.Where(x => x.nalog_id == model.OrderID).FirstOrDefault();

                        nalogDB.vrijeme_kreiranja = UnixTime.GetUnixTimeNow();
                        nalogDB.vozilo_id = model.VehicleID;
                        nalogDB.vozac_id = model.DriverID;

                        _db.Entry(nalogDB).State = EntityState.Modified;

                        var previousTaskList = _db.zadaci.Where(x => x.nalog_id == model.OrderID).ToList();

                        if (previousTaskList != null)
                        {
                            foreach (var previousTask in previousTaskList)
                            {
                                previousTask.nalog_id = null;
                                _db.Entry(previousTask).State = EntityState.Modified;
                            }
                        }

                        foreach (var taskID in model.TaskIDs)
                        {
                            zadaci taskDB = _db.zadaci.Where(x => x.zadatak_id == taskID).FirstOrDefault();

                            if (taskDB != null)
                            {
                                taskDB.nalog_id = nalogDB.nalog_id;
                                taskDB.broj_zadatka = model.TaskAndDriverStopNumbers.Where(x => x.ID == taskDB.zadatak_id && x.IsTask).FirstOrDefault().Number;
                                _db.Entry(taskDB).State = EntityState.Modified;
                            }
                        }

                        var previousDriverStops = _db.stajalista_nalozi.Where(x => x.nalog_id == model.OrderID).ToList();

                        if (previousDriverStops != null)
                        {
                            _db.stajalista_nalozi.RemoveRange(previousDriverStops);
                        }

                        if (model.DriverStopIDs != null)
                        {
                            List<stajalista_nalozi> driverStopsOrders = new List<stajalista_nalozi>();

                            foreach (var driverStopID in model.DriverStopIDs)
                            {
                                stajalista_nalozi driverStopOrderDB = new stajalista_nalozi();
                                driverStopOrderDB.stajaliste_id = (int)driverStopID;
                                driverStopOrderDB.nalog_id = nalogDB.nalog_id;
                                driverStopOrderDB.broj_stajalista = model.TaskAndDriverStopNumbers.Where(x => x.ID == driverStopOrderDB.stajaliste_id && !x.IsTask).FirstOrDefault().Number;
                                driverStopsOrders.Add(driverStopOrderDB);
                            }

                            _db.stajalista_nalozi.AddRange(driverStopsOrders);
                        }

                        _db.SaveChanges();

                        return RedirectToAction(actionName: "Index");
                    }
                }
                catch (Exception)
                {

                }
            }

            model.Drivers = _dropdownMaker.GetDrivers();
            model.Vehicles = _dropdownMaker.GetVehicles();

            model.Tasks = _dropdownMaker.GetUnattachedAndSpecificOrderAttachedTasks(model.OrderID);
            model.DriverStops = _dropdownMaker.GetDriverStops();

            return View(viewName: "Edit", model: model);
        }

        [HttpGet]
        public ActionResult Delete(int orderID)
        {
            using (TruckTransportDbContext _db = new TruckTransportDbContext())
            {
                nalozi order = _db.nalozi.Where(x => x.nalog_id == orderID).FirstOrDefault();

                if (order != null)
                {
                    var tasks = _db.zadaci.Where(x => x.nalog_id == order.nalog_id).ToList();

                    if (tasks != null && tasks.Count != 0)
                    {
                        _db.zadaci.RemoveRange(tasks);
                    }


                    var geoLocations = _db.geotacke.Where(x => x.nalog_id == order.nalog_id).ToList();

                    if (geoLocations != null && geoLocations.Count != 0)
                    {
                        _db.geotacke.RemoveRange(geoLocations);
                    }

                    _db.nalozi.Remove(order);
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
        public ActionResult AddSetTaskAndDriverStopNumbers(int[] taskIDs, int?[] driverStopIDs)
        {
            AddOrderVM model = new AddOrderVM();
            model.TaskAndDriverStopNumbers = new List<AssignTaskAndDriverStopNumbersInOrderVM>();

            using (TruckTransportDbContext _db = new TruckTransportDbContext())
            {
                foreach(var taskID in taskIDs)
                {
                    AssignTaskAndDriverStopNumbersInOrderVM taskAndDriverStopNumberModel = new AssignTaskAndDriverStopNumbersInOrderVM();
                    var task = _db.zadaci.Where(x => x.zadatak_id == taskID).AsNoTracking().FirstOrDefault();

                    taskAndDriverStopNumberModel.ID = task.zadatak_id;
                    taskAndDriverStopNumberModel.Name = task.naziv;
                    taskAndDriverStopNumberModel.IsTask = true;
                    taskAndDriverStopNumberModel.Number = task.broj_zadatka;

                    model.TaskAndDriverStopNumbers.Add(taskAndDriverStopNumberModel);
                }

                if (driverStopIDs != null)
                {
                    foreach (var driverStopID in driverStopIDs)
                    {
                        if (driverStopID != null)
                        {
                            AssignTaskAndDriverStopNumbersInOrderVM taskAndDriverStopNumberModel = new AssignTaskAndDriverStopNumbersInOrderVM();
                            var driverStop = _db.stajalista.Where(x => x.stajaliste_id == driverStopID).AsNoTracking().FirstOrDefault();

                            taskAndDriverStopNumberModel.ID = driverStop.stajaliste_id;
                            taskAndDriverStopNumberModel.Name = driverStop.naziv;
                            taskAndDriverStopNumberModel.IsTask = false;
                            taskAndDriverStopNumberModel.Number = 1;

                            model.TaskAndDriverStopNumbers.Add(taskAndDriverStopNumberModel);
                        }
                    }
                }
            }

            return PartialView(viewName: "_AddAssignTaskAndDriverStopNumbers", model: model);
        }

        [HttpGet]
        public ActionResult EditSetTaskAndDriverStopNumbers(int[] taskIDs, int?[] driverStopIDs)
        {
            EditOrderVM model = new EditOrderVM();
            model.TaskAndDriverStopNumbers = new List<AssignTaskAndDriverStopNumbersInOrderVM>();

            using (TruckTransportDbContext _db = new TruckTransportDbContext())
            {
                foreach (var taskID in taskIDs)
                {
                    AssignTaskAndDriverStopNumbersInOrderVM taskAndDriverStopNumberModel = new AssignTaskAndDriverStopNumbersInOrderVM();
                    var task = _db.zadaci.Where(x => x.zadatak_id == taskID).AsNoTracking().FirstOrDefault();

                    if (task != null)
                    {
                        taskAndDriverStopNumberModel.ID = task.zadatak_id;
                        taskAndDriverStopNumberModel.Name = task.naziv;
                        taskAndDriverStopNumberModel.IsTask = true;
                        taskAndDriverStopNumberModel.Number = task.broj_zadatka;

                        model.TaskAndDriverStopNumbers.Add(taskAndDriverStopNumberModel);
                    }
                }

                if (driverStopIDs != null)
                {
                    foreach (var driverStopID in driverStopIDs)
                    {
                        if (driverStopID != null)
                        {
                            AssignTaskAndDriverStopNumbersInOrderVM taskAndDriverStopNumberModel = new AssignTaskAndDriverStopNumbersInOrderVM();
                            var driverStopOrder = _db.stajalista_nalozi.Include(x => x.stajalista).Where(x => x.stajaliste_id == driverStopID).AsNoTracking().FirstOrDefault();

                            if (driverStopOrder != null)
                            {
                                taskAndDriverStopNumberModel.ID = driverStopOrder.stajaliste_id;
                                taskAndDriverStopNumberModel.Name = driverStopOrder.stajalista.naziv;
                                taskAndDriverStopNumberModel.IsTask = false;
                                taskAndDriverStopNumberModel.Number = driverStopOrder.broj_stajalista;

                                model.TaskAndDriverStopNumbers.Add(taskAndDriverStopNumberModel);
                            }
                            else
                            {
                                var driverStop = _db.stajalista.Where(x => x.stajaliste_id == driverStopID).AsNoTracking().FirstOrDefault();

                                taskAndDriverStopNumberModel.ID = driverStop.stajaliste_id;
                                taskAndDriverStopNumberModel.Name = driverStop.naziv;
                                taskAndDriverStopNumberModel.IsTask = false;
                                taskAndDriverStopNumberModel.Number = 1;

                                model.TaskAndDriverStopNumbers.Add(taskAndDriverStopNumberModel);
                            }
                        }
                    }
                }
            }

            return PartialView(viewName: "_EditAssignTaskAndDriverStopNumbers", model: model);
        }
    }
}