using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using TruckTransport_Web.Helper;

namespace TruckTransport_Web.Models
{
    public class AddDriverVM
    {
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

        [Required(ErrorMessage = "Lozinka je obavezno polje!")]
        [StringLength(Constants.MaximumPasswordLength, ErrorMessage = "Maksimalna dužina lozinke je 32 karaktera.")]
        [RegularExpression(@"^(?=(.*\d){2})(?=.*[a-z])(?=.*[A-Z])(?=.*[^a-zA-Z\d]).{8,}$", ErrorMessage = "Lozinka mora da ima minimalno 8 karaktera uključujući po 1 veliko i malo slovo, 2 broja i 1 specijalni karakter!")]
        public string Password { get; set; }

        [Required(ErrorMessage = "Potvrda lozinke je obavezno polje!")]
        [StringLength(Constants.MaximumPasswordLength, ErrorMessage = "Maksimalna dužina potvrde lozinke je 32 karaktera.")]
        [Compare("Password", ErrorMessage = "Potvrda lozinke ne odgovara lozinci!")]
        public string PasswordConfirm { get; set; }
    }
}