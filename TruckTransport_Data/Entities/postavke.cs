namespace TruckTransport_Data.Entities {

    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;

    [Table("postavke")]
    public partial class postavke 
    {
        [Key]
        public int postavka_id { get; set; }

        [Required]
        [StringLength(500)]
        public string kljuc { get; set; }

        [Required]
        [StringLength(500)]
        public string vrijednost { get; set; }
    }
}
