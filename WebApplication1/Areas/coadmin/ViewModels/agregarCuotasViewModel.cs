using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using WebApplication1.Concrete;
using WebApplication1.Models;

namespace WebApplication1.Areas.coadmin.ViewModels
{
    public class agregarCuotasViewModel
    {
        public string side_menu { get; set; }
        public string side_sub_menu { get; set; }
        public string communityName { get; set; }
        public string communityApart { get; set; }
        public List<community> communityList { get; set; }
        public List<document_type> document_category_list { get; set; }
        public user curUser { get; set; }
        public int messageCount { get; set; }
        public List<task> pubTaskList { get; set; }
        public List<ShowMessage> pubMessageList { get; set; }
        public List<bank> bankList { get; set; }
        public long feedId { get; set; }
        public string feeName { get; set; }
        public decimal cost { get; set; }
        public decimal taxCharge { get; set; }
        public decimal penalty { get; set; }
        public string merchantAccount { get; set; }
        public long bankId { get; set; }
    }
}