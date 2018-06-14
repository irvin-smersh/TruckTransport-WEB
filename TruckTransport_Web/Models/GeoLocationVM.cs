using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace TruckTransport_Web.Models
{
    public class GeoLocationVM
    {
        public int ID { get; set; }
        public string Info { get; set; }
        public string Latitude { get; set; }
        public string Longitude { get; set; }
    }
}