using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace TruckTransport_Web.Models
{
    public class ChatMessagesVM
    {
        public int MessageID { get; set; }
        public long UnixCreationTime { get; set; }
        public string CreationTime { get; set; }
        public string MessageText { get; set; }

        //check if dispatcher is sender or receiver
        public sbyte SByteFromDriver { get; set; }
        public bool FromDriver { get; set; }
        public int DriverID { get; set; }
    }
}