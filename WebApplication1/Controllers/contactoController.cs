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
    public class contactoController : Controller
    {
        pjrdev_condominiosEntities entities = new pjrdev_condominiosEntities();
        EFPublicRepository ep = new EFPublicRepository();
        List<Titulo> titulosList = new List<Titulo>();
        List<community> listComunities = new List<community>();

        // GET: contacto
        //public ActionResult informacion()
        //{
        //    if (Session["USER_ID"] != null)
        //    {
        //        try
        //        {
        //            long userId = 0;
        //            if (Convert.ToInt32(Session["USER_ROLE"]) == 1)
        //            {
        //                userId = (long)Session["USER_ID"];
        //            }
        //            else if (Convert.ToInt32(Session["USER_ROLE"]) > 1
        //            && Session["ACC_USER_ID"] != null)
        //            {
        //                userId = (long)Session["ACC_USER_ID"];
        //            }

        //            user curUser = entities.users.Find(userId);
        //            List<ShowMessage> pubMessageList = ep.GetChatMessages(userId);
        //            contactinfo contactinfo = entities.contactinfoes
        //                .Where(m => m.user_id == curUser.create_userid).FirstOrDefault();
        //            contactoViewModel viewModel = new contactoViewModel();

        //            titulosList = ep.GetTitulosByTitular(userId);
        //            listComunities = ep.GetCommunityListByTitular(titulosList);
        //            viewModel.communityList = listComunities;
        //            viewModel.communityID1 = Convert.ToInt64(Session["CURRENT_COMU"]);
        //            viewModel.side_menu = "contacto";
        //            viewModel.side_sub_menu = "";
        //            viewModel.document_category_list = entities.document_type.ToList();
        //            viewModel.curUser = curUser;
        //            viewModel.contactinfo = contactinfo;
        //            viewModel.pubTaskList = ep.GetNotifiTaskList(userId);
        //            viewModel.pubMessageList = pubMessageList;
        //            viewModel.messageCount = ep.GetUnreadMessageCount(pubMessageList);                    
        //            return View(viewModel);
        //        }
        //        catch(Exception ex)
        //        {
        //            return Redirect(Url.Action("Index", "Error"));
        //        }
        //    }
        //    else
        //    {
        //        return Redirect(Url.Action("iniciar", "iniciar"));
        //    }
                
        //}

        public ActionResult listado(string searchStr = "")
        {
            if (Session["USER_ID"] != null)
            {
                try
                {
                    long userId = (long)Session["USER_ID"];                    
                    user curUser = entities.users.Find(userId);
                    List<ShowMessage> pubMessageList = ep.GetChatMessages(userId);
                    List<contactinfo> contactList = new List<contactinfo>();

                    titulosList = ep.GetTitulosByTitular(userId);
                    listComunities = ep.GetCommunityListByTitular(titulosList);                    

                    if (listComunities.Count != 0)
                    {
                        long communityAct = Convert.ToInt64(Session["CURRENT_COMU"]);
                        if (searchStr == "")
                        {
                            var query1 = (from r in entities.contactinfoes where r.community_id == communityAct select r);
                            contactList = query1.ToList();
                        }
                        else
                        {
                            var query2 = (from r in entities.contactinfoes
                                          where r.community_id == communityAct && r.company_admin.Contains(searchStr) == true
                                          select r);
                            contactList = query2.ToList();
                        }
                    }                    

                    contactoViewModel viewModel = new contactoViewModel();
                    viewModel.communityList = listComunities;
                    viewModel.side_menu = "contacto";
                    viewModel.side_sub_menu = "contacto_informacion";
                    viewModel.contactList = contactList;
                    viewModel.document_category_list = entities.document_type.ToList();
                    viewModel.curUser = curUser;
                    viewModel.searchStr = searchStr;
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

        public ActionResult ver(long? id, string Error)
        {

            if (Session["USER_ID"] != null)
            {
                if (id != null)
                {
                    long communityAct = Convert.ToInt64(Session["CURRENT_COMU"]);
                    long userId = (long)Session["USER_ID"];
                    contactinfo infoContact = entities.contactinfoes.Find(id);
                    List<ShowMessage> pubMessageList = ep.GetChatMessages(userId);
                    user curUser = entities.users.Find(userId);
                    contactoViewModel viewModel = new contactoViewModel();
                    viewModel.side_menu = "contacto";
                    viewModel.side_sub_menu = "manage_edit_headlines";
                    viewModel.document_category_list = entities.document_type.ToList();
                    viewModel.curUser = curUser;
                    viewModel.messageCount = ep.GetUnreadMessageCount(pubMessageList);
                    viewModel.editContactInfo = infoContact;
                    viewModel.pubTaskList = ep.GetNotifiTaskList(userId);
                    viewModel.pubMessageList = pubMessageList;
                    viewModel.messageCount = ep.GetUnreadMessageCount(pubMessageList);
                    ViewBag.msgError = Error;
                    return View(viewModel);
                }
                else
                {
                    return Redirect(Url.Action("NotFound", "Error"));
                }
            }
            else
            {
                return Redirect(ep.GetLogoutUrl());
            }
        }
    }
}