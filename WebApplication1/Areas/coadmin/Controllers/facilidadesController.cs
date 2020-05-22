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
        List<community> communityList = new List<community>();

        // GET: coadmin/facilidades
        public ActionResult disponibles(string Error, string searchStr = "")
        {
            if (Session["USER_ID"] != null)
            {
                try
                {
                    long userId = (long)Session["USER_ID"];
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
                    communityList = ep.GetCommunityList(userId);
                    viewModel.communityList = communityList;
                    viewModel.side_menu = "facilidades";
                    viewModel.side_sub_menu = "facilidades_disponibles";
                    viewModel.document_category_list = entities.document_type.ToList();
                    viewModel.efacList = efacList;
                    viewModel.curUser = curUser;
                    viewModel.searchStr = searchStr;
                    viewModel.pubTaskList = ep.GetNotifiTaskList(userId);
                    viewModel.pubMessageList = pubMessageList;
                    ViewBag.msgError = Error;
                    viewModel.messageCount = ep.GetUnreadMessageCount(pubMessageList);
                    return View(viewModel);
                }
                catch(Exception ex)
                {
                    return Redirect(Url.Action("error", "control", new { area = "coadmin", Error = "Facilidades disponibles: " + ex.Message }));
                }               
            } else
            {
                return Redirect(ep.GetLogoutUrl());
            }
               
        }

        public ActionResult agregar(string Error)
        {
            if (Session["USER_ID"] != null)
            {
                if (Session["CURRENT_COMU"] != null)
                {
                    try
                    {
                        long userId = (long)Session["USER_ID"];                        
                        user curUser = entities.users.Find(userId);
                        List<ShowMessage> pubMessageList = ep.GetChatMessages(userId);
                        agregarFacViewModel viewModel = new agregarFacViewModel();
                        communityList = ep.GetCommunityList(userId);
                        viewModel.communityList = communityList;
                        viewModel.side_menu = "facilidades";
                        viewModel.side_sub_menu = "facilidades_agregar";
                        viewModel.document_category_list = entities.document_type.ToList();

                        int[] timeList = new int[24];
                        for (int i = 1; i < 25; i++)
                        {
                            timeList.SetValue(i, i - 1);
                        }

                        TimeSpan minInicio = new TimeSpan(8, 0, 0);
                        TimeSpan maxInicio = new TimeSpan(21, 0, 0);
                        TimeSpan minFinal = new TimeSpan(9, 0, 0);
                        TimeSpan maxFinal = new TimeSpan(22, 0, 0);
                        TimeSpan masU = TimeSpan.FromHours(1);

                        List<string> timeListIni = new List<string>();
                        List<string> timeListFin = new List<string>();
                        while (minInicio < maxInicio)
                        {
                            timeListIni.Add(
                                minInicio.ToString("hh':'mm"));

                            minInicio = minInicio.Add(masU);
                        }

                        while (minFinal < maxFinal)
                        {
                            timeListFin.Add(
                                minFinal.ToString("hh':'mm"));

                            minFinal = minFinal.Add(masU);
                        }

                        viewModel.timeList = timeList;
                        viewModel.timeListIni = timeListIni;
                        viewModel.timeListFin = timeListFin;
                        viewModel.curUser = curUser;
                        viewModel.pubTaskList = ep.GetNotifiTaskList(userId);
                        viewModel.pubMessageList = pubMessageList;
                        viewModel.messageCount = ep.GetUnreadMessageCount(pubMessageList);
                        ViewBag.msgError = Error;
                        return View(viewModel);
                    }
                    catch (Exception ex)
                    {
                        return Redirect(Url.Action("error", "control", new { area = "coadmin", Error = "Agregar facilidad " + ex.Message }));
                    }
                }
                else
                {
                    return Redirect(Url.Action("disponibles", "facilidades", new { area = "coadmin", Error = "No puede agregar facilidades. Usted no administra ninguna comunidad. Comuníquese con el Webmaster..." }));                  
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
                int start = Convert.ToInt32(start_time);
                int end = Convert.ToInt32(end_time);
                TimeSpan Inicio = TimeSpan.FromHours(start);
                TimeSpan Final = TimeSpan.FromHours(end);
                TimeSpan min = new TimeSpan(8, 0, 0);
                TimeSpan max = new TimeSpan(21, 0, 0);
                if (Inicio < min || Final > max)
                {
                    return Redirect(Url.Action("agregar", "facilidades", new { area = "coadmin", Error = "La hora de inicio y de fin no estan entre las permitidas..." }));
                }
                else
                {
                    newFac.start_time = Inicio;
                    newFac.end_time = Final;
                    newFac.cost_reservation = Convert.ToDecimal(cost_reservation.Replace('.', ','));
                    newFac.duration = duration;
                    newFac.created_at = DateTime.Now;
                    newFac.community_id = Convert.ToInt64(Session["CURRENT_COMU"]);
                    newFac.status = 1;
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
                    else
                    {
                        return Redirect(Url.Action("agregar", "facilidades", new { area = "coadmin", Error = "Debe cargar el reglamento" }));
                    }
                    entities.efacs.Add(newFac);
                    entities.SaveChanges();
                    return Redirect(Url.Action("disponibles", "facilidades", new { area = "coadmin" }));
                }                
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

        //public ActionResult reservar()
        //{
        //    if (Session["USER_ID"] != null)
        //    {
        //        if (Session["CURRENT_COMU"] != null)
        //        {
        //            try
        //            {
        //                long userId = (long)Session["USER_ID"];
        //                user curUser = entities.users.Find(userId);
        //                List<ShowMessage> pubMessageList = ep.GetChatMessages(userId);
        //                facilidadesViewModel viewModel = new facilidadesViewModel();

        //                communityList = ep.GetCommunityList(userId);
        //                viewModel.communityList = communityList;

        //                viewModel.side_menu = "facilidades";
        //                viewModel.side_sub_menu = "facilidades_reservar";
        //                viewModel.document_category_list = entities.document_type.ToList();
        //                viewModel.curUser = curUser;
        //                viewModel.pubTaskList = ep.GetNotifiTaskList(userId);
        //                viewModel.pubMessageList = pubMessageList;
        //                viewModel.messageCount = ep.GetUnreadMessageCount(pubMessageList);
        //                return View(viewModel);
        //            }
        //            catch (Exception ex)
        //            {
        //                return Redirect(Url.Action("error", "control", new { area = "coadmin", Error = "Reservar facilidad: " + ex.Message }));
        //            }                    
        //        }
        //        else
        //        {
        //            return Redirect(Url.Action("disponibles", "facilidades", new { area = "coadmin", Error = "No puede reservar facilidades. Usted no administra ninguna comunidad. Comuníquese con el Webmaster..." }));
        //        }
                
        //    } else
        //    {
        //        return Redirect(ep.GetLogoutUrl());
        //    }
                
        //}

        public ActionResult editar(string Error, long? facID)
        {
            if (Session["USER_ID"] != null)
            {
                if (Session["CURRENT_COMU"] != null)
                {
                    if (facID != null)
                    {
                        long communityAct = Convert.ToInt64(Session["CURRENT_COMU"]);
                        efac editFac = entities.efacs.Where(x=> x.id == facID && x.community_id == communityAct).FirstOrDefault();
                        if (editFac != null)
                        {
                            try
                            {
                                long userId = (long)Session["USER_ID"];
                                user curUser = entities.users.Find(userId);
                                List<ShowMessage> pubMessageList = ep.GetChatMessages(userId);
                                editarFacViewModel viewModel = new editarFacViewModel();
                                communityList = ep.GetCommunityList(userId);
                                viewModel.communityList = communityList;
                                viewModel.side_menu = "facilidades";
                                viewModel.side_sub_menu = "facilidades_editar";
                                viewModel.document_category_list = entities.document_type.ToList();
                                viewModel.editFac = editFac;
                                int[] timeList = new int[24];
                                for (int i = 1; i < 25; i++)
                                {
                                    timeList.SetValue(i, i - 1);

                                }

                                TimeSpan minInicio = new TimeSpan(8, 0, 0);
                                TimeSpan maxInicio = new TimeSpan(21, 0, 0);
                                TimeSpan minFinal = new TimeSpan(9, 0, 0);
                                TimeSpan maxFinal = new TimeSpan(22, 0, 0);
                                TimeSpan masU = TimeSpan.FromHours(1);

                                List<string> timeListIni = new List<string>();
                                List<string> timeListFin = new List<string>();
                                while (minInicio < maxInicio)
                                {
                                    timeListIni.Add(
                                        minInicio.ToString("hh':'mm"));

                                    minInicio = minInicio.Add(masU);
                                }

                                while (minFinal < maxFinal)
                                {
                                    timeListFin.Add(
                                        minFinal.ToString("hh':'mm"));

                                    minFinal = minFinal.Add(masU);
                                }

                                string ini = Convert.ToString(editFac.start_time);
                                string fin = Convert.ToString(editFac.end_time);
                                TimeSpan horaInicio = TimeSpan.Parse(ini);
                                TimeSpan horaFin = TimeSpan.Parse(fin);
                              
                                viewModel.inicioEfac = horaInicio.ToString("hh':'mm");
                                viewModel.finalEfac = horaFin.ToString("hh':'mm");
                                viewModel.timeListIni = timeListIni;
                                viewModel.timeListFin = timeListFin;
                                viewModel.timeList = timeList;
                                viewModel.curUser = curUser;
                                viewModel.pubTaskList = ep.GetNotifiTaskList(userId);
                                viewModel.pubMessageList = pubMessageList;
                                viewModel.messageCount = ep.GetUnreadMessageCount(pubMessageList);
                                ViewBag.msgError = Error;
                                return View(viewModel);
                            }
                            catch (Exception ex)
                            {
                                return Redirect(Url.Action("error", "control", new { area = "coadmin", Error = "Editar facilidad: " + ex.Message }));
                            }                            
                        }
                        else
                        {
                            return Redirect(Url.Action("disponibles", "facilidades", new { area = "coadmin", Error = "No existe ese elemento" }));
                        }
                        
                    }
                    else
                    {
                        return Redirect(Url.Action("disponibles", "facilidades", new { area = "coadmin"}));
                    }
                }
                else
                {
                    return Redirect(Url.Action("disponibles", "facilidades", new { area = "coadmin", Error = "No puede editar facilidades. Usted no administra ninguna comunidad. Comuníquese con el Webmaster..." }));
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
                int start = Convert.ToInt32(start_time);
                int end = Convert.ToInt32(end_time);
                TimeSpan Inicio = TimeSpan.FromHours(start);
                TimeSpan Final = TimeSpan.FromHours(end);
                TimeSpan min = new TimeSpan(8, 0, 0);
                TimeSpan max = new TimeSpan(21, 0, 0);
                if (Inicio < min || Final > max)
                {
                    return Redirect(Url.Action("editar", "facilidades", new { area = "coadmin", facID = facID, Error = "La hora de inicio y de fin no estan entre las permitidas..." }));
                }
                else
                {
                    editFac.start_time = Inicio;
                    editFac.end_time = Final;
                    editFac.duration = duration;
                    editFac.cost_reservation = Convert.ToDecimal(cost_reservation.Replace('.', ','));
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
                        editFac.upload_regulation = fileName;                        
                    }
                    entities.SaveChanges();
                    return Redirect(Url.Action("disponibles", "facilidades", new { area = "coadmin" }));
                }
                                
            } 
            catch(Exception ex)
            {
                return Redirect(Url.Action("editar", "facilidades",new { area = "coadmin", facID = facID, Error = "Error al editar la facilidad: ", ex.Message }));
            }
        }

        //public ActionResult reservartwo()
        //{
        //    if (Session["USER_ID"] != null)
        //    {
        //        if (Session["CURRENT_COMU"] != null)
        //        {
        //            long userId = (long)Session["USER_ID"];
        //            user curUser = entities.users.Find(userId);
        //            facilidadesViewModel viewModel = new facilidadesViewModel();

        //            communityList = ep.GetCommunityList(userId);
        //            viewModel.communityList = communityList;

        //            viewModel.side_menu = "facilidades";
        //            viewModel.side_sub_menu = "facilidades_reservartwo";
        //            viewModel.document_category_list = entities.document_type.ToList();
        //            viewModel.curUser = curUser;
        //            viewModel.pubTaskList = ep.GetNotifiTaskList(userId);
        //            viewModel.pubMessageList = ep.GetChatMessages(userId);
        //            return View(viewModel);
        //        }
        //        else
        //        {
        //            return Redirect(Url.Action("disponibles", "facilidades", new { area = "coadmin", Error = "No puede reservar facilidades. Usted no administra ninguna comunidad. Comuníquese con el Webmaster..." }));
        //        }
               
        //    } else
        //    {
        //        return Redirect(ep.GetLogoutUrl());
        //    }
                
        //}

        public ActionResult solicitudes(string searchStr = "")
        {
            if (Session["USER_ID"] != null)
            {
                try
                {
                    long userId = (long)Session["USER_ID"];
                    user curUser = entities.users.Find(userId);
                    List<book> bookList = new List<book>();
                    long communityAct = Convert.ToInt64(Session["CURRENT_COMU"]);

                    if (searchStr == "")
                    {
                        var query = (from r in entities.books where r.community_id == communityAct select r);
                        bookList = query.ToList();
                    }
                    else
                    {
                        var query1 = (from r in entities.books where r.first_name.Contains(searchStr) == true && r.community_id == communityAct select r);
                        bookList = query1.ToList();
                    }

                    facilidadesViewModel viewModel = new facilidadesViewModel();
                    communityList = ep.GetCommunityList(userId);
                    viewModel.communityList = communityList;
                    viewModel.side_menu = "facilidades";
                    viewModel.side_sub_menu = "facilidades_solicitudes";
                    viewModel.document_category_list = entities.document_type.ToList();
                    viewModel.searchStr = searchStr;
                    viewModel.bookList = bookList;
                    viewModel.curUser = curUser;
                    viewModel.pubTaskList = ep.GetNotifiTaskList(userId);
                    viewModel.pubMessageList = ep.GetChatMessages(userId);
                    return View(viewModel);
                }
                catch (Exception ex)
                {
                    return Redirect(Url.Action("error", "control", new { area = "coadmin", Error = "Solicitudes facilidad: " + ex.Message }));
                }
               
            } else
            {
                return Redirect(ep.GetLogoutUrl());
            }
                
        }

        //public ActionResult registrado(long? id)
        //{
        //    if (Session["USER_ID"] != null)
        //    {
        //        if (id != null)
        //        {
        //            long userId = (long)Session["USER_ID"];
        //            user curUser = entities.users.Find(userId);
        //            @event event_item = entities.events.Find(id);
        //            registradoViewModel viewmodel = new registradoViewModel();

        //            communityList = ep.GetCommunityList(userId);
        //            viewmodel.communityList = communityList;

        //            viewmodel.side_menu = "facilidades";
        //            viewmodel.side_sub_menu = "facilidades_registrado";
        //            viewmodel.event_name = event_item.name;
        //            viewmodel.event_date = Convert.ToDateTime(event_item.created_at).ToString("dd/MM/yyyy");
        //            viewmodel.event_time = Convert.ToDateTime(event_item.created_at).ToString("hh:mm tt");
        //            viewmodel.place = event_item.place;
        //            viewmodel.description = event_item.description;
        //            viewmodel.note = event_item.note;
        //            viewmodel.document_category_list = entities.document_type.ToList();
        //            viewmodel.curUser = curUser;
        //            viewmodel.pubTaskList = ep.GetNotifiTaskList(userId);
        //            viewmodel.pubMessageList = ep.GetChatMessages(userId);                   
        //            return View(viewmodel);
        //        }
        //        else
        //        {
        //            return Redirect(Url.Action("NotFound", "Error"));
        //        }               
        //    }
        //    else
        //    {
        //        return Redirect(ep.GetLogoutUrl());
        //    }
        //}

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
                newBook.community_id = Convert.ToInt64(Session["CURRENT_COMU"]);
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

        public JsonResult onAprobar(long id)
        {
            try
            {

                book delf = entities.books.Find(id);
                if(delf.status==1)
                {
                    delf.status = 2;
                }else if(delf.status == 2)
                {
                    delf.status = 3;
                }else if(delf.status == 3)
                {
                    delf.status = 2;
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