using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace TruckTransport_Web.Models
{
    public class AuditLoggingVM
    {
        public long AuditLoggingID { get; set; }
        public string Username { get; set; }
        public string IPAddress { get; set; }
        public string URL { get; set; }
        public string EventTime { get; set; }
    }
}