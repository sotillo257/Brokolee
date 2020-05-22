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
        public ActionResult listado(string searchStr = "")
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
                    viewModel.document_category_list = entities.document_type.ToList();
                    viewModel.curUser = curUser;
                    viewModel.searchStr = searchStr;
                    viewModel.pubMessageList = pubMessageList;
                    viewModel.pubTaskList = ep.GetNotifiTaskList(userId);
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

        public ActionResult completadas(string searchString = "")
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
                        var query = (from r in entities.tasks
                                     where r.task_name.Contains(searchString) == true && r.community_id == communityAct
                                     && r.completed_date != null
                                     select r);
                        taskList = query.ToList();
                    }
                    else
                    {
                        taskList.Clear();
                    }                        
                    tareasViewModel viewModel = new tareasViewModel();
                    titulosList = ep.GetTitulosByTitular(userId);
                    listComunities = ep.GetCommunityListByTitular(titulosList);
                    viewModel.communityList = listComunities;          
                    viewModel.side_menu = "tareas";
                    viewModel.side_sub_menu = "tareas_completadas";
                    viewModel.taskList = taskList;
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
                    viewModel.document_category_list = entities.document_type.ToList();
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

        public ActionResult sugerirtarea()
        {
            if (Session["USER_ID"] != null)
            {
                if (Session["CURRENT_COMU"] != null)
                {
                    try
                    {
                        long userId = (long)Session["USER_ID"];                       
                        user curUser = entities.users.Find(userId);
                        tareasViewModel viewModel = new tareasViewModel();
                        titulosList = ep.GetTitulosByTitular(userId);
                        listComunities = ep.GetCommunityListByTitular(titulosList);
                        viewModel.communityList = listComunities;
                        viewModel.side_menu = "tareas";
                        viewModel.side_sub_menu = "tareas_sugerirtarea";
                        viewModel.document_category_list = entities.document_type.ToList();
                        viewModel.curUser = curUser;
                        viewModel.pubTaskList = ep.GetNotifiTaskList(userId);
                        viewModel.pubMessageList = ep.GetChatMessages(userId);
                        return View(viewModel);
                    }
                    catch (Exception ex)
                    {
                        return Redirect(Url.Action("Index", "Error"));
                    }
                }
                else
                {
                    return Redirect(Url.Action("listado", "tareas"));
                }
                
            } else
            {
                return Redirect(Url.Action("iniciar", "iniciar"));
            }
                
        }

        [HttpPost]
        public ActionResult newtask(string task_name, string description, string commentary,
            string responsable, string start_date, string start_time)
        {
            try
            {
                //string[] listStr = start_date.Split('-');
                //string date_str = listStr[0] + "/" + listStr[1] + "/" + listStr[2];
                task task = new task();
                task.comments = commentary;
                task.description = description;
                task.task_name = task_name;
                task.description = description;
                task.responsable = responsable;
                task.status = 2;
                task.community_id = Convert.ToInt64(Session["CURRENT_COMU"]);
                string datetime_str = start_date + " " + start_time;
                DateTime datetime = DateTime.ParseExact(datetime_str, "yyyy-MM-dd HH:mm", 
                    System.Globalization.CultureInfo.CurrentCulture);
                task.created_at = datetime;
                task.share = 1;
                entities.tasks.Add(task);
                entities.SaveChanges();
                long userId = (long)Session["USER_ID"];
                taskuser taskuser = entities.taskusers.Where(m => m.user_id == userId).FirstOrDefault();
                if (taskuser == null)
                {
                    taskuser = new taskuser();
                    taskuser.user_id = userId;
                    taskuser.task_list = task.id.ToString();
                    entities.taskusers.Add(taskuser);
                } else
                {
                    string taskList = taskuser.task_list;
                    if (taskList == null)
                    {
                        taskuser.task_list = task.id.ToString();
                    } else
                    {
                        taskuser.task_list = taskList + "," + task.id.ToString();
                    }
                }
                entities.SaveChanges();
                return Redirect(Url.Action("listado", "tareas"));
            } catch(Exception)
            {
                return Redirect(Url.Action("sugerirtarea", "tareas"));
            }
        }

        [HttpPost]
        public ActionResult SeacrhResult(string searchStr)
        {
            return Redirect(Url.Action("listado", "tareas", new { searchStr = searchStr }));
        }
    }
}