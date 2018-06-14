using System.ComponentModel.DataAnnotations;

namespace TruckTransport_Web.Models {
    public class LoginVM 
    {
        [Required(ErrorMessage = "Korisničko ime je obavezno polje!")]
        public string Username { get; set; }

        [Required(ErrorMessage = "Lozinka je obavezno polje!")]
        public string Password { get; set; }
       
        public bool RememberMe { get; set; }
    }
}