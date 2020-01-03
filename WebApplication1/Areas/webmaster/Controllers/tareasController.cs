using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WebApplication1.Areas.webmaster.ViewModels;
using WebApplication1.Concrete;
using WebApplication1.Models;

namespace WebApplication1.Areas.webmaster.Controllers
{
    public class tareasController : Controller
    {
        pjrdev_condominiosEntities entities = new pjrdev_condominiosEntities();
        EFPublicRepository ep = new EFPublicRepository();
        // GET: webmaster/tareas
        public ActionResult listado(string searchStr = "")
        {
            if (Session["USER_ID"] != null)
            {
                long userId = (long)Session["USER_ID"];
                user curUser = entities.users.Find(userId);
                List<ShowMessage> pubMessageList = ep.GetChatMessages(userId);
                List<task> taskList = new List<task>();

                if (searchStr == "")
                {
                    var query = (from r in entities.tasks select r);
                    taskList = query.ToList();
                }
                else
                {
                    var query1 = (from r in entities.tasks
                                  where
                                  r.task_name.Contains(searchStr) == true
                                  select r);
                    taskList = query1.ToList();
                }

                listadoTareasViewModel viewModel = new listadoTareasViewModel();
                viewModel.side_menu = "tareas";
                viewModel.side_sub_menu = "tareas_listado";
                viewModel.document_category_list = entities.document_type.ToList();
                viewModel.taskList = taskList;
                viewModel.searchStr = searchStr;
                viewModel.curUser = curUser;
                viewModel.pubMessageList = pubMessageList;
                viewModel.messageCount = ep.GetUnreadMessageCount(pubMessageList);
                viewModel.pubTaskList = ep.GetNotifiTaskList(userId);
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
                List<ShowMessage> pubMessageList = ep.GetChatMessages(userId);
                agregarTareasViewModel viewModel = new agregarTareasViewModel();
                viewModel.side_menu = "tareas";
                viewModel.side_sub_menu = "tareas_agregar";
                viewModel.document_category_list = entities.document_type.ToList();
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

        public ActionResult completadas(string searchStr = "")
        {
            if (Session["USER_ID"] != null)
            {
                long userId = (long)Session["USER_ID"];
                user curUser = entities.users.Find(userId);
                List<ShowMessage> pubMessageList = ep.GetChatMessages(userId);
                List<task> taskList = new List<task>();
                if (searchStr == "")
                {
                    var query = (from r in entities.tasks where r.status == 2 select r);
                    taskList = query.ToList();
                }
                else
                {
                    var query1 = (from r in entities.tasks
                                  where r.status == 2 && r.task_name.Contains(searchStr) == true
                                  select r);
                }
                completadasTareasViewModel viewModel = new completadasTareasViewModel();
                viewModel.side_menu = "tareas";
                viewModel.side_sub_menu = "tareas_completadas";
                viewModel.document_category_list = entities.document_type.ToList();
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

        public ActionResult editar(long? editID)
        {
            if (Session["USER_ID"] != null)
            {
                if (editID != null)
                {
                    long userId = (long)Session["USER_ID"];
                    user curUser = entities.users.Find(userId);
                    List<ShowMessage> pubMessageList = ep.GetChatMessages(userId);
                    task editTask = entities.tasks.Find(editID);
                    editarTareasViewModel viewModel = new editarTareasViewModel();
                    viewModel.side_menu = "tareas";
                    viewModel.side_sub_menu = "tareas_editar";
                    viewModel.document_category_list = entities.document_type.ToList();
                    viewModel.editTask = editTask;
                    viewModel.curUser = curUser;
                    viewModel.pubTaskList = ep.GetNotifiTaskList(userId);
                    viewModel.pubMessageList = pubMessageList;
                    viewModel.messageCount = ep.GetUnreadMessageCount(pubMessageList);
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

        public ActionResult ver(long? viewID)
        {
            if (Session["USER_ID"] != null)
            {
                if (viewID != null)
                {
                    long userId = (long)Session["USER_ID"];
                    user curUser = entities.users.Find(userId);
                    List<ShowMessage> pubMessageList = ep.GetChatMessages(userId);
                    List<TaskVerItem> taskcommentList = new List<TaskVerItem>();
                    List<taskcomment> commentList = entities.taskcomments
                        .Where(m => m.task_id == viewID).ToList();
                    task viewTask = entities.tasks.Find(viewID);
                    foreach (var item in commentList)
                    {
                        TaskVerItem taskVerItem = new TaskVerItem();
                        taskVerItem.comment_datetime = item.created_at;
                        taskVerItem.task_comment = item.comment;
                        long taskUserId = item.user_id;
                        user taskUser = entities.users.Find(taskUserId);
                        taskVerItem.comment_username = taskUser.first_name1 + " " + taskUser.last_name1;
                        taskcommentList.Add(taskVerItem);
                    }
                    taskuser taskuser = entities.taskusers.Where(m => m.user_id == userId).FirstOrDefault();
                    string taskList = taskuser.task_list;
                    if (taskList != null)
                    {
                        string[] strList = taskList.Split(',');
                        var list = new List<string>(strList);
                        list.Remove(viewID.ToString());
                        string tempListStr = "";

                        foreach (var item in list)
                        {
                            if (item == list.Last())
                            {
                                tempListStr += item;
                            }
                            else
                            {
                                tempListStr += item + ",";
                            }
                        }

                        if (tempListStr == "")
                        {
                            taskuser.task_list = null;
                        }
                        else
                        {
                            taskuser.task_list = tempListStr;
                        }

                        entities.SaveChanges();
                    }
                    verTareasViewModel viewModel = new verTareasViewModel();
                    viewModel.side_menu = "tareas";
                    viewModel.side_sub_menu = "tareas_ver";
                    viewModel.document_category_list = entities.document_type.ToList();
                    viewModel.viewTask = viewTask;
                    viewModel.curUser = curUser;
                    viewModel.pubTaskList = ep.GetNotifiTaskList(userId);
                    viewModel.pubMessageList = pubMessageList;
                    viewModel.messageCount = ep.GetUnreadMessageCount(pubMessageList);
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

        [HttpPost]
        public ActionResult edittask(long editID, string task_name,
           string description, string responsable, string task_date,
           string estimated_date, string comments, int share, int status)
        {
            try
            {
                task editTask = entities.tasks.Find(editID);
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
                entities.SaveChanges();
                return Redirect(Url.Action("listado", "tareas", new { area = "webmaster" }));
            }
            catch (Exception ex)
            {
                return Redirect(Url.Action("editar", "tareas", 
                    new {
                        area = "webmaster",
                        editID = editID,
                        exception = ex.Message
                    }));
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
                return Redirect(Url.Action("listado", "tareas", new { area = "webmaster" }));
            }
            catch (Exception ex)
            {
                return Redirect(Url.Action("agregar", "tareas", 
                    new {
                        area = "webmaster",
                        exception = ex.Message
                    }));
            }
        }

        [HttpPost]
        public ActionResult SeacrhResult(string searchStr)
        {
            return Redirect(Url.Action("listado", "tareas", 
                new {
                    area = "webmaster",
                    searchStr = searchStr
                }));
        }

        [HttpPost]
        public ActionResult SeacrhCompleteResult(string searchStr)
        {
            return Redirect(Url.Action("completadas", "tareas", 
                new {
                    area = "webmaster",
                    searchStr = searchStr
                }));
        }

        public JsonResult DeleteTask(long delID)
        {
            try
            {
                task delTask = entities.tasks.Find(delID);
                entities.tasks.Remove(delTask);
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