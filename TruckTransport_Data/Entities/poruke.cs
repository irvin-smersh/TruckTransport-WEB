namespace TruckTransport_Data.Entities
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("poruke")]
    public partial class poruke
    {
        [Key]
        public int poruka_id { get; set; }

        public long vrijeme { get; set; }

        [Required]
        [StringLength(500)]
        public string text { get; set; }

        public sbyte odvozaca { get; set; }

        public int vozac_id { get; set; }

        public virtual vozaci vozaci { get; set; }
    }
}
