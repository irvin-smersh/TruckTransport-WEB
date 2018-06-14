using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace TruckTransport_Web.Models
{
    public class EditVehicleVM
    {
        public int VehicleID { get; set; }

        [Required(ErrorMessage = "Naziv je obavezno polje!")]
        [StringLength(100, ErrorMessage = "Maksimalna dužina naziva je 100 karaktera!")]
        public string Name { get; set; }

        [Required(ErrorMessage = "Marka je obavezno polje!")]
        [StringLength(100, ErrorMessage = "Maksimalna dužina marke vozila je 100 karaktera!")]
        public string Manufacturer { get; set; }

        [Required(ErrorMessage = "Tip je obavezno polje!")]
        [StringLength(100, ErrorMessage = "Maksimalna dužina tipa je 100 karaktera!")]
        public string Type { get; set; }

        [Required(ErrorMessage = "Godina proizvodnje je obavezno polje!")]
        [Range(1960, long.MaxValue, ErrorMessage = "Godina proizvodnje ne može biti manja od 1960!")]
        public long YearMade { get; set; }

        [Required(ErrorMessage = "Nosivost je obavezno polje!")]
        [StringLength(100, ErrorMessage = "Maksimalna dužina nosivosti je 100 karaktera!")]
        public string LoadCapacity { get; set; }
    }
}