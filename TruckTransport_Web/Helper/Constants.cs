using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace TruckTransport_Web.Helper {
    public class Constants 
    {
        public static string CookieLabel = "Cookie";

        public static string UserSessionLabel = "LoggedInUser";
        public const int MaximumPasswordLength = 32;

        public static int NumberOfMessages = 0;
        public static int NumberOfSOSMessages = 0;
    }
}