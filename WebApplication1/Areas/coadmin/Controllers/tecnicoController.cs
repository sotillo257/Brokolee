using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WebApplication1.Areas.coadmin.ViewModels;
using WebApplication1.Concrete;
using WebApplication1.Models;

namespace WebApplication1.Areas.coadmin.Controllers
{
    public class tecnicoController : Controller
    {
        pjrdev_condominiosEntities entities = new pjrdev_condominiosEntities();
        EFPublicRepository ep = new EFPublicRepository();
        List<community> communityList = new List<community>();

        // GET: coadmin/tecnico
        public ActionResult soporte()
        {
            if (Session["USER_ID"] != null)
            {
                try
                {
                    long userId = (long)Session["USER_ID"];
                    long communityAct = Convert.ToInt64(Session["CURRENT_COMU"]);                    
                    user curUser = entities.users.Find(userId);
                    List<ShowMessage> pubMessageList = ep.GetChatMessages(userId);
                    long tecUserId = (long)curUser.create_userid;
                    user tecUser = entities.users.Find(tecUserId);
                    tecnicoViewModel viewModel = new tecnicoViewModel();

                    communityList = ep.GetCommunityList(userId);
                    viewModel.communityList = communityList;

                    viewModel.side_menu = "tecnico";
                    viewModel.side_sub_menu = "";
                     viewModel.document_category_list = entities.document_type.Where(x => x.community_id == communityAct).ToList();
                    viewModel.chatmessageList = entities.chatmessages.Where(
                            m => (m.from_user_id == userId && m.to_user_id == tecUserId)
                            || (m.from_user_id == tecUserId && m.to_user_id == userId)).ToList();
                    viewModel.curUser = curUser;
                    viewModel.tecUser = tecUser;
                    viewModel.pubTaskList = ep.GetNotifiTaskList(userId);
                    viewModel.pubMessageList = pubMessageList;
                    viewModel.messageCount = ep.GetUnreadMessageCount(pubMessageList);
                    return View(viewModel);
                }
                catch (Exception ex)
                {
                    return Redirect(Url.Action("Index", "Error"));
                }

            }
            else
            {
                return Redirect(ep.GetLogoutUrl());
            }

        }
    }
}