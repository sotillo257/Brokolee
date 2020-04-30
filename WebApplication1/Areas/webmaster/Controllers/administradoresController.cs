using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using WebApplication1.Areas.webmaster.ViewModels;
using WebApplication1.Concrete;
using WebApplication1.Models;
using Westwind.Web.Mvc;

namespace WebApplication1.Areas.webmaster.Controllers
{
    public class administradoresController : Controller
    {
        pjrdev_condominiosEntities entities = new pjrdev_condominiosEntities();
        EFPublicRepository ep = new EFPublicRepository();
        // GET: webmaster/administradores
        public ActionResult listado()
        {
            if (Session["USER_ID"] != null)
            {
                long userId = (long)Session["USER_ID"];
                user curUser = entities.users.Find(userId);
                List<ShowMessage> pubMessageList = ep.GetChatMessages(userId);
                List<user> coadminList = new List<user>();
                Dictionary<long, string> communityDict = new Dictionary<long, string>();
                var query = (from r in entities.users
                             where (r.role == 2 || r.role == 4) && r.is_del != true
                             select r);
                coadminList = query.ToList();

                foreach (var item in coadminList)
                {
                    communuser communuser = entities.communusers.Where(m => m.user_id == item.id).FirstOrDefault();
                    if (communuser != null)
                    {
                        community community = entities.communities.Find(communuser.commun_id);
                        communityDict.Add(item.id, community.first_name);
                    } else
                    {
                        communityDict.Add(item.id, "Sin comunidades");
                    }
                }
                listadoAdminViewModel viewModel = new listadoAdminViewModel();
                viewModel.side_menu = "administradores";
                viewModel.side_sub_menu = "administradores_listado";
                viewModel.document_category_list = entities.document_type.ToList();
                viewModel.curUser = curUser;
                viewModel.coadminList = coadminList;
                viewModel.communityDict = communityDict;
                viewModel.pubTaskList = ep.GetNotifiTaskList(userId);
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
                agregarAdminViewModel viewModel = new agregarAdminViewModel();
                viewModel.side_menu = "administradores";
                viewModel.side_sub_menu = "administradores_agregar";
                viewModel.document_category_list = entities.document_type.ToList();
                viewModel.curUser = curUser;
                viewModel.communityList = entities.communities.Where(m => m.is_active == true).ToList();
                viewModel.pubTaskList = ep.GetNotifiTaskList(userId);
                viewModel.pubMessageList = pubMessageList;
                viewModel.messageCount = ep.GetUnreadMessageCount(pubMessageList);
                return View(viewModel);
            } else
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
                    user editAdmin = entities.users.Find(id);
                    List<ShowMessage> pubMessageList = ep.GetChatMessages(userId);
                    List<communuser> communuserResult = entities.communusers.Where(m => m.user_id == id).ToList();
                    editarAdminViewModel viewModel = new editarAdminViewModel();
                    viewModel.side_menu = "administradores";
                    viewModel.side_sub_menu = "administradores_editar";
                    viewModel.document_category_list = entities.document_type.ToList();
                    viewModel.curUser = curUser;
                    viewModel.editAdmin = editAdmin;
                    viewModel.communityID1 = communuserResult;
                    viewModel.editPassword = ep.Decrypt(editAdmin.password);
                    viewModel.communityList = entities.communities.Where(x => x.is_active == true).ToList();
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

        public ActionResult editar(long? id)
        {
            if (Session["USER_ID"] != null)
            {
                if (id != null)
                {
                    long userId = (long)Session["USER_ID"];
                    user curUser = entities.users.Find(userId);
                    user editAdmin = entities.users.Find(id);
                    List<ShowMessage> pubMessageList = ep.GetChatMessages(userId);
                    List<communuser> communuserResult = entities.communusers.Where(m => m.user_id == id).ToList();
                    editarAdminViewModel viewModel = new editarAdminViewModel();
                    viewModel.side_menu = "administradores";
                    viewModel.side_sub_menu = "administradores_editar";
                    viewModel.document_category_list = entities.document_type.ToList();
                    viewModel.curUser = curUser;
                    viewModel.editAdmin = editAdmin;
                    viewModel.communityID1 = communuserResult;                    
                    viewModel.editPassword = ep.Decrypt(editAdmin.password);
                    viewModel.communityList = entities.communities.Where(x => x.is_active == true).ToList();
                    viewModel.pubTaskList = ep.GetNotifiTaskList(userId);
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

        [HttpPost]
        public async Task<ActionResult> newadmin(HttpPostedFileBase user_logo, string first_name1,
            string last_name1, string mother_last_name1, string email, string password,
            List<string> communityID
            )
        {
            try
            {
                string emailTemplate = "";
                long userId = (long)Session["USER_ID"];
                user curUser = entities.users.Find(userId);
                user newAdmin = new user();
                newAdmin.is_active = true;
                newAdmin.acq_date = DateTime.Now;
                newAdmin.role = 2;
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
                entities.users.Add(newAdmin);
                entities.SaveChanges();

                foreach (var item in communityID)
                {
                   communuser communuser = new communuser();
                   communuser.user_id = newAdmin.id;
                   communuser.commun_id = long.Parse(item);
                   entities.communusers.Add(communuser);
                   entities.SaveChanges();                                                                         
                }

                //community community = entities.communities.Find(communityID);
                //community.is_active = true;
                
                string patialView = "~/Views/iniciar/emailing.cshtml";
                emailingViewModel viewModel = new emailingViewModel();
                viewModel.firstName = first_name1;
                viewModel.lastName = last_name1;
                viewModel.fromEmail = curUser.email;
                viewModel.toEmail = email;
                emailTemplate = ViewRenderer.RenderPartialView(patialView, viewModel);
                int a = await ep.SendAlertEmail(curUser.email, email, curUser.first_name1 + " " + curUser.last_name1,
                    "añadir coadmin", "has sido añadido como coadmin\n password: " + password, emailTemplate);
                return Redirect(Url.Action("listado", "administradores", new { area = "webmaster" }));
            }
            catch (Exception ex)
            {
                return Redirect(Url.Action("agregar", "administradores", new { area = "webmaster", exception = ex.Message }));
            }
        }

        [HttpPost]
        public ActionResult editadmin(long adminID, HttpPostedFileBase user_logo,
            string first_name1, string last_name1, string mother_last_name1,
            string email, string password, List<string> communityID)
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

                List<communuser> comxuserAnterior = entities.communusers.Where(x=> x.user_id == editAdmin.id).ToList();                
                foreach (var item in comxuserAnterior)
                {
                    entities.communusers.Remove(item);
                    entities.SaveChanges();                    
                }

                foreach (var item in communityID)
                {
                    long itemID = Convert.ToInt64(item);
                    communuser communuserResult = entities.communusers.Where(m => m.user_id == editAdmin.id && m.commun_id == itemID).FirstOrDefault();
                    if (communuserResult == null)
                    {
                        communuser communuser = new communuser();
                        communuser.user_id = editAdmin.id;
                        communuser.commun_id = itemID;
                        entities.communusers.Add(communuser);
                        entities.SaveChanges();
                    }
                }               
                return Redirect(Url.Action("listado", "administradores", new { area = "webmaster" }));
            } catch(Exception ex)
            {
                return Redirect(Url.Action("editar", "administradores", 
                    new { area = "webmaster", id = adminID , exception = ex.Message }));
            }
        }

        public JsonResult AdminInactive(long id)
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
                return Json(new
                {
                    result = "error",
                    exception = ex.Message
                });
            }
        }

        public JsonResult AdminActive(long id)
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
                return Json(new
                {
                    result = "error",
                    exception = ex.Message
                });
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
                foreach(var item in blogs)
                {
                    List<blogcomment> blogcomments = entities.blogcomments.Where(m => m.blog_id == item.id).ToList();
                    entities.blogcomments.RemoveRange(blogcomments);
                }
                entities.blogs.RemoveRange(blogs);

                ///Delete comunities
                List<communuser> comxuserAnterior = entities.communusers.Where(x => x.user_id == delID).ToList();
                foreach (var item in comxuserAnterior)
                {
                    entities.communusers.Remove(item);
                    entities.SaveChanges();
                }

                // Delete bank info
                List<bank> banks = entities.banks.Where(m => m.user_id == delID).ToList();
                foreach(var item in banks)
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
                delUser.is_del = true;
                entities.SaveChanges();
                return Json(new { result = "success" });
            } catch(Exception ex)
            {
                return Json(new {
                    result = "error",
                    exception = ex.Message
                });
            }
        }

        public ActionResult SeacrhResult(string searchStr)
        {
            return Redirect(Url.Action("listado", "administradores", 
                new {
                    area = "webmaster",
                    searchStr = searchStr
                }));
        }
    }
}