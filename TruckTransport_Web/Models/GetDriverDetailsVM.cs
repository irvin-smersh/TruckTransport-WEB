using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace TruckTransport_Web.Models
{
    public class GetDriverDetailsVM
    {
        public int DriverID { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string PersonUniqueID { get; set; }
        public string Username { get; set; }
        public string Status { get; set; }

        public int NumberOfFinishedOrders { get; set; }
        public int NumberOfGivenOrders { get; set; }
        public int NumberOfFinishedTasks { get; set; }
        public int NumberOfGivenTasks { get; set; }

        public string MostUsedVehicle { get; set; }
    }
}