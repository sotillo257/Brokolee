using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity.Infrastructure;
using System.Data.Entity.Validation;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using WebApplication1.Areas.coadmin.ViewModels;
using WebApplication1.Concrete;
using WebApplication1.Models;
using Westwind.Web.Mvc;
using System.Net;
using System.Net.Mail;

namespace WebApplication1.Areas.coadmin.Controllers
{
    public class titularesController : Controller
    {
        private const string V = "garras";
        pjrdev_condominiosEntities entities = new pjrdev_condominiosEntities();
        EFPublicRepository ep = new EFPublicRepository();
        // GET: coadmin/titulares
        public ActionResult listado(string searchStr = "")
        {
            if (Session["USER_ID"] != null)
            {
                long userId = (long)Session["USER_ID"];
                user curUser = entities.users.Find(userId);
                List<ShowMessage> pubMessageList = ep.GetChatMessages(userId);
                Dictionary<long, string> communityDict = new Dictionary<long, string>();
                List<user> titularList = new List<user>();
                if (searchStr == "")
                {
                    var query1 = (from r in entities.users where r.role == 1 select r);
                    titularList = query1.ToList();
                } else
                {
                    var query = (from r in entities.users
                                 where r.role == 1 && (r.first_name1.Contains(searchStr) == true || r.last_name1.Contains(searchStr) == true)
                                 select r);
                    titularList = query.ToList();
                }

                foreach (var item in titularList)
                {
                    string communityName = ep.GetCommunityCoInfo(item.id)[0];
                    communityDict.Add(item.id, communityName);
                }

                titularesViewModel viewModel = new titularesViewModel();
                viewModel.side_menu = "titulares";
                viewModel.side_sub_menu = "titulares_listado";
                viewModel.document_category_list = entities.document_type.ToList();
                viewModel.titularList = titularList;
                viewModel.searchStr = searchStr;
                viewModel.curUser = curUser;
                viewModel.pubTaskList = ep.GetNotifiTaskList(userId);
                viewModel.pubMessageList = pubMessageList;
                viewModel.communityDict = communityDict;
                viewModel.messageCount = ep.GetUnreadMessageCount(pubMessageList);
                viewModel.communityName = ep.GetCommunityInfo(userId)[0];
                viewModel.communityApart = ep.GetCommunityInfo(userId)[1];
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
                titularesViewModel viewModel = new titularesViewModel();
                viewModel.side_menu = "titulares";
                viewModel.side_sub_menu = "titulares_agregar";
                viewModel.document_category_list = entities.document_type.ToList();
                viewModel.curUser = curUser;
                viewModel.pubTaskList = ep.GetNotifiTaskList(userId);
                viewModel.pubMessageList = pubMessageList;
                viewModel.messageCount = ep.GetUnreadMessageCount(pubMessageList);
                viewModel.communityList = entities.communities.ToList();
                viewModel.communityName = ep.GetCommunityInfo(userId)[0];
                viewModel.communityApart = ep.GetCommunityInfo(userId)[1];
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
                    user editUser = entities.users.Find(id);
                    List<ShowMessage> pubMessageList = ep.GetChatMessages(userId);
                    titularesEditViewModel viewModel = new titularesEditViewModel();
                    viewModel.side_menu = "titulares";
                    viewModel.side_sub_menu = "titulares_ver";
                    viewModel.document_category_list = entities.document_type.ToList();
                    viewModel.editUser = editUser;
                    viewModel.view_resident_logo = "~/App_Data/User_Logo/" + editUser.user_img;
                    viewModel.curUser = curUser;
                    viewModel.password = ep.Decrypt(editUser.password);
                    viewModel.pubTaskList = ep.GetNotifiTaskList(userId);
                    viewModel.pubMessageList = pubMessageList;
                    viewModel.messageCount = ep.GetUnreadMessageCount(pubMessageList);
                    viewModel.communityName = ep.GetCommunityInfo(userId)[0];
                    viewModel.communityApart = ep.GetCommunityInfo(userId)[1];
                    Session["ACC_USER_ID"] = Convert.ToInt64(id);
                    return View(viewModel);
                    //return Redirect(Url.Action("ver", "titulares", new { area = "" }));
                }
                else
                {
                    return Redirect(Url.Action("ver", "titulares"));
                }
            }
            else
            {
                return Redirect(ep.GetLogoutUrl());
            }
        }

