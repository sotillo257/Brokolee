using Stripe;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using WebApplication1.Concrete;
using WebApplication1.Models;
using WebApplication1.ViewModels;
using Westwind.Web.Mvc;

namespace WebApplication1.Controllers
{
    public class suscribeController : Controller
    {
        pjrdev_condominiosEntities entities = new pjrdev_condominiosEntities();
        EFPublicRepository ep = new EFPublicRepository();       

        // GET: suscribe
        //Login Page to Suscribe
        public ActionResult Index(long? packageId)
        {
            SuscribeLoginViewModel viewModel = new SuscribeLoginViewModel();
            viewModel.Password = "";
            viewModel.Email = "";
            viewModel.WarningMsg = "";
            viewModel.PackageId = Convert.ToInt64(packageId);
            if (Session["Login_Result"] != null)
            {
                if (packageId != null)
                {
                    string LoginResult = Convert.ToString(Session["Login_Result"]);

                    if (LoginResult.Equals("success"))
                    {
                        Session["Sus_login"] = "ok";
                        //Session["SUS_USER_ID"] = communuser.user_id;
                        return Redirect(Url.Action("panel", "control", new { area = "coadmin" }));
                        //community community = entities.communities.Where(m => m.package_id == packageId).FirstOrDefault();
                        //if (community != null)
                        //{

                        //}
                        //else
                        //{
                        //    viewModel.Password = Convert.ToString(Session["Sus_Pass"]);
                        //    viewModel.Email = Convert.ToString(Session["Sus_Email"]);
                        //    viewModel.WarningMsg = "Community does not exist!";
                        //    return View(viewModel);
                        //}
                        //communuser communuser = entities.communusers.Where(m => m.commun_id == community.id).FirstOrDefault();
                        //if (communuser != null)
                        //{

                        //}
                        //else
                        //{
                        //    viewModel.Password = Convert.ToString(Session["Sus_Pass"]);
                        //    viewModel.Email = Convert.ToString(Session["Sus_Email"]);
                        //    viewModel.WarningMsg = "Coadmin does not exist!";
                        //    return View(viewModel);
                        //}
                    }
                    else
                    {
                        viewModel.Password = Convert.ToString(Session["Sus_Pass"]);
                        viewModel.Email = Convert.ToString(Session["Sus_Email"]);
                        viewModel.WarningMsg = LoginResult;
                        return View(viewModel);
                    }
                }
                else
                {
                    return Redirect(Url.Action("NotFound", "Error"));
                }
            }
            return View(viewModel);
        }
        //Register  Page to Suscribe
        public ActionResult Register(long? packageId)
        {
            SuscribeRegisterViewModel viewModel = new SuscribeRegisterViewModel();
            viewModel.Name = "";
            viewModel.LastName = "";
            viewModel.CommunityName = "";
            viewModel.Address = "";
            viewModel.PhoneNumber = "";
            viewModel.Email = "";
            viewModel.Password = "";
            viewModel.ConfirmPassword = "";
            viewModel.WarningMsg = "";           

            if (packageId != null)
            {
                viewModel.PackageId = Convert.ToInt64(packageId);
                if (Session["Register_Result"] != null)
                {
                    string RegResult = Convert.ToString(Session["Register_Result"]);

                    if (RegResult.Equals("success"))
                    {
                        Session["Sus_login"] = "ok";
                        return Redirect(Url.Action("panel", "control", new { area = "coadmin" }));
                        //community community = entities.communities.Where(m => m.package_id
                        //                    == packageId).FirstOrDefault();
                        //if (community != null)
                        //{
                        //    communuser communuser = entities.communusers.Where(m => m.commun_id
                        //                            == community.id).FirstOrDefault();
                        //    if (communuser != null)
                        //    {

                        //    }
                        //    else
                        //    {
                        //        viewModel.Password = Convert.ToString(Session["Sus_Pass"]);
                        //        viewModel.Email = Convert.ToString(Session["Sus_Email"]);
                        //        viewModel.WarningMsg = "Coadmin does not exist!";
                        //        return View(viewModel);
                        //    }
                        //}
                        //else
                        //{
                        //    viewModel.Password = Convert.ToString(Session["Sus_Pass"]);
                        //    viewModel.Email = Convert.ToString(Session["Sus_Email"]);
                        //    viewModel.WarningMsg = "Community does not exist!";
                        //    return View(viewModel);
                        //}
                    }
                    else
                    {
                        viewModel.Name = Convert.ToString(Session["Reg_Name"]);
                        viewModel.LastName = Convert.ToString(Session["Reg_LastName"]);
                        viewModel.CommunityName = Convert.ToString(Session["Reg_CommunityName"]);
                        viewModel.Address = Convert.ToString(Session["Reg_Address"]);
                        viewModel.PhoneNumber = Convert.ToString(Session["Reg_PhoneNumber"]);
                        viewModel.Email = Convert.ToString(Session["Reg_Email"]);
                        viewModel.Password = Convert.ToString(Session["Reg_Password"]);
                        viewModel.ConfirmPassword = Convert.ToString(Session["Reg_ConfirmPassword"]);
                        viewModel.WarningMsg = RegResult;
                        return View(viewModel);
                    }
                }
                return View(viewModel);
            }
            else
            {
                return Redirect(Url.Action("NotFound", "Error"));
            }           
        }

