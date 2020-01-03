using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using WebApplication1.Models;

namespace WebApplication1.Areas.webmaster.ViewModels
{
    public class selUserViewModel
    {
        public List<chatmessage> chatmessageList { get; set; }
        public user curUser { get; set; }
        public onlineuser selUser { get; set; }
    }
}