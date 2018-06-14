namespace TruckTransport_Data.Entities
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("vozila")]
    public partial class vozila
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public vozila()
        {
            nalozi = new HashSet<nalozi>();
        }

        [Key]
        public int vozilo_id { get; set; }

        [Required]
        [StringLength(500)]
        public string naziv { get; set; }

        [Required]
        [StringLength(500)]
        public string marka { get; set; }

        [Required]
        [StringLength(500)]
        public string tip { get; set; }

        public long godiste { get; set; }

        [Required]
        [StringLength(500)]
        public string nosivost { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<nalozi> nalozi { get; set; }
    }
}
