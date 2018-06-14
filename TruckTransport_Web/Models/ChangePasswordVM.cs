using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using TruckTransport_Web.Helper;

namespace TruckTransport_Web.Models {
    public class ChangePasswordVM
    {       
        [Required(ErrorMessage = "Trenutna lozinka je obavezno polje!")]
        public string CurrentPassword { get; set; }

        [Required(ErrorMessage = "Nova lozinka je obavezno polje!")]
        [StringLength(Constants.MaximumPasswordLength, ErrorMessage = "Maksimalna dužina nove lozinke je 32 karaktera.")]
        [RegularExpression(@"^(?=(.*\d){2})(?=.*[a-z])(?=.*[A-Z])(?=.*[^a-zA-Z\d]).{8,}$", ErrorMessage = "Nova lozinka mora da ima minimalno 8 karaktera uključujući po 1 veliko i malo slovo, 2 broja i 1 specijalni karakter!")]
        public string NewPassword { get; set; }

        [Required(ErrorMessage = "Potvrda lozinke je obavezno polje!")]
        [Compare(otherProperty: "NewPassword", ErrorMessage = "Lozinke se ne poklapaju!")]
        public string NewPasswordConfirm { get; set; }
    }
}