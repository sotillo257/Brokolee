using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebApplication1.ViewModels
{
    public class onlineUserDetailsViewModel
    {
        public long userID { get; set; }
        public List<string> connectionID { get; set; }
        public string name { get; set; }
        public string profilePicture { get; set; }
        public bool isOnline { get; set; }
         public int UnReadMessageCount { get; set; }
        public DateTime LastUpdationTime { get; set; }
    }
}