using System;
using System.Collections.Generic;
using System.Data.Entity.Validation;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WebApplication1.Areas.coadmin.ViewModels;
using WebApplication1.Concrete;
using WebApplication1.Models;
using Westwind.Web.Mvc;

namespace WebApplication1.Areas.coadmin.Controllers
{
    public class administradorController : Controller
    {
        pjrdev_condominiosEntities entities = new pjrdev_condominiosEntities();
        EFPublicRepository ep = new EFPublicRepository();
        List<community> communityList = new List<community>();

        // GET: coadmin/administrador
        public ActionResult perfil()
        {
            if (Session["USER_ID"] != null)
            {
                try
                {
                    long userId = (long)Session["USER_ID"];
                    long communityAct = Convert.ToInt64(Session["CURRENT_COMU"]);                   

                    List<ShowMessage> pubMessageList = ep.GetChatMessages(userId);
                    user curUser = entities.users.Find(userId);
                    perfilViewModel viewModel = new perfilViewModel();

                    communityList = ep.GetCommunityList(userId);
                    viewModel.communityList = communityList;

                    viewModel.side_menu = "";
                    viewModel.side_sub_menu = "";
                    viewModel.curUser = curUser;
                     viewModel.document_category_list = entities.document_type.Where(x => x.community_id == communityAct).ToList();
                    viewModel.pubTaskList = ep.GetNotifiTaskList(userId);
                    viewModel.pubMessageList = pubMessageList;
                    viewModel.password = ep.Decrypt(curUser.password);
                    viewModel.messageCount = ep.GetUnreadMessageCount(pubMessageList);                                     
                    return View(viewModel);
                }
                catch(Exception ex)
                {
                    return Redirect(Url.Action("Index", "Error"));
                } 
            } else
            {
                return Redirect(ep.GetLogoutUrl());
            }
        }

        [HttpPost]
        public ActionResult logout_suscribe()
        {
            long packId = 0;
            try
            {              
                if (Session.Keys.Count > 0)
                {
                    packId = Convert.ToInt64(Session["PACK_ID"]);
                    Session.RemoveAll();
                }
            }
            catch(Exception ex)
            {

            }

            return Redirect(ep.GetSuscribeLogout() + packId.ToString());
        }

        [HttpPost]
        public ActionResult logout()
        {
            try
            {
                long userId;

                if (Session.Keys.Count == 0 || string.IsNullOrEmpty(Session["USER_ID"] as string))
                {
                    HttpCookie userIdCookie = Request.Cookies.Get("UserID");
                    userId = Convert.ToInt64(userIdCookie.Value);
                    user curUser = entities.users.Find(userId);
                    curUser.is_logged = false;
                }
                else
                {
                    userId = (long)Session["USER_ID"];
                    user curUser = entities.users.Find(userId);
                    curUser.is_logged = false;
                }
                entities.SaveChanges();
                Session.RemoveAll();
            }
            catch(Exception ex)
            {

            }

            return Redirect(ep.GetLogoutUrl());
        }

