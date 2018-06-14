using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using TruckTransport_Data.Entities;

namespace TruckTransport_Web.Models
{
    public class GetAuditLoggingsVM
    {
        public List<AuditLoggingVM> AuditLoggingsList { get; set; }
    }
}