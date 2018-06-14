namespace TruckTransport_Data.Entities
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("geotacke")]
    public partial class geotacke
    {
        [Key]
        public int geotacka_id { get; set; }

        public int lokalna_id { get; set; }

        [Required]
        [StringLength(500)]
        public string duzina { get; set; }

        [Required]
        [StringLength(500)]
        public string sirina { get; set; }

        public long vrijeme { get; set; }

        public int nalog_id { get; set; }

        public virtual nalozi nalozi { get; set; }
    }
}
