using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WebApplication1.Concrete;
using WebApplication1.Models;
using WebApplication1.ViewModels;

namespace WebApplication1.Controllers
{
    public class controlController : Controller
    {
        pjrdev_condominiosEntities entities = new pjrdev_condominiosEntities();
        EFPublicRepository ep = new EFPublicRepository();
        // GET: community
        public ActionResult community()
        {
            if (Session["USER_ID"] != null)
            {
                try
                {
                    long userId = 0;
                    if (Convert.ToInt32(Session["USER_ROLE"]) == 1)
                    {
                        userId = (long)Session["USER_ID"];
                    }
                    else if (Convert.ToInt32(Session["USER_ROLE"]) > 1
                    && Session["ACC_USER_ID"] != null)
                    {
                        userId = (long)Session["ACC_USER_ID"];
                    }

                    user curUser = entities.users.Find(userId);
                    List<ShowMessage> pubMessageList = ep.GetChatMessages(userId);
                    List<community> communityList = entities.communities.ToList();
                    communityControlViewModel viewModel = new communityControlViewModel();
                    viewModel.side_menu = "control";
                    viewModel.side_sub_menu = "control";
                    viewModel.document_category_list = entities.document_type.ToList();
                    viewModel.curUser = curUser;
                    viewModel.pubTaskList = ep.GetNotifiTaskList(userId);
                    viewModel.pubMessageList = pubMessageList;
                    viewModel.messageCount = ep.GetUnreadMessageCount(pubMessageList);
                    viewModel.communityName = ep.GetCommunityCoInfo(userId)[0];
                    viewModel.communityApart = ep.GetCommunityCoInfo(userId)[1];
                    viewModel.communityList = communityList;
                    return View(viewModel);
                }
                catch(Exception ex)
                {
                    return Redirect(Url.Action("Index", "Error"));
                }                
            }
            else
            {
                return Redirect(Url.Action("iniciar", "iniciar"));
            }
        }
        // GET: control
        public ActionResult panel(long? id)
        {
            if (Session["USER_ID"] != null)
            {
                if (id != null)
                {
                    try
                    {
                        long userId = 0;
                        if (Convert.ToInt32(Session["USER_ROLE"]) == 1)
                        {
                            userId = (long)Session["USER_ID"];
                        }
                        else if (Convert.ToInt32(Session["USER_ROLE"]) > 1
                        && Session["ACC_USER_ID"] != null)
                        {
                            userId = (long)Session["ACC_USER_ID"];
                        }

                        long comId = Convert.ToInt64(id);
                        List<blog> blogList = new List<blog>();
                        user curUser = entities.users.Find(userId);
                        List<ShowMessage> pubMessageList = ep.GetChatMessages(userId);
                        communuser communuser = entities.communusers.Where(m => m.commun_id == comId).FirstOrDefault();
                        if (communuser != null)
                        {
                            long comUserId = communuser.user_id;
                            blogList = entities.blogs.Where(m => m.user_id == comUserId).ToList();
                        }

                        controlViewModel viewModel = new controlViewModel();
                        viewModel.ComID = comId;
                        viewModel.side_menu = "control";
                        viewModel.document_category_list = entities.document_type.ToList();
                        viewModel.curUser = curUser;
                        viewModel.pubTaskList = ep.GetNotifiTaskList(userId);
                        viewModel.pubMessageList = pubMessageList;
                        viewModel.messageCount = ep.GetUnreadMessageCount(pubMessageList);
                        viewModel.communityName = ep.GetCommunityCoInfo(userId)[0];
                        viewModel.communityApart = ep.GetCommunityCoInfo(userId)[1];
                        viewModel.blogList = blogList;
                        return View(viewModel);
                    }
                    catch(Exception ex)
                    {
                        return Redirect(Url.Action("Index", "Error"));
                    }                   
                }
                else
                {
                    return Redirect(Url.Action("NotFound", "Error"));
                }
            }
            else
            {
                return Redirect(Url.Action("iniciar", "iniciar"));
            } 
        }
    }
}