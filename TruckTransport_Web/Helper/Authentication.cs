using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using TruckTransport_Data.DbContext;
using TruckTransport_Data.Entities;

namespace TruckTransport_Web.Helper {
    public class Authentication 
    {
        private const string _loggedInUser = "LoggedInUser";
        private const string _session = "Session";

        public static void StartSession(dispecerlogin user, bool rememberMe = false)
        {
            HttpContext.Current.Session.Add(Constants.UserSessionLabel, user.dispecerlogin_id);

            if (rememberMe) 
            {
                HttpCookie cookie = new HttpCookie(Constants.CookieLabel, user.dispecerlogin_id.ToString() ?? "");
                cookie.Expires = DateTime.Now.AddHours(8);
                HttpContext.Current.Response.Cookies.Add(cookie);
            }
        }

        public static void ClearSession() 
        {
            ClearSessionValues();
            ClearCookieValues();
        }

        private static void ClearSessionValues() 
        {
            if (HttpContext.Current.Session != null)
                HttpContext.Current.Session[Constants.UserSessionLabel] = null;
        }

        private static void ClearCookieValues()
        {
            HttpContext.Current.Response.Cookies[Constants.CookieLabel].Value = null;
        }

        public static dispecerlogin GetLoggedInUser() 
        {
            dispecerlogin currentUser = GetUserIfAny();

            if (currentUser != null)
                return currentUser;

            HttpCookie cookie = HttpContext.Current.Request.Cookies.Get(Constants.CookieLabel);

            if (cookie != null) 
            {
                int userID = 0;
                bool found = int.TryParse(cookie.Value, out userID);

                if (found) 
                {
                    dispecerlogin userToLogin = null;
                    try {
                        using (TruckTransportDbContext db = new TruckTransportDbContext()) 
                        {
                            userToLogin = db.dispecerlogin.AsNoTracking().Where(x=> x.dispecerlogin_id == userID).SingleOrDefault();
                            StartSession(userToLogin, true);
                        }
                        return userToLogin;
                    }
                    catch 
                    {
                        return null;
                    }
                }
            }

            return null;
        }

        public static dispecerlogin GetUserIfAny() 
        {
            if (HttpContext.Current.Session[Constants.UserSessionLabel] != null) 
            {
                int userID = (int)HttpContext.Current.Session[Constants.UserSessionLabel];

                using (TruckTransportDbContext db = new TruckTransportDbContext())
                {
                    return db.dispecerlogin.AsNoTracking().Where(x => x.dispecerlogin_id == userID).SingleOrDefault();
                }
            }

            return null;
        }

        public static int? GetUserID()
        {
            if (HttpContext.Current.Session[Constants.UserSessionLabel] != null)
            {
                dispecerlogin currentUser = GetUserIfAny();
                return currentUser.dispecerlogin_id;
            }

            return null;
        }

    }
}