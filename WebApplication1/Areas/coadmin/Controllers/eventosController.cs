using System;
using System.Collections.Generic;
using System.Data.Entity.Validation;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WebApplication1.Areas.coadmin.ViewModels;
using WebApplication1.Concrete;
using WebApplication1.Models;

namespace WebApplication1.Areas.coadmin.Controllers
{
    public class eventosController : Controller
    {
        pjrdev_condominiosEntities entities = new pjrdev_condominiosEntities();
        EFPublicRepository ep = new EFPublicRepository();
        // GET: coadmin/eventos
        public ActionResult registrados(string searchStr = "")
        {
            if (Session["USER_ID"] != null)
            {
                try
                {
                    long userId = 0;
                    if (Convert.ToInt32(Session["USER_ROLE"]) == 2)
                    {
                        userId = (long)Session["USER_ID"];
                    }
                    else if (Convert.ToInt32(Session["USER_ROLE"]) > 2
                    && Session["ACC_USER_ID"] != null)
                    {
                        userId = (long)Session["ACC_USER_ID"];
                    }
                    user curUser = entities.users.Find(userId);
                    List<ShowMessage> pubMessageList = ep.GetChatMessages(userId);
                    List<@event> eventList = new List<@event>();

                    if (searchStr == "")
                    {
                        var query = (from r in entities.events select r);
                        eventList = query.ToList();
                    }
                    else
                    {
                        var query = (from r in entities.events
                                     where r.name.Contains(searchStr) == true
                                     select r);
                        eventList = query.ToList();
                    }

                    eventosViewModel viewModel = new eventosViewModel();
                    viewModel.side_menu = "event_calendar";
                    viewModel.side_sub_menu = "event_calendar_recorded";
                    viewModel.document_category_list = entities.document_type.ToList();
                    viewModel.eventList = eventList;
                    viewModel.curUser = curUser;
                    viewModel.searchStr = searchStr;
                    viewModel.pubTaskList = ep.GetNotifiTaskList(userId);
                    viewModel.pubMessageList = pubMessageList;
                    viewModel.messageCount = ep.GetUnreadMessageCount(pubMessageList);
                    viewModel.communityName = ep.GetCommunityInfo(userId)[0];
                    viewModel.communityApart = ep.GetCommunityInfo(userId)[1];
                    return View(viewModel);
                }
                catch(Exception ex)
                {
                    return Redirect(Url.Action("Index", "Error"));
                }
            } else
            {
                return Redirect(ep.GetLogoutUrl());
            }

        }

        public ActionResult otros(string searchStr = "")
        {
            if (Session["USER_ID"] != null)
            {
                try
                {
                    long userId = 0;
                    if (Convert.ToInt32(Session["USER_ROLE"]) == 2)
                    {
                        userId = (long)Session["USER_ID"];
                    }
                    else if (Convert.ToInt32(Session["USER_ROLE"]) > 2
                    && Session["ACC_USER_ID"] != null)
                    {
                        userId = (long)Session["ACC_USER_ID"];
                    }
                    user curUser = entities.users.Find(userId);
                    List<ShowMessage> pubMessageList = ep.GetChatMessages(userId);
                    List<document_type> document_category_list = entities.document_type.ToList();
                    eventosViewModel viewModel = new eventosViewModel();
                    viewModel.side_menu = "event_calendar";
                    viewModel.side_sub_menu = "event_calendar_otros";
                    viewModel.curUser = curUser;
                    viewModel.document_category_list = document_category_list;
                    viewModel.pubTaskList = ep.GetNotifiTaskList(userId);
                    viewModel.searchStr = searchStr;
                    viewModel.pubMessageList = pubMessageList;
                    viewModel.messageCount = ep.GetUnreadMessageCount(pubMessageList);
                    viewModel.communityName = ep.GetCommunityInfo(userId)[0];
                    viewModel.communityApart = ep.GetCommunityInfo(userId)[1];
                    return View(viewModel);
                }
                catch(Exception ex)
                {
                    return Redirect(Url.Action("Index", "Error"));
                }                
            } else
            {
                return Redirect(ep.GetLogoutUrl());
            }

        }

