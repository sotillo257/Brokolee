using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebApplication1.Areas.webmaster.ViewModels
{
    public class CalEventView
    {
        public string title { get; set; }
        public long id { get; set; }
        public int year { get; set; }
        public int month { get; set; }
        public int day { get; set; }
        public int hour { get; set; }
        public int minute { get; set; }
    }
}