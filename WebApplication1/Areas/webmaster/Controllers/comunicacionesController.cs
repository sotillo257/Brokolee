﻿using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WebApplication1.Areas.webmaster.ViewModels;
using WebApplication1.Concrete;
using WebApplication1.Models;
using Westwind.Web.Mvc;

namespace WebApplication1.Areas.webmaster.Controllers
{
    public class comunicacionesController : Controller
    {
        pjrdev_condominiosEntities entities = new pjrdev_condominiosEntities();
        EFPublicRepository ep = new EFPublicRepository();
        // GET: webmaster/comunicaciones
        public ActionResult blog()
        {
            if (Session["USER_ID"] != null)
            {
                long userId = (long)Session["USER_ID"];
                user curUser = entities.users.Find(userId);
                List<ShowMessage> pubMessageList = ep.GetChatMessages(userId);
                blogComViewModel viewModel = new blogComViewModel();
                viewModel.side_menu = "comunicaciones";
                viewModel.side_sub_menu = "comunicaciones_blog";
                viewModel.document_category_list = entities.document_type.ToList();
                viewModel.curUser = curUser;
                viewModel.blogList = entities.blogs.Where(m => m.user_id == userId).ToList();
                viewModel.Content = userId.ToString();
                List<blog> blogs = entities.blogs.Where(m => m.user_id == userId).ToList();               
              //  ViewBag.blogList = JsonConvert.SerializeObject(blogs);
                viewModel.pubTaskList = ep.GetNotifiTaskList(userId);
                viewModel.pubMessageList = pubMessageList;
                viewModel.messageCount = ep.GetUnreadMessageCount(pubMessageList);
                return View(viewModel);
            } else
            {
                return Redirect(ep.GetLogoutUrl());

            }
                
        }

        public ActionResult agregarblog()
        {
            if (Session["USER_ID"] != null)
            {
                long userId = (long)Session["USER_ID"];
                user curUser = entities.users.Find(userId);
                List<ShowMessage> pubMessageList = ep.GetChatMessages(userId);
                agregarBlogViewModel viewModel = new agregarBlogViewModel();
                viewModel.side_menu = "agregarblog";
                viewModel.side_sub_menu = "agregarblog";
                viewModel.document_category_list = entities.document_type.ToList();
                viewModel.curUser = curUser;
                viewModel.pubTaskList = ep.GetNotifiTaskList(userId);
                viewModel.pubMessageList = pubMessageList;
                viewModel.messageCount = ep.GetUnreadMessageCount(pubMessageList);
                return View(viewModel);
            }
            else
            {
                return Redirect(ep.GetLogoutUrl());
            }

        }

