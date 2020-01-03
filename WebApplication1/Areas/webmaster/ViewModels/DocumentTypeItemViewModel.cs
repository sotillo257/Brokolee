using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebApplication1.Areas.webmaster.ViewModels
{
    public class DocumentTypeItemViewModel
    {
        public int ID { get; set; }
        public string DocumentTypeName { get; set; }
        public bool Share { get; set; }
        public int Documents { get; set; }
    }
}