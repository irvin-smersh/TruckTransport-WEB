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
    public class OrderConditionsController : Controller
    {     
        [HttpGet]
        public ActionResult Index()
        {
            GetOrderConditionsVM model = new GetOrderConditionsVM();

            using (TruckTransportDbContext _db = new TruckTransportDbContext()) 
            {
                model.OrderConditionsList = _db.stanja.Include(x => x.nalozi).AsNoTracking().ToList();
            }

            return View(viewName: "Index", model: model);
        }

        [HttpGet]
        public ActionResult Add()
        {
            AddOrderConditionVM model = new AddOrderConditionVM();
            return PartialView(viewName: "_Add", model: model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Add(AddOrderConditionVM model)
        {
            using (TruckTransportDbContext _db = new TruckTransportDbContext()) 
            {
                if (_db.stanja.AsNoTracking().Where(x => x.opis == model.Description).FirstOrDefault() != null)
                {
                    ModelState.AddModelError("", "Opis stanja naloga već postoji!");
                }

                if (ModelState.IsValid) 
                {
                    stanja newOrderConditionDB = new stanja {
                        opis = model.Description
                    };

                    _db.stanja.Add(newOrderConditionDB);
                    _db.SaveChanges();

                    return Json(data: new { success = true });
                }
            }

            return PartialView(viewName: "_Add", model: model);
        }

        [HttpGet]
        public ActionResult Edit(int orderConditionID) 
        {
            EditOrderConditionVM model = new EditOrderConditionVM();

            using (TruckTransportDbContext _db = new TruckTransportDbContext())
            {
                var orderCondition = _db.stanja.AsNoTracking().Where(x => x.stanje_id == orderConditionID).FirstOrDefault();

                if (orderCondition != null)
                {
                    model.OrderConditionID = orderCondition.stanje_id;
                    model.Description = orderCondition.opis;
                }
            }

            return PartialView(viewName: "_Edit", model: model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(EditOrderConditionVM model)
        {
            using (TruckTransportDbContext _db = new TruckTransportDbContext())
            {
                if (_db.stanja.AsNoTracking().Where(x => x.opis == model.Description && x.stanje_id != model.OrderConditionID).FirstOrDefault() != null)
                {
                    ModelState.AddModelError("", "Opis stanja naloga već postoji!");
                }

                if (ModelState.IsValid)
                {
                    var orderConditionDB = _db.stanja.Where(x => x.stanje_id == model.OrderConditionID).FirstOrDefault();

                    orderConditionDB.opis = model.Description;
                    _db.SaveChanges();

                    return Json(data: new { success = true });
                }
            }

            return PartialView(viewName: "_Edit", model: model);
        }

        public ActionResult Delete(int orderConditionID)
        {
            using (TruckTransportDbContext _db = new TruckTransportDbContext())
            {
                var orderConditionDB = _db.stanja.Where(x => x.stanje_id == orderConditionID).FirstOrDefault();

                if (orderConditionDB != null)
                {
                    _db.stanja.Remove(orderConditionDB);
                    _db.SaveChanges();

                    return Json(data: new { success = true }, behavior: JsonRequestBehavior.AllowGet);
                }
            }

            return Json(data: new { success = false }, behavior: JsonRequestBehavior.AllowGet);
        }
    }
}