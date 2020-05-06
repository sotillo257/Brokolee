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
        public ActionResult listado(string searchStr = "", int searchCategoryId = 0)
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
                viewModel.side_sub_menu = "suplidores_listado";
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
                    supplier editSupplier = entities.suppliers.Find(editID);
                    editarSuplidoresViewModel viewModel = new editarSuplidoresViewModel();
                    viewModel.side_menu = "suplidores";
                    viewModel.side_sub_menu = "suplidores_editar";
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
                    return Redirect(Url.Action("NotFound", "Error"));
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
                    long userId = (long)Session["USER_ID"];
                    user curUser = entities.users.Find(userId);
                    List<ShowMessage> pubMessageList = ep.GetChatMessages(userId);
                    var query = (from r in entities.comments where r.supplier_id == viewID select r);
                    supplier supplier = entities.suppliers.Find(viewID);
                    category category = entities.categories.Find(supplier.category_id);
                    verSuplidoresViewModel viewModel = new verSuplidoresViewModel();
                    viewModel.side_menu = "suplidores";
                    viewModel.side_sub_menu = "suplidores_ver";
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
                    return Redirect(Url.Action("NotFound", "Error"));
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
                long userId = (long)Session["USER_ID"];
                user curUser = entities.users.Find(userId);
                List<ShowMessage> pubMessageList = ep.GetChatMessages(userId);
                agregarSuplidoresViewModel viewModel = new agregarSuplidoresViewModel();
                viewModel.side_menu = "suplidores";
                viewModel.side_sub_menu = "suplidores_agregar";
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
                return Redirect(ep.GetLogoutUrl());
            }
        }

        [HttpPost]
        public ActionResult editsupplier(long editID, HttpPostedFileBase supply_logo,
           string company, string contact_name,
           string type_service, int category, string address,
           string phone, string email,
           string supplier_from, string web_page)
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
    }
}