        [HttpPost]
        public ActionResult editprofile(HttpPostedFileBase user_logo,
            string email, string password, string first_name1,
            string last_name1, string first_name2, string mother_last_name1,
            string last_name2, string mother_last_name2, string phone_number1,
            string phone_number2, string postal_address, string residential_address,
            HttpPostedFileBase upload_writing, string siono, string since,
            string until, string tenant_first_name1, string tenant_last_name1,
            string tenant_mother_last_name1, string tenant_first_name2,
            string tenant_last_name2, string tenant_mother_last_name2,
            string leased_postal_address, string leased_residential_address,
            HttpPostedFileBase leased_upload_file, string brand, string model,
            string colour, string year, string clapboard, string stamp_number)
        {
            try
            {
                long user_id = (long)Session["USER_ID"];
                user curUser = entities.users.Find(user_id);
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
                    curUser.user_img = fileName;
                }
                curUser.email = email;
                curUser.password = ep.Encrypt(password);
                curUser.first_name1 = first_name1;
                curUser.last_name1 = last_name1;
                curUser.first_name2 = first_name2;
                curUser.last_name2 = last_name2;
                curUser.mother_last_name1 = mother_last_name1;
                curUser.mother_last_name2 = mother_last_name2;
                curUser.postal_address = postal_address;
                curUser.residential_address = residential_address;
                curUser.phone_number1 = phone_number1;
                curUser.phone_number2 = phone_number2;
                if (upload_writing != null && upload_writing.ContentLength > 0)
                {
                    var fileName = Path.GetFileName(upload_writing.FileName);
                    if (!Directory.Exists(Path.Combine(Server.MapPath("~/"), "Upload")))
                    {
                        Directory.CreateDirectory(Path.Combine(Server.MapPath("~/"), "Upload"));
                    }

                    if (!Directory.Exists(Path.Combine(Server.MapPath("~/Upload/"), "Upload_Writing")))
                    {
                        Directory.CreateDirectory(Path.Combine(Server.MapPath("~/Upload/"), "Upload_Writing"));
                    }
                    // store the file inside ~/App_Data/uploads folder
                    var path = Path.Combine(Server.MapPath("~/Upload/Upload_Writing"), fileName);
                    upload_writing.SaveAs(path);
                    curUser.upload_writing = fileName;
                }

                if (since == "")
                {
                    curUser.since = null;
                } else
                {
                    curUser.since = DateTime.ParseExact(since, "yyyy-MM-dd",
                       System.Globalization.CultureInfo.InvariantCulture);
                }

                if (until == "")
                {
                    curUser.until = null;
                } else
                {
                    curUser.until = DateTime.ParseExact(until, "yyyy-MM-dd",
                        System.Globalization.CultureInfo.InvariantCulture);
                }
               

                if (leased_upload_file != null && leased_upload_file.ContentLength > 0)
                {
                    var fileName = Path.GetFileName(leased_upload_file.FileName);
                    if (!Directory.Exists(Path.Combine(Server.MapPath("~/"), "Upload")))
                    {
                        Directory.CreateDirectory(Path.Combine(Server.MapPath("~/"), "Upload"));
                    }

                    if (!Directory.Exists(Path.Combine(Server.MapPath("~/Upload/"), "Upload_Contract")))
                    {
                        Directory.CreateDirectory(Path.Combine(Server.MapPath("~/Upload/"), "Upload_Contract"));
                    }
                    // store the file inside ~/App_Data/uploads folder
                    var path = Path.Combine(Server.MapPath("~/Upload/Upload_Contract"), fileName);
                    leased_upload_file.SaveAs(path);
                    curUser.user_img = fileName;
                }
                curUser.tenant_first_name1 = tenant_first_name1;
                curUser.tenant_first_name2 = tenant_first_name2;
                curUser.tenant_last_name1 = tenant_last_name1;
                curUser.tenant_last_name2 = tenant_last_name2;
                curUser.tenant_mother_last_name1 = tenant_mother_last_name1;
                curUser.tenant_mother_last_name2 = tenant_mother_last_name2;
                curUser.leased_postal_address = leased_postal_address;
                curUser.leased_residential_address = leased_residential_address;
                curUser.brand = brand;
                curUser.model = model;
                
                if (year != "")
                {
                    curUser.year = Convert.ToInt32(year);
                } else
                {
                    curUser.year = null;
                }
                
                curUser.colour = colour;
                curUser.clapboard = clapboard;

                if (stamp_number != "")
                {
                    curUser.stamp_number = Convert.ToInt32(stamp_number);
                } else
                {
                    curUser.stamp_number = null;
                }
                
                entities.SaveChanges();
                return Redirect(Url.Action("panel", "control", new { area = "coadmin" }));
            }
            catch (DbEntityValidationException e)
            {
                foreach (var eve in e.EntityValidationErrors)
                {
                    Console.WriteLine("Entity of type \"{0}\" in state \"{1}\" has the following validation errors:",
                        eve.Entry.Entity.GetType().Name, eve.Entry.State);
                    foreach (var ve in eve.ValidationErrors)
                    {
                        Console.WriteLine("- Property: \"{0}\", Error: \"{1}\"",
                            ve.PropertyName, ve.ErrorMessage);
                        return Redirect(Url.Action("perfil", "administrador", new { area = "coadmin", t = ve.PropertyName + "/" +ve.ErrorMessage }));
                    }
                }
                return Redirect(Url.Action("perfil", "administrador", new { area = "coadmin" }));
            }
        }

        public JsonResult CheckTimeout(string SessionTimout, long SelUserID)
        {
            if (Session["USER_ID"] == null)
            {
                if (Session["SUS_USER_ID"] == null)
                {
                    return Json(new { result = "success" });
                }
                else
                {
                    // User is on online Status

                    return Json(new { result = "error" });
                }
            } else
            {
                // User is on online Status
                int selStatus = 0;
                long userId = (long)Session["USER_ID"];
                if (SelUserID != 0)
                {
                    user selUser = entities.users.Find(SelUserID);
                    if (selUser.is_logged == true)
                    {
                        selStatus = 1;
                    }
                }

                ConnectedUserViewModel viewmodel = new ConnectedUserViewModel();
                List<onlineuser> onlineUserList = entities.onlineusers.Where(m => m.user_id != userId).ToList();
                viewmodel.onlineUserList = onlineUserList;
                string patialView = "~/Areas/coadmin/Views/comunicaciones/_connectedusers.cshtml";
                string postsHtml = ViewRenderer.RenderPartialView(patialView, viewmodel);
                return Json(new
                {
                    result = "error",
                    connectedusers_html = postsHtml,
                    selLoginStatus = selStatus
                });
            }
        }
    }
}