using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using TruckTransport_Data.Entities;

namespace TruckTransport_Web.Helper {
    public class Authorization : FilterAttribute, IAuthorizationFilter 
    {
        private readonly bool _isLoggedIn;

        public Authorization(bool isLoggedIn = false)
        {
            _isLoggedIn = isLoggedIn;
        }

        public void OnAuthorization(AuthorizationContext filterContext)
        {
            dispecerlogin user = Authentication.GetLoggedInUser();
            bool accessGranted = false;

            if(user != null && _isLoggedIn == true)
            {
                accessGranted = true;
            }

            if (!accessGranted)
                filterContext.HttpContext.Response.Redirect("/SystemAccess/Login");
        }
    }
}