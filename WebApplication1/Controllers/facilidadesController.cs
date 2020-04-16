using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WebApplication1.Concrete;
using WebApplication1.Models;
using WebApplication1.ViewModels;

namespace WebApplication1.Controllers
{
    public class facilidadesController : Controller
    {
        pjrdev_condominiosEntities entities = new pjrdev_condominiosEntities();
        EFPublicRepository ep = new EFPublicRepository();

        List<Titulo> titulosList = new List<Titulo>();
        List<community> listComunities = new List<community>();

        // GET: facilidades
        public ActionResult disponibles(string searchString = "")
        {
            if (Session["USER_ID"] != null)
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

                    long communityAct = Convert.ToInt64(Session["CURRENT_COMU"]);

                    var query = (from r in entities.efacs where r.first_name.Contains(searchString) == true && r.community_id == communityAct select r);
                    List<efac> facilitieList = query.ToList();
                    facilidadesViewModel viewModel = new facilidadesViewModel();

                    titulosList = ep.GetTitulosByTitular(userId);
                    listComunities = ep.GetCommunityListByTitular(titulosList);
                    viewModel.communityList = listComunities;
                    viewModel.communityID1 = Convert.ToInt64(Session["CURRENT_COMU"]);
                    viewModel.side_menu = "facilidades";
                    viewModel.side_sub_menu = "facilidades_disponibles";
                    viewModel.facilitieList = facilitieList;
                    viewModel.document_category_list = entities.document_type.ToList();
                    viewModel.curUser = curUser;
                    viewModel.pubTaskList = ep.GetNotifiTaskList(userId);
                    viewModel.pubMessageList = pubMessageList;
                    viewModel.messageCount = ep.GetUnreadMessageCount(pubMessageList);                   
                    return View(viewModel);
                }
                catch(Exception ex)
                {
                    return Redirect(Url.Action("Index", "Error"));
                }
            } else
            {
                return Redirect(Url.Action("iniciar", "iniciar"));
            }
        }

        private string GetTimeStr(TimeSpan timespan)
        {
            DateTime time = DateTime.Today.Add(timespan);
            string displayTime = time.ToString("hh:mm tt");
            return displayTime;
        }

        public ActionResult reservas()
        {
            if (Session["USER_ID"] != null)
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

                    long communityAct = Convert.ToInt64(Session["CURRENT_COMU"]);

                    var query = (from r in entities.efacs where r.community_id == communityAct select r);
                    List<efac> efacList = query.ToList();
                    reservasViewModel viewModel = new reservasViewModel();

                    titulosList = ep.GetTitulosByTitular(userId);
                    listComunities = ep.GetCommunityListByTitular(titulosList);
                    viewModel.communityList = listComunities;
                    viewModel.communityID1 = Convert.ToInt64(Session["CURRENT_COMU"]);
                    viewModel.side_menu = "facilidades";
                    viewModel.side_sub_menu = "facilidades_reservas";
                    viewModel.document_category_list = entities.document_type.ToList();
                    viewModel.efacList = efacList;
                    viewModel.curUser = curUser;
                    viewModel.pubTaskList = ep.GetNotifiTaskList(userId);
                    viewModel.pubMessageList = pubMessageList;
                    viewModel.messageCount = ep.GetUnreadMessageCount(pubMessageList);
                    
                    return View(viewModel);
                }
                catch(Exception ex)
                {
                    return Redirect(Url.Action("Index", "Error"));
                }
            } else
            {
                return Redirect(Url.Action("iniciar", "iniciar"));
            } 
        }

        public ActionResult reservar()
        {
            if (Session["USER_ID"] != null)
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
                    facilidadesViewModel viewModel = new facilidadesViewModel();

                    titulosList = ep.GetTitulosByTitular(userId);
                    listComunities = ep.GetCommunityListByTitular(titulosList);
                    viewModel.communityList = listComunities;
                    viewModel.communityID1 = Convert.ToInt64(Session["CURRENT_COMU"]);
                    viewModel.side_menu = "reservar";
                    viewModel.side_sub_menu = "facilidades_reservar";
                    viewModel.curUser = curUser;
                    viewModel.document_category_list = entities.document_type.ToList();
                    viewModel.pubTaskList = ep.GetNotifiTaskList(userId);
                    viewModel.pubMessageList = pubMessageList;
                    viewModel.messageCount = ep.GetUnreadMessageCount(pubMessageList);
                    viewModel.communityName = ep.GetCommunityCoInfo(userId)[0];
                    viewModel.communityApart = ep.GetCommunityCoInfo(userId)[1];
                    return View(viewModel);
                }
                catch(Exception ex)
                {
                    return Redirect(Url.Action("Index", "Error"));
                }
            } else
            {
                return Redirect(Url.Action("iniciar", "iniciar"));
            }
        }

        public ActionResult reservartwo(long id)
        {
            if (Session["USER_ID"] != null)
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
                    facilidadesViewModel viewModel = new facilidadesViewModel();

                    titulosList = ep.GetTitulosByTitular(userId);
                    listComunities = ep.GetCommunityListByTitular(titulosList);
                    viewModel.communityList = listComunities;
                    viewModel.communityID1 = Convert.ToInt64(Session["CURRENT_COMU"]);
                    viewModel.side_menu = "reservartwo";
                    viewModel.side_sub_menu = "facilidades_reservartwo";
                    viewModel.curUser = curUser;
                    viewModel.document_category_list = entities.document_type.ToList();
                    viewModel.pubTaskList = ep.GetNotifiTaskList(userId);
                    viewModel.pubMessageList = pubMessageList;
                    viewModel.messageCount = ep.GetUnreadMessageCount(pubMessageList);                   
                    return View(viewModel);
                }
                catch(Exception ex)
                {
                    return Redirect(Url.Action("Index", "Error"));
                }
            }
            else
            {
                return Redirect(Url.Action("iniciar", "iniciar"));
            }
                
        }

        [HttpPost]
        public ActionResult addbook(string first_name, string description,
       string start_time, string end_time, string requested_date,
       string cost_per_reserve)
        {
            try
            {
                book newBook = new book();
                newBook.first_name = first_name;
                newBook.start_time = TimeSpan.ParseExact(start_time, "h\\:mm",
                    System.Globalization.CultureInfo.CurrentCulture, TimeSpanStyles.None);
                newBook.end_time = TimeSpan.ParseExact(end_time, "h\\:mm",
                    System.Globalization.CultureInfo.CurrentCulture, TimeSpanStyles.None);
                newBook.requested_date = DateTime.ParseExact(requested_date, "yyyy-MM-dd",
                                                    System.Globalization.CultureInfo.InvariantCulture);
                newBook.description = description;
                newBook.cost_per_reservation = Convert.ToDecimal(cost_per_reserve);
                newBook.created_at = DateTime.Now;                
                entities.books.Add(newBook);
                entities.SaveChanges();
                return Redirect(Url.Action("reservar", "facilidades"));
            }
            catch (Exception ex)
            {
                return Redirect(Url.Action("reservartwo", "facilidades",
                    new {
                        exception = ex.Message
                    }));
            }
        }
    }
}