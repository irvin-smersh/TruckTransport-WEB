using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using TruckTransport_Data.Entities;

namespace TruckTransport_Web.Models
{
    public class GetTasksVM
    {
        public List<TaskVM> TasksList { get; set; }

        public class TaskVM
        {
            public int TaskID { get; set; }
            public string Name { get; set; }
            public string Description { get; set; }
            public long? CheckIn { get; set; }
            public long? CheckOut { get; set; }
            public string KnownLocation { get; set; }
            public int? OrderID { get; set; }
            public int TaskNumber { get; set; }
        }
    }
}