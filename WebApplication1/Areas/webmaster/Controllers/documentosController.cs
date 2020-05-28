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
    public class documentosController : Controller
    {
        pjrdev_condominiosEntities entities = new pjrdev_condominiosEntities();
        EFPublicRepository ep = new EFPublicRepository();
        // GET: webmaster/documentos
        public ActionResult listado(int? id, string searchStr = "")
        {
            if (Session["USER_ID"] != null)
            {
                if (id != null)
                {
                    long userId = (long)Session["USER_ID"];
                    user curUser = entities.users.Find(userId);
                    List<ShowMessage> pubMessageList = ep.GetChatMessages(userId);
                    List<document> document_list = new List<document>();

                    long communityAct = Convert.ToInt64(Session["CURRENT_COMU"]);

                    if (searchStr == "")
                    {
                        var query = (from r in entities.documents where r.type_id == id && r.community_id == communityAct select r);
                        document_list = query.ToList();
                    }
                    else
                    {
                        var query1 = (from r in entities.documents
                                      where r.type_id == id &&
                                      r.first_name.Contains(searchStr) == true && r.community_id == communityAct
                                      select r);
                        document_list = query1.ToList();
                    }
                    List<document_type> document_category_list = entities.document_type.ToList();
                    listadoDocumentViewModel viewModel = new listadoDocumentViewModel();
                    document_type document_type = entities.document_type.Find(id);
                    viewModel.side_menu = "documentos";
                    viewModel.side_sub_menu = "documentos_" + document_type.type_name;
                    viewModel.document_category_list = document_category_list;
                    viewModel.document_list = document_list;
                    viewModel.searchStr = searchStr;
                    viewModel.typeID = Convert.ToInt32(id);
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

        public ActionResult categoria()
        {
            if (Session["USER_ID"] != null)
            {
                long userId = (long)Session["USER_ID"];
                user curUser = entities.users.Find(userId);
                List<ShowMessage> pubMessageList = ep.GetChatMessages(userId);
                List<document_type> document_category_list = entities.document_type.ToList();
                List<DocumentTypeItemViewModel> documentTypeItemList = new List<DocumentTypeItemViewModel>();
                foreach (var item in document_category_list)
                {
                    int ID = item.id;
                    DocumentTypeItemViewModel itemViewModel = new DocumentTypeItemViewModel();
                    itemViewModel.ID = ID;
                    itemViewModel.DocumentTypeName = item.type_name;
                    itemViewModel.Documents = entities.documents.Where(m => m.type_id == ID).ToList().Count; ;
                    itemViewModel.Share = true;
                    documentTypeItemList.Add(itemViewModel);
                }
                categoriaViewModel viewModel = new categoriaViewModel();
                viewModel.side_menu = "documentos";
                viewModel.side_sub_menu = "documentos_categoria";
                
                viewModel.categoryLiest = entities.categories.ToList();
                viewModel.curUser = curUser;
                viewModel.pubTaskList = ep.GetNotifiTaskList(userId);
                viewModel.pubMessageList = pubMessageList;
                viewModel.messageCount = ep.GetUnreadMessageCount(pubMessageList);
                viewModel.documentTypeItemList = documentTypeItemList;
                return View(viewModel);
            } else
            {
                return Redirect(ep.GetLogoutUrl());
            }
        }

        public ActionResult editarcategoria(int? typeID)
        {
            if (Session["USER_ID"] != null)
            {
                if (typeID != null)
                {
                    long userId = (long)Session["USER_ID"];
                    user curUser = entities.users.Find(userId);
                    List<ShowMessage> pubMessageList = ep.GetChatMessages(userId);
                    document_type documentCategory = entities.document_type.Find(typeID);
                    List<document_type> document_category_list = entities.document_type.ToList();
                    List<DocumentTypeItemViewModel> documentTypeItemList = new List<DocumentTypeItemViewModel>();
                    foreach (var item in document_category_list)
                    {
                        int ID = item.id;
                        DocumentTypeItemViewModel itemViewModel = new DocumentTypeItemViewModel();
                        itemViewModel.ID = ID;
                        itemViewModel.DocumentTypeName = item.type_name;
                        itemViewModel.Documents = entities.documents.Where(m => m.type_id == ID).ToList().Count; ;
                        itemViewModel.Share = true;
                        documentTypeItemList.Add(itemViewModel);
                    }
                    editarcategoriaViewModel viewModel = new editarcategoriaViewModel();
                    viewModel.side_menu = "";
                    viewModel.side_sub_menu = "";
                    viewModel.document_category_list = document_category_list;
                    viewModel.editDocumentType = documentCategory;
                    viewModel.curUser = curUser;
                    viewModel.pubTaskList = ep.GetNotifiTaskList(userId);
                    viewModel.pubMessageList = pubMessageList;
                    viewModel.messageCount = ep.GetUnreadMessageCount(pubMessageList);
                    viewModel.documentTypeItemList = documentTypeItemList;
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
                agregarDocumentViewModel viewModel = new agregarDocumentViewModel();
                viewModel.side_menu = "documentos";
                viewModel.side_sub_menu = "documentos_agregar";
                
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
                    document editDocument = entities.documents.Find(editID);
                    editDocumentViewModel viewModel = new editDocumentViewModel();
                    viewModel.side_menu = "documentos";
                    viewModel.side_sub_menu = "documentos_editar";
                    
                    viewModel.editDocument = editDocument;
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

        [HttpPost]
        public ActionResult editdocumenttype(int typeID, string type_name)
        {
            try
            {
                document_type editDocumentType = entities.document_type.Find(typeID);
                editDocumentType.type_name = type_name;
                entities.SaveChanges();
                return Redirect(Url.Action("editarcategoria", "documentos",
                    new {
                        area = "webmaster",
                        typeID = typeID
                    }));
            }
            catch (Exception ex)
            {
                return Redirect(Url.Action("editarcategoria", "documentos",
                    new {
                        area = "webmaster",
                        typeID = typeID,
                        exception = ex.Message
                    }));
            }
        }

        [HttpPost]
        public ActionResult createdocumenttype(string type_name)
        {
            try
            {
                document_type newDocumentType = new document_type();
                newDocumentType.type_name = type_name;
                entities.document_type.Add(newDocumentType);
                entities.SaveChanges();
                return Redirect(Url.Action("categoria", "documentos", new { area = "webmaster" }));
            }
            catch (Exception ex)
            {
                return Redirect(Url.Action("categoria", "documentos",
                    new {
                        area = "webmaster",
                        exception = ex.Message
                    }));
            }
        }

        [HttpPost]
        public ActionResult editdocument(long editID, string first_name,
            int document_category, HttpPostedFileBase upload_document,
            string link, string uploaded_by, string uploaded_date, int share)
        {
            try
            {
                if (document_category != 0)
                {
                    document editDocument = entities.documents.Find(editID);
                    editDocument.first_name = first_name;
                    editDocument.type_id = document_category;
                    editDocument.uploaded_by = uploaded_by;
                    editDocument.link = link;
                    editDocument.community_id = Convert.ToInt64(Session["CURRENT_COMU"]);
                    editDocument.uploaded_date = DateTime.ParseExact(uploaded_date, "yyyy-MM-dd",
                        System.Globalization.CultureInfo.CurrentCulture);
                    editDocument.share = share;
                    if (upload_document != null && upload_document.ContentLength > 0)
                    {
                        var fileName = Path.GetFileName(upload_document.FileName);
                        if (!Directory.Exists(Path.Combine(Server.MapPath("~/"), "Upload")))
                        {
                            Directory.CreateDirectory(Path.Combine(Server.MapPath("~/"), "Upload"));
                        }

                        if (!Directory.Exists(Path.Combine(Server.MapPath("~/Upload/"), "Upload_Document")))
                        {
                            Directory.CreateDirectory(Path.Combine(Server.MapPath("~/Upload/"), "Upload_Document"));
                        }
                        var path = Path.Combine(Server.MapPath("~/Upload/Upload_Document"), fileName);
                        upload_document.SaveAs(path);
                        editDocument.upload_document = fileName;
                    }
                    editDocument.updated_at = DateTime.Now;
                    entities.SaveChanges();
                    return Redirect(Url.Action("listado", "documentos", new { area = "webmaster", id = document_category }));
                }
                else
                {
                    return Redirect(Url.Action("editar", "documentos", new { area = "webmaster", editID = editID }));
                }

            }
            catch (Exception ex)
            {
                return Redirect(Url.Action("editar", "documentos", 
                    new {
                        area = "webmaster",
                        editID = editID,
                        exception = ex.Message
                    }));
            }
        }

        [HttpPost]
        public ActionResult newdocument(string first_name, int document_category,
            HttpPostedFileBase upload_document, string link, string uploaded_by,
            string uploaded_date, int share, int notify_email)
        {
            try
            {
                if (document_category != 0)
                {
                    document document = new document();
                    document.first_name = first_name;
                    document.type_id = document_category;
                    document.link = link;
                    document.uploaded_by = uploaded_by;
                    document.community_id = Convert.ToInt64(Session["CURRENT_COMU"]);
                    document.uploaded_date = DateTime.ParseExact(uploaded_date, "yyyy-MM-dd",
                        System.Globalization.CultureInfo.CurrentCulture);
                    if (upload_document != null && upload_document.ContentLength > 0)
                    {
                        var fileName = Path.GetFileName(upload_document.FileName);
                        if (!Directory.Exists(Path.Combine(Server.MapPath("~/"), "Upload")))
                        {
                            Directory.CreateDirectory(Path.Combine(Server.MapPath("~/"), "Upload"));
                        }

                        if (!Directory.Exists(Path.Combine(Server.MapPath("~/Upload/"), "Upload_Document")))
                        {
                            Directory.CreateDirectory(Path.Combine(Server.MapPath("~/Upload/"), "Upload_Document"));
                        }
                        var path = Path.Combine(Server.MapPath("~/Upload/Upload_Document"), fileName);
                        upload_document.SaveAs(path);
                        document.upload_document = fileName;
                    }
                    document.share = share;
                    if (notify_email == 1)
                    {
                        document.notify_email = true;
                    }
                    else
                    {
                        document.notify_email = false;
                    }
                    document.created_at = DateTime.Now;
                    entities.documents.Add(document);
                    entities.SaveChanges();
                    return Redirect(Url.Action("listado", "documentos", 
                        new {
                            area = "webmaster",
                            id = document_category
                        }));
                }
                else
                {
                    return Redirect(Url.Action("agregar", "documentos", new { area = "webmaster" }));
                }

            }
            catch (Exception ex)
            {
                return Redirect(Url.Action("agregar", "documentos", 
                    new {
                        area = "webmaster",
                        exception = ex.Message
                    }));
            }
        }

        [HttpPost]
        public ActionResult SeacrhResult(int id, string searchStr)
        {
            return Redirect(Url.Action("listado", "documentos",
                new {
                    area = "webmaster",
                    id = id,
                    searchStr = searchStr
                }));
        }

        public JsonResult DeleteDocument(long delID)
        {
            try
            {
                document delDocument = entities.documents.Find(delID);
                entities.documents.Remove(delDocument);
                entities.SaveChanges();
                return Json(new { result = "success" });
            }
            catch (Exception ex)
            {
                return Json(new { result = "error", exception = ex.HResult });
            }
        }

        public JsonResult DeleteDocumentType(int delID)
        {
            try
            {
                List<document> documentList = entities.documents.Where(m => m.type_id == delID).ToList();
                entities.documents.RemoveRange(documentList);
                document_type document_Type = entities.document_type.Find(delID);
                entities.document_type.Remove(document_Type);
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