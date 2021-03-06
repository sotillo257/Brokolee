﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WebApplication1.Concrete;
using WebApplication1.Models;
using WebApplication1.ViewModels;

namespace WebApplication1.Controllers
{
    public class eventosController : Controller
    {
        pjrdev_condominiosEntities entities = new pjrdev_condominiosEntities();
        EFPublicRepository ep = new EFPublicRepository();

        List<Titulo> titulosList = new List<Titulo>();
        List<community> listComunities = new List<community>();

        // GET: events
        public ActionResult calendario(string searchStr = "")
        {
            if (Session["USER_ID"] != null)
            {
                try
                {
                    long userId = (long)Session["USER_ID"];
                    long communityAct = Convert.ToInt64(Session["CURRENT_COMU"]);
                    user curUser = entities.users.Find(userId);
                    List<ShowMessage> pubMessageList = ep.GetChatMessages(userId);
                    eventosViewModel viewModel = new eventosViewModel();
                    titulosList = ep.GetTitulosByTitular(userId);
                    listComunities = ep.GetCommunityListByTitular(titulosList);
                    viewModel.communityList = listComunities;                  
                    viewModel.side_menu = "eventos";
                     viewModel.document_category_list = entities.document_type.Where(x => x.community_id == communityAct).ToList();
                    viewModel.curUser = curUser;
                    viewModel.searchStr = searchStr;
                    viewModel.pubTaskList = ep.GetNotifiTaskList(userId);
                    viewModel.pubMessageList = pubMessageList;
                    viewModel.messageCount = ep.GetUnreadMessageCount(pubMessageList);
                    return View(viewModel);
                }
                catch(Exception ex)
                {
                    return Redirect(Url.Action("error", "control", new { Error = "Eventos: " + ex.Message }));
                }
            } else
            {
                return Redirect(Url.Action("iniciar", "iniciar"));
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
                    if (Session["CURRENT_COMU"] != null)
                    {
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
                    }
                    else
                    {
                        eventList.Clear();
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


        public ActionResult registrado(long? id)
        {
            if (Session["USER_ID"] != null)
            {
                if(id != null)
                {
                    long communityAct = Convert.ToInt64(Session["CURRENT_COMU"]);
                    @event event_item = entities.events.Where(x=> x.id == id && x.community_id == communityAct).FirstOrDefault();
                    if (event_item != null)
                    {
                        try
                        {
                            long userId = (long)Session["USER_ID"];
                            user curUser = entities.users.Find(userId);
                            List<ShowMessage> pubMessageList = ep.GetChatMessages(userId);
                            registradoViewModel viewModel = new registradoViewModel();
                            titulosList = ep.GetTitulosByTitular(userId);
                            listComunities = ep.GetCommunityListByTitular(titulosList);
                            viewModel.communityList = listComunities;
                            viewModel.side_menu = "eventos";
                            viewModel.event_name = event_item.name;
                            viewModel.event_date = Convert.ToDateTime(event_item.created_at).ToString("dd/MM/yyyy");
                            viewModel.event_time = Convert.ToDateTime(event_item.created_at).ToString("hh:mm tt");
                            viewModel.place = event_item.place;
                            viewModel.description = event_item.description;
                            viewModel.note = event_item.note;
                             viewModel.document_category_list = entities.document_type.Where(x => x.community_id == communityAct).ToList();
                            viewModel.curUser = curUser;
                            viewModel.pubTaskList = ep.GetNotifiTaskList(userId);
                            viewModel.pubMessageList = pubMessageList;
                            viewModel.messageCount = ep.GetUnreadMessageCount(pubMessageList);
                            return View(viewModel);
                        }
                        catch (Exception ex)
                        {
                            return Redirect(Url.Action("error", "control", new { Error = "Ver evento: " + ex.Message }));
                        }
                    }
                    else
                    {
                        return Redirect(Url.Action("calendario", "eventos", new { Error = "No existe ese elemento: " }));
                    }                                    
                }
                else
                {
                    return Redirect(Url.Action("calendario", "eventos"));
                }               
            } else
            {
                return Redirect(ep.GetLogoutUrl());
            }
        }

        [HttpPost]
        public ActionResult SeacrhResult(string searchStr)
        {
            return Redirect(Url.Action("calendario", "eventos",
                new
                {
                    searchStr = searchStr
                }));
        }
    }
}