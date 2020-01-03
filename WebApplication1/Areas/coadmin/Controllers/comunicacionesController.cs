﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WebApplication1.Areas.coadmin.ViewModels;
using WebApplication1.Concrete;
using WebApplication1.Models;
using Westwind.Web.Mvc;

namespace WebApplication1.Areas.coadmin.Controllers
{
    public class comunicacionesController : Controller
    {
        pjrdev_condominiosEntities entities = new pjrdev_condominiosEntities();
        EFPublicRepository ep = new EFPublicRepository();
        // GET: coadmin/comunicaciones
        public ActionResult blog()
        {
            if (Session["USER_ID"] != null)
            {
                long userId = (long)Session["USER_ID"];
                user curUser = entities.users.Find(userId);
                List<ShowMessage> pubMessageList = ep.GetChatMessages(userId);
                List<document_type> document_category_list = entities.document_type.ToList();
                comunicacionesViewModel viewModel = new comunicacionesViewModel();
                viewModel.side_menu = "comunicaciones";
                viewModel.side_sub_menu = "comunicaciones_blog";
                viewModel.document_category_list = document_category_list;
                viewModel.curUser = curUser;
                viewModel.blogList = entities.blogs.Where(m => m.user_id == userId).ToList();
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

        public ActionResult agregarblog()
        {
            if (Session["USER_ID"] != null)
            {
                long userId = (long)Session["USER_ID"];
                user curUser = entities.users.Find(userId);
                List<ShowMessage> pubMessageList = ep.GetChatMessages(userId);
                agregarBlogViewModel viewModel = new agregarBlogViewModel();
                viewModel.side_menu = "comunicaciones";
                viewModel.side_sub_menu = "comunicaciones_agregarblog";
                viewModel.document_category_list = entities.document_type.ToList();
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
                    viewModel.side_menu = "comunicaciones";
                    viewModel.side_sub_menu = "comunicaciones_agregarcomentario";
                    viewModel.document_category_list = entities.document_type.ToList();
                    viewModel.curUser = curUser;
                    viewModel.blogID = Convert.ToInt64(blogID);
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
            }
            else
            {
                return Redirect(ep.GetLogoutUrl());
            }
        }

        public ActionResult susprivados()
        {
            if (Session["SUS_USER_ID"] != null)
            {
                long userId = (long)Session["SUS_USER_ID"];
                user curUser = entities.users.Find(userId);
                long suscribeID = (long)Session["SUS_ID"];
                user susUser = entities.users.Find(suscribeID);
                List<ShowMessage> pubMessageList = ep.GetChatMessages(userId);
                List<onlineuser> onlineUserList = entities.onlineusers
                    .Where(m => m.user_id != userId).ToList();
                
                List<document_type> document_category_list = entities.document_type.ToList();
                comunicacionesViewModel viewModel = new comunicacionesViewModel();
                viewModel.side_menu = "comunicaciones_susprivados";
                viewModel.side_sub_menu = "comunicaciones_susprivados";
                viewModel.document_category_list = document_category_list;
                viewModel.curUser = curUser;
                viewModel.susUser = susUser;
                viewModel.onlineUserList = onlineUserList;
                if (onlineUserList.Count > 0)
                {
                    onlineuser selUser = onlineUserList.First();
                    viewModel.selUser = selUser;
                    long selUserID = selUser.id;
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
                viewModel.communityName = ep.GetCommunityInfo(userId)[0];
                viewModel.communityApart = ep.GetCommunityInfo(userId)[1];
                return View(viewModel);
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
                    .Where(m => m.user_id != userId && m.is_online == true).ToList();
                List<document_type> document_category_list = entities.document_type.ToList();
                comunicacionesViewModel viewModel = new comunicacionesViewModel();
                viewModel.side_menu = "comunicaciones";
                viewModel.side_sub_menu = "comunicaciones_privados";
                viewModel.document_category_list = document_category_list;
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
                viewModel.communityName = ep.GetCommunityInfo(userId)[0];
                viewModel.communityApart = ep.GetCommunityInfo(userId)[1];
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
                    editarblogViewModel viewModel = new editarblogViewModel();
                    List<ShowMessage> pubMessageList = ep.GetChatMessages(userId);
                    user curUser = entities.users.Find(userId);
                    blog blog = entities.blogs.Find(blogID);
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
                    viewModel.communityName = ep.GetCommunityInfo(userId)[0];
                    viewModel.communityApart = ep.GetCommunityInfo(userId)[1];
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
                    viewModel.communityName = ep.GetCommunityInfo(userId)[0];
                    viewModel.communityApart = ep.GetCommunityInfo(userId)[1];
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
        public ActionResult newblogcomentario(string name, string comment, long blogID)
        {
            try
            {
                blogcomment blogcomment = new blogcomment();
                blogcomment.name = name;
                blogcomment.comment = comment;
                blogcomment.blog_id = blogID;
                blogcomment.postdate = DateTime.Now;
                entities.blogcomments.Add(blogcomment);
                entities.SaveChanges();
                return Redirect(Url.Action("verblog", "comunicaciones",
                    new { blogID = blogID, area = "coadmin" }));
            }
            catch (Exception)
            {
                return Redirect(Url.Action("agregarcomentario", "comunicaciones", 
                    new { blogID = blogID, area = "coadmin" }));
            }
        }

        [HttpPost]
        public ActionResult editblog(long editID, string title, string author, string content)
        {
            try
            {
                blog editBlog = entities.blogs.Find(editID);
                editBlog.title = title;
                editBlog.author = author;
                editBlog.content = content;
                entities.SaveChanges();
                return Redirect(Url.Action("blog", "comunicaciones", new { area = "coadmin" }));
            }
            catch
            {
                return Redirect(Url.Action("editarblog", "comunicaciones", 
                    new {
                        area = "coadmin",
                        blogID = editID
                    }));
            }
        }

        [HttpPost]
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
                return Redirect(Url.Action("blog", "comunicaciones", new { area = "coadmin" }));
            }
            catch (Exception)
            {
                return Redirect(Url.Action("agregarblog", "comunicaciones", new { area = "coadmin" }));
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
                    return Json(new {
                        result = "success",
                        fileLink = fileLink,
                        fileName = fileName
                    });
                } else
                {
                    return Json(new
                    {
                        result = "error"
                    });
                }
            } catch(Exception ex)
            {
                return Json(new { result = "error", exception = ex.Message });
            }
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
                string patialView = "~/Areas/coadmin/Views/comunicaciones/_chatnotification.cshtml";
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
                string patialView = "~/Areas/coadmin/Views/comunicaciones/_chatbox.cshtml";
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
                string patialView = "~/Areas/coadmin/Views/comunicaciones/_chatbox.cshtml";
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
                string patialView = "~/Areas/coadmin/Views/comunicaciones/_chatbox.cshtml";
                string postsHtml = ViewRenderer.RenderPartialView(patialView, viewModel);
                return Json(new
                {
                    result = "success",
                    imgpath = imgpath,
                    imgtype = imgtype,
                    html = postsHtml,
                    online = online,
                    selUserId = uID.ToString(),
                    name = selUser.first_name + " " + selUser.last_name+":Graras4"
                });

            //}
            //catch (Exception ex)
            //{
            //    return Json(new
            //    {
            //        result = "error",
            //        exception = ex.Message
            //    });
            //}
        }

    }
}