using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;
using WebApplication1.Models;
using WebApplication1.ScheduledTasks;

namespace WebApplication1
{
    public class MvcApplication : System.Web.HttpApplication
    {

        protected void Application_Start()
        {
            AreaRegistration.RegisterAllAreas();
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            BundleConfig.RegisterBundles(BundleTable.Bundles);
            JobScheduler.Start();
        }

        protected void Session_OnEnd(object sender, EventArgs e)
        {
            // insert code here
            //HttpCookie userIdCookie = Request.Cookies.Get("UserID");
            //long userId = Convert.ToInt64(userIdCookie.Value);
            //using (pjrdev_condominiosEntities entities = new pjrdev_condominiosEntities())
            //{
            //    user curUser = entities.users.Find(userId);
            //    curUser.is_logged = false;
            //    entities.SaveChanges();
            //}

        }
    }
}
