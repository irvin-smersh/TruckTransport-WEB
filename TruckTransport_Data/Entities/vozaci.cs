namespace TruckTransport_Data.Entities
{ 
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("vozaci")]
    public partial class vozaci
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public vozaci()
        {
            nalozi = new HashSet<nalozi>();
            poruke = new HashSet<poruke>();
        }

        [Key]
        public int vozac_id { get; set; }

        [Required]
        [StringLength(500)]
        public string user { get; set; }

        [Required]
        [StringLength(500)]
        public string pass { get; set; }

        [Required]
        [StringLength(500)]
        public string ime { get; set; }

        [Required]
        [StringLength(500)]
        public string prezime { get; set; }

        [Required]
        [StringLength(14)]
        public string jmbg { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<nalozi> nalozi { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<poruke> poruke { get; set; }
    }
}
