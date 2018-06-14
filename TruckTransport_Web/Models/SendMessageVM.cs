using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace TruckTransport_Web.Models
{
    public class SendMessageVM
    {
        public DateTime CreationTime { get; set; }

        [Required(ErrorMessage = "Tekst poruke je obavezno polje!")]
        [StringLength(500, ErrorMessage = "Maksimalna dužina poruke je 500 karaktera!")]
        public string MessageText { get; set; }

        //check if dispatcher is sender or receiver
        public bool FromDriver { get; set; }
        public int DriverID { get; set; }
    }
}