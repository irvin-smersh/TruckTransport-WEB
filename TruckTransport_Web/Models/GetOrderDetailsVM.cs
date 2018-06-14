using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace TruckTransport_Web.Models
{
    public class GetOrderDetailsVM
    {
        public int OrderID { get; set; }
        public string OrderCreationTime { get; set; }
        public string OrderCondition { get; set; }
        public string Vehicle { get; set; }
        public string Driver { get; set; }

        public List<TaskDetails> Tasks { get; set; }
        public List<DriverStopDetails> DriverStops { get; set; }

    
        public class TaskDetails
        {
            public int TaskID { get; set; }
            public int TaskNumber { get; set; }
            public string Name { get; set; }
            public string Description { get; set; }

            public string CheckIn { get; set; }
            public string CheckOut { get; set; }

            //location
            public string KnownLocation { get; set; }
            public string Latitude { get; set; }
            public string Longitude { get; set; }
        }

        public class DriverStopDetails
        {
            public int DriverStopID { get; set; }
            public int DriverStopNumber { get; set; }
            public string DriverStop { get; set; }

            public string CheckIn { get; set; }
            public string CheckOut { get; set; }

            public string Latitude { get; set; }
            public string Longitude { get; set; }
        }
    }
}