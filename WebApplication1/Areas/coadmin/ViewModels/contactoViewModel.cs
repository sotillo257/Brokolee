﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using WebApplication1.Concrete;
using WebApplication1.Models;

namespace WebApplication1.Areas.coadmin.ViewModels
{
    public class contactoViewModel
    {
        public string side_menu { get; set; }
        public string side_sub_menu { get; set; }
        public string communityName { get; set; }
        public string communityApart { get; set; }
        public List<community> communityList { get; set; }
        public List<document_type> document_category_list { get; set; }
        public List<contactinfo> contactList { get; set; }
        public contactinfo editContactInfo { get; set; }
        public user curUser { get; set; }
        public int messageCount { get; set; }

        public string searchStr { get; set; }
        public List<task> pubTaskList { get; set; }
        public List<ShowMessage> pubMessageList { get; set; }
    }
}