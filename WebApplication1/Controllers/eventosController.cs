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
                    eventosViewModel viewModel = new eventosViewModel();

                    titulosList = ep.GetTitulosByTitular(userId);
                    listComunities = ep.GetCommunityListByTitular(titulosList);
                    viewModel.communityList = listComunities;                  

                    viewModel.side_menu = "eventos";
                    viewModel.document_category_list = entities.document_type.ToList();
                    viewModel.curUser = curUser;
                    viewModel.searchStr = searchStr;
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
                        @event event_item = entities.events.Find(id);
                        registradoViewModel viewModel = new registradoViewModel();

                        titulosList = ep.GetTitulosByTitular(userId);
                        listComunities = ep.GetCommunityListByTitular(titulosList);
                        viewModel.communityList = listComunities;

                        viewModel.side_menu = "registrado";
                        viewModel.event_name = event_item.name;
                        viewModel.event_date = Convert.ToDateTime(event_item.created_at).ToString("dd/MM/yyyy");
                        viewModel.event_time = Convert.ToDateTime(event_item.created_at).ToString("hh:mm tt");
                        viewModel.place = event_item.place;
                        viewModel.description = event_item.description;
                        viewModel.note = event_item.note;
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
                }
                else
                {
                    return Redirect(Url.Action("NotFound", "Error"));
                }               
            } else
            {
                return Redirect(Url.Action("iniciar", "iniciar"));
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