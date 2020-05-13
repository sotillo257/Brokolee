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
    public class paquetesController : Controller
    {
        pjrdev_condominiosEntities entities = new pjrdev_condominiosEntities();
        EFPublicRepository ep = new EFPublicRepository();
        // GET: webmaster/paquetes
        public ActionResult listado(string searchStr = "")
        {
            if (Session["USER_ID"] != null)
            {
                long userId = (long)Session["USER_ID"];
                user curUser = entities.users.Find(userId);
                List<ShowMessage> pubMessageList = ep.GetChatMessages(userId);
                List<package> packageList = new List<package>();
                if (searchStr == "")
                {
                    var query = (from r in entities.packages select r);
                    packageList = query.ToList();

                } else
                {
                    var query = (from r in entities.packages
                                 where r.first_name.Contains(searchStr) == true select r);
                    packageList = query.ToList();
                }
                listadoPaqViewModel viewModel = new listadoPaqViewModel();
                viewModel.side_menu = "paquete";
                viewModel.side_sub_menu = "paquete_listado";
                viewModel.document_category_list = entities.document_type.ToList();
                viewModel.curUser = curUser;
                viewModel.pubTaskList = ep.GetNotifiTaskList(userId);
                viewModel.packageList = packageList;
                viewModel.searchStr = searchStr;
                viewModel.pubMessageList = pubMessageList;
                viewModel.messageCount = ep.GetUnreadMessageCount(pubMessageList);
                return View(viewModel);
            } else
            {
                return Redirect(ep.GetLogoutUrl());
            }
               
        }

        public ActionResult agregar()
        {
            if (Session["USER_ID"] != null)
            {
                long userId = (long)Session["USER_ID"];
                user curUser = entities.users.Find(userId);
                List<ShowMessage> pubMessageList = ep.GetChatMessages(userId);
                agregarPaqViewModel viewModel = new agregarPaqViewModel();
                viewModel.side_menu = "paquete";
                viewModel.side_sub_menu = "paquete_agregar";
                viewModel.document_category_list = entities.document_type.ToList();
                viewModel.curUser = curUser;
                viewModel.pubTaskList = ep.GetNotifiTaskList(userId);
                viewModel.pubMessageList = pubMessageList;
                viewModel.messageCount = ep.GetUnreadMessageCount(pubMessageList);
                return View(viewModel);
            } else
            {
                return Redirect(ep.GetLogoutUrl());
            }
              
        }

        public ActionResult ver(long? viewID)
        {
            if (Session["USER_ID"] != null)
            {
                if (viewID != null)
                {
                    long userId = (long)Session["USER_ID"];
                    user curUser = entities.users.Find(userId);
                    List<ShowMessage> pubMessageList = ep.GetChatMessages(userId);
                    package viewPackage = entities.packages.Find(viewID);
                    verPaqViewModel viewModel = new verPaqViewModel();
                    viewModel.side_menu = "paquete";
                    viewModel.side_sub_menu = "paquete_ver";
                    viewModel.curUser = curUser;
                    viewModel.document_category_list = entities.document_type.ToList();
                    viewModel.pubTaskList = ep.GetNotifiTaskList(userId);
                    viewModel.viewPackage = viewPackage;
                    viewModel.pubMessageList = pubMessageList;
                    viewModel.messageCount = ep.GetUnreadMessageCount(pubMessageList);
                    return View(viewModel);
                }
                else
                {
                    return Redirect(Url.Action("NotFound", "Error"));
                }              
            } else
            {
                return Redirect(ep.GetLogoutUrl());
            }      
        }

        public ActionResult editar(long? editID)
        {
            if (Session["USER_ID"] != null)
            {
                if (editID != null)
                {
                    package editPackage = entities.packages.Find(editID);
                    editarPaqViewModel viewModel = new editarPaqViewModel();
                    long userId = (long)Session["USER_ID"];
                    List<ShowMessage> pubMessageList = ep.GetChatMessages(userId);
                    user curUser = entities.users.Find(userId);
                    viewModel.side_menu = "paquete";
                    viewModel.side_sub_menu = "paquete_ver";
                    viewModel.curUser = curUser;
                    viewModel.document_category_list = entities.document_type.ToList();
                    viewModel.pubTaskList = ep.GetNotifiTaskList(userId);
                    viewModel.editPackage = editPackage;
                    viewModel.pubMessageList = pubMessageList;
                    viewModel.messageCount = ep.GetUnreadMessageCount(pubMessageList);
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

        public JsonResult DeletePackage(long delID)
        {
            try
            {
                package delPackage = entities.packages.Find(delID);
                entities.packages.Remove(delPackage);
                entities.SaveChanges();
                return Json(new { result = "success" });
            } catch(Exception ex)
            {
                return Json(new { result = "error", exception = ex.Message });
            }
        }

        [HttpPost]
        public ActionResult editpaq(long editID, string first_name,
            string description, string payment, string community,
            string admin_email)
        {
            try
            {
                string suscribe_link = "70.35.195.31/suscribe?packageId=" + editID.ToString();
                package editPackage = entities.packages.Find(editID);
                editPackage.first_name = first_name;
                editPackage.admin_email = admin_email;
                editPackage.community = community;
                editPackage.description = description;
                string ppp = payment.Replace('.', ',');
                editPackage.payment = Convert.ToDecimal(ppp);
                editPackage.updated_at = DateTime.Now;
                editPackage.suscribe_link = suscribe_link;
                entities.SaveChanges();
                return Redirect(Url.Action("listado", "paquetes", new { area = "webmaster" }));
            }catch(Exception ex)
            {
                return Redirect(Url.Action("editar", "paquetes", 
                    new {
                        editID = editID,
                        area = "webmaster",
                        exception = ex.Message
                    }));
            }
        }

        [HttpPost]
        public ActionResult newpaq(string first_name, string description, string payment,
            string community, string admin_email)
        {
            try
            {
                package newPackage = new package();
                newPackage.first_name = first_name;
                newPackage.community = community;
                newPackage.admin_email = admin_email;
                newPackage.description = description;
                newPackage.created_at = DateTime.Now;                               
                string ppp = payment.Replace('.', ',');               
                newPackage.payment = Convert.ToDecimal(ppp);                
                entities.packages.Add(newPackage);
                entities.SaveChanges();
                newPackage = entities.packages.Find(newPackage.id);
                string suscribe_link = "70.35.195.31/suscribe?packageId=" + newPackage.id.ToString();
                newPackage.suscribe_link = suscribe_link;
                entities.SaveChanges();
                return Redirect(Url.Action("listado", "paquetes", new { area = "webmaster" }));
            }catch(Exception ex)
            {
                return Redirect(Url.Action("agregar", "paquetes", 
                    new {
                        area = "webmaster",
                        exception = ex.Message
                    }));
            }
        }

        [HttpPost]
        public ActionResult SearchResult(string searchStr)
        {
            return Redirect(Url.Action("listado", "paquetes",
                new {
                    area = "webmaster",
                    searchStr = searchStr
                }));
        }
    }
}