        public ActionResult editar(long id)
        { 

                
            if (Session["USER_ID"] != null)
            { 

                try
                {
                    long userId = id;
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
                    user editUser = entities.users.Find(id);
                    List<ShowMessage> pubMessageList = ep.GetChatMessages(userId);
                    titularesEditViewModel viewModel = new titularesEditViewModel();
                    viewModel.side_menu = "titulares";
                    viewModel.side_sub_menu = "titulares_editar";
                    viewModel.document_category_list = entities.document_type.ToList();
                    viewModel.editUser = editUser;
                    viewModel.curUser = curUser;
                    viewModel.password = ep.Decrypt(editUser.password);
                    viewModel.pubTaskList = ep.GetNotifiTaskList(userId);
                    viewModel.pubMessageList = pubMessageList;
                    viewModel.messageCount = ep.GetUnreadMessageCount(pubMessageList);
                    viewModel.communityName = ep.GetCommunityInfo(userId)[0];
                    viewModel.communityApart = ep.GetCommunityInfo(userId)[1];
                    return View(viewModel);
                }
                catch (Exception ex)
                {
                    return Redirect(Url.Action("Index", "Error"));
                }
            }
            else
            {
                return Redirect(ep.GetLogoutUrl());
            }
        }


        [HttpPost]
        public async Task<ActionResult> newresident(HttpPostedFileBase user_logo,
    string email, string password, string acq_date, string first_name1,
    string last_name1, string mother_last_name1, string phone_number1,
    string postal_address, string residential_address, string apartment, HttpPostedFileBase writing_script,
    string siono, string brand, string model, string colour, string year, string clapboard,
    string stamp_number)
        {
            if (Session["USER_ID"] != null)
            {
                string emailTemplate = "";
                long userId = (long)Session["USER_ID"];
                user curUser = entities.users.Find(userId);
                user newResident = new user();

                if (user_logo != null && user_logo.ContentLength > 0)
                {
                    var fileName = Path.GetFileName(user_logo.FileName);
                    // store the file inside ~/App_Data/uploads folder
                    var path = Path.Combine(Server.MapPath("~/Upload/User_Logo"), fileName);
                    user_logo.SaveAs(path);
                    newResident.user_img = fileName;
                }
                else
                {
                    newResident.user_img = null;
                }


                if (writing_script != null && writing_script.ContentLength > 0)
                {
                    var fileName = Path.GetFileName(writing_script.FileName);
                    // store the file inside ~/App_Data/uploads folder
                    var path = Path.Combine(Server.MapPath("~/Upload/Upload_Writing"), fileName);
                    writing_script.SaveAs(path);
                    newResident.upload_writing = fileName;
                }
                else
                {
                    newResident.user_img = null;
                }
                newResident.email = email;
                newResident.password = ep.Encrypt(password);
                DateTime acqDate = DateTime.ParseExact(acq_date, "yyyy-MM-dd",
                    System.Globalization.CultureInfo.InvariantCulture);
                newResident.acq_date = acqDate;
                newResident.apartment = apartment;
                newResident.first_name1 = first_name1;
                newResident.last_name1 = last_name1;
                newResident.mother_last_name1 = mother_last_name1;
                newResident.phone_number1 = phone_number1;
                newResident.postal_address = postal_address;
                newResident.residential_address = residential_address;
                if (siono == "boxyes")
                {
                    newResident.is_leased = true;
                }
                else
                {
                    newResident.is_leased = false;
                }
                newResident.brand = brand;
                newResident.model = model;
                newResident.colour = colour;
                if (year != "")
                {
                    newResident.year = Convert.ToInt32(year);
                }
                else
                {
                    newResident.year = null;
                }

                newResident.clapboard = clapboard;
                try
                {
                    newResident.stamp_number = Convert.ToInt32(stamp_number);
                }
                catch (Exception)
                {
                    newResident.stamp_number = null;
                }

                newResident.role = 1;
                newResident.create_userid = userId;
                entities.users.Add(newResident);
                entities.SaveChanges();
                emailtheme emailtheme = entities.emailthemes.Where(m => m.user_id == userId
                                    && m.type_id == 1).FirstOrDefault();
                //user g = entities.users.Where(m => m.email == email).FirstOrDefault();

                //if (g!=null)
                //{
                //    return Redirect(Url.Action("agregar", "titulares", new { area = "coadmin" }));
                //}
                //if (emailtheme != null)
                //{
                //    //emailTemplate = emailtheme.htmcontent;
                //    string patialView = "~/Areas/webmaster/Views/titulares/emailing.cshtml";
                //    emailingViewModel viewModel = new emailingViewModel();
                //    emailTemplate = ViewRenderer.RenderPartialView(patialView, viewModel);
                //} else
                //{

                //}

                // **********
                MailMessage msj = new MailMessage();
                SmtpClient cli = new SmtpClient();

                String email_g = email;
                String name_g = first_name1 + " " + last_name1;
                string user_g = email;
                string pass_g = password;

                msj.From = new MailAddress("o.olaya@zerosystempr.com");
                msj.To.Add(new MailAddress(email_g));
                msj.Subject = "Bienvenido";
                msj.Body = "Hola " + name_g + " bienvenido a brokole, tu usuario es:" + " "  + user_g + "y tu contraseña es: " + " " + pass_g ;
                msj.IsBodyHtml = false;

                cli.Host = "mail.zerosystempr.com";
                cli.Port = 587;
                cli.Credentials = new NetworkCredential("o.olaya@zerosystempr.com", "temporero");
                cli.EnableSsl = true;
                cli.Send(msj);

                //*********

                string patialView = "~/Views/iniciar/emailing.cshtml";
                emailingViewModel viewModel = new emailingViewModel();
                viewModel.firstName = first_name1;
                viewModel.lastName = last_name1;
                viewModel.fromEmail = curUser.email;
                viewModel.toEmail = email;
                emailTemplate = ViewRenderer.RenderPartialView(patialView, viewModel);
                int a = await ep.SendAlertEmail(curUser.email, email, curUser.first_name1 + " " + curUser.last_name1,
                    "añadir título", "has sido añadido como titular\n password: " + password, emailTemplate);
                return Redirect(Url.Action("listado", "titulares", new { area = "coadmin" }));
            }
            else
            {
                return Redirect(Url.Action("agregar", "titulares", new { area = "coadmin" }));
            }
        }