        public ActionResult agregar()
        {
            if (Session["USER_ID"] != null)
            {
                try
                {
                    long userId = 0;
                    if (Convert.ToInt32(Session["USER_ROLE"]) == 2)
                    {
                        userId = (long)Session["USER_ID"];
                    }
                    else if (Convert.ToInt32(Session["USER_ROLE"]) > 2
                    && Session["ACC_USER_ID"] != null)
                    {
                        userId = (long)Session["ACC_USER_ID"];
                    }

                    user curUser = entities.users.Find(userId);
                    List<ShowMessage> pubMessageList = ep.GetChatMessages(userId);
                    List<document_type> document_category_list = entities.document_type.ToList();
                    eventosViewModel viewModel = new eventosViewModel();
                    viewModel.side_menu = "event_calendar";
                    viewModel.side_sub_menu = "event_calendar_agregar";
                    viewModel.curUser = curUser;
                    viewModel.document_category_list = document_category_list;
                    viewModel.pubTaskList = ep.GetNotifiTaskList(userId);
                    viewModel.pubMessageList = pubMessageList;
                    viewModel.messageCount = ep.GetUnreadMessageCount(pubMessageList);
                    viewModel.communityName = ep.GetCommunityInfo(userId)[0];
                    viewModel.communityApart = ep.GetCommunityInfo(userId)[1];
                    return View(viewModel);
                }
                catch(Exception ex)
                {
                    return Redirect(Url.Action("Index", "Error"));
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
                try
                {
                    long userId = 0;
                    if (Convert.ToInt32(Session["USER_ROLE"]) == 2)
                    {
                        userId = (long)Session["USER_ID"];
                    }
                    else if (Convert.ToInt32(Session["USER_ROLE"]) > 2
                    && Session["ACC_USER_ID"] != null)
                    {
                        userId = (long)Session["ACC_USER_ID"];
                    }
                    user curUser = entities.users.Find(userId);
                    List<ShowMessage> pubMessageList = ep.GetChatMessages(userId);
                    List<@event> eventList = new List<@event>();

                    if (searchStr == "")
                    {
                        var query = (from r in entities.events where r.is_active == false select r);
                        eventList = query.OrderBy(m => m.created_at).ToList();
                    }
                    else
                    {
                        var query = (from r in entities.events
                                     where r.name.Contains(searchStr) == true
                                    && r.is_active == false
                                     select r);
                        eventList = query.OrderBy(m => m.created_at).ToList();
                    }

                    List<document_type> document_category_list = entities.document_type.ToList();
                    eventosViewModel viewModel = new eventosViewModel();
                    viewModel.side_menu = "event_calendar";
                    viewModel.side_sub_menu = "event_calendar_inactivos";
                    viewModel.document_category_list = document_category_list;
                    viewModel.eventList = eventList;
                    viewModel.curUser = curUser;
                    viewModel.searchStr = searchStr;
                    viewModel.pubTaskList = ep.GetNotifiTaskList(userId);
                    viewModel.pubMessageList = pubMessageList;
                    viewModel.messageCount = ep.GetUnreadMessageCount(pubMessageList);
                    viewModel.communityName = ep.GetCommunityInfo(userId)[0];
                    viewModel.communityApart = ep.GetCommunityInfo(userId)[1];
                    return View(viewModel);
                }
                catch(Exception ex)
                {
                    return Redirect(Url.Action("Index", "Error"));
                }               
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
                    try
                    {
                        long userId = 0;
                        if (Convert.ToInt32(Session["USER_ROLE"]) == 2)
                        {
                            userId = (long)Session["USER_ID"];
                        }
                        else if (Convert.ToInt32(Session["USER_ROLE"]) > 2
                        && Session["ACC_USER_ID"] != null)
                        {
                            userId = (long)Session["ACC_USER_ID"];
                        }
                        user curUser = entities.users.Find(userId);
                        List<ShowMessage> pubMessageList = ep.GetChatMessages(userId);
                        editEventViewModel viewModel = new editEventViewModel();
                        @event editEvent = entities.events.Find(editID);
                        viewModel.side_menu = "event_calendar";
                        viewModel.side_sub_menu = "event_calendar_editar";
                        viewModel.editEvent = editEvent;
                        viewModel.curUser = curUser;
                        viewModel.document_category_list = entities.document_type.ToList();
                        viewModel.pubTaskList = ep.GetNotifiTaskList(userId);
                        viewModel.pubMessageList = pubMessageList;
                        viewModel.messageCount = ep.GetUnreadMessageCount(pubMessageList);
                        viewModel.communityName = ep.GetCommunityInfo(userId)[0];
                        viewModel.communityApart = ep.GetCommunityInfo(userId)[1];
                        return View(viewModel);
                    }
                    catch(Exception ex)
                    {
                        return Redirect(Url.Action("Index", "Error"));
                    }
                                       
                }
                else
                {
                    return Redirect(Url.Action("NotFound", "Error"));
                }                
            } else
            {
                return Redirect(ep.GetLogoutUrl());
            }
        }

