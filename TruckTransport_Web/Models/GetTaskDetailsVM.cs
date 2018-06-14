using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace TruckTransport_Web.Models
{
    public class GetTaskDetailsVM
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

        //order
        public int? OrderID { get; set; }
        public string OrderCreationTime { get; set; }
        public string OrderCondition { get; set; }
        public string Driver { get; set; }
        public string Vehicle { get; set; }
    }
}