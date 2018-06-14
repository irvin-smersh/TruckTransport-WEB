using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TruckTransport_Web.Models
{
    public class SetSOSVM
    {
        public int DriverID { get; set; }
        public string DriverFullName { get; set; }
        public string SOSMessageTime { get; set; }
        public string SOSMessageText { get; set; }

        public string Latitude { get; set; }
        public string Longitude { get; set; }

        public string ReplyText { get; set; }
    }
}
