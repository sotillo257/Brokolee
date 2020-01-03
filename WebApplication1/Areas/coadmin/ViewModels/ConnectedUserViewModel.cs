using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using WebApplication1.Models;

namespace WebApplication1.Areas.coadmin.ViewModels
{
    public class ConnectedUserViewModel
    {
        public List<onlineuser> onlineUserList { get; set; }
    }
}