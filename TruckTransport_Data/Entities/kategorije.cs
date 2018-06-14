namespace TruckTransport_Data.Entities
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("kategorije")]
    public partial class kategorije
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public kategorije()
        {
            poznatelokacije = new HashSet<poznatelokacije>();
        }

        [Key]
        public int kategorija_id { get; set; }

        [Required]
        [StringLength(500)]
        public string naziv { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<poznatelokacije> poznatelokacije { get; set; }
    }
}
