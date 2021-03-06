﻿using System;
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
        public ActionResult panel()
        {
            controlViewModel viewModel = new controlViewModel();
            if (Session["USER_ID"] != null)
            {
                try
                {
                    long userId = (long)Session["USER_ID"];                   
                    user curUser = entities.users.Find(userId);
                    List<ShowMessage> pubMessageList = ep.GetChatMessages(userId);
                    List<document_type> document_category_list = entities.document_type.ToList();                                       

                    communityList = ep.GetCommunityList(userId);                    

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
                        viewModel.communityList = communityList;
                        return View(viewModel);
                    }
                    else
                    {
                        if (Session["CURRENT_COMU"] == null)
                        {
                            community firstCommu = communityList.FirstOrDefault();
                            long selectedCommu = firstCommu.id;
                            Session["CURRENT_COMU"] = selectedCommu;
                        }
                        else
                        {

                            long Id = Convert.ToInt64(Session["CURRENT_COMU"]);
                            List<community> ordenCommunity = new List<community>();
                            community selectedCommu = entities.communities.Find(Id);
                            ordenCommunity.Add(selectedCommu);
                            foreach (community item in communityList)
                            {
                                if (item.id != selectedCommu.id)
                                {
                                    ordenCommunity.Add(item);
                                }                                
                            }

                            communityList.Clear();

                            foreach (community item2 in ordenCommunity)
                            {
                                communityList.Add(item2);
                            }                            
                        }
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

        public JsonResult ChangeCommunity(long id)
        {
            try
            {

                Session["CURRENT_COMU"] = id;
                return Json(new { result = "success" });
            }
            catch (Exception ex)
            {
                return Json(new
                {
                    result = "error",
                    exception = ex.Message
                });
            }
        }

        public ActionResult error(string Error)
        {
            controlViewModel viewModel = new controlViewModel();
            if (Session["USER_ID"] != null)
            {
                if (Error != "")
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
                    ViewBag.msgError = Error;
                    return View(viewModel);
                }
                else
                {
                    return Redirect(Url.Action("panel", "control", new { area = "coadmin" }));
                }                
            }           
            else
            {
                return Redirect(ep.GetLogoutUrl());
            }
        }

        [HttpPost]
        public ActionResult confirmarCorreo(long? id, string correo)
        {
            try
            {
                if (id == null) //Validar correo en AGREGAR NUEVO
                {
                    user user = entities.users.Where(x => x.email == correo).FirstOrDefault();
                    if (user != null)
                    {
                        if (user.role == 2) //Si el usuario es Coadmin
                        {
                            if (user.is_active == true)
                            {
                                return Json(new { result = "adExist" }); //Si el coadmin existe
                            }
                            else
                            {
                                return Json(new { result = "adInact" }); //Si el coadmin existe y esta inactivo
                            }
                        }
                        else if (user.role == 3) //Si el usuario es Webmaster
                        {
                            if (user.is_active == true)
                            {
                                return Json(new { result = "wbExist" }); //Si el webmaster existe
                            }
                            else
                            {
                                return Json(new { result = "wbInact" }); //Si el webmaster existe y esta inactivo
                            }
                        }
                        else //Si el usuario es Titular
                        {
                            if (user.is_active == true)
                            {
                                return Json(new { result = "tiExist" }); //Si el titular existe
                            }
                            else
                            {
                                return Json(new { result = "tiInact" }); //Si el titular existe y esta inactivo
                            }

                        }
                    }
                    else //Si no se encontro ningun usuario con ese email
                    {
                        return Json(new { result = "noExist" });
                    }
                }
                else //Validar correo en EDITAR
                {
                    user user = entities.users.Where(x => x.email == correo).FirstOrDefault();
                    if (user != null)
                    {
                        if (user.id == id)
                        {
                            return Json(new { result = "userEdit" }); //Si se usara el mismo email permitir guardar
                        }
                        else
                        {
                            if (user.role == 2) //Si el usuario es Coadmin
                            {
                                if (user.is_active == true)
                                {
                                    return Json(new { result = "adExist" }); //Si el coadmin existe
                                }
                                else
                                {
                                    return Json(new { result = "adInact" }); //Si el coadmin existe y esta inactivo
                                }
                            }
                            else if (user.role == 3) //Si el usuario es Webmaster
                            {
                                if (user.is_active == true)
                                {
                                    return Json(new { result = "wbExist" }); //Si el coadmin existe
                                }
                                else
                                {
                                    return Json(new { result = "wbInact" }); //Si el coadmin existe y esta inactivo
                                }

                            }
                            else //Si el usuario es Titular
                            {
                                if (user.is_active == true)
                                {
                                    return Json(new { result = "tiExist" }); //Si el coadmin existe
                                }
                                else
                                {
                                    return Json(new { result = "tiInact" }); //Si el coadmin existe y esta inactivo
                                }

                            }
                        }
                    }
                    else //Si no se encontro ningun usuario con ese email
                    {
                        return Json(new { result = "noExistEdit" });
                    }
                }
            }
            catch (Exception ex)
            {
                return Redirect(Url.Action("error", "control", new { area = "coadmin", Error = "Error al validar correo existente: " + ex.Message }));
            }
        }

    }
}