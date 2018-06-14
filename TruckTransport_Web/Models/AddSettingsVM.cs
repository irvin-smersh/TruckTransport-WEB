using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace TruckTransport_Web.Models {
    public class AddSettingsVM 
    {    
        [Required(ErrorMessage = "Naziv je obavezno polje!")]
        [StringLength(50, ErrorMessage = "Maksimalna dužina naziva je 50 karaktera!")]
        public string KeyName { get; set; }

        [Required(ErrorMessage = "Vrijednost je obavezno polje!")]
        [StringLength(100, ErrorMessage = "Maksimalna dužina naziva je 100 karaktera!")]
        public string Value { get; set; }
    }
}