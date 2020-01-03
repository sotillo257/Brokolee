using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using WebApplication1.Concrete;
using WebApplication1.Models;
using WebApplication1;

namespace WebApplication1.Areas.webmaster.ViewModels
{
    public class agregarCommunViewModel
    {
        public string side_menu { get; set; }
        public string side_sub_menu { get; set; }
        public string communityName { get; set; }
        public string communityApart { get; set; }
        public user curUser { get; set; }
        public int messageCount { get; set; }
        public List<document_type> document_category_list { get; set; }
        public List<task> pubTaskList { get; set; }
        public List<ShowMessage> pubMessageList { get; set; }
        public List<package> packageList { get; set; }

        public List<user> coadminList { get; set; }
    }
}