using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WebApplication1.Concrete;
using WebApplication1.Models;
using WebApplication1.ViewModels;

namespace WebApplication1.Controllers
{
    public class administradorController : Controller
    {
        pjrdev_condominiosEntities entities = new pjrdev_condominiosEntities();
        EFPublicRepository ep = new EFPublicRepository();

        List<Titulo> titulosList = new List<Titulo>();
        List<community> listComunities = new List<community>();

        // GET: administrador
        public ActionResult perfil()
        {
            if (Session["USER_ID"] != null )
            {
                try
                {
                    long userId = 0;
                    if (Convert.ToInt32(Session["USER_ROLE"]) == 1)
                    {
                        userId = (long)Session["USER_ID"];
                    }
                    else if (Convert.ToInt32(Session["USER_ROLE"]) > 1
                    && Session["ACC_USER_ID"] != null)
                    {
                        userId = (long)Session["ACC_USER_ID"];
                    }
                    
                    user curUser = entities.users.Find(userId);
                    List<ShowMessage> pubMessageList = ep.GetChatMessages(userId);
                    administradorViewModel viewModel = new administradorViewModel();

                    titulosList = ep.GetTitulosByTitular(userId);
                    listComunities = ep.GetCommunityListByTitular(titulosList);
                    viewModel.communityList = listComunities;

                    viewModel.side_menu = "administrador";
                    viewModel.curUser = curUser;
                    viewModel.document_category_list = entities.document_type.ToList();
                    viewModel.pubTaskList = ep.GetNotifiTaskList(userId);
                    viewModel.pubMessageList = pubMessageList;
                    viewModel.messageCount = ep.GetUnreadMessageCount(pubMessageList);
                    viewModel.password = ep.Decrypt(curUser.password);                    
                    return View(viewModel);
                }
                catch(Exception ex)
                {
                    return Redirect(Url.Action("Index", "Error"));
                }                
            } 
            else
            {
                return Redirect(Url.Action("Index", "iniciar"));
            }
              
        }

        [HttpPost]
        public ActionResult editprofile(HttpPostedFileBase user_logo, string email, string password,
            string acq_date, string first_name1, string last_name1, string mother_last_name1,
            string first_name2, string last_name2, string mother_last_name2, string phone_nmuber1,
            string phone_number2, string postal_address, string residential_address, 
            HttpPostedFileBase upload_writing, string siono, string since , string until,
            string tenant_first_name1, string tenant_last_name1, string tenant_mother_last_name1,
            string tenant_first_name2, string tenant_last_name2, string tenant_mother_last_name2,
            string leased_postal_address, string leased_residential_address, 
            HttpPostedFileBase leased_upload_file,string brand, string model,
            string colour, string year, string clapboard, string stamp_number
            )
        {
            try
            {
                long userId = (long)Session["USER_ID"];
                user user_s = entities.users.Find(userId);

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
                    user_s.user_img = fileName;
                }

                
                if (acq_date != "0001-01-01")
                {
                    user_s.acq_date = DateTime.ParseExact(acq_date, "yyyy-MM-dd",
                        System.Globalization.CultureInfo.InvariantCulture);
                }

                user_s.email = email;
                user_s.password = ep.Encrypt(password);
                user_s.first_name1 = first_name1;
                user_s.first_name2 = first_name2;
                user_s.last_name1 = last_name1;
                user_s.last_name2 = last_name2;
                user_s.mother_last_name1 = mother_last_name1;
                user_s.mother_last_name2 = mother_last_name2;
                user_s.phone_number1 = phone_nmuber1;
                user_s.phone_number2 = phone_number2;
                user_s.postal_address = postal_address;
                user_s.residential_address = residential_address;
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
                    user_s.upload_writing = fileName;
                }

                user_s.tenant_first_name1 = tenant_first_name1;
                user_s.tenant_first_name2 = tenant_first_name2;
                user_s.tenant_last_name1 = tenant_last_name1;
                user_s.tenant_last_name2 = tenant_last_name2;
                user_s.tenant_mother_last_name1 = tenant_mother_last_name1;
                user_s.tenant_mother_last_name2 = tenant_mother_last_name2;
                user_s.leased_postal_address = leased_postal_address;
                user_s.leased_residential_address = leased_residential_address;
                user_s.siono = siono;
                user_s.brand = brand;
                user_s.model = model;
                if (year != "")
                {
                    user_s.year = Convert.ToInt32(year);
                } else
                {
                    user_s.year = null;
                }
                
                user_s.clapboard = clapboard;

                if (stamp_number != "")
                {
                    user_s.stamp_number = Convert.ToInt32(stamp_number);
                } else {
                    user_s.stamp_number = null;
                } 

                if (since == "0001-01-01")
                {
                    user_s.since = null;
                }
                else
                {
                    user_s.since = DateTime.ParseExact(since, "yyyy-MM-dd",
                        System.Globalization.CultureInfo.InvariantCulture);
                }

                if (until == "0001-01-01")
                {
                    user_s.until = null;
                }
                else
                {
                    user_s.until = DateTime.ParseExact(until, "yyyy-MM-dd",
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
                    user_s.user_img = fileName;
                }

                //user_s.since = since;
                entities.SaveChanges();

                return Redirect(Url.Action("panel", "control"));
            }
            catch(Exception)
            {
                return Redirect(Url.Action("perfil", "administrador"));
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

            return Redirect(Url.Action("iniciar", "iniciar"));
        }
    }
}