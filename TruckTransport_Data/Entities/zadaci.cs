namespace TruckTransport_Data.Entities
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("zadaci")]
    public partial class zadaci
    {
        public zadaci()
        {
            
        }

        [Key]
        public int zadatak_id { get; set; }

        [Required]
        [StringLength(500)]
        public string naziv { get; set; }

        [Required]
        [StringLength(500)]
        public string opis { get; set; }

        public long? checkin { get; set; }

        public long? checkout { get; set; }

        public int poznatalokacija_id { get; set; }

        public int? nalog_id { get; set; }

        [Column("broj zadatka")]
        public int broj_zadatka { get; set; }

        public virtual nalozi nalozi { get; set; }

        public virtual poznatelokacije poznatelokacije { get; set; }     
    }
}
