using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WebApplication1.Areas.coadmin.ViewModels;
using WebApplication1.Concrete;
using WebApplication1.Models;

namespace WebApplication1.Areas.coadmin.Controllers
{
    public class suplidoresController : Controller
    {
        pjrdev_condominiosEntities entities = new pjrdev_condominiosEntities();
        EFPublicRepository ep = new EFPublicRepository();
        // GET: coadmin/suplidores
        public ActionResult listado(string searchStr = "", int searchCategoryId = 0)
        {
            if (Session["USER_ID"] != null)
            {
                try
                {
                    long userId = 0;
                    if (Convert.ToInt32(Session["USER_ROLE"]) == 2)
                    {
                        userId = (long)Session["USER_ID"];
                    }
                    else if (Convert.ToInt32(Session["USER_ROLE"]) > 2
                    && Session["ACC_USER_ID"] != null)
                    {
                        userId = (long)Session["ACC_USER_ID"];
                    }

                    user curUser = entities.users.Find(userId);
                    List<ShowMessage> pubMessageList = ep.GetChatMessages(userId);
                    List<supplier> supplierList = new List<supplier>();
                    Dictionary<long, string> categoryDict = new Dictionary<long, string>();
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
                    suplidoresViewModel viewModel = new suplidoresViewModel();
                    viewModel.side_menu = "supplier_directory";
                    viewModel.side_sub_menu = "supplier_directory_listado";
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

        public ActionResult versuplidor(long? selID)
        {
            if (Session["USER_ID"] != null)
            {
                if (selID != null)
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
                        var query = (from r in entities.comments where r.supplier_id == selID select r);
                        supplier supplier = entities.suppliers.Find(selID);
                        category category = entities.categories.Find(supplier.category_id);
                        versuplidorViewModel viewModel = new versuplidorViewModel();
                        viewModel.side_menu = "supplier_directory";
                        viewModel.side_sub_menu = "supplier_directory_versuplidor";
                        viewModel.commentList = query.ToList();
                        viewModel.viewSupplier = supplier;
                        viewModel.document_category_list = entities.document_type.ToList();
                        viewModel.supplierCategory = category;
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
                    agregarViewModel viewModel = new agregarViewModel();
                    viewModel.side_menu = "supplier_directory";
                    viewModel.side_sub_menu = "supplier_directory_agregar";
                    viewModel.document_category_list = entities.document_type.ToList();
                    viewModel.categoryList = entities.categories.ToList();
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

        public ActionResult editar(long? editID)
        {
            if (Session["USER_ID"] != null)
            {
                if (editID != null)
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
                        supplier editSupplier = entities.suppliers.Find(editID);
                        editarViewModel viewModel = new editarViewModel();
                        viewModel.side_menu = "supplier_directory";
                        viewModel.side_sub_menu = "supplier_directory_editar";
                        viewModel.editSupplier = editSupplier;
                        viewModel.document_category_list = entities.document_type.ToList();
                        viewModel.categoryList = entities.categories.ToList();
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
        [HttpPost]
        public ActionResult editsupplier(long editID, HttpPostedFileBase supply_logo, string company, string contact_name,
            string type_service, int category, string address, string phone, string email,
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
                return Redirect(Url.Action("listado", "suplidores"));
            } catch(Exception)
            {
                return Redirect(ep.GetLogoutUrl());
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
                } else
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
                supplier.supplier_from = DateTime.ParseExact(supplier_from, "yyyy-MM-dd",
                    System.Globalization.CultureInfo.CurrentCulture);
                supplier.category_id = category;
                supplier.created_at = DateTime.Now;
                entities.suppliers.Add(supplier);
                entities.SaveChanges();
                return Redirect(Url.Action("listado", "suplidores", new { area = "coadmin" }));
            }catch(Exception)
            {
                return Redirect(Url.Action("newsupplier", "suplidores"));
            }
        }

        public JsonResult DeleteSupplier(long delID)
        {
            try
            {
            
            List<comment> comments = entities.comments.Where(m => m.supplier_id == delID).ToList();
            entities.comments.RemoveRange(comments);
            //comment delComment = entities.comments.Find(delID);
            //    entities.comments.Remove(delComment);

                supplier delSupplier = entities.suppliers.Find(delID);
                entities.suppliers.Remove(delSupplier);

                entities.SaveChanges();
                return Json(new { result = "success" });
            } catch(Exception ex)
            {
              return Json(new { result = "error", exception = ex.HResult });
            }
        }

        [HttpPost]
        public ActionResult SeacrhResult(string searchStr, int searchCategoryId)
        {
            return Redirect(Url.Action("listado", "suplidores", new
            {
                area = "coadmin",
                searchStr = searchStr,
                searchCategoryId = searchCategoryId
            }));
        }

        public JsonResult SetCommentSupplier(long supplierID, string commentary)
        {
            try
            {
                long userId = (long)Session["USER_ID"];
                user curUser = entities.users.Find(userId);
                supplier supplier = entities.suppliers.Find(supplierID);
                comment comment = entities.comments.Where(m => m.supplier_id == supplierID && m.user_id == userId).FirstOrDefault();
                if (comment == null)
                {
                    comment = new comment();
                    comment.user_id = userId;
                    comment.supplier_id = supplierID;
                    comment.commentary = commentary;
                    comment.created_at = DateTime.Now;
                    comment.user_name = curUser.first_name1 + " " + curUser.last_name1;
                    entities.comments.Add(comment);
                }
                else
                {
                    comment.commentary = commentary;
                    comment.created_at = DateTime.Now;
                    comment.user_name = curUser.first_name1 + " " + curUser.last_name1;
                }
                entities.SaveChanges();
                return Json(new { result = "success" });
            }
            catch (Exception)
            {
                return Json(new { result = "error" });
            }
        }

        public JsonResult SetRateSupplier(long supplierID, int rating)
        {
            try
            {
                long userId = (long)Session["USER_ID"];
                user curUser = entities.users.Find(userId);
                supplier supplier = entities.suppliers.Find(supplierID);
                comment comment = entities.comments.Where(m => m.supplier_id == supplierID && m.user_id == userId).FirstOrDefault();
                if (comment == null)
                {
                    comment = new comment();
                    comment.user_id = userId;
                    comment.supplier_id = supplierID;
                    comment.assessment = (int)rating;
                    comment.created_at = DateTime.Now;
                    comment.user_name = curUser.first_name1 + " " + curUser.last_name1;
                    entities.comments.Add(comment);
                } else
                {
                    comment.assessment = (int)rating;
                    comment.created_at = DateTime.Now;
                    comment.user_name = curUser.first_name1 + " " + curUser.last_name1;
                }
                entities.SaveChanges();
                List<comment> commentList = entities.comments.Where(m => m.supplier_id == supplierID).ToList();
                int sum = 0;
                foreach(var item in commentList)
                {
                    if (item.assessment != null)
                    {
                        sum = sum + (int)item.assessment;
                    }
                }
                supplier.assessment = (int)Math.Round((double)sum/commentList.Count);
                entities.SaveChanges();
                return Json(new { result = "success" });
            }
            catch (Exception)
            {
                return Json(new { result = "error" });
            }
        }
    }
}