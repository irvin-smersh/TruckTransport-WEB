using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using TruckTransport_Data.DbContext;
using TruckTransport_Data.Entities;
using TruckTransport_Web.Helper;
using TruckTransport_Web.Models;

namespace TruckTransport_Web.Controllers {
   
    [Audit]
    public class SystemAccessController : Controller
    {
        [HttpGet]
        public ActionResult Login()
        {
            if(Authentication.GetLoggedInUser() != null)
            {
               return RedirectToAction(actionName: "Index", controllerName: "Home");
            }

            LoginVM model = new LoginVM();

            return View(viewName: "Login", model: model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Login(LoginVM model) 
        {                                  
            if (ModelState.IsValid) 
            {
                using (TruckTransportDbContext _db = new TruckTransportDbContext())
                {
                    dispecerlogin userToFind = _db.dispecerlogin.AsNoTracking().Where(x => x.username == model.Username).SingleOrDefault();

                    if (userToFind != null && userToFind.password == model.Password) {
                        Authentication.StartSession(user: userToFind, rememberMe: model.RememberMe);

                        userToFind.lastlogin = UnixTime.GetUnixTimeNow();
                        _db.SaveChanges();

                        return RedirectToAction(actionName: "Index", controllerName: "Home");
                    }

                    ModelState.AddModelError("", "Pogrešno korisničko ime ili lozinka!");
                }
            }

            return View(viewName: "Login", model: model);                                          
        }
       
        public ActionResult Logout() 
        {
            Authentication.ClearSession();
            return RedirectToAction(actionName: "Login", controllerName: "SystemAccess");
        }
    }
}