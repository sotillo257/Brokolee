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
    public class usoController : Controller
    {
        pjrdev_condominiosEntities entities = new pjrdev_condominiosEntities();
        EFPublicRepository ep = new EFPublicRepository();

        List<Titulo> titulosList = new List<Titulo>();
        List<community> listComunities = new List<community>();

        // GET: uso
        public ActionResult privacidad()
        {
            if (Session["USER_ID"] != null)
            {
                try
                {
                    long userId = (long)Session["USER_ID"];
                    long communityAct = Convert.ToInt64(Session["CURRENT_COMU"]);                    
                    user curUser = entities.users.Find(userId);
                    List<ShowMessage> pubMessageList = ep.GetChatMessages(userId);
                    usoViewModel viewModel = new usoViewModel();
                    List<uso> usoList = entities.usoes.Where(m => m.create_userid == curUser.create_userid).ToList();
                    if (usoList.Count > 0)
                    {
                        viewModel.uso = usoList.First();
                    }
                    else
                    {
                        viewModel.uso = null;
                    }

                    titulosList = ep.GetTitulosByTitular(userId);
                    listComunities = ep.GetCommunityListByTitular(titulosList);
                    viewModel.communityList = listComunities;

                    viewModel.side_menu = "uso";
                    viewModel.side_sub_menu = "uso_privacidad";
                     viewModel.document_category_list = entities.document_type.Where(x => x.community_id == communityAct).ToList();
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
                return Redirect(Url.Action("iniciar", "iniciar"));
            }
                
        }

        public ActionResult terminos()
        {
            if (Session["USER_ID"] != null)
            {
                long userId = (long)Session["USER_ID"];
                long communityAct = Convert.ToInt64(Session["CURRENT_COMU"]);
                user curUser = entities.users.Find(userId);
                List<ShowMessage> pubMessageList = ep.GetChatMessages(userId);
                usoViewModel viewModel = new usoViewModel();
                List<uso> usoList = entities.usoes.Where(m => m.create_userid == curUser.create_userid).ToList();
                if (usoList.Count > 0)
                {
                    viewModel.uso = usoList.First();
                } else
                {
                    viewModel.uso = null;
                }

                titulosList = ep.GetTitulosByTitular(userId);
                listComunities = ep.GetCommunityListByTitular(titulosList);
                viewModel.communityList = listComunities;

                viewModel.side_menu = "uso";
                viewModel.side_sub_menu = "uso_terminos";
                 viewModel.document_category_list = entities.document_type.Where(x => x.community_id == communityAct).ToList();
                viewModel.curUser = curUser;
                viewModel.pubTaskList = ep.GetNotifiTaskList(userId);
                viewModel.pubMessageList = pubMessageList;
                viewModel.messageCount = ep.GetUnreadMessageCount(pubMessageList);                
                return View(viewModel);
            } else
            {
                return Redirect(Url.Action("iniciar", "iniciar"));
            }
            
        }

        public ActionResult derechos()
        {
            if (Session["USER_ID"] != null)
            {
                long userId = (long)Session["USER_ID"];
                long communityAct = Convert.ToInt64(Session["CURRENT_COMU"]);
                user curUser = entities.users.Find(userId);
                List<ShowMessage> pubMessageList = ep.GetChatMessages(userId);
                usoViewModel viewModel = new usoViewModel();
                long createUserID = (long)curUser.create_userid;
                List<uso> usoList = entities.usoes.Where(m => m.create_userid == createUserID).ToList();
                if (usoList.Count > 0)
                {
                    viewModel.uso = usoList.First();
                } else
                {
                    viewModel.uso = null;
                }

                titulosList = ep.GetTitulosByTitular(userId);
                listComunities = ep.GetCommunityListByTitular(titulosList);
                viewModel.communityList = listComunities;
             
                viewModel.side_menu = "uso";
                viewModel.side_sub_menu = "uso_derechos";
                 viewModel.document_category_list = entities.document_type.Where(x => x.community_id == communityAct).ToList();
                viewModel.curUser = curUser;
                viewModel.pubTaskList = ep.GetNotifiTaskList(userId);
                viewModel.pubMessageList = pubMessageList;
                viewModel.messageCount = ep.GetUnreadMessageCount(pubMessageList);                
                return View(viewModel);
            } else
            {
                return Redirect(Url.Action("iniciar", "iniciar"));
            }     
        }
    }
}