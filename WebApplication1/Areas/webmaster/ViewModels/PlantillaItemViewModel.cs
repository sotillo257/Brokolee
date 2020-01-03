using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebApplication1.Areas.webmaster.ViewModels
{
    public class PlantillaItemViewModel
    {
        public long ID { get; set; }
        public string TypeName { get; set; }
        public int TypeID { get; set; }
        public string Content { get; set; }
    }
}