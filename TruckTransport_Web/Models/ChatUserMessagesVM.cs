using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace TruckTransport_Web.Models
{
    public class ChatUserMessagesVM
    {
        public int DriverID { get; set; }
        public string DriverFullName { get; set; }
        public List<ChatMessagesVM> UserMessages { get; set; }
    }
}