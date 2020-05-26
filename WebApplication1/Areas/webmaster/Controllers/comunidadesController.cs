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
        public ActionResult listado(string Error, string searchStr = "")
        {
            if (Session["USER_ID"] != null)
            {
                try
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

                        string Admins = "";
                        bool ban = true;
                        foreach (var CoUser in item.communusers)
                        {
                            if (ban)
                            {
                                Admins += CoUser.user.email;
                                ban = false;
                            }
                            else
                            {
                                Admins += " / " + CoUser.user.email;
                            }

                        }
                        item.admin_email = Admins;
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
                    ViewBag.msgError = Error;
                    return View(viewModel);
                }
                catch (Exception ex)
                {
                    return Redirect(Url.Action("error", "control", new { area = "webmaster", Error = "Error al obtener listado de comunidades: " + ex.Message }));
                }                
            }
            else
            {
                return Redirect(ep.GetLogoutUrl());
            }
        }

        public ActionResult agregar(string Error)
        {
            if (Session["USER_ID"] != null)
            {
                try
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
                    ViewBag.msgError = Error;
                    viewModel.messageCount = ep.GetUnreadMessageCount(pubMessageList);
                    return View(viewModel);
                }
                catch (Exception ex)
                {
                    return Redirect(Url.Action("listado", "comunidades", new { area = "webmaster", Error = "Agregar comunidad: " + ex.Message }));
                }                
            }
            else
            {
                return Redirect(ep.GetLogoutUrl());
            }
        }

        public ActionResult editar(string Error, long? id)
        {
            if (Session["USER_ID"] != null)
            {
                if (id != null)
                {
                    community editCommunity = entities.communities.Find(id);
                    if (editCommunity != null)
                    {
                        try
                        {
                            long userId = (long)Session["USER_ID"];
                            user curUser = entities.users.Find(userId);
                            List<ShowMessage> pubMessageList = ep.GetChatMessages(userId);                            
                            editarCommunViewModel viewModel = new editarCommunViewModel();
                            viewModel.curUser = curUser;
                            viewModel.side_menu = "comunidades";
                            viewModel.side_sub_menu = "comunidades_editar";
                            viewModel.packageList = entities.packages.ToList();
                            viewModel.editCommunity = editCommunity;
                            viewModel.pubTaskList = ep.GetNotifiTaskList(userId);
                            viewModel.pubMessageList = pubMessageList;
                            viewModel.messageCount = ep.GetUnreadMessageCount(pubMessageList);
                            ViewBag.msgError = Error;
                            return View(viewModel);
                        }
                        catch (Exception ex)
                        {
                            return Redirect(Url.Action("listado", "comunidades", new { area = "webmaster", Error = "Editar comunidad: " + ex.Message }));
                        }
                    }
                    else
                    {
                        return Redirect(Url.Action("listado", "comunidades", new { area = "webmaster", Error = "No existe ese elemento" }));                    
                    }                    
                }
                else
                {
                    return Redirect(Url.Action("listado", "comunidades", new { area = "webmaster"}));
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
                    community viewCommunity = entities.communities.Find(id);
                    if (viewCommunity != null)
                    {
                        try
                        {
                            long userId = (long)Session["USER_ID"];
                            user curUser = entities.users.Find(userId);
                            List<ShowMessage> pubMessageList = ep.GetChatMessages(userId);
                            List<communuser> commonuserList = entities.communusers.Where(m => m.commun_id == id).ToList();
                            List<user> titularList = new List<user>();
                            foreach (var item in commonuserList)
                            {
                                user coadminUser = entities.users.Find(item.user_id);
                                List<user> subTitularList = entities.users.Where(m => m.create_userid == coadminUser.id).ToList();
                                titularList.AddRange(subTitularList);
                            }
                            List<user> coadminList = new List<user>();
                            var query = (from r in entities.users
                                         where (r.role == 2 || r.role == 4) && r.is_del != true
                                         select r);
                            coadminList = query.ToList();
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
                            viewModel.coadminList = coadminList;
                            return View(viewModel);
                        }
                        catch (Exception ex)
                        {
                            return Redirect(Url.Action("listado", "comunidades", new { area = "webmaster", Error = "Ver comunidad: " + ex.Message }));
                        }

                    }
                    else
                    {
                        return Redirect(Url.Action("listado", "comunidades", new { area = "webmaster", Error = "No existe ese elemento" }));
                    }                    
                }
                else
                {
                    return Redirect(Url.Action("listado", "comunidades", new { area = "webmaster" }));
                }
            }
            else
            {
                return Redirect(ep.GetLogoutUrl());
            }
        }

        [HttpPost]        
        public ActionResult newcommunity(string first_name,
            string description, long packageId, string apartment)
        {
            try
            {
                community newCommunity = new community();
                newCommunity.first_name = first_name;
              //  newCommunity.admin_email = admin_email;
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
                return Redirect(Url.Action("agregar", "comunidades", new { area = "webmaster", Error = "Error al agregar " + ex.Message }));
            }
        }

        [HttpPost]
        public ActionResult editcommunity(long communityID, string first_name,
            string description, long packageId, string apartment)
        {
            try
            {
                community editCommunity = entities.communities.Find(communityID);
               
                editCommunity.updated_at = DateTime.Now;
                editCommunity.description = description;
                editCommunity.first_name = first_name;
                editCommunity.package_id = packageId;
                editCommunity.apart = apartment;
                entities.SaveChanges();
                entities.communusers.RemoveRange(editCommunity.communusers);
               
                return Redirect(Url.Action("listado", "comunidades", new { area = "webmaster" }));
            }
            catch (Exception ex)
            {
                return Redirect(Url.Action("editar", "comunidades", new { area = "webmaster", id = communityID, Error = "Error al agregar " + ex.Message }));
            }
        }

        public JsonResult DeleteCommunity(long id)
        {
            try
            {

                List<communuser> communusers = entities.communusers.Where(m => m.commun_id == id).ToList();
                List<long> idAdmins = new List<long>();

                //Desactivando los Coadmins que administran esta comunidad y almacenando Id de los Coadmins
                foreach (var item in communusers)
                {
                    user userE = entities.users.Find(item.user_id);
                    userE.is_active = false;
                    entities.SaveChanges();
                    idAdmins.Add(userE.id);
                }
                entities.communusers.RemoveRange(communusers);

                //Reactivar Coadmin que administran otras comunidades
                foreach (var idItem in idAdmins)
                {
                    List<communuser> comunS = entities.communusers.Where(x => x.user_id == idItem && x.commun_id != id).ToList();
                    if (comunS.Count > 0)
                    {
                        user userE = entities.users.Find(idItem);
                        userE.is_active = true;
                        entities.SaveChanges();
                    }
                }

                List<Titulo> titulosC = entities.Titulos.Where(m => m.IdCommunity == id).ToList();
                List<long> idsTitulares = new List<long>();
                //Almacenando Id de los Titulares para eliminar
                foreach (var item2 in titulosC)
                {
                    idsTitulares.Add(item2.IdUser);
                }

                foreach (var item3 in idsTitulares)
                {
                    List<Titulo> tituloDiferente = entities.Titulos.Where(x => x.IdUser == item3 && x.IdCommunity != id).ToList();
                    if (tituloDiferente.Count == 0)
                    {
                        List<chatmessage> chatmessages = entities.chatmessages.Where(m => m.from_user_id == item3
                            || m.to_user_id == item3).ToList();
                        entities.chatmessages.RemoveRange(chatmessages);
                        
                        List<bank> banks = entities.banks.Where(m => m.user_id == item3).ToList();
                        foreach (var itembk in banks)
                        {
                            List<fee> fees = entities.fees.Where(m => m.bank_id == itembk.id).ToList();
                            entities.fees.RemoveRange(fees);
                        }
                        entities.banks.RemoveRange(banks);
                        // Delete CreditCards
                        List<creditcard> creditcards = entities.creditcards.Where(m => m.user_id == item3).ToList();
                        entities.creditcards.RemoveRange(creditcards);
                        List<taskuser> taskusers = entities.taskusers.Where(m => m.user_id == item3).ToList();
                        entities.taskusers.RemoveRange(taskusers);
                        List<uso> usos = entities.usoes.Where(m => m.create_userid == item3).ToList();
                        entities.usoes.RemoveRange(usos);
                        List<emailtheme> emailthemes = entities.emailthemes.Where(m => m.user_id == item3).ToList();
                        entities.emailthemes.RemoveRange(emailthemes);                        
                        List<payment> payments = entities.payments.Where(m => m.user_id == item3).ToList();
                        entities.payments.RemoveRange(payments);
                        List<onlineuser> onlineusers = entities.onlineusers.Where(m => m.user_id == item3).ToList();
                        entities.onlineusers.RemoveRange(onlineusers);
                    }
                }

                // Delete blogs
                List<blog> blogs = entities.blogs.Where(m => m.community_id == id).ToList();
                foreach (var itemb in blogs)
                {
                    List<blogcomment> blogcomments = entities.blogcomments.Where(m => m.blog_id == itemb.id).ToList();
                    entities.blogcomments.RemoveRange(blogcomments);
                }
                entities.blogs.RemoveRange(blogs);

                // Delete Tareas
                List<task> tareas = entities.tasks.Where(m => m.community_id == id).ToList();
                foreach (var itemTareas in tareas)
                {
                    List<taskcomment> taskcomments = entities.taskcomments.Where(m => m.task_id == itemTareas.id).ToList();
                    entities.taskcomments.RemoveRange(taskcomments);
                }
                entities.tasks.RemoveRange(tareas);

                // Delete Contactos
                List<contactinfo> contactos = entities.contactinfoes.Where(m => m.community_id == id).ToList();                
                entities.contactinfoes.RemoveRange(contactos);

                // Delete Facilidades
                List<efac> facilidades = entities.efacs.Where(m => m.community_id == id).ToList();
                foreach (var itemEfac in facilidades)
                {
                    List<book> bookList = entities.books.Where(m => m.id_efac == itemEfac.id).ToList();
                    entities.books.RemoveRange(bookList);
                }
                entities.efacs.RemoveRange(facilidades);

                List<Vehiculo> vehiculos = entities.Vehiculos.Where(x => x.Titulo.IdCommunity == id).ToList();
                entities.Vehiculos.RemoveRange(vehiculos);
                List<Titulo> titulos = entities.Titulos.Where(x => x.IdCommunity == id).ToList();
                entities.Titulos.RemoveRange(titulos);

                List<@event> eventos = entities.events.Where(x=>x.community_id == id).ToList();
                entities.Titulos.RemoveRange(titulos);

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
                    exception = ex.Message
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

