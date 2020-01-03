using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WebApplication1.Concrete;
using WebApplication1.Models;
using WebApplication1.ViewModels;

namespace WebApplication1.Controllers
{
    public class iniciarController : Controller
    {
        pjrdev_condominiosEntities entities = new pjrdev_condominiosEntities();
        EFPublicRepository ep = new EFPublicRepository();
        // GET: iniciar
        public ActionResult Index()
        {
            return Redirect(Url.Action("iniciar", "iniciar"));
        }

        private bool IsValidEmail(string name)
        {
            try
            {
                var addr = new System.Net.Mail.MailAddress(name);
                return addr.Address == name;
            }
            catch
            {
                return false;
            }
        }

        public JsonResult PerformLogin(string email, string password, bool rememberme)
        {
            try
            {
                var response = HttpContext.Response;
                HttpCookie emailCookie = new HttpCookie("Email");
                HttpCookie passwordCookie = new HttpCookie("Password");
                HttpCookie userIdCookie = new HttpCookie("UserID");
                if (rememberme == true)
                {
                    emailCookie.Expires = DateTime.Now.AddDays(30);
                    passwordCookie.Expires = DateTime.Now.AddDays(30);
                    emailCookie.Value = email.Trim();
                    passwordCookie.Value = password.Trim();
                } else
                {
                    emailCookie.Expires = DateTime.Now.AddDays(-1);
                    passwordCookie.Expires = DateTime.Now.AddDays(-1);
                }
                response.Cookies.Remove("Email");
                response.Cookies.Remove("Password");
                response.Cookies.Add(emailCookie);
                response.Cookies.Add(passwordCookie);
                List<user> result = new List<user>();
                List<user> usernameResult = entities.users.Where(m => m.email == email).ToList();
                password = ep.Encrypt(password);
                if (usernameResult.Count > 0)
                {
                    user username_result_item = usernameResult.First();

                    if (string.Compare(email, username_result_item.email, StringComparison.CurrentCulture) == 0)
                    {
                        result = entities.users.Where(m => m.email == email).Where(m => m.password == password).ToList();
                        if (result.Count > 0)
                        {
                            user current_user = result[0];

                            if (string.Compare(password, current_user.password, StringComparison.CurrentCulture) == 0)
                            {
                                if (current_user.is_active)
                                {
                                    Session["USER_ID"] = current_user.id;
                                    Session["USER_NAME"] = current_user.first_name1 + " " + current_user.last_name1;
                                    Session["USER_IMAGE"] = current_user.user_img;
                                    Session["USER_ADMIN"] = current_user.is_admin;
                                    Session["USER_ROLE"] = current_user.role;
                                    Session["Active"] = current_user.is_active;
                                    userIdCookie.Expires = DateTime.Now.AddDays(100);
                                    userIdCookie.Value = current_user.id.ToString();
                                    response.Cookies.Remove("UserID");
                                    response.Cookies.Add(userIdCookie);
                                    current_user.is_logged = true;
                                    entities.SaveChanges();
                                    return Json(new { result = "success" });
                                }
                                else
                                {
                                    return Json(new { result = "Lo sentimos, este usuario ha sido temporaremente desactivado" });
                                }
                               
                            }
                            else
                            {
                                return Json(new { result = "password is wrong" });
                            }
                        }
                        else
                        {
                            return Json(new { result = "password is wrong" });
                        }
                    }
                    else
                    {
                        return Json(new { result = "username does not exist" });
                    }
                }
                else
                {
                    return Json(new { result = "username does not exist" });
                }
            } catch (Exception ex)
            {
                return Json(new { result = "server is error", exception = ex.Message });
            }
        }

        public ActionResult ImageShow()
        {
            try
            {
                var imageData = Session["USER_IMAGE"];
                return File((byte[])imageData, "image/jpg");
            }
            catch (Exception)
            {
                return null;
            }
        }

        public ActionResult iniciar()
        {
            if (Session["USER_ID"] != null)
            {
                int role = (int)Session["USER_ROLE"];
                if (role == 3)
                {
                    return Redirect(Url.Action("panel", "control", new { area = "webmaster" }));
                } else if(role == 2)
                {
                    Session["Sus_login"] = "no";
                    return Redirect(Url.Action("panel", "control", new { area = "coadmin"}));
                } else
                {
                    return Redirect(Url.Action("community", "control"));
                }
                
            }
            else
            {
                iniciarViewModel viewModel = new iniciarViewModel();
                if (Request.Cookies.Count > 0)
                {
                    HttpCookie emailCookie = Request.Cookies.Get("Email");
                    HttpCookie passwordCookie = Request.Cookies.Get("Password");

                    if (emailCookie != null && passwordCookie != null)
                    {
                        viewModel.email = emailCookie.Value;
                        viewModel.password = passwordCookie.Value;
                        viewModel.rememberme = true;
                    }
                    else
                    {
                        viewModel.email = "";
                        viewModel.password = "";
                        viewModel.rememberme = false;
                    }
                } else
                {
                    viewModel.email = "";
                    viewModel.password = "";
                    viewModel.rememberme = false;
                }

                return View(viewModel);
            }
        }

        public ActionResult recuperar()
        {
            if (Session["USER_ID"] != null)
            {
                return Redirect(Url.Action("panel", "control"));
            }
            else
            {
                return View();
            }
        }
    }
}