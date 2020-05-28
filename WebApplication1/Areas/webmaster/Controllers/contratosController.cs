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
    public class contratosController : Controller
    {
        pjrdev_condominiosEntities entities = new pjrdev_condominiosEntities();
        EFPublicRepository ep = new EFPublicRepository();
        // GET: webmaster/contratos
        public ActionResult listado(string searchStr = "")
        {
            if (Session["USER_ID"] != null)
            {
                long userId = (long)Session["USER_ID"];
                List<ShowMessage> pubMessageList = ep.GetChatMessages(userId);
                user curUser = entities.users.Find(userId);
                List<contract> contractList = new List<contract>();

                if (searchStr == "")
                {
                    var query1 = (from r in entities.contracts select r);
                    contractList = query1.ToList();
                }
                else
                {
                    var query2 = (from r in entities.contracts
                                  where r.first_name.Contains(searchStr) == true
                                  select r);
                    contractList = query2.ToList();
                }

                listadoContratosViewModel viewModel = new listadoContratosViewModel();
                viewModel.side_menu = "contratos";
                viewModel.side_sub_menu = "contratos_listado";
                viewModel.contractList = contractList;
                
                viewModel.curUser = curUser;
                viewModel.searchStr = searchStr;
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

        public ActionResult agregar()
        {
            if (Session["USER_ID"] != null)
            {
                long userId = (long)Session["USER_ID"];
                user curUser = entities.users.Find(userId);
                List<ShowMessage> pubMessageList = ep.GetChatMessages(userId);
                agregarContratosViewModel viewModel = new agregarContratosViewModel();
                viewModel.side_menu = "contratos";
                viewModel.side_sub_menu = "contratos_agregar";
                
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

        public ActionResult archivados(string searchStr = "")
        {
            if (Session["USER_ID"] != null)
            {
                long userId = (long)Session["USER_ID"];
                user curUser = entities.users.Find(userId);
                List<ShowMessage> pubMessageList = ep.GetChatMessages(userId);
                List<contract> contractList = new List<contract>();
                if (searchStr == "")
                {
                    var query = (from r in entities.contracts where r.status == 2 select r);
                    contractList = query.ToList();
                }
                else
                {
                    var query1 = (from r in entities.contracts where r.status == 2 && r.first_name.Contains(searchStr) == true select r);
                    contractList = query1.ToList();
                }
                archivadosContratosViewModel viewModel = new archivadosContratosViewModel();
                viewModel.side_menu = "contratos";
                viewModel.side_sub_menu = "contratos_archivados";
                
                viewModel.curUser = curUser;
                viewModel.contractList = contractList;
                viewModel.searchStr = searchStr;
                viewModel.pubTaskList = ep.GetNotifiTaskList(userId);
                viewModel.pubMessageList = pubMessageList;
                viewModel.messageCount = ep.GetUnreadMessageCount(pubMessageList);
                return View(viewModel);
            } else
            {
                return Redirect(ep.GetLogoutUrl());
            }
                
        }

        public ActionResult editar(long? contractID)
        {
            if (Session["USER_ID"] != null)
            {
                if (contractID != null)
                {
                    long userId = (long)Session["USER_ID"];
                    user curUser = entities.users.Find(userId);
                    List<ShowMessage> pubMessageList = ep.GetChatMessages(userId);
                    contract editContract = entities.contracts.Find(contractID);
                    editarContratosViewModel viewModel = new editarContratosViewModel();
                    viewModel.side_menu = "contratos";
                    viewModel.side_sub_menu = "contratos_editar";
                    
                    viewModel.curUser = curUser;
                    viewModel.editContract = editContract;
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
        public ActionResult editcontract(long contractID, string first_name, string date_contract,
           string date_notification, HttpPostedFileBase upload_contract, string description, int status)
        {
            try
            {
                contract editContract = entities.contracts.Find(contractID);
                editContract.first_name = first_name;
                editContract.date_contract = DateTime.ParseExact(date_contract, "yyyy-MM-dd",
                    System.Globalization.CultureInfo.CurrentCulture
                    );
                editContract.date_notification = DateTime.ParseExact(date_notification, "yyyy-MM-dd",
                    System.Globalization.CultureInfo.CurrentCulture
                    );
                editContract.description = description;
                if (upload_contract != null && upload_contract.ContentLength > 0)
                {
                    var fileName = Path.GetFileName(upload_contract.FileName);
                    if (!Directory.Exists(Path.Combine(Server.MapPath("~/"), "Upload")))
                    {
                        Directory.CreateDirectory(Path.Combine(Server.MapPath("~/"), "Upload"));
                    }

                    if (!Directory.Exists(Path.Combine(Server.MapPath("~/Upload/"), "Regulation")))
                    {
                        Directory.CreateDirectory(Path.Combine(Server.MapPath("~/Upload/"), "Regulation"));
                    }
                    var path = Path.Combine(Server.MapPath("~/Upload/Upload_Contract"), fileName);
                    upload_contract.SaveAs(path);
                    editContract.upload_contract = fileName;
                }
                editContract.status = status;
                entities.SaveChanges();
                return Redirect(Url.Action("listado", "contratos", new { area = "webmaster" }));
            }
            catch (Exception ex)
            {
                return Redirect(Url.Action("editar", "contratos", 
                    new {
                        area = "webmaster",
                        id = contractID,
                        exception = ex.Message
                    }));
            }
        }

        [HttpPost]
        public ActionResult newcontract(string first_name, string date_contract,
            string date_notification, HttpPostedFileBase upload_contract, string description)
        {
            try
            {
                contract newContract = new contract();
                newContract.first_name = first_name;
                newContract.date_contract = DateTime.ParseExact(date_contract, "yyyy-MM-dd",
                    System.Globalization.CultureInfo.CurrentCulture);
                newContract.date_notification = DateTime.ParseExact(date_notification, "yyyy-MM-dd",
                    System.Globalization.CultureInfo.CurrentCulture);
                newContract.description = description;
                if (upload_contract != null && upload_contract.ContentLength > 0)
                {
                    var fileName = Path.GetFileName(upload_contract.FileName);
                    if (!Directory.Exists(Path.Combine(Server.MapPath("~/"), "Upload")))
                    {
                        Directory.CreateDirectory(Path.Combine(Server.MapPath("~/"), "Upload"));
                    }

                    if (!Directory.Exists(Path.Combine(Server.MapPath("~/Upload/"), "Upload_Contract")))
                    {
                        Directory.CreateDirectory(Path.Combine(Server.MapPath("~/Upload/"), "Upload_Contract"));
                    }
                    var path = Path.Combine(Server.MapPath("~/Upload/Upload_Contract"), fileName);
                    upload_contract.SaveAs(path);
                    newContract.upload_contract = fileName;
                }
                newContract.status = 1;
                entities.contracts.Add(newContract);
                entities.SaveChanges();
                return Redirect(Url.Action("listado", "contratos", new { area = "webmaster" }));
            }
            catch (Exception ex)
            {
                return Redirect(Url.Action("agregar", "contratos", 
                    new {
                        area = "webmaster",
                        exception = ex.Message
                    }));
            }
        }

        public JsonResult DeleteContract(long delID)
        {
            try
            {
                contract delContract = entities.contracts.Find(delID);
                entities.contracts.Remove(delContract);
                entities.SaveChanges();
                return Json(new { result = "success" });
            }
            catch (Exception ex)
            {
                return Json(new { result = "error", exception = ex.HResult });
            }
        }

        [HttpPost]
        public ActionResult SeacrhResult(string searchStr)
        {
            return Redirect(Url.Action("listado", "contratos",
                new {
                    area = "webmaster",
                    searchStr = searchStr
                }));
        }

        [HttpPost]
        public ActionResult SeacrhArchieveResult(string searchStr)
        {
            return Redirect(Url.Action("archivados", "contratos", 
                new {
                    area = "webmaster",
                    searchStr = searchStr
                }));
        }
    }
}