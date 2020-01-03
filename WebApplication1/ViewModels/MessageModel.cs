using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebApplication1.ViewModels
{
    public class MessageModel
    {
        public long ChatMessageID { get; set; }
        public long FromUserID { get; set; }
        public string FromUserName { get; set; }
        public long ToUserID { get; set; }
        public string ToUserName { get; set; }
        public string Message { get; set; }
        public string Status { get; set; }
        public string CreatedOn { get; set; }
        public string UpdatedOn { get; set; }
        public string ReceivedOn { get; set; }
        public string ViewedOn { get; set; }
        public bool IsActive { get; set; }
    }
}