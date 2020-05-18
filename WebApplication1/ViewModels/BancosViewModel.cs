using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using WebApplication1.Concrete;
using WebApplication1.Models;

namespace WebApplication1.ViewModels
{
    public class BancosViewModel
    {
        public string side_menu { get; set; }
        public string side_sub_menu { get; set; }       
        public List<community> communityList { get; set; }
        public List<document_type> document_category_list { get; set; }
        public user curUser { get; set; }
        public int messageCount { get; set; }
        public List<task> pubTaskList { get; set; }
        public List<ShowMessage> pubMessageList { get; set; }
        public List<bank> banks { get; set; }
        public bank bankItem { get; set; }
        public string searchString { get; set; }
    }
}