        // Monthly Payment
        public ActionResult MonthlyPayment(long packageId)
        {
            SuscribePaymentViewModel viewModel = new SuscribePaymentViewModel();
            viewModel.WarningMsg = "";
            return View(viewModel);
        }

        [HttpPost]
        public async Task<ActionResult> SubmitRegister(string Name, string LastName, string LastNameMaterno, string CommunityName,
            string Address, string PhoneNumber, string Email, string Password, 
            string ConfirmPassword, long PackageId)
        {
            Session["Reg_Name"] = Name;
            Session["Reg_LastName"] = LastName;
            Session["Reg_LastNameMaterno"] = LastNameMaterno;
            Session["Reg_CommunityName"] = CommunityName;
            Session["Reg_Address"] = Address;
            Session["Reg_PhoneNumber"] = PhoneNumber;
            Session["Reg_Email"] = Email;
            Session["Reg_Password"] = Password;
            Session["Reg_ConfirmPassword"] = ConfirmPassword;

            if (Password != ConfirmPassword)
            {
                Session["Register_Result"] = "Confirm password have to matach with password";
            } else
            {
                try
                {
                    suscribe suscribe = entities.suscribes.Where(m => m.email.Equals(Email)
                    && m.package_id == PackageId).FirstOrDefault();
                    if(suscribe != null)
                    {
                        Session["Register_Result"] = "User already exist for same email";
                    } else
                    {
                        // Check PackageId is valid
                        package package = entities.packages.Where(m => m.id == PackageId).FirstOrDefault();

                        if (package != null)
                        {
                            // Insert new suscribe coadmin
                            user user1 = new user();
                            user1.first_name1 = Name;
                            user1.last_name1 = LastName;
                            user1.mother_last_name1 = LastNameMaterno;
                            user1.first_name2 = CommunityName;
                            user1.postal_address = Address;
                            user1.phone_number1 = PhoneNumber;
                            user1.email = Email;
                            user1.password = ep.Encrypt(Password);
                            user1.is_admin = false;
                            user1.is_logged = false;
                            user1.is_active = false;
                            user1.role = 2;
                            user1.acq_date = DateTime.Now;
                            user1.package_id = PackageId;
                            user1.is_paid = false;
                            entities.users.Add(user1);
                            entities.SaveChanges();
                            // create new community for current coadmin
                            community community = new community();
                            community.admin_email = Email;
                            community.package_id = PackageId;
                            community.description = "";
                            community.first_name = CommunityName;
                            community.created_at = DateTime.Now;
                            community.updated_at = DateTime.Now;
                            community.is_active = true;
                            community.apart = "";
                            entities.communities.Add(community);
                            entities.SaveChanges();
                            // Create communuser 
                            communuser communuser = new communuser();
                            communuser.commun_id = community.id;
                            communuser.user_id = user1.id;
                            entities.communusers.Add(communuser);
                            entities.SaveChanges();
                            Session["USER_ID"] = user1.id;
                            Session["USER_ROLE"] = 2;
                            Session["Active"] = false;
                            Session["PACK_ID"] = PackageId;
                            Session["Register_Result"] = "success";
                            string patialView = "~/Views/iniciar/emailing.cshtml";
                            emailingViewModel viewModel = new emailingViewModel();
                            viewModel.firstName = CommunityName;
                            viewModel.lastName = LastName;
                            viewModel.fromEmail = "d.gonzalez@zerosystempr.com";
                            viewModel.toEmail = Email;
                            string emailTemplate = ViewRenderer.RenderPartialView(patialView, viewModel);
                            int a = await ep.SendAlertEmail("", Email, CommunityName + " " + LastName,
                                "añadir título", "has sido añadido como titular\n password: " + Password, emailTemplate);
                        }
                        else
                        {
                            Session["Register_Result"] = "PackageId is invalid";
                        }
                    }
                }
                catch(Exception ex)
                {
                    Session["Register_Result"] = ex.Message;
                }               
            }

            return Redirect(Url.Action("Register", "suscribe", new { packageId = PackageId }));
        }

