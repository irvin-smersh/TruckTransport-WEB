using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using TruckTransport_Data.Entities;

namespace TruckTransport_Web.Models
{
    public class GetKnownLocationsVM
    {
        public List<KnownLocationVM> KnownLocationsList { get; set; }

        public class KnownLocationVM
        {
            public int KnownLocationID { get; set; }
            public string Name { get; set; }
            public string Description { get; set; }
            //duzina
            public string Longitude { get; set; }
            //sirina
            public string Latitude { get; set; }
            public string Category { get; set; }
        }
    }
}