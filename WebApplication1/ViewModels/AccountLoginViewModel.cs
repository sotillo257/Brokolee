using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebApplication1.ViewModels
{
    public class AccountLoginViewModel
    {
        public string Name { get; set; }
        public string Password { get; set; }
        public string ReturnUrl { get; set; }
    }
}