using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebApplication1.Areas.webmaster.ViewModels
{
    public class PlanViewModel
    {
        public string packageName { get; set; }
        public string adminEmail { get; set; }
        public string adminName { get; set; }
        public decimal payment { get; set; }
    }
}