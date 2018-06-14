using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace TruckTransport_Web.Models
{
    public class EditOrderVM
    {
        public int OrderID { get; set; }

        [Required(ErrorMessage = "Vozač je obavezno polje!")]
        public int DriverID { get; set; }
        public IEnumerable<SelectListItem> Drivers { get; set; }

        [Required(ErrorMessage = "Vozilo je obavezno polje!")]
        public int VehicleID { get; set; }
        public IEnumerable<SelectListItem> Vehicles { get; set; }

        [Required(ErrorMessage = "Nalog mora imati barem jedan zadatak!")]
        public int[] TaskIDs { get; set; }
        public IEnumerable<SelectListItem> Tasks { get; set; }
   
        public int?[] DriverStopIDs { get; set; }
        public IEnumerable<SelectListItem> DriverStops { get; set; }
   
        public List<AssignTaskAndDriverStopNumbersInOrderVM> TaskAndDriverStopNumbers { get; set; }
    }
}