        [HttpPost]
        public ActionResult editresident(long editID, HttpPostedFileBase user_logo,
            string password, string acq_date, string first_name1, string last_name1,
            string mother_last_name1, string phone_number1, string postal_address,
            string residential_address, string siono, string since, string until,
            string tenant_first_name1, string tenant_last_name1, string tenant_mother_last_name1,
            HttpPostedFileBase script_file, string leased_postal_address, string leased_residential_address,
            string brand, string model, string colour, string year,
            string clapboard, string stamp_number, string apartment)
        {
            if (Session["USER_ID"] != null)
            {
                user editResident = entities.users.Find(editID);

                if (user_logo != null && user_logo.ContentLength > 0)
                {
                    var fileName = Path.GetFileName(user_logo.FileName);
                    // store the file inside ~/App_Data/uploads folder
                    var path = Path.Combine(Server.MapPath("~/Upload/User_Logo"), fileName);
                    user_logo.SaveAs(path);
                    editResident.user_img = fileName;
                }

                if (script_file != null && script_file.ContentLength > 0)
                {
                    var fileName = Path.GetFileName(script_file.FileName);
                    // store the file inside ~/App_Data/uploads folder
                    var path = Path.Combine(Server.MapPath("~/Upload/Upload_Writing"), fileName);
                    script_file.SaveAs(path);
                    editResident.upload_writing = fileName;
                }

                editResident.password = ep.Encrypt(password);
                DateTime acqDate = DateTime.ParseExact(acq_date, "yyyy-MM-dd",
                    System.Globalization.CultureInfo.InvariantCulture);
                editResident.acq_date = acqDate;
                editResident.first_name1 = first_name1;
                editResident.last_name1 = last_name1;
                editResident.mother_last_name1 = mother_last_name1;
                editResident.phone_number1 = phone_number1;
                editResident.apartment = apartment;
                editResident.postal_address = postal_address;
                editResident.residential_address = residential_address;
                if (siono == "boxyes")
                {
                    editResident.is_leased = true;
                }
                else
                {
                    editResident.is_leased = false;
                }

                if (since == "")
                {
                    editResident.since = null;
                } else
                {
                    editResident.since = DateTime.ParseExact(since, "yyyy-MM-dd",
                            System.Globalization.CultureInfo.InvariantCulture);
                }

                if (until == "")
                {
                    editResident.until = null;
                } else
                {
                    editResident.until = DateTime.ParseExact(until, "yyyy-MM-dd",
                            System.Globalization.CultureInfo.InvariantCulture);
                }

                editResident.tenant_first_name1 = tenant_first_name1;
                editResident.tenant_last_name1 = tenant_last_name1;
                editResident.tenant_mother_last_name1 = tenant_mother_last_name1;
                editResident.leased_postal_address = leased_postal_address;
                editResident.leased_residential_address = leased_residential_address;
                editResident.brand = brand;
                editResident.model = model;
                editResident.colour = colour;
                if (year != "")
                {
                    editResident.year = Convert.ToInt32(year);
                } else
                {
                    editResident.year = null;
                }

                editResident.clapboard = clapboard;

                if (stamp_number != "")
                {
                    editResident.stamp_number = Convert.ToInt32(stamp_number);
                } else
                {
                    editResident.stamp_number = null;
                }

                entities.SaveChanges();
                return Redirect(Url.Action("listado", "titulares", new { area = "coadmin" }));
            } else
            {
                return Redirect(Url.Action("editar", "titulares", new { area = "coadmin", id = editID }));
            }
        }

