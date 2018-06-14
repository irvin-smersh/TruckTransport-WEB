using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace TruckTransport_Web.Models
{
    public class GetOrdersVM
    {
        public List<OrderVM> OrdersList { get; set; }

        public class OrderVM
        {
            public int OrderID { get; set; }
            public long UnixCreationTime { get; set; }
            public string CreationTime { get; set; }
            public string OrderCondition { get; set; }
            public string Vehicle { get; set; }        
            public string Driver { get; set; }
        }
    }
}