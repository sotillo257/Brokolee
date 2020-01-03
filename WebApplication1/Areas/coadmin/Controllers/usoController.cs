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
    public class usoController : Controller
    {
        pjrdev_condominiosEntities entities = new pjrdev_condominiosEntities();
        EFPublicRepository ep = new EFPublicRepository();
        // GET: coadmin/uso
        public ActionResult aviso()
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
                    var query = (from r in entities.usoes where r.create_userid == userId select r);
                    List<uso> usoList = new List<uso>();
                    usoList = query.ToList();
                    usoViewModel viewModel = new usoViewModel();
                    if (usoList.Count > 0)
                    {
                        viewModel.uso = usoList.First();
                    }
                    else
                    {
                        viewModel.uso = null;
                    }
                    viewModel.side_menu = "aviso";
                    viewModel.side_menu = "aviso_terminos";
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
        public ActionResult terminos()
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
                    var query = (from r in entities.usoes where r.create_userid == userId select r);
                    List<uso> usoList = new List<uso>();
                    usoList = query.ToList();
                    usoViewModel viewModel = new usoViewModel();
                    if (usoList.Count > 0)
                    {
                        viewModel.uso = usoList.First();
                    }
                    else
                    {
                        viewModel.uso = null;
                    }
                    viewModel.side_menu = "uso";
                    viewModel.side_menu = "uso_terminos";
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

        public ActionResult derechos()
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
                    usoViewModel viewModel = new usoViewModel();
                    List<uso> usoList = entities.usoes.Where(m => m.create_userid == userId).ToList();
                    if (usoList.Count > 0)
                    {
                        viewModel.uso = usoList.First();
                    }
                    else
                    {
                        viewModel.uso = null;
                    }
                    viewModel.side_menu = "uso";
                    viewModel.side_menu = "uso_derechos";
                    viewModel.document_category_list = entities.document_type.ToList();
                    viewModel.curUser = curUser;
                    viewModel.pubTaskList = ep.GetNotifiTaskList(userId);
                    viewModel.messageCount = ep.GetUnreadMessageCount(pubMessageList);
                    viewModel.pubMessageList = pubMessageList;
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
        public ActionResult editaviso(string aviso)
        {
            try
            {
                long userId = (long)Session["USER_ID"];
                List<uso> usoList = entities.usoes.Where(m => m.create_userid == userId).ToList();
                if (usoList.Count > 0)
                {
                    uso editUso = usoList.First();
                    editUso.updated_at = DateTime.Now;
                    editUso.aviso = aviso;
                } else
                {
                    uso newUso = new uso();
                    newUso.created_at = DateTime.Now;
                    newUso.updated_at = DateTime.Now;
                    newUso.aviso = aviso;
                    newUso.create_userid = userId;
                    entities.usoes.Add(newUso);    
                }
                entities.SaveChanges();
                return Redirect(Url.Action("aviso", "uso", new { area = "coadmin" }));
            } catch(Exception ex)
            {
                return Redirect(Url.Action("aviso", "uso", new { area = "coadmin", exceptio = ex.Message }));
            }
        }

        [HttpPost]
        public ActionResult editterminos(string terminos)
        {
            try
            {
                long userId = (long)Session["USER_ID"];
                List<uso> usoList = entities.usoes.Where(m => m.create_userid == userId).ToList();
                if (usoList.Count > 0)
                {
                    uso editUso = usoList.First();
                    editUso.updated_at = DateTime.Now;
                    editUso.terminos = terminos;
                } else
                {
                    uso newUso = new uso();
                    newUso.created_at = DateTime.Now;
                    newUso.updated_at = DateTime.Now;
                    newUso.terminos = terminos;
                    newUso.create_userid = userId;
                    entities.usoes.Add(newUso);
                }
                
                entities.SaveChanges();
                return Redirect(Url.Action("terminos", "uso", new { area = "coadmin" }));
            }
            catch(Exception ex)
            {
                return Redirect(Url.Action("terminos", "uso", new { area = "coadmin", exception = ex.Message }));
            }
        }

        [HttpPost]
        public ActionResult editderechos(string derechos)
        {
            try
            {
                long userId = (long)Session["USER_ID"];
                List<uso> usoList = entities.usoes.Where(m => m.create_userid == userId).ToList();
                if(usoList.Count > 0)
                {
                    uso editUso = usoList.First();
                    editUso.reservados = derechos;
                    editUso.updated_at = DateTime.Now;

                } else
                {
                    uso newUso = new uso();
                    newUso.created_at = DateTime.Now;
                    newUso.updated_at = DateTime.Now;
                    newUso.reservados = derechos;
                    newUso.create_userid = userId;
                    entities.usoes.Add(newUso);
                }
                entities.SaveChanges();
                return Redirect(Url.Action("derechos", "uso", new { area = "coadmin" }));
            }
            catch (Exception ex)
            {
                return Redirect(Url.Action("derechos", "uso", new { area = "coadmin" , exception = ex.Message }));
            }
        }
    }
}