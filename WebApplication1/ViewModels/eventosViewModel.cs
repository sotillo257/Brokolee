using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using WebApplication1.Concrete;
using WebApplication1.Models;

namespace WebApplication1.ViewModels
{
    public class eventosViewModel
    {
        public string side_menu { get; set; }
        public string communityName { get; set; }
        public string communityApart { get; set; }
        public long communityID1 { get; set; }
        public List<community> communityList { get; set; }
        public List<CalEventView> eventList { get; set; }
        public List<document_type> document_category_list { get; set; }
        public user curUser { get; set; }
        public int messageCount { get; set; }
        public string searchStr { get; set; }
        public List<task> pubTaskList { get; set; }
        public List<ShowMessage> pubMessageList { get; set; }
    }
}