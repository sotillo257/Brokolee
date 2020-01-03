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
    public class tecnicoController : Controller
    {
        pjrdev_condominiosEntities entities = new pjrdev_condominiosEntities();
        EFPublicRepository ep = new EFPublicRepository();
        // GET: webmaster/tecnico
        public ActionResult soporte()
        {
            if (Session["USER_ID"] != null)
            {
                long userId = (long)Session["USER_ID"];
                user curUser = entities.users.Find(userId);
                List<ShowMessage> pubMessageList = ep.GetChatMessages(userId);
                long tecUserID = 0;
                if(curUser.create_userid != null)
                {
                    tecUserID = (long)curUser.create_userid;
                }
                soporteTecnicoViewModel viewModel = new soporteTecnicoViewModel();
                if (tecUserID != 0)
                {
                    user tecUser = entities.users.Find(tecUserID);
                    viewModel.tecUser = tecUser;
                    viewModel.chatmessageList = entities.chatmessages.Where(
                       m => (m.from_user_id == userId && m.to_user_id == tecUserID)
                       || (m.from_user_id == tecUserID && m.to_user_id == userId)).ToList();
                }
                else
                {
                    viewModel.tecUser = null;
                    viewModel.chatmessageList = null;
                }
                viewModel.side_menu = "tecnico";
                viewModel.side_sub_menu = "tecnico";
                viewModel.curUser = curUser;
                viewModel.document_category_list = entities.document_type.ToList();
                viewModel.pubTaskList = ep.GetNotifiTaskList(userId);
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