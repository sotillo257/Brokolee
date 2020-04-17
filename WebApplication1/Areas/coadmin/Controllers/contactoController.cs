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
    public class contactoController : Controller
    {
        pjrdev_condominiosEntities entities = new pjrdev_condominiosEntities();
        EFPublicRepository ep = new EFPublicRepository();
        // GET: coadmin/contacto
        public ActionResult listado(string Error, string searchStr = "")
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
                    List<contactinfo> contactList = new List<contactinfo>();

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

                    contactoViewModel viewModel = new contactoViewModel();
                    viewModel.side_menu = "contacto";
                    viewModel.side_sub_menu = "contacto_informacion";
                    viewModel.contactList = contactList;
                    viewModel.document_category_list = entities.document_type.ToList();
                    viewModel.curUser = curUser;
                    viewModel.searchStr = searchStr;
                    viewModel.pubTaskList = ep.GetNotifiTaskList(userId);
                    viewModel.pubMessageList = pubMessageList;
                    viewModel.messageCount = ep.GetUnreadMessageCount(pubMessageList);
                    ViewBag.msgError = Error;
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

        public ActionResult informacion()
        {
            if (Session["USER_ID"] != null)
            {
                try
                {
                    long communityAct = Convert.ToInt64(Session["CURRENT_COMU"]);
                    List<contactinfo> contactinfoList = entities.contactinfoes.Where(m => m.community_id == communityAct).ToList();
                    if (contactinfoList == null || contactinfoList.Count == 0)
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
                        contactoViewModel viewModel = new contactoViewModel();
                        viewModel.side_menu = "contacto";
                        viewModel.side_sub_menu = "contacto_informacion";
                        viewModel.document_category_list = document_category_list;
                        if (contactinfoList.Count > 0)
                        {
                            viewModel.editContactInfo = contactinfoList.First();
                        }
                        else
                        {
                            viewModel.editContactInfo = null;
                        }
                        viewModel.curUser = curUser;
                        viewModel.pubTaskList = ep.GetNotifiTaskList(userId);
                        viewModel.pubMessageList = pubMessageList;
                        viewModel.messageCount = ep.GetUnreadMessageCount(pubMessageList);
                        return View(viewModel);
                    }
                    else
                    {
                        return Redirect(Url.Action("listado", "contacto", new { Error = "Solo puede existir una lista de contactos por comunidad" }));
                    }

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

        public ActionResult editar(long? id, string Error)
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



        [HttpPost]
        public ActionResult editcontact(long contactID, string company_admin,
            string coordinator, string president, string vice_president,
            string treasurer, string secretary, string vocal1, string vocal2,
            string vocal3, string phy_address, string postal_address,
            string phone_number1, string phone_number2, string email
            )
        {
            try
            {
                long communityAct = Convert.ToInt64(Session["CURRENT_COMU"]);

                long user_id = (long)Session["USER_ID"];
                contactinfo contactinfo = entities.contactinfoes.Find(contactID);
                contactinfo.company_admin = company_admin;
                contactinfo.coordinator = coordinator;
                contactinfo.president = president;
                contactinfo.vice_president = vice_president;
                contactinfo.treasurer = treasurer;
                contactinfo.secretary = secretary;
                contactinfo.vocal1 = vocal1;
                contactinfo.vocal2 = vocal2;
                contactinfo.vocal3 = vocal3;
                contactinfo.phy_address = phy_address;
                contactinfo.postal_address = postal_address;
                contactinfo.phone_number1 = phone_number1;
                contactinfo.phone_number2 = phone_number2;
                contactinfo.email = email;
                contactinfo.user_id = user_id;
                entities.SaveChanges();
                return Redirect(Url.Action("listado", "contacto", new { area = "coadmin" }));
            }
            catch (Exception ex)
            {
                return Redirect(Url.Action("informacion", "contacto",
                    new
                    {
                        area = "coadmin",
                        exception = ex.Message
                    }));
            }
        }

        public JsonResult DeleteContact(long delID)
        {
            try
            {
                contactinfo delContact = entities.contactinfoes.Find(delID);
                entities.contactinfoes.Remove(delContact);
                entities.SaveChanges();
                return Json(new { result = "success" });
            }
            catch (Exception ex)
            {
                return Json(new { result = "error", exception = ex.HResult });
            }
        }

        [HttpPost]
        public ActionResult SeacrhResult(string searchStr)
        {
            return Redirect(Url.Action("listado", "contacto", new { area = "coadmin", searchStr = searchStr }));
        }

        [HttpPost]
        public ActionResult informacionNew(long? contactID, string company_admin,
            string coordinator, string president, string vice_president,
            string treasurer, string secretary, string vocal1, string vocal2,
            string vocal3, string phy_address, string postal_address,
            string phone_number1, string phone_number2, string email
            )
        {
            long communityAct = Convert.ToInt64(Session["CURRENT_COMU"]);
            List<contactinfo> contactinfoList = entities.contactinfoes.Where(m => m.community_id == communityAct).ToList();
            if(contactinfoList == null || contactinfoList.Count == 0)
            {
                try
                {

                    long user_id = (long)Session["USER_ID"];
                    contactinfo contactinfo = new contactinfo();
                    contactinfo.company_admin = company_admin;
                    contactinfo.coordinator = coordinator;
                    contactinfo.president = president;
                    contactinfo.vice_president = vice_president;
                    contactinfo.treasurer = treasurer;
                    contactinfo.secretary = secretary;
                    contactinfo.vocal1 = vocal1;
                    contactinfo.vocal2 = vocal2;
                    contactinfo.vocal3 = vocal3;
                    contactinfo.phy_address = phy_address;
                    contactinfo.postal_address = postal_address;
                    contactinfo.phone_number1 = phone_number1;
                    contactinfo.phone_number2 = phone_number2;
                    contactinfo.email = email;
                    contactinfo.user_id = user_id;
                    contactinfo.community_id = communityAct;
                    entities.contactinfoes.Add(contactinfo);
                    entities.SaveChanges();
                    return Redirect(Url.Action("listado", "contacto", new { area = "coadmin" }));
                }
                catch (Exception ex)
                {
                    return Json(new { result = "error", exception = ex.HResult });
                }
            }
            else
            {
                return Redirect(Url.Action("listado", "contacto", new { Error = "Solo puede existir una lista de contactos por comunidad"}));
            }
            
            //try
            //{
            //if (contactID != 0)
            //    {
            //        contactinfo contactinfo = entities.contactinfoes.Find(contactID);
            //        contactinfo.company_admin = company_admin;
            //        contactinfo.coordinator = coordinator;
            //        contactinfo.president = president;
            //        contactinfo.vice_president = vice_president;
            //        contactinfo.treasurer = treasurer;
            //        contactinfo.secretary = secretary;
            //        contactinfo.vocal1 = vocal1;
            //        contactinfo.vocal2 = vocal2;
            //        contactinfo.vocal3 = vocal3;
            //        contactinfo.phy_address = phy_address;
            //        contactinfo.postal_address = postal_address;
            //        contactinfo.phone_number1 = phone_number1;
            //        contactinfo.phone_number2 = phone_number2;
            //        contactinfo.email = email;
            //        contactinfo.user_id = user_id;
            //        entities.SaveChanges();

            //    }
            //    else
            //    {
            //        contactinfo contactinfo = new contactinfo();
            //        contactinfo.company_admin = company_admin;
            //        contactinfo.coordinator = coordinator;
            //        contactinfo.president = president;
            //        contactinfo.vice_president = vice_president;
            //        contactinfo.treasurer = treasurer;
            //        contactinfo.secretary = secretary;
            //        contactinfo.vocal1 = vocal1;
            //        contactinfo.vocal2 = vocal2;
            //        contactinfo.vocal3 = vocal3;
            //        contactinfo.phy_address = phy_address;
            //        contactinfo.postal_address = postal_address;
            //        contactinfo.phone_number1 = phone_number1;
            //        contactinfo.phone_number2 = phone_number2;
            //        contactinfo.email = email;
            //        contactinfo.user_id = user_id;
            //        entities.contactinfoes.Add(contactinfo);
            //        entities.SaveChanges();
            //    }                          
            //catch (Exception ex)
            //{
            //        return Redirect(Url.Action("informacion", "contacto",
            //            new
            //            {
            //                area = "coadmin",
            //                exception = ex.Message
            //            }));
            //    }
            //}
        }
    }
}