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
        // GET: coadmin/tareas
        public ActionResult listado(string searchStr = "")
        {
            if (Session["USER_ID"] != null)
            {
                try
                {
                    long userId = 0;
                    if (Convert.ToInt32(Session["USER_ROLE"]) >= 1)
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
                    List<task> taskList = new List<task>();

                    long communityAct = Convert.ToInt64(Session["CURRENT_COMU"]);

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

                    tareasViewModel viewModel = new tareasViewModel();
                    List<user> titularList = new List<user>();
                    var query2 = (from r in entities.users
                                 where ( r.role == 4) && r.is_del != true
                                 select r);
                    titularList = query2.ToList();
                    viewModel.side_menu = "task_process";
                    viewModel.side_sub_menu = "task_process_listado";
                    viewModel.document_category_list = entities.document_type.ToList();
                    viewModel.taskList = taskList;
                    viewModel.searchStr = searchStr;
                    viewModel.curUser = curUser;
                    viewModel.pubMessageList = pubMessageList;
                    viewModel.messageCount = ep.GetUnreadMessageCount(pubMessageList);                    
                    viewModel.pubTaskList = ep.GetNotifiTaskList(userId);
                    viewModel.titularList = titularList;
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
                    if (Convert.ToInt32(Session["USER_ROLE"]) >= 1)
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
                    tareasagregarViewModel viewModel = new tareasagregarViewModel();
                    viewModel.side_menu = "task_process";
                    viewModel.side_sub_menu = "task_process_agregar";
                    viewModel.document_category_list = entities.document_type.ToList();
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
            if (Session["USER_ID"] != null)
            {
                if (taskID != null)
                {
                    long userId = (long)Session["USER_ID"];
                    user curUser = entities.users.Find(userId);
                    List<ShowMessage> pubMessageList = ep.GetChatMessages(userId);
                    task editTask = entities.tasks.Find(taskID);
                    editarTareasViewModel viewModel = new editarTareasViewModel();
                    viewModel.side_menu = "task_process";
                    viewModel.side_sub_menu = "task_process_editar";
                    viewModel.document_category_list = entities.document_type.ToList();
                    viewModel.editTask = editTask;
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
                if (searchStr == "")
                {
                    var query = (from r in entities.tasks where r.completed_date != null && r.community_id == communityAct select r);
                    taskList = query.ToList();
                } else
                {
                    var query1 = (from r in entities.tasks
                                  where r.completed_date != null && r.task_name.Contains(searchStr) == true && r.community_id == communityAct
                                  select r);
                }
                tareasViewModel viewModel = new tareasViewModel();
                viewModel.side_menu = "task_process";
                viewModel.side_sub_menu = "task_process_completadas";
                viewModel.document_category_list = entities.document_type.ToList();
                viewModel.searchStr = searchStr;
                viewModel.taskList = taskList;
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

        public ActionResult vertarea(long? taskID)
        {
            if (Session["USER_ID"] != null)
            {
                if (taskID != null)
                {
                    long userId = (long)Session["USER_ID"];
                    user curUser = entities.users.Find(userId);
                    task viewTask = entities.tasks.Find(taskID);
                    List<ShowMessage> pubMessageList = ep.GetChatMessages(userId);
                    List<TaskVerItem> taskcommentList = new List<TaskVerItem>();
                    List<taskcomment> commentList = entities.taskcomments
                        .Where(m => m.task_id == taskID).ToList();
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
                        list.Remove(taskID.ToString());
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

                    vertareaViewModel viewModel = new vertareaViewModel();
                    viewModel.side_menu = "task_process";
                    viewModel.side_sub_menu = "task_process_ver";
                    viewModel.document_category_list = entities.document_type.ToList();
                    viewModel.viewTask = viewTask;
                    viewModel.taskcommentList = taskcommentList;
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
    }
}