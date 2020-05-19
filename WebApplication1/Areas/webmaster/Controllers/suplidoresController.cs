using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WebApplication1.Areas.webmaster.ViewModels;
using WebApplication1.Concrete;
using WebApplication1.Models;

namespace WebApplication1.Areas.webmaster.Controllers
{
    public class suplidoresController : Controller
    {
        pjrdev_condominiosEntities entities = new pjrdev_condominiosEntities();
        EFPublicRepository ep = new EFPublicRepository();
        // GET: webmaster/suplidores
        public ActionResult listado(string Error, string searchStr = "", int searchCategoryId = 0)
        {
            if (Session["USER_ID"] != null)
            {
                long userId = (long)Session["USER_ID"];
                user curUser = entities.users.Find(userId);
                List<ShowMessage> pubMessageList = ep.GetChatMessages(userId);
                List<supplier> supplierList = new List<supplier>();
                Dictionary<long, string> categoryDict = new Dictionary<long, string>();

                long communityAct = Convert.ToInt64(Session["CURRENT_COMU"]);

                if (searchStr == "" && searchCategoryId == 0)
                {
                    var query = (from r in entities.suppliers select r);
                    supplierList = query.ToList();
                }
                else if (searchStr != "" && searchCategoryId == 0)
                {
                    var query1 = (from r in entities.suppliers
                                  where r.contact_name.Contains(searchStr) == true
                                  select r);
                    supplierList = query1.ToList();
                }
                else if (searchStr == "" && searchCategoryId != 0)
                {
                    var query2 = (from r in entities.suppliers
                                  where r.category_id == searchCategoryId
                                  select r
                                  );
                    supplierList = query2.ToList();
                }
                else
                {
                    var query3 = (from r in entities.suppliers
                                  where r.contact_name.Contains(searchStr) == true &&
                                  r.category_id == searchCategoryId 
                                  select r);
                    supplierList = query3.ToList();
                }

                foreach (var item in supplierList)
                {
                    category category = entities.categories.Find(item.category_id);
                    categoryDict.Add(item.id, category.name);
                }
                listadoSuplidoresViewModel viewModel = new listadoSuplidoresViewModel();
                viewModel.side_menu = "suplidores";
                viewModel.side_sub_menu = "supplier_directory";
                viewModel.supplierList = supplierList;
                viewModel.categoryDict = categoryDict;
                viewModel.categoryList = entities.categories.ToList();
                viewModel.document_category_list = entities.document_type.ToList();
                viewModel.curUser = curUser;
                viewModel.searchStr = searchStr;
                viewModel.searchCategoryId = searchCategoryId;
                viewModel.pubTaskList = ep.GetNotifiTaskList(userId);
                viewModel.pubMessageList = pubMessageList;
                viewModel.messageCount = ep.GetUnreadMessageCount(pubMessageList);
                ViewBag.msgError = Error;
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
                    List<category> listC = entities.categories.ToList();
                    if (listC.Count > 0)
                    {
                        supplier editSupplier = entities.suppliers.Find(editID);
                        if (editSupplier != null)
                        {
                            long userId = (long)Session["USER_ID"];
                            user curUser = entities.users.Find(userId);
                            List<ShowMessage> pubMessageList = ep.GetChatMessages(userId);
                            editarSuplidoresViewModel viewModel = new editarSuplidoresViewModel();
                            viewModel.side_menu = "suplidores";
                            viewModel.side_sub_menu = "supplier_directory";
                            viewModel.editSupplier = editSupplier;
                            viewModel.document_category_list = entities.document_type.ToList();
                            viewModel.categoryList = entities.categories.ToList();
                            viewModel.curUser = curUser;
                            viewModel.pubTaskList = ep.GetNotifiTaskList(userId);
                            viewModel.pubMessageList = pubMessageList;
                            viewModel.messageCount = ep.GetUnreadMessageCount(pubMessageList);
                            return View(viewModel);
                        }
                        else
                        {
                            return Redirect(Url.Action("listado", "suplidores", new { area = "webmaster", Error = "No existe ese elemento" }));
                        }
                    }
                    else
                    {
                        return Redirect(Url.Action("listado", "suplidores", new { area = "webmaster", Error = "Se necesita crear una categoria para editar suplidores" }));
                    }                    
                    
                }
                else
                {
                    return Redirect(Url.Action("listado", "suplidores", new { area = "webmaster" }));
                }                
            } else
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
                    supplier supplier = entities.suppliers.Find(viewID);
                    if (supplier != null)
                    {
                        long userId = (long)Session["USER_ID"];
                        user curUser = entities.users.Find(userId);
                        List<ShowMessage> pubMessageList = ep.GetChatMessages(userId);
                        var query = (from r in entities.comments where r.supplier_id == viewID select r);                       
                        category category = entities.categories.Find(supplier.category_id);
                        verSuplidoresViewModel viewModel = new verSuplidoresViewModel();
                        viewModel.side_menu = "suplidores";
                        viewModel.side_sub_menu = "supplier_directory";
                        viewModel.commentList = query.ToList();
                        viewModel.viewSupplier = supplier;
                        viewModel.document_category_list = entities.document_type.ToList();
                        viewModel.supplierCategory = category;
                        viewModel.curUser = curUser;
                        viewModel.pubTaskList = ep.GetNotifiTaskList(userId);
                        viewModel.pubMessageList = pubMessageList;
                        viewModel.messageCount = ep.GetUnreadMessageCount(pubMessageList);
                        return View(viewModel);
                    }
                    else
                    {
                        return Redirect(Url.Action("listado", "suplidores", new { area = "webmaster", Error = "No existe ese elemento" }));
                    }
                   
                }
                else
                {
                    return Redirect(Url.Action("listado", "suplidores", new { area = "webmaster" }));
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
                List<category> listC = entities.categories.ToList();
                if (listC.Count > 0)
                {
                    long userId = (long)Session["USER_ID"];
                    user curUser = entities.users.Find(userId);
                    List<ShowMessage> pubMessageList = ep.GetChatMessages(userId);
                    agregarSuplidoresViewModel viewModel = new agregarSuplidoresViewModel();
                    viewModel.side_menu = "suplidores";
                    viewModel.side_sub_menu = "supplier_directory";
                    viewModel.document_category_list = entities.document_type.ToList();
                    viewModel.categoryList = entities.categories.ToList();
                    viewModel.curUser = curUser;
                    viewModel.pubTaskList = ep.GetNotifiTaskList(userId);
                    viewModel.pubMessageList = pubMessageList;
                    viewModel.messageCount = ep.GetUnreadMessageCount(pubMessageList);
                    return View(viewModel);
                }
                else
                {
                    return Redirect(Url.Action("listado", "suplidores", new { area = "webmaster", Error = "Se necesita crear una categoria para registrar suplidores" }));
                }
                
            }
            else
            {
                return Redirect(ep.GetLogoutUrl());
            }
        }

        [HttpPost]
        public ActionResult editsupplier(long editID, HttpPostedFileBase supply_logo,
           string company, string contact_name,
           string type_service, int category, string address,
           string phone, string email,
           string supplier_from, string web_page, int status)
        {
            try
            {
                supplier supplier = entities.suppliers.Find(editID);
                if (supply_logo != null && supply_logo.ContentLength > 0)
                {
                    var fileName = Path.GetFileName(supply_logo.FileName);
                    if (!Directory.Exists(Path.Combine(Server.MapPath("~/"), "Upload")))
                    {
                        Directory.CreateDirectory(Path.Combine(Server.MapPath("~/"), "Upload"));
                    }

                    if (!Directory.Exists(Path.Combine(Server.MapPath("~/Upload/"), "Supplier_Logo")))
                    {
                        Directory.CreateDirectory(Path.Combine(Server.MapPath("~/Upload/"), "Supplier_Logo"));
                    }
                    var path = Path.Combine(Server.MapPath("~/Upload/Supplier_Logo"), fileName);
                    supply_logo.SaveAs(path);
                    supplier.logo = fileName;
                }

                supplier.company = company;
                supplier.contact_name = contact_name;
                supplier.type_service = type_service;
                supplier.category_id = category;
                supplier.address = address;
                supplier.phone = phone;
                supplier.email = email;
                supplier.status = status;
                supplier.supplier_from = DateTime.ParseExact(supplier_from, "yyyy-MM-dd",
                    System.Globalization.CultureInfo.CurrentCulture);
                supplier.web_page = web_page;
                entities.SaveChanges();
                return Redirect(Url.Action("listado", "suplidores", new { area = "webmaster" }));
            }
            catch (Exception ex)
            {
                return Redirect(Url.Action("editar", "suplidores", 
                    new {
                        area = "webmaster",
                        exception = ex.Message
                    }));
            }
        }

        [HttpPost]
        public ActionResult newsupplier(HttpPostedFileBase logo_file,
            string company, string contact_name, string type_service,
            int category, string address, string phone, string email,
            string web_page, string supplier_from)
        {
            try
            {
                supplier supplier = new supplier();

                if (logo_file != null && logo_file.ContentLength > 0)
                {
                    var fileName = Path.GetFileName(logo_file.FileName);
                    if (!Directory.Exists(Path.Combine(Server.MapPath("~/"), "Upload")))
                    {
                        Directory.CreateDirectory(Path.Combine(Server.MapPath("~/"), "Upload"));
                    }

                    if (!Directory.Exists(Path.Combine(Server.MapPath("~/Upload/"), "Supplier_Logo")))
                    {
                        Directory.CreateDirectory(Path.Combine(Server.MapPath("~/Upload/"), "Supplier_Logo"));
                    }

                    var path = Path.Combine(Server.MapPath("~/Upload/Supplier_Logo"), fileName);
                    logo_file.SaveAs(path);
                    supplier.logo = fileName;
                }
                else
                {
                    supplier.logo = null;
                }
                supplier.company = company;
                supplier.contact_name = contact_name;
                supplier.type_service = type_service;
                supplier.email = email;
                supplier.address = address;
                supplier.phone = phone;
                supplier.web_page = web_page;
                supplier.status = 1;
                //supplier.community_id = Convert.ToInt64(Session["CURRENT_COMU"]);
                supplier.supplier_from = DateTime.ParseExact(supplier_from, "yyyy-MM-dd",
                    System.Globalization.CultureInfo.CurrentCulture);
                supplier.category_id = category;
                supplier.created_at = DateTime.Now;
                entities.suppliers.Add(supplier);
                entities.SaveChanges();
                return Redirect(Url.Action("listado", "suplidores", new { area = "webmaster" }));
            }
            catch (Exception ex)
            {
                return Redirect(Url.Action("newsupplier", "suplidores", 
                    new {
                        area = "webmaster",
                        exception = ex.Message
                    }));
            }
        }

        public JsonResult DeleteSupplier(long delID)
        {
            try
            {
                supplier delSupplier = entities.suppliers.Find(delID);
                entities.suppliers.Remove(delSupplier);
                entities.SaveChanges();
                return Json(new { result = "success" });
            } catch(Exception ex)
            {
                return Json(new {
                    result = "error",
                    exception = ex.Message
                });
            }
        }

        public JsonResult onActivateSupp(long id)
        {
            try
            {
                supplier delf = entities.suppliers.Find(id);
                if (delf.status == 1)
                {
                    delf.status = 2;
                }
                else
                {
                    delf.status = 1;
                }

                entities.SaveChanges();
                return Json(new { result = "success" });
            }
            catch (Exception ex)
            {
                return Json(new { result = "error", exception = ex.HResult });
            }
        }

        [HttpPost]
        public ActionResult SeacrhResult(string searchStr, int searchCategoryId)
        {
            return Redirect(Url.Action("listado", "suplidores", new
            {
                area = "webmaster",
                searchStr = searchStr,
                searchCategoryId = searchCategoryId
            }));
        }




        //Categorias

        public ActionResult listadoCategorias(string Error, string searchStr = "")
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
                viewModel.side_menu = "suplidores";
                viewModel.side_sub_menu = "category_directory";
                viewModel.document_category_list = entities.document_type.ToList();
                viewModel.categoryList = catList;
                viewModel.searchStr = searchStr;
                viewModel.curUser = curUser;
                viewModel.pubMessageList = pubMessageList;
                viewModel.messageCount = ep.GetUnreadMessageCount(pubMessageList);
                viewModel.pubTaskList = ep.GetNotifiTaskList(userId);
                ViewBag.msgError = Error;
                return View(viewModel);
            }
            else
            {
                return Redirect(ep.GetLogoutUrl());
            }
        }

        public ActionResult agregarCategoria()
        {
            if (Session["USER_ID"] != null)
            {
                long userId = (long)Session["USER_ID"];
                user curUser = entities.users.Find(userId);
                List<ShowMessage> pubMessageList = ep.GetChatMessages(userId);
                categoriesTareasViewModel viewModel = new categoriesTareasViewModel();
                viewModel.side_menu = "suplidores";
                viewModel.side_sub_menu = "category_directory";
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

        public ActionResult editarCategoria(long? editID)
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
                    viewModel.side_menu = "suplidores";
                    viewModel.side_sub_menu = "category_directory";
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
                return Redirect(Url.Action("listadoCategorias", "suplidores", new { area = "webmaster" }));
            }
            catch (Exception ex)
            {
                return Redirect(Url.Action("editarCategoria", "suplidores",
                    new
                    {
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
                return Redirect(Url.Action("listadoCategorias", "suplidores", new { area = "webmaster" }));
            }
            catch (Exception ex)
            {
                return Redirect(Url.Action("agregarCategoria", "suplidores",
                    new
                    {
                        area = "webmaster",
                        exception = ex.Message
                    }));
            }
        }

        [HttpPost]
        public ActionResult SeacrhResult(string searchStr)
        {
            return Redirect(Url.Action("listadoCategorias", "suplidores",
                new
                {
                    area = "webmaster",
                    searchStr = searchStr
                }));
        }

        public JsonResult DeleteCatg(long delID)
        {
            try
            {
                List<supplier> supplidr = entities.suppliers.Where(x=> x.category_id == delID).ToList();
                if (supplidr.Count == 0)
                {
                    category catItem = entities.categories.Find(delID);
                    entities.categories.Remove(catItem);
                    entities.SaveChanges();
                    return Json(new { result = "success" });
                }
                else
                {
                    return Json(new { result = "NotAlowed" });
                }               
               
            }
            catch (Exception ex)
            {
                return Json(new { result = "error", exception = ex.HResult });
            }
        }

    }
}