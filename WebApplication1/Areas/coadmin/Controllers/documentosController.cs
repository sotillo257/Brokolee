using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WebApplication1.Areas.coadmin.ViewModels;
using WebApplication1.Concrete;
using WebApplication1.Models;
using System.Net;
using System.Net.Mail;

namespace WebApplication1.Areas.coadmin.Controllers
{
    public class documentosController : Controller
    {
        // GET: coadmin/documentos
        pjrdev_condominiosEntities entities = new pjrdev_condominiosEntities();
        EFPublicRepository ep = new EFPublicRepository();
        List<community> communityList = new List<community>();

        public ActionResult listado(string Error, string searchStr = "", int idCategory = 0)
        {
            if (Session["USER_ID"] != null)
            {                                   
                        long userId = (long)Session["USER_ID"];                      
                        user curUser = entities.users.Find(userId);
                        List<ShowMessage> pubMessageList = ep.GetChatMessages(userId);
                        List<document> document_list = new List<document>();

                        long communityAct = Convert.ToInt64(Session["CURRENT_COMU"]);
                        
                Dictionary<long, string> categoryDict = new Dictionary<long, string>();
                if (searchStr == "" && idCategory == 0)
                {
                    var query = (from r in entities.documents where r.community_id == communityAct select r);
                    document_list = query.ToList();
                }
                else if (searchStr != "" && idCategory == 0)
                {
                    var query1 = (from r in entities.documents
                                  where r.first_name.Contains(searchStr) == true && r.community_id == communityAct
                                  select r);
                    document_list = query1.ToList();
                }
                else if (searchStr == "" && idCategory != 0)
                {
                    var query2 = (from r in entities.documents
                                  where r.document_type.id == idCategory && r.community_id == communityAct
                                  select r
                                  );
                    document_list = query2.ToList();
                }
                else
                {
                    var query3 = (from r in entities.documents
                                  where r.first_name.Contains(searchStr) == true &&
                                  r.document_type.id == idCategory && r.community_id == communityAct
                                  select r);
                    document_list = query3.ToList();
                }              

                List<document_type> document_category_list = entities.document_type.ToList();
                documentosViewModel viewModel = new documentosViewModel();

                communityList = ep.GetCommunityList(userId);
                viewModel.communityList = communityList;

                document_type document_type = entities.document_type.Find(idCategory);
                viewModel.side_menu = "documentos";
                if (idCategory != 0)
                {
                    viewModel.side_sub_menu = "documentos_"+ document_type.type_name;
                }
                else
                {
                    viewModel.side_sub_menu = "documentos_listado";
                }                
                viewModel.document_category_list = document_category_list;
                viewModel.document_list = document_list;
                viewModel.searchStr = searchStr;
                viewModel.typeID = idCategory;
                viewModel.curUser = curUser;
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

        public ActionResult editarcategoria(int? typeID)
        {
            if (Session["USER_ID"] != null)
            {
                if (Session["CURRENT_COMU"] != null)
                {
                    if (typeID != null)
                    {
                        document_type documentCategory = entities.document_type.Find(typeID);
                        if (documentCategory != null)
                        {
                            try
                            {
                                long userId = (long)Session["USER_ID"];
                                long communityAct = Convert.ToInt64(Session["CURRENT_COMU"]);
                                user curUser = entities.users.Find(userId);
                                List<ShowMessage> pubMessageList = ep.GetChatMessages(userId);

                                List<document_type> document_category_list = new List<document_type>();
                                document_category_list = entities.document_type.ToList();
                                List<DocumentTypeItemViewModel> documentTypeItemList = new List<DocumentTypeItemViewModel>();

                                foreach (var item in document_category_list)
                                {
                                    int ID = item.id;
                                    DocumentTypeItemViewModel itemViewModel = new DocumentTypeItemViewModel();
                                    itemViewModel.ID = ID;
                                    itemViewModel.DocumentTypeName = item.type_name;
                                    itemViewModel.Documents = entities.documents.Where(m => m.type_id == ID && m.community_id == communityAct).ToList().Count;
                                    itemViewModel.Share = (int)item.share;//item.share;
                                    documentTypeItemList.Add(itemViewModel);
                                }
                                editarcategoriaViewModel viewModel = new editarcategoriaViewModel();

                                communityList = ep.GetCommunityList(userId);
                                viewModel.communityList = communityList;

                                viewModel.side_menu = "documentos";
                                viewModel.side_sub_menu = "documentos_editarcategoria";
                                viewModel.document_category_list = document_category_list;
                                viewModel.editDocumentType = documentCategory;
                                viewModel.curUser = curUser;
                                viewModel.pubTaskList = ep.GetNotifiTaskList(userId);
                                viewModel.pubMessageList = pubMessageList;
                                viewModel.messageCount = ep.GetUnreadMessageCount(pubMessageList);
                                viewModel.documentTypeItemList = documentTypeItemList;
                                return View(viewModel);
                            }
                            catch (Exception ex)
                            {
                                return Redirect(Url.Action("listado", "documentos", new { area = "coadmin", Error = "Problema interno " + ex.Message }));
                            }
                        }
                        else
                        {
                            return Redirect(Url.Action("listado", "documentos", new { area = "coadmin", Error = "No existe ese elemento" }));
                        }
                    }
                    else
                    {
                        return Redirect(Url.Action("listado", "documentos", new { area = "coadmin" }));
                    }
                }
                else
                {
                    return Redirect(Url.Action("listado", "documentos", new { area = "coadmin", Error = "No puede editar categorias. Usted no administra ninguna comunidad. Comuníquese con el Webmaster..." }));
                }                                                           
            } else
            {
                return Redirect(ep.GetLogoutUrl());
            }
        }

        public ActionResult categoria(string Error)
        {
            if (Session["USER_ID"] != null)
            {
                if (Session["CURRENT_COMU"] != null)
                {
                    try
                    {
                        long userId = (long)Session["USER_ID"];
                        long communityAct = Convert.ToInt64(Session["CURRENT_COMU"]);
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
                            itemViewModel.Documents = entities.documents.Where(m => m.type_id == ID && m.community_id == communityAct).ToList().Count; ;
                            itemViewModel.Share = (int)item.share;
                            documentTypeItemList.Add(itemViewModel);
                        }
                        categoriaViewModel viewModel = new categoriaViewModel();

                        communityList = ep.GetCommunityList(userId);
                        viewModel.communityList = communityList;

                        viewModel.side_menu = "documentos";
                        viewModel.side_sub_menu = "documentos_categoria";
                        viewModel.document_category_list = document_category_list;
                        viewModel.categoryList = entities.categories.ToList();
                        viewModel.curUser = curUser;
                        viewModel.pubTaskList = ep.GetNotifiTaskList(userId);
                        viewModel.pubMessageList = pubMessageList;
                        viewModel.messageCount = ep.GetUnreadMessageCount(pubMessageList);
                        viewModel.documentTypeItemList = documentTypeItemList;
                        ViewBag.msgError = Error;
                        return View(viewModel);
                    }
                    catch (Exception ex)
                    {
                        return Redirect(Url.Action("categoria", "documentos", new { area = "coadmin", Error = "Problema interno " + ex.Message }));
                    }
                }
                else
                {
                    return Redirect(Url.Action("listado", "documentos", new { area = "coadmin", Error = "No permitido. Usted no administra ninguna comunidad. Comuníquese con el Webmaster..." }));
                }                              
            }
            else
            {
                return Redirect(ep.GetLogoutUrl());
            }
                
        }

        public ActionResult agregar(string Error)
        {
            if (Session["USER_ID"] != null)
            {
                if (Session["CURRENT_COMU"] != null)
                {
                    try
                    {
                        long userId = (long)Session["USER_ID"];                      
                        user curUser = entities.users.Find(userId);
                        List<ShowMessage> pubMessageList = ep.GetChatMessages(userId);
                        documentosViewModel viewModel = new documentosViewModel();

                        communityList = ep.GetCommunityList(userId);
                        viewModel.communityList = communityList;

                        viewModel.side_menu = "documentos";
                        viewModel.side_sub_menu = "documentos_agregar";
                        viewModel.document_category_list = entities.document_type.ToList();
                        viewModel.curUser = curUser;
                        viewModel.pubTaskList = ep.GetNotifiTaskList(userId);
                        viewModel.pubMessageList = pubMessageList;
                        viewModel.messageCount = ep.GetUnreadMessageCount(pubMessageList);
                        ViewBag.msgError = Error;
                        return View(viewModel);

                    }
                    catch (Exception ex)
                    {
                        return Redirect(Url.Action("listado", "documentos", new { area = "coadmin", Error = "Problema interno " + ex.Message }));
                    }
                }
                else
                {
                    return Redirect(Url.Action("listado", "documentos", new { area = "coadmin", Error = "No puede agregar documentos. Usted no administra ninguna comunidad. Comuníquese con el Webmaster..." }));
                }
                             
            }
            else
            {
                return Redirect(ep.GetLogoutUrl());
            }
        }

        public ActionResult ver(long? viewID)
        {
            if (Session["USER_ID"] != null)
            {
                if (Session["CURRENT_COMU"] != null)
                {
                    if (viewID != null)
                    {
                        long communityAct = Convert.ToInt64(Session["CURRENT_COMU"]);
                        document viewDocument = entities.documents.Where(x=> x.id == viewID && x.community_id == communityAct).FirstOrDefault();
                        if (viewDocument != null)
                        {
                            try
                            {
                                long userId = (long)Session["USER_ID"];

                                user curUser = entities.users.Find(userId);
                                List<ShowMessage> pubMessageList = ep.GetChatMessages(userId);
                                verDocumentViewModel viewModel = new verDocumentViewModel();

                                communityList = ep.GetCommunityList(userId);
                                viewModel.communityList = communityList;

                                viewModel.side_menu = "documentos";
                                viewModel.side_sub_menu = "documentos_ver";
                                viewModel.document_category_list = entities.document_type.ToList();
                                viewModel.viewDocument = viewDocument;
                                viewModel.curUser = curUser;
                                viewModel.pubTaskList = ep.GetNotifiTaskList(userId);
                                viewModel.pubMessageList = pubMessageList;
                                viewModel.messageCount = ep.GetUnreadMessageCount(pubMessageList);
                                return View(viewModel);
                            }
                            catch (Exception ex)
                            {
                                return Redirect(Url.Action("listado", "documentos", new { area = "coadmin", Error = "Problema interno " + ex.Message }));
                            }
                        }
                        else
                        {
                            return Redirect(Url.Action("listado", "documentos", new { area = "coadmin", Error = "No existe ese elemento" }));
                        }                        
                    }
                    else
                    {
                        return Redirect(Url.Action("listado", "documentos", new { area = "coadmin" }));
                    }
                }
                else
                {
                    return Redirect(Url.Action("listado", "documentos", new { area = "coadmin", Error = "No permitido. Usted no administra ninguna comunidad. Comuníquese con el Webmaster..." }));
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
                if (Session["CURRENT_COMU"] != null)
                {
                    if (editID != null)
                    {
                        long communityAct = Convert.ToInt64(Session["CURRENT_COMU"]);
                        document editDocument = entities.documents.Where(x=> x.id == editID && x.community_id == communityAct).FirstOrDefault();
                        if (editDocument != null)
                        {
                            try
                            {
                                long userId = (long)Session["USER_ID"];
                                user curUser = entities.users.Find(userId);
                                List<ShowMessage> pubMessageList = ep.GetChatMessages(userId);
                                editDocumentViewModel viewModel = new editDocumentViewModel();

                                communityList = ep.GetCommunityList(userId);
                                viewModel.communityList = communityList;

                                viewModel.side_menu = "documentos";
                                viewModel.side_sub_menu = "documentos_editar";
                                viewModel.document_category_list = entities.document_type.ToList();
                                viewModel.editDocument = editDocument;
                                viewModel.curUser = curUser;
                                viewModel.pubTaskList = ep.GetNotifiTaskList(userId);
                                viewModel.pubMessageList = pubMessageList;
                                viewModel.messageCount = ep.GetUnreadMessageCount(pubMessageList);
                                return View(viewModel);
                            }
                            catch (Exception ex)
                            {
                                return Redirect(Url.Action("listado", "documentos", new { area = "coadmin", Error = "Problema interno " + ex.Message }));
                            }
                        }
                        else
                        {
                            return Redirect(Url.Action("listado", "documentos", new { area = "coadmin", Error = "No existe ese elemento" }));
                        }
                       
                    }
                    else
                    {
                        return Redirect(Url.Action("listado", "documentos", new { area = "coadmin" }));
                    }
                }
                else
                {
                    return Redirect(Url.Action("listado", "documentos", new { area = "coadmin", Error = "No puede editar documentos. Usted no administra ninguna comunidad. Comuníquese con el Webmaster..." }));
                }
                                
            } else
            {
                return Redirect(ep.GetLogoutUrl());
            }
        }


        [HttpPost]
        public ActionResult editdocumenttype(int typeID, string type_name, int documentTypeShare)
        {
            try
            {
                document_type editDocumentType = entities.document_type.Find(typeID);
                editDocumentType.type_name = type_name;
                editDocumentType.share = documentTypeShare;             
                entities.SaveChanges();
                return Redirect(Url.Action("categoria", "documentos", new { area = "coadmin" }));
            }
            catch(Exception ex)
            {                
                return Redirect(Url.Action("categoria", "documentos", new { area = "coadmin", Error = "Problema interno " + ex.Message }));
            }
        }

        [HttpPost]
        public ActionResult createdocumenttype(string type_name, int documentTypeShare)
        {
            try
            {
                document_type newDocumentType = new document_type();
                newDocumentType.type_name = type_name;
                newDocumentType.share = documentTypeShare;
                entities.document_type.Add(newDocumentType);
                entities.SaveChanges();
                return Redirect(Url.Action("categoria", "documentos", new { area = "coadmin" }));
            }
            catch(Exception ex)
            {
                return Redirect(Url.Action("categoria", "documentos", new { area = "coadmin", Error = "Problema interno " + ex.Message }));
            }
        }

        [HttpPost]
        public ActionResult editdocument(long editID, string first_name, 
            int document_category, HttpPostedFileBase upload_document,
            string uploaded_by, string uploaded_date, int share)
        {
            try
            {               
                    document editDocument = entities.documents.Find(editID);
                    editDocument.first_name = first_name;
                    editDocument.type_id = document_category;
                    editDocument.uploaded_by = uploaded_by;
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
                    return Redirect(Url.Action("listado", "documentos", new { area = "coadmin" }));               
                
            }
            catch(Exception ex)
            {
                return Redirect(Url.Action("editar", "documentos", new { area = "coadmin", Error = "Problema interno " + ex.Message, editID = editID }));
            }
        }

        [HttpPost]
        public ActionResult newdocument(string first_name, int document_category,
            HttpPostedFileBase upload_document, string uploaded_by,
            string uploaded_date, int share, int notify_email)
        {
            try
            {
                if (upload_document != null)
                {          
                    document document = new document();
                    document.first_name = first_name;
                    document.type_id = document_category;
                    document.community_id = Convert.ToInt64(Session["CURRENT_COMU"]);
                    //document.link = link;
                    document.uploaded_by = uploaded_by;
                    document.uploaded_date = DateTime.ParseExact(uploaded_date, "yyyy-MM-dd",
                        System.Globalization.CultureInfo.CurrentCulture);
                    var path = "";
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
                         path = Path.Combine(Server.MapPath("~/Upload/Upload_Document"), fileName);
                        upload_document.SaveAs(path);
                        document.upload_document = fileName;
                    }
                    document.share = share;

                    long userId = (long)Session["USER_ID"];
                    var query1 = (from r in entities.users where r.id == userId select r.email);

                    if (notify_email == 1)
                    {
                        document.notify_email = true;

                        MailMessage msj = new MailMessage();
                        SmtpClient cli = new SmtpClient();

                        String email_g = query1.First();
                        String name_g = first_name;                       

                        msj.Attachments.Add(new Attachment(path));

                        msj.From = new MailAddress("o.olaya@zerosystempr.com");
                        msj.To.Add(new MailAddress(email_g));
                        msj.Subject = "Bienvenido";
                        msj.Body = "Nombre de documento" ;
                        msj.IsBodyHtml = false;

                        cli.Host = "mail.zerosystempr.com";
                        cli.Port = 587;
                        cli.Credentials = new NetworkCredential("o.olaya@zerosystempr.com", "temporero");
                        cli.EnableSsl = true;
                        cli.Send(msj);
                    }
                    else
                    {
                        document.notify_email = false;
                    }
                    document.created_at = DateTime.Now;
                    entities.documents.Add(document);
                    entities.SaveChanges();
                    return Redirect(Url.Action("listado", "documentos", new { area = "coadmin"}));
                }
                else
                {
                    return Redirect(Url.Action("agregar", "documentos", new { area = "coadmin", Error = "Debe cargar el documentos" }));                 
                }
                
            }catch(Exception ex)
            {
                return Redirect(Url.Action("agregar", "documentos", new { area = "coadmin", Error = "Problema interno " + ex.Message }));
            }
        }

        [HttpPost]
        public ActionResult SeacrhResult(string searchStr, int idDocumentT)
        {
            return Redirect(Url.Action("listado", "documentos", new { area = "coadmin", searchStr = searchStr, idCategory = idDocumentT }));
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
            catch(Exception ex)
            {
                return Json(new { result = "error", exception = ex.HResult });
            }
        }

        public JsonResult DeleteDocumentType(int delID)
        {
            try
            {
                List<document> documentList = entities.documents.Where(m => m.type_id == delID).ToList();
                if(documentList.Count == 0)
                {                    
                    document_type document_Type = entities.document_type.Find(delID);
                    entities.document_type.Remove(document_Type);
                    entities.SaveChanges();
                    return Json(new { result = "success" });
                }
                else
                {
                    return Json(new { result = "NotAlowed" });
                }                
            } catch(Exception ex)
            {
                return Json(new { result = "error", exception = ex.HResult });
            }
        }
    }
}