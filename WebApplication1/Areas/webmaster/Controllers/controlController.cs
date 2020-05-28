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
    public class controlController : Controller
    {
        pjrdev_condominiosEntities entities = new pjrdev_condominiosEntities();
        EFPublicRepository ep = new EFPublicRepository();
        List<community> communityList = new List<community>();
        // GET: webmaster/control
        public ActionResult panel()
        {
            if (Session["USER_ID"] != null)
            {
                long userId = (long)Session["USER_ID"];
                user curUser = entities.users.Find(userId);
                List<ShowMessage> pubMessageList = ep.GetChatMessages(userId);
                panelControlViewModel viewModel = new panelControlViewModel();
                viewModel.curUser = curUser;
                
                viewModel.pubTaskList = ep.GetNotifiTaskList(userId);
                viewModel.side_menu = "control_panel";
                viewModel.side_sub_menu = "control_panel";
                viewModel.pubMessageList = pubMessageList;
                viewModel.messageCount = ep.GetUnreadMessageCount(pubMessageList);
                return View(viewModel);
            } else
            {
                return Redirect(ep.GetLogoutUrl());
            }     
        }

        public ActionResult error(string Error)
        {
            panelControlViewModel viewModel = new panelControlViewModel();
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
                return Redirect(Url.Action("error", "control", new { area = "webmaster", Error = "Error al validar correo existente: " + ex.Message }));
            }
        }
    }
}