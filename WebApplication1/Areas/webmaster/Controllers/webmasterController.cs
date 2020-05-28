using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WebApplication1.Areas.webmaster.ViewModels;
using WebApplication1.Concrete;
using WebApplication1.Models;

namespace WebApplication1.Areas.webmaster.Controllers
{
    public class webmasterController : Controller
    {
        pjrdev_condominiosEntities entities = new pjrdev_condominiosEntities();
        EFPublicRepository ep = new EFPublicRepository();
        // GET: webmaster/administradores
        public ActionResult listado(string searchStr = "")
        {
            if (Session["USER_ID"] != null)
            {
                long userId = (long)Session["USER_ID"];
                user curUser = entities.users.Find(userId);
                List<ShowMessage> pubMessageList = ep.GetChatMessages(userId);
                List<user> coadminList = new List<user>();
                Dictionary<long, string> communityDict = new Dictionary<long, string>();
                if (searchStr == "")
                {
                    var query = (from r in entities.users where r.role == 3 select r);
                    coadminList = query.ToList();
                }
                else
                {
                    var query = (from r in entities.users
                                 where r.role == 3
                                 && (r.first_name1.Contains(searchStr)
                                 || r.last_name1.Contains(searchStr))
                                 select r);
                    coadminList = query.ToList();
                }

                foreach (var item in coadminList)
                {
                    communuser communuser = entities.communusers.Where(m => m.user_id == item.id).FirstOrDefault();
                    if (communuser != null)
                    {
                        community community = entities.communities.Find(communuser.commun_id);
                        communityDict.Add(item.id, community.first_name);
                    }
                    else
                    {
                        communityDict.Add(item.id, "No community");
                    }
                }
                listadoAdminViewModel viewModel = new listadoAdminViewModel();
                viewModel.side_menu = "webmaster";
                viewModel.side_sub_menu = "webmaster_listado";
                
                viewModel.curUser = curUser;
                viewModel.coadminList = coadminList;
                viewModel.communityDict = communityDict;
                viewModel.pubTaskList = ep.GetNotifiTaskList(userId);
                viewModel.pubMessageList = pubMessageList;
                viewModel.messageCount = ep.GetUnreadMessageCount(pubMessageList);
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
                agregarAdminViewModel viewModel = new agregarAdminViewModel();
                viewModel.side_menu = "webmaster";
                viewModel.side_sub_menu = "webmaster_agregar";
                
                viewModel.curUser = curUser;
                viewModel.communityList = entities.communities.ToList();
                viewModel.pubTaskList = ep.GetNotifiTaskList(userId);
                viewModel.pubMessageList = pubMessageList;
                viewModel.messageCount = ep.GetUnreadMessageCount(pubMessageList);
                return View(viewModel);
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
                    user viewAdmin = entities.users.Find(id);
                    verAdminViewModel viewModel = new verAdminViewModel();
                    viewModel.side_menu = "webmaster";
                    viewModel.side_sub_menu = "webmaster_ver";
                    
                    viewModel.curUser = curUser;
                    viewModel.viewAdmin = viewAdmin;
                    viewModel.pubTaskList = ep.GetNotifiTaskList(userId);
                    viewModel.pubMessageList = pubMessageList;
                    viewModel.messageCount = ep.GetUnreadMessageCount(pubMessageList);
                    viewModel.viewPassword = ep.Decrypt(viewAdmin.password);
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

        public ActionResult editar(long? id, string Tipo)
        {
            if (Session["USER_ID"] != null)
            {
                if (id != null)
                {
                    if (Tipo == "Perfil")
                    {
                        ViewBag.TituloPagina = "Perfil WebMaster";
                    }
                    else
                    {
                        ViewBag.TituloPagina = "Editar Webmaster";
                    }
                    long userId = (long)Session["USER_ID"];
                    user curUser = entities.users.Find(userId);
                    List<ShowMessage> pubMessageList = ep.GetChatMessages(userId);
                    user editAdmin = entities.users.Find(id);
                    editarAdminViewModel viewModel = new editarAdminViewModel();
                    viewModel.side_menu = "webmaster";
                    viewModel.side_sub_menu = "webmaster_editar";
                    
                    viewModel.curUser = curUser;
                    viewModel.editAdmin = editAdmin;
                    viewModel.editPassword = ep.Decrypt(editAdmin.password);
                    viewModel.communityList = entities.communities.ToList();
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

        [HttpPost]
        public ActionResult newwebmaster(HttpPostedFileBase user_logo, string first_name1,
            string last_name1, string mother_last_name1, string email, string password
            )
        {
            try
            {
                long userId = (long)Session["USER_ID"];
                user newAdmin = new user();
                newAdmin.is_active = true;
                newAdmin.acq_date = DateTime.Now;
                newAdmin.role = 3;
                newAdmin.first_name1 = first_name1;
                newAdmin.last_name1 = last_name1;
                newAdmin.mother_last_name1 = mother_last_name1;
                newAdmin.email = email;
                newAdmin.password = ep.Encrypt(password);
                if (user_logo != null && user_logo.ContentLength > 0)
                {
                    var fileName = Path.GetFileName(user_logo.FileName);
                    if (!Directory.Exists(Path.Combine(Server.MapPath("~/"), "Upload")))
                    {
                        Directory.CreateDirectory(Path.Combine(Server.MapPath("~/"), "Upload"));
                    }

                    if (!Directory.Exists(Path.Combine(Server.MapPath("~/Upload/"), "User_Logo")))
                    {
                        Directory.CreateDirectory(Path.Combine(Server.MapPath("~/Upload/"), "User_Logo"));
                    }
                    // store the file inside ~/App_Data/uploads folder
                    var path = Path.Combine(Server.MapPath("~/Upload/User_Logo"), fileName);
                    user_logo.SaveAs(path);
                    newAdmin.user_img = fileName;
                }
                newAdmin.create_userid = userId;
                entities.users.Add(newAdmin);              
                entities.SaveChanges();
                return Redirect(Url.Action("listado", "webmaster", new { area = "webmaster" }));
            }
            catch (Exception ex)
            {
                return Redirect(Url.Action("agregar", "webmaster", new { area = "webmaster", exception = ex.Message }));
            }
        }

        [HttpPost]
        public ActionResult editwebmaster(long adminID, HttpPostedFileBase user_logo,
            string first_name1, string last_name1, string mother_last_name1,
            string email, string password)//long communityID1, long communityID2
        {
            try
            {
                user editAdmin = entities.users.Find(adminID);
                editAdmin.first_name1 = first_name1;
                editAdmin.last_name1 = last_name1;
                editAdmin.mother_last_name1 = mother_last_name1;
                editAdmin.email = email;
                editAdmin.password = ep.Encrypt(password);
                if (user_logo != null && user_logo.ContentLength > 0)
                {
                    var fileName = Path.GetFileName(user_logo.FileName);
                    if (!Directory.Exists(Path.Combine(Server.MapPath("~/"), "Upload")))
                    {
                        Directory.CreateDirectory(Path.Combine(Server.MapPath("~/"), "Upload"));
                    }

                    if (!Directory.Exists(Path.Combine(Server.MapPath("~/Upload/"), "User_Logo")))
                    {
                        Directory.CreateDirectory(Path.Combine(Server.MapPath("~/Upload/"), "User_Logo"));
                    }
                    // store the file inside ~/App_Data/uploads folder
                    var path = Path.Combine(Server.MapPath("~/Upload/User_Logo"), fileName);
                    user_logo.SaveAs(path);
                    editAdmin.user_img = fileName;
                }
                entities.SaveChanges();
                return Redirect(Url.Action("listado", "webmaster", new { area = "webmaster" }));
            }
            catch (Exception ex)
            {
                return Redirect(Url.Action("editar", "webmaster",
                    new { area = "webmaster", id = adminID , exception = ex.Message }));
            }
        }

        public JsonResult DeleteAdmin(long delID)
        {
            try
            {
                // Delete Forign records
                // Delete USO
                // Delete related records in other tables
                List<chatmessage> chatmessages = entities.chatmessages.Where(m => m.from_user_id == delID
                || m.to_user_id == delID).ToList();
                entities.chatmessages.RemoveRange(chatmessages);
                // Delete blogs
                List<blog> blogs = entities.blogs.Where(m => m.user_id == delID).ToList();
                foreach (var item in blogs)
                {
                    List<blogcomment> blogcomments = entities.blogcomments.Where(m => m.blog_id == item.id).ToList();
                    entities.blogcomments.RemoveRange(blogcomments);
                }
                entities.blogs.RemoveRange(blogs);
                // Delete bank info
                List<bank> banks = entities.banks.Where(m => m.user_id == delID).ToList();
                foreach (var item in banks)
                {
                    List<fee> fees = entities.fees.Where(m => m.bank_id == item.id).ToList();
                    entities.fees.RemoveRange(fees);
                }
                entities.banks.RemoveRange(banks);
                // Delete CreditCards
                List<creditcard> creditcards = entities.creditcards.Where(m => m.user_id == delID).ToList();
                entities.creditcards.RemoveRange(creditcards);
                List<taskuser> taskusers = entities.taskusers.Where(m => m.user_id == delID).ToList();
                entities.taskusers.RemoveRange(taskusers);
                List<uso> usos = entities.usoes.Where(m => m.create_userid == delID).ToList();
                entities.usoes.RemoveRange(usos);
                List<emailtheme> emailthemes = entities.emailthemes.Where(m => m.user_id == delID).ToList();
                entities.emailthemes.RemoveRange(emailthemes);
                user delUser = entities.users.Find(delID);
                entities.users.Remove(delUser);
                entities.SaveChanges();
                return Json(new { result = "success" });
            }
            catch (Exception ex)
            {
                return Json(new { result = "error", exception = ex });
            }
        }

        public ActionResult SeacrhResult(string searchStr)
        {
            return Redirect(Url.Action("listado", "webmaster",
                new
                {
                    area = "webmaster",
                    searchStr = searchStr
                }));
        }

        public JsonResult ActiveWeb(long id)
        {
            try
            {
                
                user user = entities.users.Find(id);
                user.is_active = true;
                entities.SaveChanges();
                return Json(new { result = "success" });
            }
            catch (Exception ex)
            {
                return Json(new { result = "error", exception = ex.HResult });
            }
        }

        public JsonResult DeactiveWeb(long id)
        {
            try
            {
                user user = entities.users.Find(id);
                user.is_active = false;
                entities.SaveChanges();
                return Json(new { result = "success" });
            }
            catch (Exception ex)
            {
                return Json(new { result = "error", exception = ex.HResult });
            }
        }

        //Creado por Jesus Sotillo 12/16/2019
        //Vista de editar el perfil Webmaster logeado
        public ActionResult editar_perfil()
        {
            if (Session["USER_ID"] != null)
            {
                long userId = (long)Session["USER_ID"];
                user curUser = entities.users.Find(userId);
                List<ShowMessage> pubMessageList = ep.GetChatMessages(userId);
                perfilViewModel viewModel = new perfilViewModel();
                viewModel.side_menu = "";
                viewModel.side_sub_menu = "";
                viewModel.curUser = curUser;
                
                viewModel.pubTaskList = ep.GetNotifiTaskList(userId);
                viewModel.pubMessageList = pubMessageList;
                viewModel.messageCount = ep.GetUnreadMessageCount(pubMessageList);
                viewModel.password = ep.Decrypt(curUser.password);
                return View(viewModel);
            }
            else
            {
                return Redirect(ep.GetLogoutUrl());
            }
        }

    }
}