using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using TruckTransport_Data.DbContext;
using TruckTransport_Web.Helper;
using TruckTransport_Web.Models;

namespace TruckTransport_Web.Controllers
{
    [Audit]
    [Authorization(isLoggedIn: true)]
    public class DispatcherProfileController : Controller
    {
        private int _dispatcherUserID;

        public DispatcherProfileController()
        {
            _dispatcherUserID = Authentication.GetUserID() ?? 0;
        }

        [HttpGet]
        public ActionResult EditProfile()
        {
            EditProfileVM model = new EditProfileVM();

            using (TruckTransportDbContext _db = new TruckTransportDbContext()) 
            {
                var dispatcherUser = _db.dispecerlogin.AsNoTracking().Where(x => x.dispecerlogin_id == _dispatcherUserID).SingleOrDefault();

                if (dispatcherUser != null) 
                {
                    model.DispatcherUserID = dispatcherUser.dispecerlogin_id;

                    model.DispatcherUser = new EditPersonalInfoVM {
                        DispatcherUserID = dispatcherUser.dispecerlogin_id,
                        Email = dispatcherUser.email,
                        Username = dispatcherUser.username,
                        LastLoginTime = dispatcherUser.lastlogin.ToString()
                    };

                    model.DispatcherPassword = new ChangePasswordVM();
                }
            }
            
            return View(viewName: "EditProfile", model: model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult DispatcherUserInfoEdit(EditPersonalInfoVM model)
        {
            using (TruckTransportDbContext _db = new TruckTransportDbContext())
            {
                if (_db.dispecerlogin.AsNoTracking().Where(x => (x.username == model.Username || x.email == model.Email) && x.dispecerlogin_id != model.DispatcherUserID).FirstOrDefault() != null)
                {
                    ModelState.AddModelError("", "Korisničko ime ili email se već koristi!");
                }

                if (ModelState.IsValid) 
                {
                    var dispatcherUser = _db.dispecerlogin.Where(x => x.dispecerlogin_id == model.DispatcherUserID).SingleOrDefault();

                    if (dispatcherUser != null) {
                        dispatcherUser.username = model.Username;
                        dispatcherUser.email = model.Email;

                        _db.SaveChanges();

                        return Json(new { success = true });
                    }
                }

            }

            return PartialView(viewName: "_DispatcherUserPersonalInfo", model: model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult DispatcherUserChangePassword(ChangePasswordVM model) 
        {
            using (TruckTransportDbContext _db = new TruckTransportDbContext()) 
            {
                if (ModelState.IsValid) 
                {
                    var dispatcherUser = _db.dispecerlogin.Where(x => x.dispecerlogin_id == _dispatcherUserID).SingleOrDefault();

                    dispatcherUser.password = model.NewPassword;
                    _db.SaveChanges();

                    return Json(new { success = true });
                }
            }

            return PartialView(viewName: "_DispatcherUserPassword", model: model);
        }
    }
}