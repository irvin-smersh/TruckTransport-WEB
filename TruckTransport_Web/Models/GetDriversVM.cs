using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using TruckTransport_Data.Entities;

namespace TruckTransport_Web.Models {
    public class GetDriversVM
    {
        public List<DriverVM> DriversList { get; set; }

        public class DriverVM
        {
            public int DriverID { get; set; }
            public string FirstName { get; set; }
            public string LastName { get; set; }
            public string PersonUniqueID { get; set; }
            public string Username { get; set; }
            public string Status { get; set; }
        }
    }
}