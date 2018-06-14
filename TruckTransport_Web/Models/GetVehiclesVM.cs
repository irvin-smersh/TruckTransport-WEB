using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using TruckTransport_Data.Entities;

namespace TruckTransport_Web.Models
{
    public class GetVehiclesVM
    {
        public List<vozila> VehiclesList { get; set; }
    }
}