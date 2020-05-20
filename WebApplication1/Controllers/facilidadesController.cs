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
                    long userId = (long)Session["USER_ID"];                   
                    user curUser = entities.users.Find(userId);
                    List<ShowMessage> pubMessageList = ep.GetChatMessages(userId);

                    long communityAct = Convert.ToInt64(Session["CURRENT_COMU"]);
                    List<efac> facilitieList = new List<efac>();

                    if (Session["CURRENT_COMU"] != null)
                    {
                        var query = (from r in entities.efacs where r.first_name.Contains(searchString) == true && r.community_id == communityAct select r);
                        facilitieList = query.ToList();
                    }
                    else
                    {
                        facilitieList.Clear();
                    }
                   
                    facilidadesViewModel viewModel = new facilidadesViewModel();

                    titulosList = ep.GetTitulosByTitular(userId);
                    listComunities = ep.GetCommunityListByTitular(titulosList);
                    viewModel.communityList = listComunities;
       
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
                    List<efac> efacList = new List<efac>();

                    if (Session["CURRENT_COMU"] != null)
                    {
                        var query = (from r in entities.efacs where r.community_id == communityAct select r);
                        efacList = query.ToList();
                    }
                    else
                    {
                        efacList.Clear();
                    }
                   
                    reservasViewModel viewModel = new reservasViewModel();

                    titulosList = ep.GetTitulosByTitular(userId);
                    listComunities = ep.GetCommunityListByTitular(titulosList);
                    viewModel.communityList = listComunities;

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

        public ActionResult reservar(long id)
        {
            if (Session["USER_ID"] != null)
            {
                try
                {
                    long userId = (long)Session["USER_ID"];
                    efac facilidad = entities.efacs.Find(id);
                    user curUser = entities.users.Find(userId);
                    List<ShowMessage> pubMessageList = ep.GetChatMessages(userId);
                    facilidadesViewModel viewModel = new facilidadesViewModel();

                    DateTime startT = Convert.ToDateTime(facilidad.start_time);
                    DateTime endT = Convert.ToDateTime(facilidad.end_time);
                    TimeSpan intervalo = startT - endT;
                    int dias = intervalo.Hours;


                    titulosList = ep.GetTitulosByTitular(userId);
                    listComunities = ep.GetCommunityListByTitular(titulosList);
                    viewModel.communityList = listComunities;

                    viewModel.facilidadSe = facilidad;
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
        public ActionResult reservarFacilidad(long idFacilidad, string dateFaci,
       string fedis)
        {
            try
            {
                efac facility = entities.efacs.Find(idFacilidad);
                book newBook = new book();
                newBook.first_name = facility.first_name;
                //newBook.start_time = dateFaci; 
                //newBook.end_time = dateFaci;
                //newBook.status = 1;
                newBook.requested_date = DateTime.ParseExact(dateFaci, "yyyy-MM-dd",System.Globalization.CultureInfo.InvariantCulture);
                newBook.description = facility.description;
                newBook.cost_per_reservation = facility.cost_reservation;
                newBook.created_at = DateTime.Now;
                newBook.community_id = facility.community_id;
                entities.books.Add(newBook);
                entities.SaveChanges();
                return Redirect(Url.Action("reservas", "facilidades"));
            }
            catch (Exception ex)
            {
                return Redirect(Url.Action("reservar", "facilidades",new { Error = "Problema interno " + ex.Message }));
            }
        }
    }
}