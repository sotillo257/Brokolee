﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WebApplication1.Concrete;
using WebApplication1.Models;
using WebApplication1.ViewModels;

namespace WebApplication1.Controllers
{
    public class tecnicoController : Controller
    {
        pjrdev_condominiosEntities entities = new pjrdev_condominiosEntities();
        EFPublicRepository ep = new EFPublicRepository();
        // GET: coadmin/tecnico
        public ActionResult soporte()
        {
            if (Session["USER_ID"] != null)
            {
                long userId = (long)Session["USER_ID"];
                List<ShowMessage> pubMessageList = ep.GetChatMessages(userId);
                user curUser = entities.users.Find(userId);
                long tecUserId = (long)curUser.create_userid;
                user tecUser = entities.users.Find(tecUserId);
                tecnicoViewModel viewModel = new tecnicoViewModel();
                viewModel.side_menu = "tecnico";
                viewModel.side_sub_menu = "";
                viewModel.document_category_list = entities.document_type.ToList();
                viewModel.curUser = curUser;
                viewModel.tecUser = tecUser;
                viewModel.chatmessageList = entities.chatmessages.Where(
                        m => (m.from_user_id == userId && m.to_user_id == tecUserId)
                        || (m.from_user_id == tecUserId && m.to_user_id == userId)).ToList();
                viewModel.pubTaskList = ep.GetNotifiTaskList(userId);
                viewModel.pubMessageList = pubMessageList;
                viewModel.messageCount = ep.GetUnreadMessageCount(pubMessageList);
                viewModel.communityName = ep.GetCommunityCoInfo(userId)[0];
                viewModel.communityApart = ep.GetCommunityCoInfo(userId)[1];
                return View(viewModel);
            }
            else
            {
                return Redirect(Url.Action("iniciar", "iniciar"));
            }

        }
    }
}