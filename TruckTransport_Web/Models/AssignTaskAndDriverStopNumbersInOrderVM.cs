using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TruckTransport_Web.Models
{
    public class AssignTaskAndDriverStopNumbersInOrderVM
    {
        public int ID { get; set; }
        public string Name { get; set; }
        public int Number { get; set; }
        public bool IsTask { get; set; }
    }
}
