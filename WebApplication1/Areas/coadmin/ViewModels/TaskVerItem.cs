using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebApplication1.Areas.coadmin.ViewModels
{
    public class TaskVerItem
    {
        public string task_comment { get; set; }
        public DateTime comment_datetime { get; set; }
        public string comment_username { get; set; }
    }
}