using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using WebApplication1.Models;

namespace WebApplication1.ViewModels
{
    public class userSearchResultViewModel
    {
        public user userInfo { get; set; }
        public string friendRequestStatus { get; set; }
        public bool isRequestReceived { get; set; }
    }
}