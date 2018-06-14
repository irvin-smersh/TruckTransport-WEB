namespace TruckTransport_Data.Entities
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("poznatelokacije")]
    public partial class poznatelokacije
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public poznatelokacije()
        {
            zadaci = new HashSet<zadaci>();
        }

        [Key]
        public int poznatalokacija_id { get; set; }

        [Required]
        [StringLength(500)]
        public string duzina { get; set; }

        [Required]
        [StringLength(500)]
        public string sirina { get; set; }

        [Required]
        [StringLength(500)]
        public string naziv { get; set; }

        [Required]
        [StringLength(500)]
        public string opis { get; set; }

        public int kategorija_id { get; set; }

        public virtual kategorije kategorije { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<zadaci> zadaci { get; set; }
    }
}
