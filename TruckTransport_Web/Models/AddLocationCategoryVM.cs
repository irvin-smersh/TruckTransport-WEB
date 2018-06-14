using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace TruckTransport_Web.Models {
    public class AddLocationCategoryVM 
    {     
        [Required(ErrorMessage = "Naziv je obavezno polje!")]
        [StringLength(100, ErrorMessage = "Maksimalna dužina naziva je 100 karaktera!")]
        public string Name { get; set; }
    }
}