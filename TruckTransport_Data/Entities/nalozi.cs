namespace TruckTransport_Data.Entities
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("nalozi")]
    public partial class nalozi
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public nalozi()
        {
            geotacke = new HashSet<geotacke>();           
            zadaci = new HashSet<zadaci>();
            stajalista_nalozi = new HashSet<stajalista_nalozi>();
        }

        [Key]
        public int nalog_id { get; set; }

        public long vrijeme_kreiranja { get; set; }

        public int stanje_id { get; set; }

        public int vozilo_id { get; set; }

        public int vozac_id { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<geotacke> geotacke { get; set; } 

        public virtual vozaci vozaci { get; set; }

        public virtual vozila vozila { get; set; }

        public virtual stanja stanja { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<zadaci> zadaci { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<stajalista_nalozi> stajalista_nalozi { get; set; }
    }
}
