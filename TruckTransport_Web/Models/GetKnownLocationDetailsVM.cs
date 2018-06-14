using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace TruckTransport_Web.Models
{
    public class GetKnownLocationDetailsVM
    {
        public int KnownLocationID { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public string LocationCategory { get; set; }
        public string Latitude { get; set; }
        public string Longitude { get; set; }
    }
}