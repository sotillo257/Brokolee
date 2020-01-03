using System.Web.Mvc;

namespace WebApplication1.Areas.webmaster
{
    public class webmasterAreaRegistration : AreaRegistration 
    {
        public override string AreaName 
        {
            get 
            {
                return "webmaster";
            }
        }

        public override void RegisterArea(AreaRegistrationContext context) 
        {
            context.MapRoute(
                "webmaster_default",
                "webmaster/{controller}/{action}/{id}",
                new { action = "Index", id = UrlParameter.Optional },
                new[] { "WebApplication1.Areas.webmaster.Controllers" }
            );
        }
    }
}