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
    public class tareasController : Controller
    {
        pjrdev_condominiosEntities entities = new pjrdev_condominiosEntities();
        EFPublicRepository ep = new EFPublicRepository();

        List<Titulo> titulosList = new List<Titulo>();
        List<community> listComunities = new List<community>();

        // GET: tareas
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
                    if (searchStr != "")
                    {
                        var query = (from r in entities.tasks
                                     where r.task_name.Contains(searchStr) == true
                                    && r.community_id == communityAct && r.status != 3
                                     select r);
                        taskList = query.ToList();
                    }
                    else
                    {
                        var query = (from r in entities.tasks where r.community_id == communityAct && r.status != 3 select r);
                        taskList = query.ToList();
                    }
                    tareasViewModel viewModel = new tareasViewModel();

                    titulosList = ep.GetTitulosByTitular(userId);
                    listComunities = ep.GetCommunityListByTitular(titulosList);
                    viewModel.communityList = listComunities;             
                    viewModel.side_menu = "tareas";
                    viewModel.side_sub_menu = "tareas_listado";
                    viewModel.taskList = taskList;
                     viewModel.document_category_list = entities.document_type.Where(x => x.community_id == communityAct).ToList();
                    viewModel.curUser = curUser;
                    viewModel.searchStr = searchStr;
                    viewModel.pubMessageList = pubMessageList;
                    viewModel.pubTaskList = ep.GetNotifiTaskList(userId);
                    viewModel.messageCount = ep.GetUnreadMessageCount(pubMessageList);
                    ViewBag.msgError = Error;
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

        public ActionResult completadas(string Error, string searchStr = "")
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
                    if (searchStr != "")
                    {
                        var query = (from r in entities.tasks
                                     where r.task_name.Contains(searchStr) == true && r.community_id == communityAct
                                     && r.completed_date != null
                                     select r);
                        taskList = query.ToList();
                    }
                    else
                    {
                        var query = (from r in entities.tasks
                                     where r.community_id == communityAct
                                     && r.completed_date != null
                                     select r);
                        taskList = query.ToList();
                    }                    
                                        
                    tareasViewModel viewModel = new tareasViewModel();
                    titulosList = ep.GetTitulosByTitular(userId);
                    listComunities = ep.GetCommunityListByTitular(titulosList);
                    viewModel.communityList = listComunities;          
                    viewModel.side_menu = "tareas";
                    viewModel.side_sub_menu = "tareas_completadas";
                    viewModel.taskList = taskList;
                     viewModel.document_category_list = entities.document_type.Where(x => x.community_id == communityAct).ToList();
                    viewModel.curUser = curUser;
                    viewModel.pubTaskList = ep.GetNotifiTaskList(userId);
                    viewModel.pubMessageList = pubMessageList;
                    viewModel.messageCount = ep.GetUnreadMessageCount(pubMessageList);
                    ViewBag.msgError = Error;
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

        public ActionResult vertarea(long id)
        {
            if (Session["USER_ID"] != null)
            {
                try
                {
                    long communityAct = Convert.ToInt64(Session["CURRENT_COMU"]);
                    long userId = (long)Session["USER_ID"];                    
                    user curUser = entities.users.Find(userId);
                    List<ShowMessage> pubMessageList = ep.GetChatMessages(userId);
                    List<TaskVerItem> taskcommentList = new List<TaskVerItem>();
                    List<taskcomment> commentList = entities.taskcomments
                        .Where(m => m.task_id == id).ToList();
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
                        list.Remove(id.ToString());
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
                    tareasViewModel viewModel = new tareasViewModel();
                    titulosList = ep.GetTitulosByTitular(userId);
                    listComunities = ep.GetCommunityListByTitular(titulosList);
                    viewModel.communityList = listComunities;           
                    viewModel.side_menu = "tareas";
                    viewModel.side_sub_menu = "tareas_vertarea";
                     viewModel.document_category_list = entities.document_type.Where(x => x.community_id == communityAct).ToList();
                    viewModel.curUser = curUser;
                    viewModel.pubTaskList = ep.GetNotifiTaskList(userId);
                    viewModel.viewTask = entities.tasks.Find(id);
                    viewModel.pubMessageList = pubMessageList;
                    viewModel.messageCount = ep.GetUnreadMessageCount(pubMessageList);
                    viewModel.taskcommentList = taskcommentList;
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

        public ActionResult sugerirtarea(string Error)
        {
            if (Session["USER_ID"] != null)
            {
                if (Session["CURRENT_COMU"] != null)
                {
                    try
                    {
                        long communityAct = Convert.ToInt64(Session["CURRENT_COMU"]);
                        long userId = (long)Session["USER_ID"];                       
                        user curUser = entities.users.Find(userId);
                        tareasViewModel viewModel = new tareasViewModel();
                        titulosList = ep.GetTitulosByTitular(userId);
                        listComunities = ep.GetCommunityListByTitular(titulosList);
                        viewModel.communityList = listComunities;
                        viewModel.side_menu = "tareas";
                        viewModel.side_sub_menu = "tareas_sugerirtarea";
                         viewModel.document_category_list = entities.document_type.Where(x => x.community_id == communityAct).ToList();
                        viewModel.curUser = curUser;
                        viewModel.pubTaskList = ep.GetNotifiTaskList(userId);
                        viewModel.pubMessageList = ep.GetChatMessages(userId);
                        ViewBag.msgError = Error;
                        return View(viewModel);
                    }
                    catch (Exception ex)
                    {
                        return Redirect(Url.Action("Index", "Error"));
                    }
                }
                else
                {                
                    return Redirect(Url.Action("listado", "tareas", new { Error = "No puede sugerir tareas, usted no pertenece a ninguna comunidad." }));
                }
                
            } else
            {
                return Redirect(Url.Action("iniciar", "iniciar"));
            }
                
        }

        [HttpPost]
        public ActionResult newtask(string task_name, string description,
            string responsable, string task_date, string task_time, string comments, string estimated_date, string estimated_time
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
                newTask.share = 1;
                newTask.status = 4;
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
                return Redirect(Url.Action("listado", "tareas"));
            } catch(Exception)
            {
                return Redirect(Url.Action("sugerirtarea", "tareas", new { Error = "No se pudo guardar la tarea" }));
            }
        }

        [HttpPost]
        public ActionResult SeacrhResult(string searchStr)
        {
            return Redirect(Url.Action("listado", "tareas", new { searchStr = searchStr }));
        }

        [HttpPost]
        public ActionResult SeacrhResultC(string searchStr)
        {
            return Redirect(Url.Action("completadas", "tareas", new { searchStr = searchStr }));
        }
    }
}