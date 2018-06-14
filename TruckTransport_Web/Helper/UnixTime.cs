using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace TruckTransport_Web.Helper
{
    public class UnixTime
    {
        public static long GetUnixTimeNow()
        {
            var timeSpan = (DateTime.UtcNow - new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc));
            return (long)timeSpan.TotalSeconds;
        }

        public static string ConvertToDateTimeString(long unixTime)
        {           
            DateTime dtDateTime = new DateTime(1970, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc);
            dtDateTime = dtDateTime.AddSeconds(unixTime).ToLocalTime();
            return dtDateTime.ToString("dd.MM.yyyy HH:mm");
        }
    }
}