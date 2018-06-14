using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace TruckTransport_Web.Models {
    public class EditPersonalInfoVM
    {
        public int DispatcherUserID { get; set; }

        [Required(ErrorMessage = "Korisničko ime je obavezno polje!")]
        public string Username { get; set; }

        [Required(ErrorMessage = "Email adresa je obavezno polje!")]
        [EmailAddress(ErrorMessage = "Email adresa mora biti u ispravnom formatu!")]
        public string Email { get; set; }
        public string LastLoginTime { get; set; }
    }
}