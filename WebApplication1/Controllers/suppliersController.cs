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
    public class suppliersController : Controller
    {
        pjrdev_condominiosEntities entities = new pjrdev_condominiosEntities();
        EFPublicRepository ep = new EFPublicRepository();

        List<Titulo> titulosList = new List<Titulo>();
        List<community> listComunities = new List<community>();

        // GET: suppliers
        public ActionResult listado(string Error, string searchStr = "", int searchCategoryId = 0)
        {
            if (Session["USER_ID"] != null)
            {
                try
                {
                    long userId = (long)Session["USER_ID"];                    
                    user curUser = entities.users.Find(userId);
                    List<ShowMessage> pubMessageList = ep.GetChatMessages(userId);
                    List<supplier> supplierList = new List<supplier>();
                    Dictionary<long, string> categoryDict = new Dictionary<long, string>();

                    long communityAct = Convert.ToInt64(Session["CURRENT_COMU"]);                   
                        if (searchStr == "" && searchCategoryId == 0)
                        {
                            var query = (from r in entities.suppliers where r.status == 1 select r);
                            supplierList = query.ToList();
                        }
                        else if (searchStr != "" && searchCategoryId == 0)
                        {
                            var query1 = (from r in entities.suppliers
                                          where r.contact_name.Contains(searchStr) == true && r.status == 1
                                          select r);
                            supplierList = query1.ToList();
                        }
                        else if (searchStr == "" && searchCategoryId != 0)
                        {
                            var query2 = (from r in entities.suppliers
                                          where r.category_id == searchCategoryId && r.status == 1
                                          select r
                                          );
                            supplierList = query2.ToList();
                        }
                        else
                        {
                            var query3 = (from r in entities.suppliers
                                          where r.contact_name.Contains(searchStr) == true &&
                                          r.category_id == searchCategoryId && r.status == 1
                                          select r);
                            supplierList = query3.ToList();
                        }
                   

                    foreach (var item in supplierList)
                    {
                        category category = entities.categories.Find(item.category_id);
                        categoryDict.Add(item.id, category.name);
                    }
                    suppliersViewModel viewModel = new suppliersViewModel();

                    titulosList = ep.GetTitulosByTitular(userId);
                    listComunities = ep.GetCommunityListByTitular(titulosList);
                    viewModel.communityList = listComunities;
                  
                    viewModel.side_menu = "suppliers";
                    viewModel.supplierList = supplierList;
                    viewModel.document_category_list = entities.document_type.ToList();
                    viewModel.categoryDict = categoryDict;
                    viewModel.curUser = curUser;
                    viewModel.categoryList = entities.categories.ToList();
                    viewModel.searchCategoryId = searchCategoryId;
                    viewModel.searchStr = searchStr;
                    viewModel.pubTaskList = ep.GetNotifiTaskList(userId);
                    viewModel.pubMessageList = pubMessageList;
                    viewModel.messageCount = ep.GetUnreadMessageCount(pubMessageList);
                    ViewBag.msgError = Error;
                    return View(viewModel);
                }
                catch(Exception ex)
                {
                    return Redirect(Url.Action("error", "control", new { Error = "Error al obtener listado de suplidores: " + ex }));
                }             
            } else
            {
                return Redirect(ep.GetLogoutUrl());
            }    
        }

        public ActionResult versuplidor(long? supplier_id) 
        {
            if (Session["USER_ID"] != null)
            {
                if (supplier_id != null)
                {
                    supplier supplier = entities.suppliers.Find(supplier_id);
                    if (supplier != null)
                    {
                        if (supplier.status == 1)
                        {
                            try
                            {
                                long userId = (long)Session["USER_ID"];
                                List<ShowMessage> pubMessageList = ep.GetChatMessages(userId);
                                user curUser = entities.users.Find(userId);                                
                                category category = entities.categories.Find(supplier.category_id);
                                versuplidorViewModel viewModel = new versuplidorViewModel();

                                titulosList = ep.GetTitulosByTitular(userId);
                                listComunities = ep.GetCommunityListByTitular(titulosList);
                                viewModel.communityList = listComunities;

                                viewModel.side_menu = "suppliers";
                                viewModel.category_name = category.name;
                                viewModel.viewSupplier = supplier;
                                List<comment> commentList = entities.comments.Where(m => m.supplier_id == supplier_id).ToList();
                                viewModel.commentList = commentList;
                                viewModel.document_category_list = entities.document_type.ToList();
                                viewModel.curUser = curUser;
                                viewModel.pubTaskList = ep.GetNotifiTaskList(userId);
                                viewModel.pubMessageList = pubMessageList;
                                viewModel.messageCount = ep.GetUnreadMessageCount(pubMessageList);
                                return View(viewModel);
                            }
                            catch (Exception ex)
                            {
                                return Redirect(Url.Action("listado", "suplidores", new { Error = "Problema interno " + ex.Message }));
                            }
                        }
                        else
                        {
                            return Redirect(Url.Action("listado", "suplidores", new { area = "coadmin", Error = "El suplidor al que desea acceder fue desactivado" }));
                        }
                    }
                    else
                    {
                        return Redirect(Url.Action("listado", "suplidores", new { area = "coadmin", Error = "No existe ese elemento" }));
                    }                   
                }
                else
                {
                    return Redirect(Url.Action("listado", "suplidores"));
                }                
            } else
            {
                return Redirect(ep.GetLogoutUrl());
            }
        }

        [HttpPost]
        public ActionResult SeacrhResult(string searchStr, int searchCategoryId)
        {
            return Redirect(Url.Action("listado", "suppliers", new
            {
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
                }
                else
                {
                    comment.assessment = (int)rating;
                    comment.created_at = DateTime.Now;
                    comment.user_name = curUser.first_name1 + " " + curUser.last_name1;
                }
                entities.SaveChanges();
                List<comment> commentList = entities.comments.Where(m => m.supplier_id == supplierID).ToList();
                int sum = 0;
                foreach (var item in commentList)
                {
                    if (item.assessment != null)
                    {
                        sum = sum + (int)item.assessment;
                    }
                }
                supplier.assessment = (int)Math.Round((double)sum / commentList.Count);
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