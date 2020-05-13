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
    public class categoriasController : Controller
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
                List<category> catList = new List<category>();

                long communityAct = Convert.ToInt64(Session["CURRENT_COMU"]);

                if (searchStr == "")
                {
                    var query = (from r in entities.categories select r);
                    catList = query.ToList();
                }
                else
                {
                    var query1 = (from r in entities.categories
                                  where
                                  r.name.Contains(searchStr) == true
                                  select r);
                    catList = query1.ToList();
                }

                categoriesTareasViewModel viewModel = new categoriesTareasViewModel();
                viewModel.side_menu = "categorias";
                viewModel.side_sub_menu = "categorias_listado";
                viewModel.document_category_list = entities.document_type.ToList();
                viewModel.categoryList = catList;
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
                categoriesTareasViewModel viewModel = new categoriesTareasViewModel();
                viewModel.side_menu = "categorias";
                viewModel.side_sub_menu = "categorias_agregar";
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

        public ActionResult editar(long? editID)
        {
            if (Session["USER_ID"] != null)
            {
                if (editID != null)
                {
                    long userId = (long)Session["USER_ID"];
                    user curUser = entities.users.Find(userId);
                    List<ShowMessage> pubMessageList = ep.GetChatMessages(userId);
                    category editCat = entities.categories.Find(editID);
                    categoriesTareasViewModel viewModel = new categoriesTareasViewModel();
                    viewModel.side_menu = "categorias";
                    viewModel.side_sub_menu = "categorias_editar";
                    viewModel.document_category_list = entities.document_type.ToList();
                    viewModel.categ = editCat;
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
        public ActionResult editCatg(long editID, string name)
        {
            try
            {
                category editCat = entities.categories.Find(editID);
                editCat.name = name;                
                entities.SaveChanges();
                return Redirect(Url.Action("listado", "categorias", new { area = "webmaster" }));
            }
            catch (Exception ex)
            {
                return Redirect(Url.Action("editar", "categorias", 
                    new {
                        area = "webmaster",
                        editID = editID,
                        exception = ex.Message
                    }));
            }
        }

        [HttpPost]
        public ActionResult newCatg(string name)
        {
            try
            {               
                category newCat = new category();
                newCat.name = name;               
                entities.categories.Add(newCat);
                entities.SaveChanges();              
                return Redirect(Url.Action("listado", "categorias", new { area = "webmaster" }));
            }
            catch (Exception ex)
            {
                return Redirect(Url.Action("agregar", "categorias", 
                    new {
                        area = "webmaster",
                        exception = ex.Message
                    }));
            }
        }

        [HttpPost]
        public ActionResult SeacrhResult(string searchStr)
        {
            return Redirect(Url.Action("listado", "categorias", 
                new {
                    area = "webmaster",
                    searchStr = searchStr
                }));
        }       

        public JsonResult DeleteCatg(long delID)
        {
            try
            {
                List<supplier> supplidr = new List<supplier>();
                var query1 = (from r in entities.suppliers
                              where
                              r.category_id == delID
                              select r);
                supplidr = query1.ToList();

                foreach (var item in supplidr)
                {
                    List<comment> comm = new List<comment>();
                    var query2 = (from r in entities.comments
                                  where
                                  r.supplier_id == item.id
                                  select r);
                    comm = query2.ToList();

                    entities.comments.RemoveRange(comm);
                    entities.SaveChanges();
                }
                
                entities.suppliers.RemoveRange(supplidr);
                entities.SaveChanges();
                category delCat = entities.categories.Find(delID);
                entities.categories.Remove(delCat);
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