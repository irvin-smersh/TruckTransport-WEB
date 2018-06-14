using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TruckTransport_Data.Entities
{
    [Table("logiranidogadjaji")]
    public class logiranidogadjaji
    {
        [Key]
        public long logirani_dogadjaj_id { get; set; }

        [Required]
        [StringLength(500)]
        public string username { get; set; }

        [Required]
        [StringLength(500)]
        public string ip_adresa { get; set; }

        [Required]
        [StringLength(500)]
        public string url { get; set; }

        //utc time
        public long vrijeme_dogadjaja { get; set; }
    }
}
