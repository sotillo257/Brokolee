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
        public ActionResult disponibles(string Error, string searchString = "")
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
                    var query = (from r in entities.efacs where r.first_name.Contains(searchString) == true && r.community_id == communityAct && r.status == 1 select r);
                    facilitieList = query.ToList();                                       
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
                    ViewBag.msgError = Error;
                    return View(viewModel);
                }
                catch(Exception ex)
                {
                    return Redirect(Url.Action("error", "control", new { Error = "Facilidades disponibles: ", ex.Message }));
                }
            } else
            {
                return Redirect(ep.GetLogoutUrl());
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
                    long userId = (long)Session["USER_ID"];                   
                    user curUser = entities.users.Find(userId);
                    List<ShowMessage> pubMessageList = ep.GetChatMessages(userId);
                    long communityAct = Convert.ToInt64(Session["CURRENT_COMU"]);
                    List<book> reservasList = new List<book>();
                    var query = (from r in entities.books where r.community_id == communityAct select r);
                    reservasList = query.ToList();                                       
                    reservasViewModel viewModel = new reservasViewModel();
                    titulosList = ep.GetTitulosByTitular(userId);
                    listComunities = ep.GetCommunityListByTitular(titulosList);
                    viewModel.communityList = listComunities;
                    viewModel.side_menu = "facilidades";
                    viewModel.side_sub_menu = "facilidades_reservas";
                    viewModel.document_category_list = entities.document_type.ToList();
                    viewModel.bookList = reservasList;
                    viewModel.curUser = curUser;
                    viewModel.pubTaskList = ep.GetNotifiTaskList(userId);
                    viewModel.pubMessageList = pubMessageList;
                    viewModel.messageCount = ep.GetUnreadMessageCount(pubMessageList);                    
                    return View(viewModel);
                }
                catch(Exception ex)
                {
                    return Redirect(Url.Action("error", "control", new { Error = "Reservas, facilidades: ", ex.Message }));
                }
            } else
            {
                return Redirect(ep.GetLogoutUrl());
            } 
        }

        public ActionResult reservar(long? id)
        {
            if (Session["USER_ID"] != null)
            {
                if (Session["CURRENT_COMU"] != null)
                {
                    if (id != null)
                    {
                        efac facilidad = entities.efacs.Find(id);
                        if (facilidad !=null)
                        {
                            try{
                                long userId = (long)Session["USER_ID"];                               
                                user curUser = entities.users.Find(userId);
                                List<ShowMessage> pubMessageList = ep.GetChatMessages(userId);
                                facilidadesViewModel viewModel = new facilidadesViewModel();
                                string inicio = Convert.ToString(facilidad.start_time);
                                string final = Convert.ToString(facilidad.end_time);
                                TimeSpan horaInicio = TimeSpan.Parse(inicio);
                                TimeSpan horaFin = TimeSpan.Parse(final);
                                TimeSpan masU = TimeSpan.FromHours(1);
                                List<string> timeList = new List<string>();
                                while (horaInicio < horaFin)
                                {
                                    timeList.Add(
                                        horaInicio.ToString("hh':'mm") + " - " +
                                        horaInicio.Add(masU).ToString("hh':'mm"));

                                    horaInicio = horaInicio.Add(masU);
                                }
                                titulosList = ep.GetTitulosByTitular(userId);
                                listComunities = ep.GetCommunityListByTitular(titulosList);
                                viewModel.communityList = listComunities;
                                viewModel.timeList = timeList;
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
                            catch (Exception ex)
                            {
                                return Redirect(Url.Action("error", "control", new { Error = "Reservar facilidad: ", ex.Message  }));
                            }
                        }
                        else
                        {
                            return Redirect(Url.Action("disponibles", "facilidades", new { Error = "No existe la facilidad" }));
                        }
                    }
                    else
                    {
                        return Redirect(Url.Action("disponibles", "facilidades"));
                    }
                }
                else
                {
                    return Redirect(Url.Action("disponibles", "facilidades", new { Error = "No permitido. Usted no pertence a ninguna comunidad" }));
                }               
            } else
            {
                return Redirect(ep.GetLogoutUrl());
            }
        }

        [HttpPost]
        public ActionResult reservarFacilidad(long idFacilidad, string dateFaci,
       string fechaDisponible)
        {
            try
            {
                TimeSpan masU = TimeSpan.FromHours(1);
                efac facility = entities.efacs.Find(idFacilidad);
                book newBook = new book();
                newBook.first_name = facility.first_name;
                int horaSelectd = Convert.ToInt32(fechaDisponible);
                TimeSpan start_time = TimeSpan.FromHours(horaSelectd);
                TimeSpan end_time = TimeSpan.FromHours(horaSelectd);
                newBook.start_time = start_time;
                newBook.end_time = end_time.Add(masU);
                newBook.status = 1;
                newBook.requested_date = DateTime.ParseExact(dateFaci, "yyyy-MM-dd", System.Globalization.CultureInfo.InvariantCulture);
                newBook.description = facility.description;
                newBook.cost_per_reservation = facility.cost_reservation;
                newBook.id_efac = idFacilidad;
                newBook.created_at = DateTime.Now;
                newBook.community_id = facility.community_id;
                entities.books.Add(newBook);
                entities.SaveChanges();
                return Redirect(Url.Action("reservas", "facilidades"));
            }
            catch (Exception ex)
            {
                return Redirect(Url.Action("reservar", "facilidades", new { Error = "Problema interno " + ex.Message }));
            }
        }

        //public ActionResult reservartwo(long id)
        //{
        //    if (Session["USER_ID"] != null)
        //    {
        //        try
        //        {
        //            long userId = 0;
        //            if (Convert.ToInt32(Session["USER_ROLE"]) == 1)
        //            {
        //                userId = (long)Session["USER_ID"];
        //            }
        //            else if (Convert.ToInt32(Session["USER_ROLE"]) > 1
        //            && Session["ACC_USER_ID"] != null)
        //            {
        //                userId = (long)Session["ACC_USER_ID"];
        //            }

        //            user curUser = entities.users.Find(userId);
        //            List<ShowMessage> pubMessageList = ep.GetChatMessages(userId);
        //            facilidadesViewModel viewModel = new facilidadesViewModel();

        //            titulosList = ep.GetTitulosByTitular(userId);
        //            listComunities = ep.GetCommunityListByTitular(titulosList);
        //            viewModel.communityList = listComunities;

        //            viewModel.side_menu = "reservartwo";
        //            viewModel.side_sub_menu = "facilidades_reservartwo";
        //            viewModel.curUser = curUser;
        //            viewModel.document_category_list = entities.document_type.ToList();
        //            viewModel.pubTaskList = ep.GetNotifiTaskList(userId);
        //            viewModel.pubMessageList = pubMessageList;
        //            viewModel.messageCount = ep.GetUnreadMessageCount(pubMessageList);                   
        //            return View(viewModel);
        //        }
        //        catch(Exception ex)
        //        {
        //            return Redirect(Url.Action("Index", "Error"));
        //        }
        //    }
        //    else
        //    {
        //        return Redirect(Url.Action("iniciar", "iniciar"));
        //    }

        //}        
    }
}