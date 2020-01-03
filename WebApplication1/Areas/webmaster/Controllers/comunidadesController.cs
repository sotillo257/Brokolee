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
    public class comunidadesController : Controller
    {
        pjrdev_condominiosEntities entities = new pjrdev_condominiosEntities();
        EFPublicRepository ep = new EFPublicRepository();
        // GET: webmaster/comunidades
        public ActionResult listado(string searchStr = "")
        {
            if (Session["USER_ID"] != null)
            {
                long userId = (long)Session["USER_ID"];
                user curUser = entities.users.Find(userId);
                List<ShowMessage> pubMessageList = ep.GetChatMessages(userId);
                Dictionary<long, string> packageDict = new Dictionary<long, string>();
                List<community> communityList = new List<community>();

                if (searchStr != "")
                {
                    var query = (from r in entities.communities
                                 where r.first_name.Contains(searchStr) == true
                                 select r);
                    communityList = query.ToList();
                }
                else
                {
                    var query = (from r in entities.communities select r);
                    communityList = query.ToList();
                }

                foreach (var item in communityList)
                {
                    package package = entities.packages.Find(item.package_id);
                    packageDict.Add(item.id, package.first_name);
                }

                listadoCommunViewModel viewModel = new listadoCommunViewModel();
                viewModel.side_menu = "comunidades";
                viewModel.side_sub_menu = "comunidades_listado";
                viewModel.document_category_list = entities.document_type.ToList();
                viewModel.curUser = curUser;
                viewModel.communityList = communityList;
                viewModel.searchStr = searchStr;
                viewModel.pubTaskList = ep.GetNotifiTaskList(userId);
                viewModel.pubMessageList = pubMessageList;
                viewModel.messageCount = ep.GetUnreadMessageCount(pubMessageList);
                viewModel.packageDict = packageDict;
                return View(viewModel);
            }
            else
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
                agregarCommunViewModel viewModel = new agregarCommunViewModel();
                listadoAdminViewModel viewModel2 = new listadoAdminViewModel();
                List<user> coadminList = new List<user>();
                Dictionary<long, string> communityDict = new Dictionary<long, string>();
                var query = (from r in entities.users
                             where (r.role == 2 || r.role == 4) && r.is_del != true
                             select r);
                coadminList = query.ToList();
                viewModel.side_menu = "comunidades";
                viewModel.side_sub_menu = "comunidades_agregar";
                viewModel.document_category_list = entities.document_type.ToList();
                viewModel.curUser = curUser;
                viewModel.pubTaskList = ep.GetNotifiTaskList(userId);
                viewModel.pubMessageList = pubMessageList;
                viewModel.packageList = entities.packages.ToList();
                viewModel.coadminList = coadminList;
                
                viewModel.messageCount = ep.GetUnreadMessageCount(pubMessageList);
                return View(viewModel);
            }
            else
            {
                return Redirect(ep.GetLogoutUrl());
            }
        }

        public ActionResult editar(long? id)
        {
            if (Session["USER_ID"] != null)
            {
                if (id != null)
                {
                    long userId = (long)Session["USER_ID"];
                    user curUser = entities.users.Find(userId);
                    List<ShowMessage> pubMessageList = ep.GetChatMessages(userId);
                    community editCommunity = entities.communities.Find(id);
                    editarCommunViewModel viewModel = new editarCommunViewModel();
                    viewModel.curUser = curUser;
                    viewModel.side_menu = "comunidades";
                    viewModel.side_sub_menu = "comunidades_editar";
                    viewModel.packageList = entities.packages.ToList();
                    viewModel.editCommunity = editCommunity;
                    viewModel.pubTaskList = ep.GetNotifiTaskList(userId);
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

        public ActionResult ver(long? id)
        {
            if (Session["USER_ID"] != null)
            {
                if (id != null)
                {
                    long userId = (long)Session["USER_ID"];
                    user curUser = entities.users.Find(userId);
                    List<ShowMessage> pubMessageList = ep.GetChatMessages(userId);
                    community viewCommunity = entities.communities.Find(id);
                    List<communuser> commonuserList = entities.communusers.Where(m => m.commun_id == id).ToList();
                    List<user> titularList = new List<user>();
                    foreach (var item in commonuserList)
                    {
                        user coadminUser = entities.users.Find(item.user_id);
                        List<user> subTitularList = entities.users.Where(m => m.create_userid == coadminUser.id).ToList();
                        titularList.AddRange(subTitularList);
                    }
                    verCommunViewModel viewModel = new verCommunViewModel();
                    viewModel.curUser = curUser;
                    viewModel.side_menu = "comunidades";
                    viewModel.side_sub_menu = "comunidades_ver";
                    viewModel.document_category_list = entities.document_type.ToList();
                    viewModel.packageList = entities.packages.ToList();
                    viewModel.viewCommunity = viewCommunity;
                    package package = entities.packages.Find(viewCommunity.package_id);
                    viewModel.package_name = package.first_name;
                    viewModel.pubTaskList = ep.GetNotifiTaskList(userId);
                    viewModel.pubMessageList = pubMessageList;
                    viewModel.messageCount = ep.GetUnreadMessageCount(pubMessageList);
                    viewModel.titularList = titularList;
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
        public ActionResult newcommunity(string first_name,
            string description, string admin_email, long packageId, string apartment)
        {
            try
            {
                community newCommunity = new community();
                newCommunity.first_name = first_name;
                newCommunity.admin_email = admin_email;
                newCommunity.package_id = packageId;
                newCommunity.description = description;
                newCommunity.created_at = DateTime.Now;
                newCommunity.apart = apartment;
                newCommunity.is_active = false;
                entities.communities.Add(newCommunity);
                entities.SaveChanges();
                return Redirect(Url.Action("listado", "comunidades", new { area = "webmaster" }));
            }
            catch (Exception ex)
            {
                return Redirect(Url.Action("agregar", "comunidades",
                    new
                    {
                        area = "webmaster",
                        exception = ex.Message
                    }));
            }
        }

        [HttpPost]
        public ActionResult editcommunity(long communityID, string first_name,
            string description, string admin_email, long packageId)
        {
            try
            {
                community editCommunity = entities.communities.Find(communityID);
                editCommunity.admin_email = admin_email;
                editCommunity.updated_at = DateTime.Now;
                editCommunity.description = description;
                editCommunity.first_name = first_name;
                editCommunity.package_id = packageId;
                entities.SaveChanges();
                return Redirect(Url.Action("listado", "comunidades", new { area = "webmaster" }));
            }
            catch (Exception ex)
            {
                return Redirect(Url.Action("editar", "comunidades",
                    new
                    {
                        area = "webmaster",
                        id = communityID,
                        exception = ex.Message
                    }));
            }
        }

        public JsonResult DeleteCommunity(long id)
        {
            try
            {

                List<communuser> communusers = entities.communusers.Where(m => m.commun_id == id).ToList();
            entities.communusers.RemoveRange(communusers);
                //communuser delcomunuser = entities.communusers.Find(id);
                //entities.communusers.Remove(delcomunuser);
                community delCommunity = entities.communities.Find(id);
                entities.communities.Remove(delCommunity);
                entities.SaveChanges();
                return Json(new { result = "success" });
            }
            catch (Exception ex)
            {
                return Json(new
                {
                    result = "error",
                    //exception = ex.Message
                });
            }
        }



        [HttpPost]
        public ActionResult SeacrhResult(string searchStr)
        {
            return Redirect(Url.Action("listado", "comunidades", new
            {
                area = "webmaster",
                searchStr = searchStr
            }));
        }


public JsonResult ActiveCom(long id)
{
    try
    {
        community user = entities.communities.Find(id);
        user.is_active = true;
        entities.SaveChanges();
        return Json(new { result = "success" });
    }
    catch (Exception ex)
    {
        return Json(new { result = "error", exception = ex.Message });
    }
}


        public JsonResult DeactiveCom(long id)
        {
            try
            {
                community user = entities.communities.Find(id);
                user.is_active = false;
                entities.SaveChanges();
                return Json(new { result = "success" });
            }
            catch (Exception ex)
            {
                return Json(new { result = "error", exception = ex.Message });
            }
        }
    }
}

