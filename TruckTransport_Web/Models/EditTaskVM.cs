using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace TruckTransport_Web.Models
{
    public class EditTaskVM
    {
        public int TaskID { get; set; }

        [Required(ErrorMessage = "Naziv je obavezno polje!")]
        [StringLength(250, ErrorMessage = "Maksimalna dužina naziva je 250 karaktera!")]
        public string Name { get; set; }

        [Required(ErrorMessage = "Opis je obavezno polje!")]
        [StringLength(500, ErrorMessage = "Maksimalna dužina opisa je 500 karaktera!")]
        public string Description { get; set; }

        [Required(ErrorMessage = "Broj zadatka je obavezno polje!")]
        public int TaskNumber { get; set; }

        [Required(ErrorMessage = "Lokacija je obavezno polje!")]
        public int KnownLocationID { get; set; }
        public IEnumerable<SelectListItem> KnownLocations { get; set; }

        public string Latitude { get; set; }
        public string Longitude { get; set; }
    }
}