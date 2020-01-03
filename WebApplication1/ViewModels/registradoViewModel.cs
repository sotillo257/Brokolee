using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using WebApplication1.Concrete;
using WebApplication1.Models;

namespace WebApplication1.ViewModels
{
    public class registradoViewModel
    {
        public string side_menu { get; set; }
        public string communityName { get; set; }
        public string communityApart { get; set; }
        public string event_name { get; set; }
        public string event_date { get; set; }
        public string event_time { get; set; }
        public string place { get; set; }
        public string description { get; set; }
        public string note { get; set; }
        public List<document_type> document_category_list { get; set; }
        public user curUser { get; set; }
        public int messageCount { get; set; }
        public List<task> pubTaskList { get; set; }
        public List<ShowMessage> pubMessageList { get; set; }
    }
}