using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using WebApplication1.Models;

namespace WebApplication1.ViewModels
{
    public class friendRequestViewModel
    {
        public user userInfo { get; set; }
        public string requestStatus { get; set; }
        public long requestorUserID { get; set; }
        public long endUserID { get; set; }
    }
}