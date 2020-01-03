using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using WebApplication1.Concrete;

namespace WebApplication1.Areas.coadmin.ViewModels
{
    public class ChatNotificationViewModel
    {
        public List<ShowMessage> pubMessageList { get; set; }
        public int messageCount { get; set; }
    }
}