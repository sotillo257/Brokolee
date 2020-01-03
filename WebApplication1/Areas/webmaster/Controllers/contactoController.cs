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
    public class contactoController : Controller
    {
        pjrdev_condominiosEntities entities = new pjrdev_condominiosEntities();
        EFPublicRepository ep = new EFPublicRepository();
        // GET: webmaster/contacto
        public ActionResult informacion()
        {
            if (Session["USER_ID"] != null)
            {
                long userId = (long)Session["USER_ID"];
                user curUser = entities.users.Find(userId);
                List<ShowMessage> pubMessageList = ep.GetChatMessages(userId);
                List<contactinfo> contactinfoList = entities.contactinfoes.Where(m => m.user_id == userId).ToList();
                List<document_type> document_category_list = entities.document_type.ToList();
                contactoViewModel viewModel = new contactoViewModel();
                viewModel.side_menu = "contacto";
                viewModel.side_sub_menu = "contacto";
                viewModel.curUser = curUser;
                viewModel.document_category_list = document_category_list;
                if (contactinfoList.Count > 0)
                {
                    viewModel.editContactInfo = contactinfoList.First();
                }
                else
                {
                    viewModel.editContactInfo = null;
                }
                viewModel.pubTaskList = ep.GetNotifiTaskList(userId);
                viewModel.pubMessageList = pubMessageList;
                viewModel.messageCount = ep.GetUnreadMessageCount(pubMessageList);
                return View(viewModel);
            } else
            {
                return Redirect(ep.GetLogoutUrl());
            }
        }

        [HttpPost]
        public ActionResult editcontact(long contactID, string company_admin,
            string coordinator, string president, string vice_president,
            string treasurer, string secretary, string vocal1, string vocal2,
            string vocal3, string phy_address, string postal_address,
            string phone_number1, string phone_number2, string email
            )
        {
            try
            {
                long user_id = (long)Session["USER_ID"];
                if (contactID != 0)
                {
                    contactinfo contactinfo = entities.contactinfoes.Find(contactID);
                    contactinfo.company_admin = company_admin;
                    contactinfo.coordinator = coordinator;
                    contactinfo.president = president;
                    contactinfo.vice_president = vice_president;
                    contactinfo.treasurer = treasurer;
                    contactinfo.secretary = secretary;
                    contactinfo.vocal1 = vocal1;
                    contactinfo.vocal2 = vocal2;
                    contactinfo.vocal3 = vocal3;
                    contactinfo.phy_address = phy_address;
                    contactinfo.postal_address = postal_address;
                    contactinfo.phone_number1 = phone_number1;
                    contactinfo.phone_number2 = phone_number2;
                    contactinfo.email = email;
                    contactinfo.user_id = user_id;
                    entities.SaveChanges();

                }
                else
                {
                    contactinfo contactinfo = new contactinfo();
                    contactinfo.company_admin = company_admin;
                    contactinfo.coordinator = coordinator;
                    contactinfo.president = president;
                    contactinfo.vice_president = vice_president;
                    contactinfo.treasurer = treasurer;
                    contactinfo.secretary = secretary;
                    contactinfo.vocal1 = vocal1;
                    contactinfo.vocal2 = vocal2;
                    contactinfo.vocal3 = vocal3;
                    contactinfo.phy_address = phy_address;
                    contactinfo.postal_address = postal_address;
                    contactinfo.phone_number1 = phone_number1;
                    contactinfo.phone_number2 = phone_number2;
                    contactinfo.email = email;
                    contactinfo.user_id = user_id;
                    entities.contactinfoes.Add(contactinfo);
                    entities.SaveChanges();
                }
                return Redirect(Url.Action("informacion", "contacto", new { area = "coadmin" }));
            }
            catch (Exception ex)
            {
                return Redirect(Url.Action("informacion", "contacto", 
                    new {
                        area = "coadmin",
                        exception = ex.Message
                    }));
            }
        }
    }
}