        [HttpPost]
        public ActionResult MakeCardPayment(long PackageId, string CardNumber,
            int ExpYear, int ExpMonth, string Cvc, int Amount)
        {
            try
            {
                var options = new TokenCreateOptions
                {
                    Card = new CreditCardOptions
                    {
                        Number = CardNumber,
                        ExpYear = ExpYear,
                        ExpMonth = ExpMonth,
                        Cvc = Cvc
                    }
                };
                var service = new TokenService();
                Token stripeToken = service.Create(options);
                StripeConfiguration.SetApiKey(ep.GetStripeSecretKey());
                var chargeOptions = new ChargeCreateOptions
                {
                    Amount = Convert.ToInt32(Amount * 100) + 20000,
                    Currency = "usd",
                    Description = "Charge for jenny.rosen@example.com",
                    SourceId = stripeToken.Id // obtained with Stripe.js,                    
                };
                var chargeService = new ChargeService();
                Charge charge = chargeService.Create(chargeOptions);
                Session["Register_Result"] = "success";
                Session["Sus_login"] = "ok";
                //Session["SUS_USER_ID"] = communuser.user_id;
                return Redirect(Url.Action("panel", "control", new { area = "coadmin" }));
            }
            catch(Exception ex)
            {
                return Redirect(Url.Action("MonthlyPayment", "suscribe", new { packageId = PackageId }));
            }   
        }

        [HttpPost]
        public ActionResult MakeAchPayment(long PackageId)
        {
            return Redirect(Url.Action("MonthlyPayment", "suscribe", new { packageId = PackageId }));
        }

        [HttpPost]
        public ActionResult SubmitLogin(string Email, string Password, long PackageId)
        {
            Session["Sus_Email"] = Email;
            Session["Sus_Pass"] = Password;
            user user1 = entities.users.Where(m => m.email.Equals(Email) && m.package_id == PackageId).FirstOrDefault();
            if (user1 == null)
            {
                Session["Login_Result"] = "No exist user";
            } else
            {
                if (user1.password.Equals(ep.Encrypt(Password)))
                {
                    if (user1.is_paid == true)
                    {
                        if (user1.paid_at != null)
                        {
                            DateTime paidAt = (DateTime)user1.paid_at;
                            if ((DateTime.Now - paidAt).Days > 30)
                            {
                                Session["Login_Result"] = "Suscribe expired";
                            }
                            else
                            {
                                Session["USER_ID"] = user1.id;
                                Session["PACK_ID"] = PackageId;
                                Session["USER_ROLE"] = 2;
                                Session["Active"] = user1.is_active;
                                Session["Login_Result"] = "success";
                            }                           
                        }
                        else
                        {
                            Session["Login_Result"] = "Suscribe expired";
                            return Redirect(Url.Action("MonthlyPayment", "suscribe", new { packageId=PackageId }));
                        }
                       
                    }
                    else
                    {
                        DateTime createAt = (DateTime)user1.acq_date;
                        if ((DateTime.Now- createAt).Days > 30)
                        {
                            Session["Login_Result"] = "Suscribe expired";
                        }
                        else
                        {
                            Session["PACK_ID"] = PackageId;
                            Session["Login_Result"] = "success";
                        }
                    }
                } else
                {
                    Session["Login_Result"] = "Password invalid";
                }
            }
            
            return Redirect(Url.Action("Index", "suscribe", new { packageId= PackageId }));
        }
    }
}