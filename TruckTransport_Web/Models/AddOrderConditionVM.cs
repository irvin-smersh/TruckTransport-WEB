using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace TruckTransport_Web.Models {
    public class AddOrderConditionVM 
    {
        [Required(ErrorMessage = "Opis je obavezno polje!")]
        [StringLength(100, ErrorMessage = "Maksimalna dužina opisa je 200 karaktera!")]
        public string Description { get; set; }
    }
}