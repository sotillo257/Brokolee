using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebApplication1.Areas.coadmin.ViewModels
{
    public class DocumentTypeItemViewModel
    {
        public int ID { get; set; }
        public string DocumentTypeName { get; set; }
        public int Share { get; set; }
        public int Documents { get; set; }
    }
}