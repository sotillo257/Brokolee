using System.Web.Mvc;

namespace WebApplication1.Areas.coadmin
{
    public class coadminAreaRegistration : AreaRegistration 
    {
        public override string AreaName 
        {
            get 
            {
                return "coadmin";
            }
        }

        public override void RegisterArea(AreaRegistrationContext context) 
        {
            context.MapRoute(
                "coadmin_default",
                "coadmin/{controller}/{action}/{id}",
                new { action = "Index", id = UrlParameter.Optional },
                new[] { "WebApplication1.Areas.coadmin.Controllers" }
            );
        }
    }
}