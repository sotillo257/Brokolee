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
    public class usoController : Controller
    {
        pjrdev_condominiosEntities entities = new pjrdev_condominiosEntities();
        EFPublicRepository ep = new EFPublicRepository();
        // GET: webmaster/uso
        public ActionResult privacidad()
        {
            if (Session["USER_ID"] != null)
            {
                long userId = (long)Session["USER_ID"];
                user curUser = entities.users.Find(userId);
                List<ShowMessage> pubMessageList = ep.GetChatMessages(userId);
                privacidadUsoViewModel viewModel = new privacidadUsoViewModel();
                List<uso> usoList = entities.usoes.Where(m => m.create_userid == userId).ToList();
                if (usoList.Count > 0)
                {
                    viewModel.uso = usoList.First();
                } else
                {
                    viewModel.uso = null;
                }
                viewModel.side_menu = "uso";
                viewModel.side_sub_menu = "uso_privacidad";
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

        public ActionResult terminos()
        {
            if (Session["USER_ID"] != null)
            {
                long userId = (long)Session["USER_ID"];
                user curUser = entities.users.Find(userId);
                List<ShowMessage> pubMessageList = ep.GetChatMessages(userId);
                terminosUsoViewModel viewModel = new terminosUsoViewModel();
                List<uso> usoList = entities.usoes.Where(m => m.create_userid == userId).ToList();
                if (usoList.Count > 0)
                {
                    viewModel.uso = usoList.First();
                } else
                {
                    viewModel.uso = null;
                }
                viewModel.side_menu = "uso";
                viewModel.side_sub_menu = "uso_privacidad";
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

        public ActionResult reservados()
        {
            if (Session["USER_ID"] != null)
            {
                long userId = (long)Session["USER_ID"];
                user curUser = entities.users.Find(userId);
                List<ShowMessage> pubMessageList = ep.GetChatMessages(userId);
                reservadosUsoViewModel viewModel = new reservadosUsoViewModel();
                List<uso> usoList = entities.usoes.Where(m => m.create_userid == userId).ToList();
                if (usoList.Count > 0)
                {
                    viewModel.uso = usoList.First();
                } else
                {
                    viewModel.uso = null;
                }
                viewModel.side_menu = "uso";
                viewModel.side_sub_menu = "uso_reservados";
                viewModel.document_category_list = entities.document_type.ToList();
                viewModel.pubTaskList = ep.GetNotifiTaskList(userId);
                viewModel.curUser = curUser;
                viewModel.pubMessageList = pubMessageList;
                viewModel.messageCount = ep.GetUnreadMessageCount(pubMessageList);
                return View(viewModel);
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
                }
                else
                {
                    uso newUso = new uso();
                    newUso.created_at = DateTime.Now;
                    newUso.updated_at = DateTime.Now;
                    newUso.aviso = aviso;
                    newUso.create_userid = userId;
                    entities.usoes.Add(newUso);
                }
                entities.SaveChanges();
                return Redirect(Url.Action("privacidad", "uso", new { area = "webmaster" }));
            }
            catch (Exception ex)
            {
                return Redirect(Url.Action("privacidad", "uso", new { area = "webmaster", exception = ex.Message }));
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
                }
                else
                {
                    uso newUso = new uso();
                    newUso.created_at = DateTime.Now;
                    newUso.updated_at = DateTime.Now;
                    newUso.terminos = terminos;
                    newUso.create_userid = userId;
                    entities.usoes.Add(newUso);
                }

                entities.SaveChanges();
                return Redirect(Url.Action("terminos", "uso", new { area = "webmaster" }));
            }
            catch (Exception ex)
            {
                return Redirect(Url.Action("terminos", "uso", new { area = "webmaster" , exception = ex.Message }));
            }
        }

        [HttpPost]
        public ActionResult editderechos(string derechos)
        {
            try
            {
                long userId = (long)Session["USER_ID"];
                List<uso> usoList = entities.usoes.Where(m => m.create_userid == userId).ToList();
                if (usoList.Count > 0)
                {
                    uso editUso = usoList.First();
                    editUso.updated_at = DateTime.Now;
                    editUso.reservados = derechos;
                }
                else
                {
                    uso newUso = new uso();
                    newUso.created_at = DateTime.Now;
                    newUso.updated_at = DateTime.Now;
                    newUso.reservados = derechos;
                    newUso.create_userid = userId;
                    entities.usoes.Add(newUso);
                }
                entities.SaveChanges();
                return Redirect(Url.Action("reservados", "uso", new { area = "webmaster" }));
            }
            catch (Exception ex)
            {
                return Redirect(Url.Action("reservados", "uso", new { area = "webmaster", exception = ex.Message }));
            }
        }
    }
}