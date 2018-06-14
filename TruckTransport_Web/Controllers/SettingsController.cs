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
    public class SettingsController : Controller
    {        
        [HttpGet]
        public ActionResult Index()
        {
            GetSettingsVM model = new GetSettingsVM();

            using (TruckTransportDbContext _db = new TruckTransportDbContext()) 
            {
                model.SettingsList = _db.postavke.AsNoTracking().ToList();
            }

            return View(viewName: "Index", model: model);
        }

        [HttpGet]
        public ActionResult Add()
        {
            AddSettingsVM model = new AddSettingsVM();
            return PartialView(viewName: "_Add", model: model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Add(AddSettingsVM model) 
        {
            using (TruckTransportDbContext _db = new TruckTransportDbContext()) 
            {
                if (_db.postavke.AsNoTracking().Where(x => x.kljuc == model.KeyName).FirstOrDefault() != null)
                {
                    ModelState.AddModelError("", "Naziv postavke već postoji!");
                }

                if (ModelState.IsValid) 
                {
                    postavke newSettingDB = new postavke {
                        kljuc = model.KeyName,
                        vrijednost = model.Value
                    };

                    _db.postavke.Add(newSettingDB);
                    _db.SaveChanges();

                    return Json(data: new { success = true });
                }
            }

            return PartialView(viewName: "_Add", model: model);
        }

        [HttpGet]
        public ActionResult Edit(int settingID) 
        {
            EditSettingsVM model = new EditSettingsVM();

            using (TruckTransportDbContext _db = new TruckTransportDbContext())
            {
                var setting = _db.postavke.AsNoTracking().Where(x => x.postavka_id == settingID).FirstOrDefault();             
                if (setting != null) 
                {
                    model.SettingID = setting.postavka_id;
                    model.KeyName = setting.kljuc;
                    model.Value = setting.vrijednost;
                }
            }

            return PartialView(viewName: "_Edit", model: model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(EditSettingsVM model)
        {
            using (TruckTransportDbContext _db = new TruckTransportDbContext()) 
            {
                if (ModelState.IsValid) 
                {
                    var settingDB = _db.postavke.Where(x => x.postavka_id == model.SettingID).FirstOrDefault();

                    settingDB.vrijednost = model.Value;
                    _db.SaveChanges();

                    return Json(data: new { success = true });
                }
            }

            return PartialView(viewName: "_Edit", model: model);
        }
      
        public ActionResult Delete(int settingID)
        {
            using (TruckTransportDbContext _db = new TruckTransportDbContext())
            {
                var settingDB = _db.postavke.Where(x => x.postavka_id == settingID).FirstOrDefault();

                if (settingDB != null) 
                {
                    _db.postavke.Remove(settingDB);
                    _db.SaveChanges();

                    return Json(data: new { success = true }, behavior: JsonRequestBehavior.AllowGet);
                }
            }

            return Json(data: new { success = false }, behavior: JsonRequestBehavior.AllowGet);
        }
    }
}