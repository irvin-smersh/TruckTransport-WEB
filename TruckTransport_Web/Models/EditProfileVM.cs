using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace TruckTransport_Web.Models {
    public class EditProfileVM 
    {
        public int DispatcherUserID { get; set; }
        public EditPersonalInfoVM DispatcherUser { get; set; }
        public ChangePasswordVM DispatcherPassword { get; set; }
    }
}