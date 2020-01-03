using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using WebApplication1.Models;

namespace WebApplication1.ViewModels
{
    public class userNotificationListViewModel
    {
        public string notificationType { get; set; }
        public long notificationID { get; set; }
        public user User { get; set; }
        public DateTime createdOn { get; set; }
        public string notificationStatus { get; set; }
        public long totalNotifications { get; set; }
    }
}