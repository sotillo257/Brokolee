using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WebApplication1.Areas.coadmin.ViewModels;
using WebApplication1.Concrete;
using WebApplication1.Models;

namespace WebApplication1.Areas.coadmin.Controllers
{
    public class facilidadesController : Controller
    {
        pjrdev_condominiosEntities entities = new pjrdev_condominiosEntities();
        EFPublicRepository ep = new EFPublicRepository();
        // GET: coadmin/facilidades
        public ActionResult disponibles(string searchStr = "")
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
                    List<efac> efacList = new List<efac>();

                    long communityAct = Convert.ToInt64(Session["CURRENT_COMU"]);

                    if (searchStr == "")
                    {
                        var query = (from r in entities.efacs where r.community_id == communityAct select r);
                        efacList = query.ToList();
                    }
                    else
                    {
                        var query1 = (from r in entities.efacs
                                      where r.first_name.Contains(searchStr) == true && r.community_id == communityAct
                                      select r);
                        efacList = query1.ToList();
                    }

                    facilidadesViewModel viewModel = new facilidadesViewModel();
                    viewModel.side_menu = "facilidades";
                    viewModel.side_sub_menu = "facilidades_disponibles";
                    viewModel.document_category_list = entities.document_type.ToList();
                    viewModel.efacList = efacList;
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
                    agregarFacViewModel viewModel = new agregarFacViewModel();
                    viewModel.side_menu = "facilidades";
                    viewModel.side_sub_menu = "facilidades_agregar";
                    viewModel.document_category_list = entities.document_type.ToList();
                    int[] timeList = new int[24];
                    for (int i = 1; i < 25; i++)
                    {
                        timeList.SetValue(i, i - 1);

                    }
                    viewModel.timeList = timeList;
                    viewModel.curUser = curUser;
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

        [HttpPost]
        public ActionResult newfac(string first_name, string description,
            string start_time, string end_time, int duration, 
            string cost_reservation, HttpPostedFileBase upload_regulation)
        {
            try
            {
                efac newFac = new efac();
                newFac.first_name = first_name;
                newFac.description = description;
                newFac.start_time = TimeSpan.ParseExact(start_time, "h\\:mm",
                    System.Globalization.CultureInfo.CurrentCulture, TimeSpanStyles.None
                    );
                newFac.end_time = TimeSpan.ParseExact(end_time, "h\\:mm",
                    System.Globalization.CultureInfo.CurrentCulture, TimeSpanStyles.None
                    );
                newFac.cost_reservation = Convert.ToDecimal(cost_reservation);
                newFac.duration = duration;
                newFac.created_at = DateTime.Now;
                newFac.community_id = Convert.ToInt64(Session["CURRENT_COMU"]);
                if (upload_regulation != null && upload_regulation.ContentLength > 0)
                {
                    var fileName = Path.GetFileName(upload_regulation.FileName);
                    if (!Directory.Exists(Path.Combine(Server.MapPath("~/"), "Upload")))
                    {
                        Directory.CreateDirectory(Path.Combine(Server.MapPath("~/"), "Upload"));
                    }

                    if (!Directory.Exists(Path.Combine(Server.MapPath("~/Upload/"), "Regulation")))
                    {
                        Directory.CreateDirectory(Path.Combine(Server.MapPath("~/Upload/"), "Regulation"));
                    }
                    var path = Path.Combine(Server.MapPath("~/Upload/Regulation"), fileName);
                    upload_regulation.SaveAs(path);
                    newFac.upload_regulation = fileName;
                }
                entities.efacs.Add(newFac);
                entities.SaveChanges();
                return Redirect(Url.Action("disponibles", "facilidades", new { area = "coadmin" }));
            }
            catch (Exception ex)
            {
                return Redirect(Url.Action("agregar", "facilidades", 
                    new {
                        area = "coadmin",
                        exception = ex.Message
                    }));
            }
        }

