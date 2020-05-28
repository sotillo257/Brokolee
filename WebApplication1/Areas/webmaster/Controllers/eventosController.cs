using System;
using System.Collections.Generic;
using System.Data.Entity.Validation;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WebApplication1.Areas.webmaster.ViewModels;
using WebApplication1.Concrete;
using WebApplication1.Models;

namespace WebApplication1.Areas.webmaster.Controllers
{
    public class eventosController : Controller
    {
        pjrdev_condominiosEntities entities = new pjrdev_condominiosEntities();
        EFPublicRepository ep = new EFPublicRepository();
        // GET: webmaster/eventos
        public ActionResult registrados(string searchStr = "")
        {
            if (Session["USER_ID"] != null)
            {
                long userId = (long)Session["USER_ID"];
                user curUser = entities.users.Find(userId);
                List<ShowMessage> pubMessageList = ep.GetChatMessages(userId);
                List<@event> eventList = new List<@event>();

                long communityAct = Convert.ToInt64(Session["CURRENT_COMU"]);

                if (searchStr == "")
                {
                    var query = (from r in entities.events where r.community_id == communityAct select r);
                    eventList = query.ToList();
                }
                else
                {
                    var query = (from r in entities.events
                                 where r.name.Contains(searchStr) == true && r.community_id == communityAct
                                 select r);
                    eventList = query.ToList();
                }
                eventosViewModel viewModel = new eventosViewModel();
                viewModel.side_menu = "eventos";
                viewModel.side_sub_menu = "eventos_registrados";
                
                viewModel.eventList = eventList;
                viewModel.curUser = curUser;
                viewModel.searchStr = searchStr;
                viewModel.pubTaskList = ep.GetNotifiTaskList(userId);
                viewModel.pubMessageList = pubMessageList;
                viewModel.messageCount = ep.GetUnreadMessageCount(pubMessageList);
                return View(viewModel);
            } else
            {
                return Redirect(ep.GetLogoutUrl());
            }
        }

        public ActionResult otros(string searchStr = "")
        {
            if (Session["USER_ID"] != null)
            {
                long userId = (long)Session["USER_ID"];
                user curUser = entities.users.Find(userId);
                List<document_type> document_category_list = entities.document_type.ToList();
                eventosViewModel viewModel = new eventosViewModel();
                viewModel.side_menu = "eventos";
                viewModel.side_sub_menu = "eventos_otros";
                viewModel.curUser = curUser;
                viewModel.document_category_list = document_category_list;
                viewModel.pubTaskList = ep.GetNotifiTaskList(userId);
                viewModel.searchStr = searchStr;
                viewModel.pubMessageList = ep.GetChatMessages(userId);
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
                List<document_type> document_category_list = entities.document_type.ToList();
                eventosViewModel viewModel = new eventosViewModel();
                viewModel.side_menu = "eventos";
                viewModel.side_sub_menu = "eventos_agregar";
                viewModel.curUser = curUser;
                viewModel.document_category_list = document_category_list;
                viewModel.pubTaskList = ep.GetNotifiTaskList(userId);
                viewModel.pubMessageList = ep.GetChatMessages(userId);
                return View(viewModel);
            } else
            {
                return Redirect(ep.GetLogoutUrl());
            }
        }

        public ActionResult editar(long? editID)
        {
            if (Session["USER_ID"] != null)
            {
                if (editID != null)
                {
                    long userId = (long)Session["USER_ID"];
                    user curUser = entities.users.Find(userId);
                    editEventViewModel viewModel = new editEventViewModel();
                    @event editEvent = entities.events.Find(editID);
                    viewModel.side_menu = "eventos";
                    viewModel.side_sub_menu = "eventos_editar";
                    viewModel.editEvent = editEvent;
                    viewModel.curUser = curUser;
                    
                    viewModel.pubTaskList = ep.GetNotifiTaskList(userId);
                    viewModel.pubMessageList = ep.GetChatMessages(userId);
                    return View(viewModel);
                }
                else
                {
                    return Redirect(Url.Action("NotFound", "Error"));
                }
            }
            else
            {
                return Redirect(ep.GetLogoutUrl());
            }
        }

        public ActionResult inactivos(string searchStr = "")
        {
            if (Session["USER_ID"] != null)
            {
                long userId = (long)Session["USER_ID"];
                user curUser = entities.users.Find(userId);
                List<@event> eventList = new List<@event>();

                long communityAct = Convert.ToInt64(Session["CURRENT_COMU"]);

                if (searchStr == "")
                {
                    var query = (from r in entities.events where r.community_id == communityAct select r);
                    eventList = query.ToList();
                }
                else
                {
                    var query = (from r in entities.events where r.name.Contains(searchStr) == true && r.community_id == communityAct select r);
                    eventList = query.ToList();
                }

                List<document_type> document_category_list = entities.document_type.ToList();
                inactivosEventosViewModel viewModel = new inactivosEventosViewModel();
                viewModel.side_menu = "eventos";
                viewModel.side_sub_menu = "eventos_inactivos";
                viewModel.document_category_list = document_category_list;
                viewModel.eventList = eventList;
                viewModel.curUser = curUser;
                viewModel.searchStr = searchStr;
                viewModel.pubTaskList = ep.GetNotifiTaskList(userId);
                viewModel.pubMessageList = ep.GetChatMessages(userId);
                return View(viewModel);
            } else
            {
                return Redirect(ep.GetLogoutUrl());
            }
        }
        public ActionResult registrado(long? id)
        {
            if (Session["USER_ID"] != null)
            {
                if (id != null)
                {
                    long userId = (long)Session["USER_ID"];
                    user curUser = entities.users.Find(userId);
                    @event event_item = entities.events.Find(id);
                    registradoViewModel viewmodel = new registradoViewModel();
                    viewmodel.side_menu = "eventos";
                    viewmodel.side_sub_menu = "eventos";
                    viewmodel.event_name = event_item.name;
                    viewmodel.event_date = Convert.ToDateTime(event_item.created_at).ToString("dd/MM/yyyy");
                    viewmodel.event_time = Convert.ToDateTime(event_item.created_at).ToString("hh:mm tt");
                    viewmodel.place = event_item.place;
                    viewmodel.description = event_item.description;
                    viewmodel.note = event_item.note;
                    
                    viewmodel.curUser = curUser;
                    viewmodel.pubTaskList = ep.GetNotifiTaskList(userId);
                    viewmodel.pubMessageList = ep.GetChatMessages(userId);
                    return View(viewmodel);
                }
                else
                {
                    return Redirect(Url.Action("NotFound", "Error"));
                }               
            }
            else
            {
                return Redirect(ep.GetLogoutUrl());
            }
        }

