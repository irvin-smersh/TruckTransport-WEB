using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TruckTransport_Data.Entities
{
    [Table("stajalista")]
    public partial class stajalista
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public stajalista()
        {           
            stajalista_nalozi = new HashSet<stajalista_nalozi>();           
        }

        [Key]
        public int stajaliste_id { get; set; }

        [Required]
        [StringLength(500)]
        public string naziv { get; set; }

        [Required]
        [StringLength(500)]
        public string opis { get; set; }

        [Required]
        [StringLength(500)]
        public string duzina { get; set; }

        [Required]
        [StringLength(500)]
        public string sirina { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<stajalista_nalozi> stajalista_nalozi { get; set; }
    }
}
