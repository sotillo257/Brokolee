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
    public class cargosController : Controller
    {

        pjrdev_condominiosEntities entities = new pjrdev_condominiosEntities();
        EFPublicRepository ep = new EFPublicRepository();
        List<community> communityList = new List<community>();

        // GET: coadmin/cargos
        public ActionResult balance()
        {
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
                    cargosViewModel viewModel = new cargosViewModel();

                    communityList = ep.GetCommunityList(userId);
                    viewModel.communityList = communityList;

                    viewModel.side_menu = "fee_charges";
                    viewModel.side_sub_menu = "fee_charges_balance";
                    viewModel.document_category_list = document_category_list;
                    viewModel.curUser = curUser;
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
        

        
        public ActionResult pago()
        {
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
                    cargosViewModel viewModel = new cargosViewModel();

                    communityList = ep.GetCommunityList(userId);
                    viewModel.communityList = communityList;

                    viewModel.side_menu = "fee_charges";
                    viewModel.side_sub_menu = "fee_charges_pago";
                    viewModel.curUser = curUser;
                    viewModel.document_category_list = document_category_list;
                    viewModel.pubTaskList = ep.GetNotifiTaskList(userId);
                    viewModel.pubMessageList = pubMessageList;
                    viewModel.messageCount = ep.GetUnreadMessageCount(pubMessageList);                   
                    return View(viewModel);
                }
                catch(Exception ex)
                {
                    return Redirect(Url.Action("Index", "Error"));
                }
            } else
            {
                return Redirect(ep.GetLogoutUrl());
            }
               
        }

        public ActionResult estado()
        {
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
                    cargosViewModel viewModel = new cargosViewModel();

                    communityList = ep.GetCommunityList(userId);
                    viewModel.communityList = communityList;

                    viewModel.side_menu = "fee_charges";
                    viewModel.side_sub_menu = "fee_charges_estado";
                    viewModel.document_category_list = document_category_list;
                    viewModel.curUser = curUser;
                    viewModel.pubTaskList = ep.GetNotifiTaskList(userId);
                    viewModel.pubMessageList = pubMessageList;
                    viewModel.messageCount = ep.GetUnreadMessageCount(pubMessageList);                    
                    return View(viewModel);
                }
                catch(Exception ex)
                {
                    return Redirect(Url.Action("Index", "Error"));
                }
            } else
            {
                return Redirect(ep.GetLogoutUrl());
            }
               
        }

        public ActionResult historial()
        {
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
                    cargosViewModel viewModel = new cargosViewModel();
                    viewModel.side_menu = "fee_charges";
                    viewModel.side_sub_menu = "fee_charges_historial";
                    viewModel.document_category_list = document_category_list;
                    viewModel.curUser = curUser;
                    viewModel.pubTaskList = ep.GetNotifiTaskList(userId);
                    viewModel.pubMessageList = pubMessageList;
                    viewModel.messageCount = ep.GetUnreadMessageCount(pubMessageList);               
                    return View(viewModel);
                }
                catch(Exception ex)
                {
                    return Redirect(Url.Action("Index", "Error"));
                }
            } else
            {
                return Redirect(ep.GetLogoutUrl());
            }  
        }
    }
}