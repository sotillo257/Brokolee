using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WebApplication1.Areas.webmaster.ViewModels;
using WebApplication1.Concrete;
using WebApplication1.Models;

namespace WebApplication1.Areas.webmaster.Controllers
{
    public class cargosController : Controller
    {
        pjrdev_condominiosEntities entities = new pjrdev_condominiosEntities();
        EFPublicRepository ep = new EFPublicRepository();
        // GET: webmaster/cargos
        public ActionResult balance()
        {
            if (Session["USER_ID"] != null)
            {
                long userId = (long)Session["USER_ID"];
                user curUser = entities.users.Find(userId);
                List<ShowMessage> pubMessageList = ep.GetChatMessages(userId);
                balanceCargosViewModel viewModel = new balanceCargosViewModel();
                viewModel.side_menu = "cargos";
                viewModel.side_sub_menu = "cargos_balance";
                viewModel.document_category_list = entities.document_type.ToList();
                viewModel.pubTaskList = ep.GetNotifiTaskList(userId);
                viewModel.curUser = curUser;
                viewModel.pubMessageList = pubMessageList;
                viewModel.messageCount = ep.GetUnreadMessageCount(pubMessageList);
                return View(viewModel);
            } else
            {
                return Redirect(ep.GetLogoutUrl());
            }
               
        }

        public ActionResult pago()
        {
            if (Session["USER_ID"] != null)
            {
                long userId = (long)Session["USER_ID"];
                user curUser = entities.users.Find(userId);
                List<ShowMessage> pubMessageList = ep.GetChatMessages(userId);
                pagoCargosViewModel viewModel = new pagoCargosViewModel();
                viewModel.side_menu = "cargos";
                viewModel.side_sub_menu = "cargos_pago";
                viewModel.document_category_list = entities.document_type.ToList();
                viewModel.pubTaskList = ep.GetNotifiTaskList(userId);
                viewModel.curUser = curUser;
                viewModel.pubMessageList = pubMessageList;
                viewModel.messageCount = ep.GetUnreadMessageCount(pubMessageList);
                return View(viewModel);
            } else
            {
                return Redirect(ep.GetLogoutUrl());
            }
                
        }

        public ActionResult estado()
        {
            if (Session["USER_ID"] != null)
            {
                long userId = (long)Session["USER_ID"];
                user curUser = entities.users.Find(userId);
                List<ShowMessage> pubMessageList = ep.GetChatMessages(userId);
                estadoCargosViewModel viewModel = new estadoCargosViewModel();
                viewModel.side_menu = "cargos";
                viewModel.side_sub_menu = "cargos_estado";
                viewModel.document_category_list = entities.document_type.ToList();
                viewModel.pubTaskList = ep.GetNotifiTaskList(userId);
                viewModel.curUser = curUser;
                viewModel.pubMessageList = pubMessageList;
                viewModel.messageCount = ep.GetUnreadMessageCount(pubMessageList);
                return View(viewModel);
            } else
            {
                return Redirect(ep.GetLogoutUrl());
            }
               
        }

        public ActionResult historial()
        {
            if (Session["USER_ID"] != null)
            {
                long userId = (long)Session["USER_ID"];
                user curUser = entities.users.Find(userId);
                List<ShowMessage> pubMessageList = ep.GetChatMessages(userId);
                historialCargosViewModel viewModel = new historialCargosViewModel();
                viewModel.side_menu = "cargos";
                viewModel.side_sub_menu = "cargos_historial";
                viewModel.document_category_list = entities.document_type.ToList();
                viewModel.pubTaskList = ep.GetNotifiTaskList(userId);
                viewModel.curUser = curUser;
                viewModel.pubMessageList = pubMessageList;
                viewModel.messageCount = ep.GetUnreadMessageCount(pubMessageList);
                return View(viewModel);
            } else
            {
                return Redirect(ep.GetLogoutUrl());
            }
                
        }
    }
}