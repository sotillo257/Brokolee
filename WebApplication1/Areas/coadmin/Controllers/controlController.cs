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
    public class controlController : Controller
    {
        pjrdev_condominiosEntities entities = new pjrdev_condominiosEntities();
        EFPublicRepository ep = new EFPublicRepository();
        List<community> communityList = new List<community>();      

        // GET: coadmin/control
        public ActionResult panel(long? Id)
        {
            controlViewModel viewModel = new controlViewModel();
            if (Session["USER_ID"] != null)
            {
                try
                {
                    long userId = 0;
                    if (Convert.ToInt32(Session["USER_ROLE"]) == 2)
                    {
                        userId = (long)Session["USER_ID"];
                    }
                    else if (Convert.ToInt32(Session["USER_ROLE"]) > 2
                    && Session["ACC_USER_ID"] != null)
                    {
                        userId = (long)Session["ACC_USER_ID"];
                    }
                    user curUser = entities.users.Find(userId);
                    List<ShowMessage> pubMessageList = ep.GetChatMessages(userId);
                    List<document_type> document_category_list = entities.document_type.ToList();                                       

                    communityList = ep.GetCommunityList(userId);
                    viewModel.communityList = communityList;

                    if(communityList.Count == 0)
                    {
                        viewModel.side_menu = "control_panel";
                        viewModel.side_sub_menu = "";
                        viewModel.document_category_list = document_category_list;
                        viewModel.curUser = curUser;
                        viewModel.pubTaskList = ep.GetNotifiTaskList(userId);
                        viewModel.pubMessageList = pubMessageList;
                        viewModel.messageCount = ep.GetUnreadMessageCount(pubMessageList);
                        viewModel.isPartial = false;
                        return View(viewModel);
                    }
                    else
                    {
                        if (Session["CURRENT_COMU"] == null)
                        {
                            community firstCommu = communityList.LastOrDefault();
                            long selectedCommu = firstCommu.id;
                            Session["CURRENT_COMU"] = selectedCommu;
                        }
                        else
                        {
                            if (Id != null)
                            {
                                Session["CURRENT_COMU"] = Id;
                            }                            
                        }
                        viewModel.side_menu = "control_panel";
                        viewModel.side_sub_menu = "";
                        viewModel.document_category_list = document_category_list;
                        viewModel.curUser = curUser;
                        viewModel.pubTaskList = ep.GetNotifiTaskList(userId);
                        viewModel.pubMessageList = pubMessageList;
                        viewModel.messageCount = ep.GetUnreadMessageCount(pubMessageList);
                        viewModel.isPartial = false;
                        return View(viewModel);
                    }
                                     
                }
                catch(Exception ex)
                {
                    return Redirect(Url.Action("Index", "Error"));
                }
            }
            else if (Session["SUS_USER_ID"] != null)
            {
                long userId = (long)Session["SUS_USER_ID"];
                user curUser = entities.users.Find(userId);
                List<ShowMessage> pubMessageList = ep.GetChatMessages(userId);
                List<document_type> document_category_list = entities.document_type.ToList();

                communityList = ep.GetCommunityList(userId);
                viewModel.communityList = communityList;

                viewModel.side_menu = "control_panel";
                viewModel.side_sub_menu = "";
                viewModel.document_category_list = document_category_list;
                viewModel.curUser = curUser;
                viewModel.pubTaskList = ep.GetNotifiTaskList(userId);
                viewModel.pubMessageList = pubMessageList;
                viewModel.messageCount = ep.GetUnreadMessageCount(pubMessageList);                
                viewModel.isPartial = false;
                return View(viewModel);
            }
            else
            {
                return Redirect(ep.GetLogoutUrl());
            }     
        }
       
    }
}