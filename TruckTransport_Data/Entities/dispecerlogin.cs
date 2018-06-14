namespace TruckTransport_Data.Entities
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("dispecerlogin")]
    public partial class dispecerlogin
    {
        [Key]
        public int dispecerlogin_id { get; set; }

        [Required]
        [StringLength(500)]
        public string username { get; set; }

        [Required]
        [StringLength(500)]
        public string password { get; set; }

        [Required]
        [StringLength(500)]
        public string email { get; set; }
               
        public long? lastlogin { get; set; }
    }
}