        public ActionResult agregarcomentario(long? blogID)
        {
            if (Session["USER_ID"] != null)
            {
                if (blogID != null)
                {
                    long userId = (long)Session["USER_ID"];
                    user curUser = entities.users.Find(userId);
                    List<ShowMessage> pubMessageList = ep.GetChatMessages(userId);
                    agregarComentarioViewModel viewModel = new agregarComentarioViewModel();
                    viewModel.side_menu = "agregarcomentario";
                    viewModel.side_sub_menu = "agregarcomentario";
                    viewModel.document_category_list = entities.document_type.ToList();
                    viewModel.curUser = curUser;
                    viewModel.blogID = Convert.ToInt64(blogID);
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

        public ActionResult privados()
        {
            if (Session["USER_ID"] != null)
            {
                long userId = (long)Session["USER_ID"];
                user curUser = entities.users.Find(userId);
                List<ShowMessage> pubMessageList = ep.GetChatMessages(userId);
                List<onlineuser> onlineUserList = entities.onlineusers
                    .Where(m => m.user_id != userId).ToList();
                privadosComViewModel viewModel = new privadosComViewModel();
                viewModel.side_menu = "comunicaciones";
                viewModel.side_sub_menu = "comunicaciones_privados";
                viewModel.document_category_list = entities.document_type.ToList();
                viewModel.curUser = curUser;
                viewModel.onlineUserList = onlineUserList;
                if (onlineUserList.Count > 0)
                {
                    onlineuser selUser = onlineUserList.First();
                    viewModel.selUser = selUser;
                    long selUserID = selUser.user_id;
                    viewModel.chatmessageList = entities.chatmessages.Where(
                        m => (m.from_user_id == userId && m.to_user_id == selUserID)
                        || (m.from_user_id == selUserID && m.to_user_id == userId)).ToList();
                }
                else
                {
                    viewModel.selUser = null;
                    viewModel.chatmessageList = null;
                }
                viewModel.pubTaskList = ep.GetNotifiTaskList(userId);
                viewModel.pubMessageList = pubMessageList;
                viewModel.messageCount = ep.GetUnreadMessageCount(pubMessageList);
                return View(viewModel);
            } else
            {
                return Redirect(ep.GetLogoutUrl());
            }      
        }

        public ActionResult editarblog(long? blogID)
        {
            if (Session["USER_ID"] != null)
            {
                if (blogID != null)
                {
                    long userId = (long)Session["USER_ID"];
                    List<ShowMessage> pubMessageList = ep.GetChatMessages(userId);
                    user curUser = entities.users.Find(userId);
                    blog blog = entities.blogs.Find(blogID);
                    editblogViewModel viewModel = new editblogViewModel();
                    viewModel.side_menu = "comunicaciones";
                    viewModel.side_sub_menu = "comunicaciones";
                    viewModel.document_category_list = entities.document_type.ToList();
                    viewModel.curUser = curUser;
                    viewModel.editBlog = blog;
                    viewModel.blogID = Convert.ToInt64(blogID);
                    viewModel.blogcommentList = entities.blogcomments.Where(m => m.blog_id == blogID).ToList();
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

        public ActionResult verblog(long? blogID)
        {
            if (Session["USER_ID"] != null)
            {
                if (blogID != null)
                {
                    long userId = (long)Session["USER_ID"];
                    List<ShowMessage> pubMessageList = ep.GetChatMessages(userId);
                    user curUser = entities.users.Find(userId);
                    blog blog = entities.blogs.Find(blogID);
                    verblogViewModel viewModel = new verblogViewModel();
                    viewModel.side_menu = "comunicaciones";
                    viewModel.side_sub_menu = "comunicaciones";
                    viewModel.document_category_list = entities.document_type.ToList();
                    viewModel.curUser = curUser;
                    viewModel.viewBlog = blog;
                    viewModel.blogID = Convert.ToInt64(blogID);
                    viewModel.blogcommentList = entities.blogcomments.Where(m => m.blog_id == blogID).ToList();
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
        public ActionResult newblogcomentario(string comment, long blogID)
        {
            try
            {
                blogcomment blogcomment = new blogcomment();
                blog blog = entities.blogs.Find(blogID);
                blogcomment.name = blog.author;
                blogcomment.comment = comment;
                blogcomment.blog_id = blogID;
                blogcomment.postdate = DateTime.Now;
                entities.blogcomments.Add(blogcomment);
                entities.SaveChanges();
                return Redirect(Url.Action("verblog", "comunicaciones", 
                    new { blogID = blogID }));
            }
            catch (Exception)
            {
                return Redirect(Url.Action("agregarcomentario", "comunicaciones",
                    new { blogID = blogID }));
            }
        }

        [HttpPost]
        [ValidateInput(false)]
        public ActionResult editblog(long editID, string title, string author, string content)
        {
            try
            {
                long userId = (long)Session["USER_ID"];
                blog blog = entities.blogs.Find(editID);
                blog.title = title;
                blog.content = content;
                blog.blogdate = DateTime.Now;
                blog.author = author;
                blog.user_id = userId;
               // entities.blogs.Add(blog);
                entities.SaveChanges();
                return Redirect(Url.Action("blog", "comunicaciones", new { area="webmaster" }));
            }
            catch(Exception ex)
            {
                return Redirect(Url.Action("editarblog", "comunicaciones", new { area="webmaster" }));
            }
        }

        [HttpPost]
        [ValidateInput(false)]
        public ActionResult newblog(string title, string author, string content)
        {
            try
            {
                long userId = (long)Session["USER_ID"];
                blog blog = new blog();
                blog.title = title;
                blog.content = content;
                blog.blogdate = DateTime.Now;
                blog.author = author;
                blog.user_id = userId;
                entities.blogs.Add(blog);
                entities.SaveChanges();
                return Redirect(Url.Action("blog", "comunicaciones"));
            }
            catch (Exception)
            {
                return Redirect(Url.Action("agregarblog", "comunicaciones"));
            }
        }

        [HttpPost]
        public JsonResult ArmarBlogs(int ID) {
            string content = "";
            List<blog> blogs = entities.blogs.Where(m => m.user_id == ID).ToList();
            foreach (var item in blogs)
            {
                
                content += "<div class='Container'><div class='row'><div class='col-sm-12'><div class='single-blog caja'><p class='fecha'>" + item.author + " <span>";
                content += "<td>" + Convert.ToDateTime(item.blogdate).ToString("dd/MM/yyyy HH:mm") + "</td></span></p><h2><a class='titulo' href='#'>" + item.title + "</a></h2>";
                content += item.content.Replace('"', '\'');
                content += "<p class='fecha'><a href='#' ><a href = '" + Url.Action("editarblog", "comunicaciones", new { area = "webmaster", blogID = item.id }).ToString() + "'><button type = 'button' class='btn btn-primary waves-effect waves-light btn-sm mr-1 mb-1'";
                content += "data-toggle='tooltip' data-placement='top' title='Editar'><i class='mdi mdi-lead-pencil'></i></button></a>";
                content += "<a href = '" + Url.Action("verblog", "comunicaciones", new { blogID = item.id, area = "webmaster" }).ToString() + "'>";
                content += "<button type = 'button' class='btn btn-success waves-effect waves-light btn-sm mr-1 mb-1' data-toggle='tooltip' data-placement='top' title='Ver'>";
                content += "<i class='mdi mdi-eye'></i></button></a></a><span><i class='fa fa-thumbs-o-up'></i> likes <i class='fa fa-comment-o'></i> Coments</span> </p></div></div></div></div>";


            }


            return Json(content);        
        }

        public JsonResult SetRead(long fromUserID, long toUserID)
        {
            try
            {
                List<chatmessage> chatmessageList = entities.chatmessages.Where(m => m.from_user_id == fromUserID
                                     && m.to_user_id == toUserID && m.is_read != true).ToList();

                foreach (var item in chatmessageList)
                {
                    item.is_read = true;
                }
                entities.SaveChanges();

                long userId = (long)Session["USER_ID"];
                List<ShowMessage> pubMessageList = ep.GetChatMessages(userId);
                ChatNotificationViewModel viewModel = new ChatNotificationViewModel();
                viewModel.pubMessageList = pubMessageList;
                viewModel.messageCount = ep.GetUnreadMessageCount(pubMessageList);
                string patialView = "~/Areas/webmaster/Views/comunicaciones/_chatnotification.cshtml";
                string postsHtml = ViewRenderer.RenderPartialView(patialView, viewModel);
                return Json(new
                {
                    result = "success",
                    html = postsHtml
                });
            }
            catch (Exception ex)
            {
                return Json(new
                {
                    result = "error",
                    exception = ex.Message
                });
            }
        }

        public JsonResult SendFile(HttpPostedFileBase sendFile)
        {
            try
            {
                string fileLink = "";
                string fileName = "";
                if (sendFile != null && sendFile.ContentLength > 0)
                {
                    fileName = Path.GetFileName(sendFile.FileName);
                    if (!Directory.Exists(Path.Combine(Server.MapPath("~/"), "Upload")))
                    {
                        Directory.CreateDirectory(Path.Combine(Server.MapPath("~/"), "Upload"));
                    }

                    if (!Directory.Exists(Path.Combine(Server.MapPath("~/Upload/"), "Upload_Send")))
                    {
                        Directory.CreateDirectory(Path.Combine(Server.MapPath("~/Upload/"), "Upload_Send"));
                    }
                    var path = Path.Combine(Server.MapPath("~/Upload/Upload_Send"), fileName);
                    sendFile.SaveAs(path);
                    fileLink = "~/Upload/Upload_Send/" + fileName;
                    return Json(new
                    {
                        result = "success",
                        fileLink = fileLink,
                        fileName = fileName
                    });
                }
                else
                {
                    return Json(new
                    {
                        result = "error"
                    });
                }
            }
            catch (Exception ex)
            {
                return Json(new { result = "error", exception = ex.Message });
            }
        }

        public JsonResult DeleteMessage(long selUserId, long messageId)
        {
            long uID = selUserId;
            long curId = (long)Session["USER_ID"];
            List<chatmessage> chatmessageList = new List<chatmessage>();
            selUserViewModel viewModel = new selUserViewModel();
            try
            {
                chatmessage chatmessage = entities.chatmessages.Find(messageId);
                entities.chatmessages.Remove(chatmessage);
                entities.SaveChanges();
                user curUser = entities.users.Find(curId);
                onlineuser selUser = entities.onlineusers.Where(m => m.user_id == uID).FirstOrDefault();
                chatmessageList = entities.chatmessages.Where(
                        m => (m.from_user_id == curId && m.to_user_id == uID)
                        || (m.from_user_id == uID && m.to_user_id == curId)).ToList();
                viewModel.chatmessageList = chatmessageList;
                viewModel.curUser = curUser;
                viewModel.selUser = selUser;
                string patialView = "~/Areas/webmaster/Views/comunicaciones/_chatbox.cshtml";
                string postsHtml = ViewRenderer.RenderPartialView(patialView, viewModel);
                return Json(new
                {
                    result = "success",                   
                    html = postsHtml,
                });
            }
            catch (Exception ex)
            {
                user curUser = entities.users.Find(curId);
                onlineuser selUser = entities.onlineusers.Where(m => m.user_id == uID).FirstOrDefault();
                chatmessageList = entities.chatmessages.Where(
                        m => (m.from_user_id == curId && m.to_user_id == uID)
                        || (m.from_user_id == uID && m.to_user_id == curId)).ToList();
                viewModel.chatmessageList = chatmessageList;
                viewModel.curUser = curUser;
                viewModel.selUser = selUser;
                string patialView = "~/Areas/webmaster/Views/comunicaciones/_chatbox.cshtml";
                string postsHtml = ViewRenderer.RenderPartialView(patialView, viewModel);
                return Json(new
                {
                    result = "error",
                    html = postsHtml,
                    exception = ex.Message
                });
            }
        }

        public JsonResult GetSelUser(string userId)
        {
            //try
            //{
                int imgtype = 1;
                int online = 1;
                string imgpath = "user.svg";
                long uID = Convert.ToInt64(userId);
                onlineuser selUser = entities.onlineusers.Where(m => m.user_id == uID).FirstOrDefault();
                if (selUser.user_img != null)
                {
                    imgpath = selUser.user_img;
                    imgtype = 2;
                }

                if (selUser.is_online == false)
                {
                    online = 0;
                }

                List<chatmessage> chatmessageList = new List<chatmessage>();
                long curId = (long)Session["USER_ID"];
                user curUser = entities.users.Find(curId);

                chatmessageList = entities.chatmessages.Where(
                        m => (m.from_user_id == curId && m.to_user_id == uID)
                        || (m.from_user_id == uID && m.to_user_id == curId)).ToList();

                selUserViewModel viewModel = new selUserViewModel();
                viewModel.chatmessageList = chatmessageList;
                viewModel.curUser = curUser;
                viewModel.selUser = selUser;
                string patialView = "~/Areas/webmaster/Views/comunicaciones/_chatbox.cshtml";
                string postsHtml = ViewRenderer.RenderPartialView(patialView, viewModel);
                return Json(new
                {
                    result = "success",
                    imgpath = imgpath,
                    imgtype = imgtype,
                    online = online,
                    html = postsHtml,
                    name = selUser.first_name + " " + selUser.last_name
                });
    //    }
    //        catch (Exception ex)
    //        {
    //            return Json(new {
    //                result = "error",
    //                exception = ex.Message
    //});
    //        }
        }

        public JsonResult DeleteBlog(long id)
        {
            //try
            //{

            //List<communuser> communusers = entities.communusers.Where(m => m.commun_id == id).ToList();
            //entities.communusers.RemoveRange(communusers);


            //community delCommunity = entities.communities.Find(id);
            //entities.communities.Remove(delCommunity);
            //blogcomment blogcom = entities.blogcomments.Find(id);
            //entities.blogcomments.Remove(blogcom);
            List<blogcomment> blogcomments = entities.blogcomments.Where(m => m.blog_id == id).ToList();
            entities.blogcomments.RemoveRange(blogcomments);
            blog blogger = entities.blogs.Find(id);
                entities.blogs.Remove(blogger);

                entities.SaveChanges();
                return Json(new { result = "success" });
            //}
            //catch (Exception ex)
            //{
                return Json(new
                {
                    result = "error",
                    //exception = ex.Message
                });
            //}
        }
    }
}