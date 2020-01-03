using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using WebApplication1.Concrete;
using WebApplication1.Models;

namespace WebApplication1.Areas.coadmin.ViewModels
{
    public class comunicacionesViewModel
    {
        public string side_menu { get; set; }
        public string side_sub_menu { get; set; }
        public string communityName { get; set; }
        public string communityApart { get; set; }
        public List<document_type> document_category_list { get; set; }
        public user curUser { get; set; }
        public int messageCount { get; set; }
        public List<task> pubTaskList { get; set; }
        public List<onlineuser> onlineUserList { get; set; }
        public onlineuser selUser { get; set; }
        public user susUser { get; set; }
        public List<blog> blogList { get; set; }
        public List<chatmessage> chatmessageList { get; set; }
        public List<ShowMessage> pubMessageList { get; set; }
    }
}