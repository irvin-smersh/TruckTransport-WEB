﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace TruckTransport_Web.Models
{
    public class AddKnownLocationVM
    {
        [Required(ErrorMessage = "Naziv je obavezno polje!")]
        [StringLength(250, ErrorMessage = "Maksimalna dužina naziva je 250 karaktera!")]
        public string Name { get; set; }

        [Required(ErrorMessage = "Opis je obavezno polje!")]
        [StringLength(500, ErrorMessage = "Maksimalna dužina opisa je 500 karaktera!")]
        public string Description { get; set; }

        [Required(ErrorMessage = "Kategorija je obavezno polje!")]
        public int LocationCategoryID { get; set; }
        public IEnumerable<SelectListItem> LocationCategories { get; set; }

        [Required(ErrorMessage = "Geo. širina je obavezno polje!")]
        public string Latitude { get; set; }

        [Required(ErrorMessage = "Geo dužina je obavezno polje!")]
        public string Longitude { get; set; }
    }
}