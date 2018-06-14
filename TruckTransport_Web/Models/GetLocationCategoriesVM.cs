using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using TruckTransport_Data.Entities;

namespace TruckTransport_Web.Models {
    public class GetLocationCategoriesVM 
    {
        public List<kategorije> LocationCategoriesList { get; set; }
    }
}