        public JsonResult DeleteUser(long delID)
        {
            try
            {
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
                List<taskcomment> taskcomments = entities.taskcomments.Where(m => m.user_id == delID).ToList();
                entities.taskcomments.RemoveRange(taskcomments);
                List<payment> payments = entities.payments.Where(m => m.user_id == delID).ToList();
                entities.payments.RemoveRange(payments);
                List<onlineuser> onlineusers = entities.onlineusers.Where(m => m.user_id == delID).ToList();
                entities.onlineusers.RemoveRange(onlineusers);
                user delUser = entities.users.Find(delID);
                
                entities.users.Remove(delUser);
                entities.SaveChanges();
                return Json(new { result = "success" });
            } catch (Exception ex)
            {
                return Json(new { result = "error", exception = ex.Message });
            }
        }

        public JsonResult ActiveUser(long delID)
        {
            try
            {
                user user = entities.users.Find(delID);
                user.is_active = true;
                entities.SaveChanges();
                return Json(new { result = "success" });
            }
            catch (Exception ex)
            {
                return Json(new { result = "error", exception = ex.HResult });
            }
        }

        public JsonResult DeactiveUser(long delID)
        {
            try
            {
                user user = entities.users.Find(delID);
                user.is_active = false;
                entities.SaveChanges();
                return Json(new { result = "success" });
            }
            catch (Exception ex)
            {
                return Json(new { result = "error", exception = ex.HResult });
            }
        }


        [HttpPost]
        public ActionResult SeacrhResult(string searchStr)
        {
            return Redirect(Url.Action("listado", "titulares", new { area = "coadmin", searchStr = searchStr }));
        }

        [HttpPost]
        public FileResult ExportCSV(string searchStr = "")
        {
            List<user> userList = new List<user>();
            if (searchStr == "")
            {
                var query = (from r in entities.users where r.role == 1 select r);
                userList = query.ToList();

            }
            else
            {
                var query1 = (from r in entities.users
                              where r.role == 1
                              && (r.first_name1.Contains(searchStr) || r.last_name1.Contains(searchStr))
                              select r);
                userList = query1.ToList();
            }

            string[] columnNames = new string[] { "First Name", "Last Name", "Acquired Date",
            "Email", "Password", "Phone Number", "Postal Address", "Residential Address",
            "Siono"};
            string csv = string.Empty;

            foreach (string columnName in columnNames)
            {
                //Add the Header row for CSV file.
                csv += columnName + ';';
            }

            //Add new line.
            csv += "\r\n";

            foreach (var item in userList)
            {
                csv += item.first_name1.ToString() + ";" ; 
                csv += item.last_name1.ToString() + ";";
                csv += item.acq_date.ToString("yyyy-MM-dd") + ";"; 
                csv += item.email.ToString() + ";";
                csv += item.password.ToString() + ";";
                if (item.phone_number1 != null)
                {
                    csv += item.phone_number1.ToString() + ";";
                }
                else
                {
                    csv += "".Replace(",", ";") + ",";
                }
                
                if (item.postal_address != null)
                {
                    csv += item.postal_address.ToString() + ";";
                } else
                {
                    csv += "".ToString();
                }

                if (item.residential_address != null)
                {
                    csv += item.residential_address.ToString() + ";";
                } else
                {
                    csv += "".ToString() + ";";
                }

                if (item.siono != null)
                {
                    csv += item.siono.ToString() + ";";
                } else
                {
                    csv += "".ToString() + ";";
                }

                //Add new line.
                csv += "\r\n";
            }

            //Download the CSV file.
            byte[] bytes = Encoding.ASCII.GetBytes(csv);
            return File(bytes, "application/text", "titular.csv");

        }

