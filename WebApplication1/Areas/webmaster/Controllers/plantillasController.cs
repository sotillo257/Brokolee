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
    public class plantillasController : Controller
    {
        pjrdev_condominiosEntities entities = new pjrdev_condominiosEntities();
        EFPublicRepository ep = new EFPublicRepository();
        // GET: webmaster/plantillas
        public ActionResult listado()
        {
            if (Session["USER_ID"] != null)
            {
                long userId = (long)Session["USER_ID"];
                user curUser = entities.users.Find(userId);
                List<ShowMessage> pubMessageList = ep.GetChatMessages(userId);
                List<emailtheme> emailthemeList = entities.emailthemes.Where(m => m.user_id == userId).ToList();
                listadoPlantillasViewModel viewModel = new listadoPlantillasViewModel();
                List<PlantillaItemViewModel> list = new List<PlantillaItemViewModel>();
                foreach (var item in emailthemeList)
                {
                    PlantillaItemViewModel plantillaItemViewModel = new PlantillaItemViewModel();
                    int typeID = item.type_id;
                    emailtype emailtype = entities.emailtypes.Find(typeID);
                    plantillaItemViewModel.TypeID = typeID;
                    plantillaItemViewModel.TypeName = emailtype.represent;
                    plantillaItemViewModel.Content = item.htmcontent;
                    plantillaItemViewModel.ID = item.id;
                    list.Add(plantillaItemViewModel);
                }
                viewModel.side_menu = "plantillas";
                viewModel.side_sub_menu = "plantillas_listado";
                
                viewModel.pubTaskList = ep.GetNotifiTaskList(userId);
                viewModel.curUser = curUser;
                viewModel.pubMessageList = pubMessageList;
                viewModel.messageCount = ep.GetUnreadMessageCount(pubMessageList);
                viewModel.list = list;
                return View(viewModel);
            } else
            {
                return Redirect(ep.GetLogoutUrl());
            }      
        }

        public ActionResult tema()
        {
            if (Session["USER_ID"] != null)
            {
                long userId = (long)Session["USER_ID"];
                user curUser = entities.users.Find(userId);
                List<ShowMessage> pubMessageList = ep.GetChatMessages(userId);
                agregarTemadeViewModel viewModel = new agregarTemadeViewModel();
                viewModel.side_menu = "plantillas";
                viewModel.side_sub_menu = "plantillas_tema";
                
                viewModel.pubTaskList = ep.GetNotifiTaskList(userId);
                viewModel.curUser = curUser;
                viewModel.pubMessageList = pubMessageList;
                viewModel.messageCount = ep.GetUnreadMessageCount(pubMessageList);
                return View(viewModel);
            }
            else
            {
                return Redirect(ep.GetLogoutUrl());
            }

        }

        public ActionResult crear()
        {
            if (Session["USER_ID"] != null)
            {
                long userId = (long)Session["USER_ID"];
                user curUser = entities.users.Find(userId);
                List<ShowMessage> pubMessageList = ep.GetChatMessages(userId);
                crearPlantillasViewModel viewModel = new crearPlantillasViewModel();
                viewModel.side_menu = "plantillas";
                viewModel.side_sub_menu = "plantillas_crear";
                viewModel.pubTaskList = ep.GetNotifiTaskList(userId);
                
                viewModel.curUser = curUser;
                viewModel.pubMessageList = pubMessageList;
                viewModel.typeList = entities.emailtypes.ToList();
                viewModel.messageCount = ep.GetUnreadMessageCount(pubMessageList);
                return View(viewModel);
            } else
            {
                return Redirect(ep.GetLogoutUrl());
            }       
        }

        [HttpPost, ValidateInput(false)]
        public ActionResult AddEmaulTheme(string typeID, string content)
        {
            try
            {
                long userID = (long)Session["USER_ID"];
                emailtheme emailtheme = new emailtheme();
                emailtheme.type_id = Convert.ToInt32(typeID);
                emailtheme.htmcontent = content;
                emailtheme.user_id = userID;
                entities.emailthemes.Add(emailtheme);
                entities.SaveChanges();
                return Redirect(Url.Action("listado", "plantillas", new { area = "webmaster" }));
            }
            catch (Exception ex)
            {
                return Redirect(Url.Action("crear", "plantillas", 
                    new {
                        area = "webmaster",
                        exception = ex.Message
                    }));
            }
        }

        [HttpPost, ValidateInput(false)]
        public ActionResult EditEmailTheme(string editID, string content)
        {
            try
            {
                long id = Convert.ToInt64(editID);
                emailtheme emailtheme = entities.emailthemes.Find(id);
                emailtheme.htmcontent = content;
                entities.SaveChanges();
                return Redirect(Url.Action("listado", "plantillas", new { area = "webmaster" }));
            }
            catch (Exception ex)
            {
                return Redirect(Url.Action("listado", "plantillas",
                    new {
                        area = "webmaster",
                        exception = ex.Message
                    }));
            }
        }
    }
}