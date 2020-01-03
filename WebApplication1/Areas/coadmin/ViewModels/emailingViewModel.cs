using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebApplication1.Areas.coadmin.ViewModels
{
    public class emailingViewModel
    {
        public string fromEmail { get; set; }
        public string toEmail { get; set; }
        public string firstName { get; set; }
        public string lastName { get; set; }
        public string emailSubject { get; set; }
    }
}