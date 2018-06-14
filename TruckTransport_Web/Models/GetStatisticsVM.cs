using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace TruckTransport_Web.Models
{
    public class GetStatisticsVM
    {
        public int NumberOfFinishedOrders { get; set; }
        public int NumberOfFinishedTasks { get; set; }
        public int NumberOfDrivers { get; set; }
        public int NumberOfVehicles { get; set; }
    }
}