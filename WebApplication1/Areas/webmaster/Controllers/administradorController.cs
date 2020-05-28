using System;
using System.Collections.Generic;
using System.Data.Entity.Validation;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WebApplication1.Areas.webmaster.ViewModels;
using WebApplication1.Concrete;
using WebApplication1.Models;

namespace WebApplication1.Areas.webmaster.Controllers
{
    public class administradorController : Controller
    {
        // GET: webmaster/administrador
        pjrdev_condominiosEntities entities = new pjrdev_condominiosEntities();
        EFPublicRepository ep = new EFPublicRepository();
        public ActionResult perfil()
        {
            if (Session["USER_ID"] != null)
            {
                long userId = (long)Session["USER_ID"];
                long communityAct = Convert.ToInt64(Session["CURRENT_COMU"]);
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
            } else
            {
                return Redirect(ep.GetLogoutUrl());
            }
        }

        [HttpPost]
        public ActionResult logout()
        {
            try
            {
                long userId = (long)Session["USER_ID"];
                user curUser = entities.users.Find(userId);
                curUser.is_logged = false;
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
            string last_name1,  string mother_last_name1,
            string phone_number1)
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
                curUser.phone_number1 = phone_number1;
                //if (upload_writing != null && upload_writing.ContentLength > 0)
                //{
                //    var fileName = Path.GetFileName(upload_writing.FileName);
                //    if (!Directory.Exists(Path.Combine(Server.MapPath("~/"), "Upload")))
                //    {
                //        Directory.CreateDirectory(Path.Combine(Server.MapPath("~/"), "Upload"));
                //    }

                //    if (!Directory.Exists(Path.Combine(Server.MapPath("~/Upload/"), "Upload_Writing")))
                //    {
                //        Directory.CreateDirectory(Path.Combine(Server.MapPath("~/Upload/"), "Upload_Writing"));
                //    }
                //    // store the file inside ~/App_Data/uploads folder
                //    var path = Path.Combine(Server.MapPath("~/Upload/Upload_Writing"), fileName);
                //    upload_writing.SaveAs(path);
                //    curUser.upload_writing = fileName;
                //}
                //if (since == "0001-01-01")
                //{
                //    curUser.since = null;
                //}
                //else
                //{
                //    curUser.since = DateTime.ParseExact(since, "yyyy-MM-dd",
                //        System.Globalization.CultureInfo.InvariantCulture);
                //}

                //if (until == "0001-01-01")
                //{
                //    curUser.until = null;
                //}
                //else
                //{
                //    curUser.until = DateTime.ParseExact(until, "yyyy-MM-dd",
                //        System.Globalization.CultureInfo.InvariantCulture);
                //}

                //if (leased_upload_file != null && leased_upload_file.ContentLength > 0)
                //{
                //    var fileName = Path.GetFileName(leased_upload_file.FileName);
                //    if (!Directory.Exists(Path.Combine(Server.MapPath("~/"), "Upload")))
                //    {
                //        Directory.CreateDirectory(Path.Combine(Server.MapPath("~/"), "Upload"));
                //    }

                //    if (!Directory.Exists(Path.Combine(Server.MapPath("~/Upload/"), "Upload_Contract")))
                //    {
                //        Directory.CreateDirectory(Path.Combine(Server.MapPath("~/Upload/"), "Upload_Contract"));
                //    }
                //    // store the file inside ~/App_Data/uploads folder
                //    var path = Path.Combine(Server.MapPath("~/Upload/Upload_Contract"), fileName);
                //    leased_upload_file.SaveAs(path);
                //    curUser.user_img = fileName;
                //}
                //curUser.tenant_first_name1 = tenant_first_name1;
                //curUser.tenant_first_name2 = tenant_first_name2;
                //curUser.tenant_last_name1 = tenant_last_name1;
                //curUser.tenant_last_name2 = tenant_last_name2;
                //curUser.tenant_mother_last_name1 = tenant_mother_last_name1;
                //curUser.tenant_mother_last_name2 = tenant_mother_last_name2;
                //curUser.leased_postal_address = leased_postal_address;
                //curUser.leased_residential_address = leased_residential_address;
                //curUser.brand = brand;
                //curUser.model = model;
                //if (year != "")
                //{
                //    curUser.year = Convert.ToInt32(year);
                //} else
                //{
                //    curUser.year = null;
                //}
                
                //curUser.colour = colour;
                //curUser.clapboard = clapboard;
                //if (stamp_number != "")
                //{
                //    curUser.stamp_number = Convert.ToInt32(stamp_number);
                //}
                //else
                //{
                //    curUser.stamp_number = null;
                //}
                
                entities.SaveChanges();
                return Redirect(Url.Action("panel", "control", new { area = "webmaster" }));
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
                        return Redirect(Url.Action("perfil", "administrador", new { area = "webmaster", t = ve.PropertyName + "/" + ve.ErrorMessage }));
                    }
                }
                return Redirect(Url.Action("perfil", "administrador", new { area = "webmaster" }));
            }
        }
    }
}