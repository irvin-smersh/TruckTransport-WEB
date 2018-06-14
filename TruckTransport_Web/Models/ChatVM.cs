using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace TruckTransport_Web.Models
{
    public class ChatVM
    {
        //for chat messages panel
        public int FirstDriverID { get; set; }
        public List<ChatUserMessagesVM> ChatUsersMessages { get; set; }
    }
}