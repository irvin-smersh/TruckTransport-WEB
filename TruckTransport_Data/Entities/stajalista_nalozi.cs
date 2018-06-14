using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TruckTransport_Data.Entities
{
    [Table("stajalista_nalozi")]
    public partial class stajalista_nalozi
    {
        [Key]
        public int stajaliste_nalog_id { get; set; }

        public int stajaliste_id { get; set; }

        public int nalog_id { get; set; }

        public long? checkin { get; set; }
        public long? checkout { get; set; }

        [Column("broj stajalista")]
        public int broj_stajalista { get; set; }

        public virtual stajalista stajalista { get; set; }
        public virtual nalozi nalozi { get; set; }
    }
}

