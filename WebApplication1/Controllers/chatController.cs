using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WebApplication1.ViewModels;

namespace WebApplication1.Controllers
{
    public class chatController : Controller
    {
        // GET: chat
        public ActionResult Index()
        {
            administradorViewModel viewModel = new administradorViewModel();
            viewModel.side_menu = "";
            return View(viewModel);
        }
    }
}