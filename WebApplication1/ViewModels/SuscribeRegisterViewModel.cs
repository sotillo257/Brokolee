using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebApplication1.ViewModels
{
    public class SuscribeRegisterViewModel
    {
        public string Name { get; set; }
        public string LastName { get; set; }
        public string CommunityName { get; set; }
        public string Address { get; set; }
        public string PhoneNumber { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
        public string ConfirmPassword { get; set; }
        public long PackageId { get; set; }
        public string WarningMsg { get; set; }
    }
}