        public ActionResult registrado(long? id, string searchStr = "")
        {
            if (Session["USER_ID"] != null)
            {
                if(id != null)
                {
                    try
                    {
                        long userId = 0;
                        if (Convert.ToInt32(Session["USER_ROLE"]) == 2)
                        {
                            userId = (long)Session["USER_ID"];
                        }
                        else if (Convert.ToInt32(Session["USER_ROLE"]) > 2
                        && Session["ACC_USER_ID"] != null)
                        {
                            userId = (long)Session["ACC_USER_ID"];
                        }
                        user curUser = entities.users.Find(userId);
                        List<ShowMessage> pubMessageList = ep.GetChatMessages(userId);
                        @event event_item = entities.events.Find(id);
                        registradoViewModel viewmodel = new registradoViewModel();
                        viewmodel.side_menu = "registrado";
                        viewmodel.event_name = event_item.name;
                        viewmodel.event_date = Convert.ToDateTime(event_item.event_date).ToString("dd/MM/yyyy");
                        viewmodel.event_time = Convert.ToDateTime(event_item.event_date).ToString("hh:mm tt");
                        viewmodel.place = event_item.place;
                        viewmodel.description = event_item.description;
                        viewmodel.note = event_item.note;
                        viewmodel.document_category_list = entities.document_type.ToList();
                        viewmodel.curUser = curUser;
                        viewmodel.eventID = Convert.ToInt64(id);
                        viewmodel.searchStr = searchStr;
                        viewmodel.pubTaskList = ep.GetNotifiTaskList(userId);
                        viewmodel.pubMessageList = pubMessageList;
                        viewmodel.messageCount = ep.GetUnreadMessageCount(pubMessageList);
                        viewmodel.communityName = ep.GetCommunityInfo(userId)[0];
                        viewmodel.communityApart = ep.GetCommunityInfo(userId)[1];
                        return View(viewmodel);
                    }
                    catch(Exception ex)
                    {
                        return Redirect(Url.Action("Index", "Error"));
                    }                   
                }
                else
                {
                    return Redirect(Url.Action("NotFound", "Error"));
                }                
            } else
            {
                return Redirect(ep.GetLogoutUrl());
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
                entities.SaveChanges();
                return Redirect(Url.Action("registrados", "eventos", new { area = "coadmin" }));
            } catch (DbEntityValidationException e)
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
                return Redirect(Url.Action("editar", "eventos", new { area = "coadmin", editID = editID }));
            }
        }

        
        public JsonResult SetEventEnable(long eventID)
        {
            try
            {
                @event updateEvent = entities.events.Find(eventID);
                updateEvent.is_active = true;
                entities.SaveChanges();
                return Json(new { result = "success" });
            }
            catch (Exception ex)
            {
                return Json(new {
                    result = "error",
                    exception = ex.HResult
                });
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
                entities.events.Add(newEvent);
                entities.SaveChanges();
                return Redirect(Url.Action("registrados", "eventos", new { area = "coadmin" }));
            } catch(Exception ex)
            {
                return Redirect(Url.Action("agregar", "eventos", 
                    new {
                        area = "coadmin",
                        exception = ex.Message
                    }));
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

                    if (searchStr == "")
                    {
                        var query = (from r in entities.events select r);
                        eventList = query.ToList();
                    }
                    else
                    {
                        var query = (from r in entities.events
                                     where r.name.Contains(searchStr) == true
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
                        ii.month = datetime.Month - 1;
                        ii.day = datetime.Day;
                        ii.hour = datetime.Hour;
                        ii.minute = datetime.Minute;
                        listCalEventList.Add(ii);
                    }
                    return new JsonResult { Data = listCalEventList, JsonRequestBehavior = JsonRequestBehavior.AllowGet };
                }
            } catch(Exception)
            {
                return new JsonResult {
                    Data = listCalEventList,
                    JsonRequestBehavior = JsonRequestBehavior.AllowGet,
                };
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
        public ActionResult SeacrhResult(string searchStr)
        {
            return Redirect(Url.Action("registrados", "eventos",
                new
                {
                    area = "coadmin",
                    searchStr = searchStr 
                }));
        }

        [HttpPost]
        public ActionResult SearchRegistrado(string searchStr, long eventID)
        {
            return Redirect(Url.Action("registrado", "eventos",
                new
                {
                    area = "coadmin",
                    searchStr = searchStr,
                    id = eventID
                }));
        }

        [HttpPost]
        public ActionResult SeacrhInResult(string searchStr)
        {
            return Redirect(Url.Action("inactivos", "eventos",
                new
                {
                    area = "coadmin",
                    searchStr = searchStr
                }));
        }

        [HttpPost]
        public ActionResult SeacrhOtResult(string searchStr)
        {
            return Redirect(Url.Action("otros", "eventos",
                new
                {
                    area = "coadmin",
                    searchStr = searchStr
                }));
        }
        public JsonResult ActiveEventos(long id)
        {
            try
            {

                @event ev = entities.events.Find(id);
                ev.is_active = true;
                //user user = entities.users.Find(id);
                //user.is_active = true;
                entities.SaveChanges();
                return Json(new { result = "success" });
            }
            catch (Exception ex)
            {
                return Json(new { result = "error", exception = ex.HResult });
            }
        }

        public JsonResult DeactiveEventos(long id)
        {
            try
            {
                //user user = entities.users.Find(id);
                //user.is_active = false;
                @event ev = entities.events.Find(id);
                ev.is_active = false;
                entities.SaveChanges();
                return Json(new { result = "success" });
            }
            catch (Exception ex)
            {
                return Json(new { result = "error", exception = ex.HResult });
            }
        }

    }
}