        public ActionResult reservar()
        {
            if (Session["USER_ID"] != null)
            {
                long userId = (long)Session["USER_ID"];
                user curUser = entities.users.Find(userId);
                List<ShowMessage> pubMessageList = ep.GetChatMessages(userId);
                facilidadesViewModel viewModel = new facilidadesViewModel();
                viewModel.side_menu = "facilidades";
                viewModel.side_sub_menu = "facilidades_reservar";
                viewModel.document_category_list = entities.document_type.ToList();
                viewModel.curUser = curUser;
                viewModel.pubTaskList = ep.GetNotifiTaskList(userId);
                viewModel.pubMessageList = pubMessageList;
                viewModel.messageCount = ep.GetUnreadMessageCount(pubMessageList);
                viewModel.communityName = ep.GetCommunityInfo(userId)[0];
                viewModel.communityApart = ep.GetCommunityInfo(userId)[1];
                return View(viewModel);
            } else
            {
                return Redirect(ep.GetLogoutUrl());
            }
                
        }

        public ActionResult editar(long? facID)
        {
            if (Session["USER_ID"] != null)
            {
                if (facID != null)
                {
                    long userId = (long)Session["USER_ID"];
                    user curUser = entities.users.Find(userId);
                    List<ShowMessage> pubMessageList = ep.GetChatMessages(userId);
                    efac editFac = entities.efacs.Find(facID);
                    editarFacViewModel viewModel = new editarFacViewModel();
                    viewModel.side_menu = "facilidades";
                    viewModel.side_sub_menu = "facilidades_editar";
                    viewModel.document_category_list = entities.document_type.ToList();
                    viewModel.editFac = editFac;
                    int[] timeList = new int[24];
                    for (int i = 1; i < 25; i++)
                    {
                        timeList.SetValue(i, i - 1);

                    }
                    viewModel.timeList = timeList;
                    viewModel.curUser = curUser;
                    viewModel.pubTaskList = ep.GetNotifiTaskList(userId);
                    viewModel.pubMessageList = pubMessageList;
                    viewModel.messageCount = ep.GetUnreadMessageCount(pubMessageList);
                    viewModel.communityName = ep.GetCommunityInfo(userId)[0];
                    viewModel.communityApart = ep.GetCommunityInfo(userId)[1];
                    return View(viewModel);
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
        public ActionResult editfac(long facID, string first_name, string description,
            string start_time, string end_time, int duration, string cost_reservation,
            HttpPostedFileBase upload_regulation)
        {
            try
            {
                efac editFac = entities.efacs.Find(facID);
                editFac.first_name = first_name;
                editFac.description = description;
                editFac.start_time = TimeSpan.ParseExact(start_time, "h\\:mm",
                    System.Globalization.CultureInfo.CurrentCulture, TimeSpanStyles.None);
                editFac.end_time = TimeSpan.ParseExact(end_time, "h\\:mm",
                    System.Globalization.CultureInfo.CurrentCulture, TimeSpanStyles.None);
                editFac.duration = duration;
                editFac.cost_reservation = Convert.ToDecimal(cost_reservation);
                if (upload_regulation != null  && upload_regulation.ContentLength > 0)
                {
                    var fileName = Path.GetFileName(upload_regulation.FileName);
                    if (!Directory.Exists(Path.Combine(Server.MapPath("~/"), "Upload")))
                    {
                        Directory.CreateDirectory(Path.Combine(Server.MapPath("~/"), "Upload"));
                    }

                    if (!Directory.Exists(Path.Combine(Server.MapPath("~/Upload/"), "Regulation")))
                    {
                        Directory.CreateDirectory(Path.Combine(Server.MapPath("~/Upload/"), "Regulation"));
                    }
                    var path = Path.Combine(Server.MapPath("~/Upload/Regulation"), fileName);
                    upload_regulation.SaveAs(path);
                    editFac.upload_regulation = fileName;
                }
                entities.SaveChanges();
                return Redirect(Url.Action("disponibles", "facilidades", new { area = "coadmin" }));
            } catch(Exception ex)
            {
                return Redirect(Url.Action("editar", "facilidades",
                    new {
                        area = "coadmin",
                        facID = facID,
                        exception = ex.Message
                    }));
            }
        }

        public ActionResult reservartwo()
        {
            if (Session["USER_ID"] != null)
            {
                long userId = (long)Session["USER_ID"];
                user curUser = entities.users.Find(userId);
                facilidadesViewModel viewModel = new facilidadesViewModel();
                viewModel.side_menu = "facilidades";
                viewModel.side_sub_menu = "facilidades_reservartwo";
                viewModel.document_category_list = entities.document_type.ToList();
                viewModel.curUser = curUser;
                viewModel.pubTaskList = ep.GetNotifiTaskList(userId);
                viewModel.pubMessageList = ep.GetChatMessages(userId);
                viewModel.communityName = ep.GetCommunityInfo(userId)[0];
                viewModel.communityApart = ep.GetCommunityInfo(userId)[1];
                return View(viewModel);
            } else
            {
                return Redirect(ep.GetLogoutUrl());
            }
                
        }

        public ActionResult solicitudes(string searchStr = "")
        {
            if (Session["USER_ID"] != null)
            {
                 long userId = (long)Session["USER_ID"];
                user curUser = entities.users.Find(userId);
                List<efac> efacList = new List<efac>();

                long communityAct = Convert.ToInt64(Session["CURRENT_COMU"]);

                if (searchStr == "")
                {
                    var query = (from r in entities.efacs where r.community_id == communityAct select r);
                    efacList = query.ToList();
                } else
                {
                    var query1 = (from r in entities.efacs where r.first_name.Contains(searchStr) == true && r.community_id == communityAct select r);
                    efacList = query1.ToList();
                }
                
                facilidadesViewModel viewModel = new facilidadesViewModel();
                viewModel.side_menu = "facilidades";
                viewModel.side_sub_menu = "facilidades_solicitudes";
                viewModel.document_category_list = entities.document_type.ToList();
                viewModel.searchStr = searchStr;
                viewModel.efacList = efacList;
                viewModel.curUser = curUser;
                viewModel.pubTaskList = ep.GetNotifiTaskList(userId);
                viewModel.pubMessageList = ep.GetChatMessages(userId);
                viewModel.communityName = ep.GetCommunityInfo(userId)[0];
                viewModel.communityApart = ep.GetCommunityInfo(userId)[1];
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
                    viewmodel.side_menu = "facilidades";
                    viewmodel.side_sub_menu = "facilidades_registrado";
                    viewmodel.event_name = event_item.name;
                    viewmodel.event_date = Convert.ToDateTime(event_item.created_at).ToString("dd/MM/yyyy");
                    viewmodel.event_time = Convert.ToDateTime(event_item.created_at).ToString("hh:mm tt");
                    viewmodel.place = event_item.place;
                    viewmodel.description = event_item.description;
                    viewmodel.note = event_item.note;
                    viewmodel.document_category_list = entities.document_type.ToList();
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
                return Redirect(Url.Action("reservar", "facilidades",
                    new { area = "coadmin" }));
            } catch(Exception ex)
            {
                return Redirect(Url.Action("reservartwo", "facilidades",
                    new {
                        area = "coadmin",
                        exception = ex.Message
                    }));
            }
        }

        public JsonResult DeleteFac(long delID)
        {
            try
            {
                efac delFac = entities.efacs.Find(delID);
                entities.efacs.Remove(delFac);
                entities.SaveChanges();
                return Json(new { result = "success" });
            }
            catch(Exception ex)
            {
                return Json(new { result = "error", exception = ex.HResult });
            }
        }

        [HttpPost]
        public ActionResult SeacrhResult(string searchStr)
        {
            return Redirect(Url.Action("disponibles", "facilidades", new { area = "coadmin", searchStr = searchStr }));
        }

        [HttpPost]
        public ActionResult SeacrhSolResult(string searchStr)
        {
            return Redirect(Url.Action("disponibles", "facilidades", new { area = "coadmin", searchStr = searchStr }));
        }

        public JsonResult onActiveSol(long id)
        {
            try
            {


                efac delf = entities.efacs.Find(id);
                if(delf.status==2)
                {
                    delf.status = 1;
                }else
                {
                    if (delf.status == 1)
                    {
                        delf.status = 2;
                    }
                }
                
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