        public JsonResult GetEvents(string searchStr = "")
        {
            List<CalEventView> listCalEventList = new List<CalEventView>();
            try
            {
                using (pjrdev_condominiosEntities dc = new pjrdev_condominiosEntities())
                {
                    List<@event> eventList = new List<@event>();

                    long communityAct = Convert.ToInt64(Session["CURRENT_COMU"]);

                    if (searchStr == "")
                    {
                        var query = (from r in entities.events where r.community_id == communityAct select r);
                        eventList = query.ToList();
                    }
                    else
                    {
                        var query = (from r in entities.events
                                     where r.name.Contains(searchStr) == true && r.community_id == communityAct
                                     select r);
                        eventList = query.ToList();
                    }

                    foreach (var item in eventList)
                    {
                        CalEventView ii = new CalEventView();
                        ii.id = item.id;
                        ii.title = item.name;
                        DateTime datetime = (DateTime)item.event_date;
                        ii.year = datetime.Year;
                        ii.month = datetime.Month;
                        ii.day = datetime.Day;
                        ii.hour = datetime.Hour;
                        ii.minute = datetime.Minute;
                        listCalEventList.Add(ii);
                    }
                    return new JsonResult { Data = listCalEventList, JsonRequestBehavior = JsonRequestBehavior.AllowGet };
                }
            } catch(Exception)
            {
                return new JsonResult { Data = listCalEventList, JsonRequestBehavior = JsonRequestBehavior.AllowGet };
            } 
        }

        public JsonResult DeleteEvent(long delID)
        {
            try
            {
                @event delEvent = entities.events.Find(delID);
                entities.events.Remove(delEvent);
                entities.SaveChanges();
                return Json(new { result = "success" });
            }
            catch (Exception ex)
            {
                return Json(new { result = "error", exception = ex.HResult });
            }
        }

        [HttpPost]
        public ActionResult editevent(long editID, string name, string event_date,
            string event_time, string place, string description, string note, int share)
        {
            try
            {
                @event editevent = entities.events.Find(editID);
                editevent.name = name;
                editevent.place = place;
                editevent.event_date = DateTime.ParseExact(event_date + " " + event_time, "yyyy-MM-dd HH:mm",
                    System.Globalization.CultureInfo.CurrentCulture);
                editevent.description = description;
                editevent.note = note;
                editevent.share = share;
                editevent.community_id = Convert.ToInt64(Session["CURRENT_COMU"]);
                entities.SaveChanges();
                return Redirect(Url.Action("registrados", "eventos", new { area = "webmaster" }));
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
                    }
                }
                return Redirect(Url.Action("editar", "eventos", new { area = "webmaster", editID = editID }));
            }
        }

        [HttpPost]
        public ActionResult newevent(string name, string event_date,
            string event_time, string place, string description, string note)
        {
            try
            {
                @event newEvent = new @event();
                newEvent.name = name;
                newEvent.event_date = DateTime.ParseExact(event_date + " " + event_time, "yyyy-MM-dd HH:mm",
                    System.Globalization.CultureInfo.CurrentCulture);
                newEvent.place = place;
                newEvent.description = description;
                newEvent.note = note;
                newEvent.share = 1;
                newEvent.community_id = Convert.ToInt64(Session["CURRENT_COMU"]);
                entities.events.Add(newEvent);
                entities.SaveChanges();
                return Redirect(Url.Action("registrados", "eventos", new { area = "webmaster" }));
            }
            catch (Exception ex)
            {
                return Redirect(Url.Action("agregar", "eventos", 
                    new {
                        area = "webmaster",
                        exception = ex.Message
                    }));
            }
        }

        [HttpPost]
        public ActionResult SeacrhResult(string searchStr)
        {
            return Redirect(Url.Action("registrados", "eventos",
               new
               {
                   area = "webmaster",
                   searchStr = searchStr
               }));
        }

        [HttpPost]
        public ActionResult SeacrhInResult(string searchStr)
        {
            return Redirect(Url.Action("inactivos", "eventos",
                new
                {
                    area = "webmaster",
                    searchStr = searchStr
                }));
        }

        [HttpPost]
        public ActionResult SeacrhOtResult(string searchStr)
        {
            return Redirect(Url.Action("otros", "coadmin",
                new
                {
                    area = "webmaster",
                    searchStr = searchStr
                }));
        }

        [HttpPost]
        public ActionResult SeacrhOtResult1(string searchStr)
        {
            return Redirect(Url.Action("registrados", "eventos",
                new
                {
                    area = "webmaster",
                    searchStr = searchStr
                }));
        }
    }
}