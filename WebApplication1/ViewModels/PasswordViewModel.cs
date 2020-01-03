using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using WebApplication1.Models;

namespace WebApplication1.ViewModels
{
    public class PasswordViewModel
    {
        public string side_menu { get; set; }
        public string side_sub_menu { get; set; }
        public List<document_type> document_category_list { get; set; }
        public string page_name { get; set; }
        public int totalCount { get; set; }
        public int curPage { get; set; }
        public int pageSize { get; set; }
        public int endPage { get; set; }
        public int curStartPage { get; set; }
        public int curEndPage { get; set; }
    }
}