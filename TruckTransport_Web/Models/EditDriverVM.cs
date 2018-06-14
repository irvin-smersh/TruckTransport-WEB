using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using TruckTransport_Web.Helper;

namespace TruckTransport_Web.Models
{
    public class EditDriverVM
    {
        public int DriverID { get; set; }

        [Required(ErrorMessage = "Ime je obavezno polje!")]
        [StringLength(100, ErrorMessage = "Maksimalna dužina imena je 100 karaktera!")]
        public string FirstName { get; set; }

        [Required(ErrorMessage = "Prezime je obavezno polje!")]
        [StringLength(100, ErrorMessage = "Maksimalna dužina prezimena je 100 karaktera!")]
        public string LastName { get; set; }

        [Required(ErrorMessage = "JMBG je obavezno polje!")]
        [StringLength(14, MinimumLength = 14, ErrorMessage = "JMBG se sastoji od 14 karaktera!")]
        public string PersonUniqueID { get; set; }

        [Required(ErrorMessage = "Korisničko ime je obavezno polje!")]
        [StringLength(100, ErrorMessage = "Maksimalna dužina korisničkog imena je 100 karaktera!")]
        public string Username { get; set; }      
    }
}