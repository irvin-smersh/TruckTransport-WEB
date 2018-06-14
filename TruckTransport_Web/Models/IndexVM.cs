using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace TruckTransport_Web.Models
{
    public class IndexVM
    {
        public GetStatisticsVM Statistics { get; set; }
        public GetDriversVM ActiveDrivers { get; set; }
        public List<GeoLocationVM> ActiveDriversLatestPositions { get; set; }
    }
}