        [HttpPost]
        public async Task<ActionResult> ImportCsv(HttpPostedFileBase csv_file)
        {
            try
            {
                string emailTemplate = "";
                long userId = (long)Session["USER_ID"];
                user curUser = entities.users.Find(userId);
                if (csv_file != null && csv_file.ContentLength > 0)
                {
                    string path1 = string.Format("{0}/{1}",
                        Server.MapPath("~/Upload/Upload_Import"), csv_file.FileName);
                    string[] validFileTypes = { ".xls", ".xlsx", ".csv" };
                    string extension = System.IO.Path.GetExtension(csv_file.FileName).ToLower();

                    if (!Directory.Exists(path1))
                    {
                        Directory.CreateDirectory(Server.MapPath("~/Upload/Upload_Import"));
                    }

                    if (validFileTypes.Contains(extension))
                    {
                        if (System.IO.File.Exists(path1))
                        {
                            System.IO.File.Delete(path1);
                        }
                        csv_file.SaveAs(path1);

                        if (extension == ".csv")
                        {
                            DataTable dt = Utility.ConvertCSVtoDataTable(path1);
                            for (int i = 0; i < dt.Rows.Count; i++)
                            {
                                emailTemplate = "";
                                user user = new user();
                                string first_name = (string)dt.Rows[i].ItemArray.GetValue(0);
                                string last_name = (string)dt.Rows[i].ItemArray.GetValue(1);
                                string acqDateStr = (string)dt.Rows[i].ItemArray.GetValue(2);
                                DateTime acq_date = DateTime.ParseExact(acqDateStr, "yyyy-MM-dd",
                                                    System.Globalization.CultureInfo.InvariantCulture);
                                string email = (string)dt.Rows[i].ItemArray.GetValue(3);
                                string password = (string)dt.Rows[i].ItemArray.GetValue(4);
                                string phone_number = (string)dt.Rows[i].ItemArray.GetValue(5);
                                string postal_address = (string)dt.Rows[i].ItemArray.GetValue(6);
                                string residential_address = (string)dt.Rows[i].ItemArray.GetValue(7);
                                string sinno = (string)dt.Rows[i].ItemArray.GetValue(8);
                                user.first_name1 = first_name;
                                user.last_name1 = last_name;
                                user.email = email;
                                user.password = password;
                                user.phone_number1 = phone_number;
                                user.postal_address = postal_address;
                                user.residential_address = residential_address;
                                user.role = 1;
                                user.acq_date = acq_date;
                                if (sinno == "boxyes")
                                {
                                    user.siono = sinno;
                                } else
                                {
                                    user.siono = "boxno";
                                }

                                user.create_userid = userId;
                                user.is_active = true;
                                user.is_admin = false;
                                user.is_logged = false;
                                entities.users.Add(user);
                                entities.SaveChanges();
                                emailtheme emailtheme = entities.emailthemes.Where(m => m.user_id == userId &&
                                                        m.type_id == 1).FirstOrDefault();
                                //if (emailtheme != null)
                                //{
                                //    emailTemplate = emailtheme.htmcontent;
                                //}
                                //else
                                //{
                                //    string patialView = "~/Areas/webmaster/Views/titulares/emailing.cshtml";
                                //    emailingViewModel viewModel = new emailingViewModel();
                                //    emailTemplate = ViewRenderer.RenderPartialView(patialView, viewModel);
                                //}

                                string patialView = "~/Views/iniciar/emailing.cshtml";
                                emailingViewModel viewModel = new emailingViewModel();
                                viewModel.firstName = first_name;
                                viewModel.lastName = last_name;
                                viewModel.fromEmail = curUser.email;
                                viewModel.toEmail = email;
                                emailTemplate = ViewRenderer.RenderPartialView(patialView, viewModel);
                                int a = await ep.SendAlertEmail(
                                    curUser.email, email,
                                    curUser.first_name1 + " " + curUser.last_name1,
                                    "añadir título",
                                    "has sido añadido como titular\n password: " + password, emailTemplate);
                            }
                        }
                    }
                }
                return Redirect(Url.Action("listado", "titulares", new { area = "coadmin" }));
            }
            catch (DbEntityValidationException ex)
            {
                foreach (DbEntityValidationResult item in ex.EntityValidationErrors)
                {
                    // Get entry

                    DbEntityEntry entry = item.Entry;
                    string entityTypeName = entry.Entity.GetType().Name;

                    // Display or log error messages

                    foreach (DbValidationError subItem in item.ValidationErrors)
                    {
                        string message = string.Format("Error '{0}' occurred in {1} at {2}",
                                 subItem.ErrorMessage, entityTypeName, subItem.PropertyName);
                        Console.WriteLine(message);
                    }
                }
                return Redirect(Url.Action("listado", "titulares", new { area = "coadmin" }));
            }
            catch (Exception ex)
            {
                return Redirect(Url.Action("listado", "titulares", new { area = "coadmin" }));
            }

        }


       

        }
    }
   