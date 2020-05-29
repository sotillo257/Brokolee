using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WebApplication1.Areas.coadmin.ViewModels;
using WebApplication1.Concrete;
using WebApplication1.Models;

namespace WebApplication1.Areas.coadmin.Controllers
{
    public class tareasController : Controller
    {
        pjrdev_condominiosEntities entities = new pjrdev_condominiosEntities();
        EFPublicRepository ep = new EFPublicRepository();
        List<community> communityList = new List<community>();

        // GET: coadmin/tareas
        public ActionResult listado(string Error, string searchStr = "")
        {
            if (Session["USER_ID"] != null)
            {
                try
                {
                    long userId = (long)Session["USER_ID"];
                    user curUser = entities.users.Find(userId);
                    List<ShowMessage> pubMessageList = ep.GetChatMessages(userId);
                    List<task> taskList = new List<task>();

                    long communityAct = Convert.ToInt64(Session["CURRENT_COMU"]);

                    if (Session["CURRENT_COMU"] != null)
                    {
                        if (searchStr == "")
                        {
                            var query = (from r in entities.tasks where r.community_id == communityAct select r);
                            taskList = query.ToList();
                        }
                        else
                        {
                            var query1 = (from r in entities.tasks
                                          where r.task_name.Contains(searchStr) == true && r.community_id == communityAct
                                          select r);
                            taskList = query1.ToList();
                        }
                    }
                    else
                    {
                        taskList.Clear();
                    }
                       

                    tareasViewModel viewModel = new tareasViewModel();
                    communityList = ep.GetCommunityList(userId);
                    viewModel.communityList = communityList;

                    List<user> titularList = new List<user>();
                    var query2 = (from r in entities.users
                                 where ( r.role == 4) && r.is_del != true
                                 select r);
                    titularList = query2.ToList();
                    viewModel.side_menu = "task_process";
                    viewModel.side_sub_menu = "task_process_listado";
                     viewModel.document_category_list = entities.document_type.Where(x => x.community_id == communityAct).ToList();
                    viewModel.taskList = taskList;
                    viewModel.searchStr = searchStr;
                    viewModel.curUser = curUser;
                    viewModel.pubMessageList = pubMessageList;
                    viewModel.messageCount = ep.GetUnreadMessageCount(pubMessageList);                    
                    viewModel.pubTaskList = ep.GetNotifiTaskList(userId);
                    viewModel.titularList = titularList;
                    ViewBag.msgError = Error;
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
                if (Session["CURRENT_COMU"] != null)
                {
                    try
                    {
                        long userId = (long)Session["USER_ID"];
                        long communityAct = Convert.ToInt64(Session["CURRENT_COMU"]);
                        user curUser = entities.users.Find(userId);
                        List<ShowMessage> pubMessageList = ep.GetChatMessages(userId);
                        tareasagregarViewModel viewModel = new tareasagregarViewModel();

                        communityList = ep.GetCommunityList(userId);
                        viewModel.communityList = communityList;

                        viewModel.side_menu = "task_process";
                        viewModel.side_sub_menu = "task_process_agregar";
                         viewModel.document_category_list = entities.document_type.Where(x => x.community_id == communityAct).ToList();
                        viewModel.curUser = curUser;
                        viewModel.pubTaskList = ep.GetNotifiTaskList(userId);
                        viewModel.pubMessageList = pubMessageList;
                        viewModel.messageCount = ep.GetUnreadMessageCount(pubMessageList);
                        return View(viewModel);
                    }
                    catch (Exception ex)
                    {
                        return Redirect(Url.Action("Index", "Error"));
                    }
                }
                else
                {
                    return Redirect(Url.Action("listado", "tareas", new { area = "coadmin", Error = "No puede agregar tareas. Usted no administra ninguna comunidad. Comuníquese con el Webmaster..." }));                 
                }
                    
                                
            } else
            {
                return Redirect(ep.GetLogoutUrl());
            }
        }

        [HttpPost]
        public ActionResult newtask(string task_name, string description,
            string responsable, string task_date, string task_time, string comments,
            int share, int status, string estimated_date, string estimated_time
            )
        {
            try
            {
                long userId = (long)Session["USER_ID"];
                task newTask = new task();
                newTask.task_name = task_name;
                newTask.description = description;
                newTask.responsable = responsable;
                newTask.comments = comments;
                newTask.task_date = DateTime.ParseExact(task_date + " " + task_time, "yyyy-MM-dd HH:mm",
                    System.Globalization.CultureInfo.CurrentCulture);
                newTask.estimated_date = DateTime.ParseExact(estimated_date + " " + estimated_time, "yyyy-MM-dd HH:mm",
                    System.Globalization.CultureInfo.CurrentCulture);
                newTask.share = share;
                newTask.status = status;
                newTask.created_at = DateTime.Now;
                newTask.updated_at = DateTime.Now;
                newTask.community_id = Convert.ToInt64(Session["CURRENT_COMU"]);
                entities.tasks.Add(newTask);
                entities.SaveChanges();
                taskuser taskuser = entities.taskusers.Where(m => m.user_id == userId).FirstOrDefault();
                if (taskuser == null)
                {
                    taskuser = new taskuser();
                    taskuser.user_id = userId;
                    taskuser.task_list = newTask.id.ToString();
                    entities.taskusers.Add(taskuser);
                }
                else
                {
                    string taskList = taskuser.task_list;
                    if (taskList == null)
                    {
                        taskuser.task_list = newTask.id.ToString();
                    }
                    else
                    {
                        taskuser.task_list = taskList + "," + newTask.id.ToString();
                    }
                }
                entities.SaveChanges();
                return Redirect(Url.Action("listado", "tareas", new { area = "coadmin" }));
            }catch(Exception)
            {
                return Redirect(ep.GetLogoutUrl());
            }
        }

        public ActionResult editar(long? taskID)
        {
            if (Session["USER_ID"] != null) { 
                if (Session["CURRENT_COMU"] != null)
                {
                    if (taskID != null)
                    {
                        long communityAct = Convert.ToInt64(Session["CURRENT_COMU"]);
                        long userId = (long)Session["USER_ID"];
                        user curUser = entities.users.Find(userId);                       
                        task editTask = entities.tasks.Where(x=> x.id == taskID && x.community_id == communityAct).FirstOrDefault();
                        if (editTask != null)
                        {
                            List<ShowMessage> pubMessageList = ep.GetChatMessages(userId);
                            editarTareasViewModel viewModel = new editarTareasViewModel();

                            communityList = ep.GetCommunityList(userId);
                            viewModel.communityList = communityList;

                            viewModel.side_menu = "task_process";
                            viewModel.side_sub_menu = "task_process_editar";
                             viewModel.document_category_list = entities.document_type.Where(x => x.community_id == communityAct).ToList();
                            viewModel.editTask = editTask;
                            viewModel.curUser = curUser;
                            viewModel.pubTaskList = ep.GetNotifiTaskList(userId);
                            viewModel.pubMessageList = pubMessageList;
                            viewModel.messageCount = ep.GetUnreadMessageCount(pubMessageList);
                            return View(viewModel);
                        }
                        else
                        {
                            return Redirect(Url.Action("listado", "tareas", new { area = "coadmin", Error = "No existe ese elemento" }));
                        }
                       
                    }
                    else
                    {
                        return Redirect(Url.Action("listado", "tareas", new { area = "coadmin" }));
                    }
                }
                else
                {
                    return Redirect(Url.Action("listado", "tareas", new { area = "coadmin", Error = "No puede editar tareas. Usted no administra ninguna comunidad. Comuníquese con el Webmaster..." }));
                }                                          
            } else
            {
                return Redirect(ep.GetLogoutUrl());
            }
        }

        [HttpPost]
        public ActionResult edittask(long taskID, string task_name,
            string description, string responsable, string task_date,
            string estimated_date, string comments, int share , int status)
        {
            try
            {
                task editTask = entities.tasks.Find(taskID);
                editTask.task_name = task_name;
                editTask.description = description;
                editTask.responsable = responsable;
                editTask.task_date = DateTime.ParseExact(task_date, "yyyy-MM-dd",
                    System.Globalization.CultureInfo.CurrentCulture);
                editTask.comments = comments;
                editTask.estimated_date = DateTime.ParseExact(estimated_date, "yyyy-MM-dd",
                    System.Globalization.CultureInfo.CurrentCulture);
                editTask.share = share;
                editTask.status = status;
                if(status ==3)
                {
                    editTask.completed_date = DateTime.Today;

                }
                
                entities.SaveChanges();
                return Redirect(Url.Action("listado", "tareas", new { area = "coadmin" }));
            } catch(Exception ex)
            {
                return Redirect(Url.Action("editar", "tareas", 
                    new {
                        area = "coadmin",
                        taskID = taskID,
                        exception = ex.Message
                    }));
            }
        }

        public ActionResult completadas(string searchStr = "")
        {
            if (Session["USER_ID"] != null)
            {
                long userId = (long)Session["USER_ID"];
                user curUser = entities.users.Find(userId);
                List<ShowMessage> pubMessageList = ep.GetChatMessages(userId);
                List<task> taskList = new List<task>();

                long communityAct = Convert.ToInt64(Session["CURRENT_COMU"]);

                if (Session["CURRENT_COMU"] != null)
                {
                    if (searchStr == "")
                    {
                        var query = (from r in entities.tasks where r.completed_date != null && r.community_id == communityAct select r);
                        taskList = query.ToList();
                    }
                    else
                    {
                        var query1 = (from r in entities.tasks
                                      where r.completed_date != null && r.task_name.Contains(searchStr) == true && r.community_id == communityAct
                                      select r);
                    }
                }
                else
                {
                    taskList.Clear();
                }
                                 
                tareasViewModel viewModel = new tareasViewModel();

                communityList = ep.GetCommunityList(userId);
                viewModel.communityList = communityList;

                viewModel.side_menu = "task_process";
                viewModel.side_sub_menu = "task_process_completadas";
                 viewModel.document_category_list = entities.document_type.Where(x => x.community_id == communityAct).ToList();
                viewModel.searchStr = searchStr;
                viewModel.taskList = taskList;
                viewModel.curUser = curUser;
                viewModel.pubTaskList = ep.GetNotifiTaskList(userId);
                viewModel.pubMessageList = pubMessageList;
                viewModel.messageCount = ep.GetUnreadMessageCount(pubMessageList);
                return View(viewModel);
            } else
            {
                return Redirect(ep.GetLogoutUrl());
            }
             
        }

        public ActionResult vertarea(long? taskID)
        {
            if (Session["USER_ID"] != null)
            {
                if (Session["CURRENT_COMU"] != null)
                {
                    if (taskID != null)
                    {
                        long userId = (long)Session["USER_ID"];
                        long communityAct = Convert.ToInt64(Session["CURRENT_COMU"]);
                        user curUser = entities.users.Find(userId);
                        task viewTask = entities.tasks.Find(taskID);

                        if (viewTask != null)
                        {                            
                            List<ShowMessage> pubMessageList = ep.GetChatMessages(userId);
                            List<taskcomment> commentList = entities.taskcomments.Where(m => m.task_id == taskID).ToList();                            

                            vertareaViewModel viewModel = new vertareaViewModel();
                            communityList = ep.GetCommunityList(userId);
                            viewModel.communityList = communityList;

                            viewModel.side_menu = "task_process";
                            viewModel.side_sub_menu = "task_process_ver";
                             viewModel.document_category_list = entities.document_type.Where(x => x.community_id == communityAct).ToList();
                            viewModel.viewTask = viewTask;
                            viewModel.taskcommentList = commentList;
                            viewModel.curUser = curUser;
                            viewModel.pubTaskList = ep.GetNotifiTaskList(userId);
                            viewModel.pubMessageList = pubMessageList;
                            viewModel.messageCount = ep.GetUnreadMessageCount(pubMessageList);
                            return View(viewModel);
                        }
                        else
                        {
                            return Redirect(Url.Action("listado", "tareas", new { area = "coadmin", Error = "No existe ese elemento" }));
                        }                        
                    }
                    else
                    {
                        return Redirect(Url.Action("listado", "tareas", new { area = "coadmin"}));
                    }
                }
                else
                {
                    return Redirect(Url.Action("listado", "tareas", new { area = "coadmin", Error = "No permitido. Usted no administra ninguna comunidad. Comuníquese con el Webmaster..." }));
                }                                 
            } else
            {
                return Redirect(ep.GetLogoutUrl());
            }
        }

        [HttpPost]
        public ActionResult SeacrhResult(string searchStr)
        {
            return Redirect(Url.Action("listado", "tareas", new { area = "coadmin", searchStr = searchStr }));
        }

        [HttpPost]
        public ActionResult SeacrhCompleteResult(string searchStr)
        {
            return Redirect(Url.Action("completadas", "tareas", new { area = "coadmin", searchStr = searchStr }));
        }

        public JsonResult DeleteTask(long delID)
        {
            try
            {
                task delTask = entities.tasks.Find(delID);
                entities.tasks.Remove(delTask);
                entities.SaveChanges();
                return Json(new { result = "success" });
            } catch(Exception ex)
            {
                return Json(new { result = "error", exception = ex.HResult });
            }
        }

        [HttpPost]
        public ActionResult newComment(string comment, long taskId)
        {           
            try
            {
                long userId = (long)Session["USER_ID"];
                taskcomment newTaskc = new taskcomment();
                newTaskc.comment = comment;
                newTaskc.task_id = taskId;
                newTaskc.user_id = userId;
                newTaskc.created_at = DateTime.Now;
                entities.taskcomments.Add(newTaskc);
                entities.SaveChanges();
                return Redirect(Url.Action("vertarea", "tareas", new { area = "coadmin", taskID = taskId }));
            }
             catch (Exception ex)
            {
                return Redirect(Url.Action("vertarea", "tareas",
                    new
                    {
                        area = "coadmin",
                        taskID = taskId,
                        exception = ex.Message
                    }));
            }
        }

        [HttpPost]
        public ActionResult changeStatusTask(long? id, int status)
        {
            try
            {
                task editTask = entities.tasks.Find(id);
                editTask.status = status;
                entities.SaveChanges();
                return Json(new { result = "success" }); 
            }
            catch
            {
                return Json(new { result = "error" }); 
            }
        }
    }
}