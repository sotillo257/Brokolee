using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WebApplication1.Areas.coadmin.ViewModels;
using WebApplication1.Concrete;
using WebApplication1.Models;

namespace WebApplication1.Areas.coadmin.Controllers
{
    public class cuotasController : Controller
    {
        pjrdev_condominiosEntities entities = new pjrdev_condominiosEntities();
        EFPublicRepository ep = new EFPublicRepository();
        List<community> communityList = new List<community>();
        // GET: coadmin/cuotas
        public ActionResult listado(string Error)
        {
            if (Session["USER_ID"] != null)
            {
                try
                {
                    long userId = (long)Session["USER_ID"];                    
                    user curUser = entities.users.Find(userId);
                    listadoCuotasViewModel viewModel = new listadoCuotasViewModel();

                    communityList = ep.GetCommunityList(userId);
                    viewModel.communityList = communityList;

                    long communityAct = Convert.ToInt64(Session["CURRENT_COMU"]);
                    List<fee> feeList = new List<fee>();
                    feeList = entities.fees.Where(m => m.community_id == communityAct).ToList();                                                       
                    List<ShowMessage> pubMessageList = ep.GetChatMessages(userId);
                    viewModel.feeList = feeList;
                    viewModel.side_menu = "cuotas";
                    viewModel.side_sub_menu = "cuotas_listado";
                    viewModel.document_category_list = entities.document_type.ToList();
                    viewModel.curUser = curUser;
                    viewModel.pubTaskList = ep.GetNotifiTaskList(userId);
                    viewModel.pubMessageList = pubMessageList;
                    viewModel.messageCount = ep.GetUnreadMessageCount(pubMessageList);
                    ViewBag.msgError = Error;
                    return View(viewModel);
                }
                catch(Exception ex)
                {
                    return Redirect(Url.Action("Index", "Error"));
                }              
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
                if(Session["CURRENT_COMU"] != null) 
                {
                    try
                    {
                        long userId = (long)Session["USER_ID"];                        
                        user curUser = entities.users.Find(userId);
                        List<bank> bankList = entities.banks.ToList();
                        List<ShowMessage> pubMessageList = ep.GetChatMessages(userId);
                        agregarCuotasViewModel viewModel = new agregarCuotasViewModel();

                        communityList = ep.GetCommunityList(userId);
                        viewModel.communityList = communityList;

                        viewModel.side_menu = "cuotas";
                        viewModel.side_sub_menu = "cuotas_agregar";
                        viewModel.document_category_list = entities.document_type.ToList();
                        viewModel.curUser = curUser;
                        viewModel.pubTaskList = ep.GetNotifiTaskList(userId);
                        viewModel.pubMessageList = pubMessageList;
                        viewModel.messageCount = ep.GetUnreadMessageCount(pubMessageList);
                        viewModel.bankList = bankList;

                        viewModel.feeName = "";
                        viewModel.cost = 15;
                        viewModel.taxCharge = 1;
                        viewModel.penalty = 1;
                        viewModel.merchantAccount = "";
                        viewModel.bankId = -1;
                        return View(viewModel);
                    }
                    catch (Exception ex)
                    {
                        return Redirect(Url.Action("Index", "Error"));
                    }
                }
                else
                {
                    return Redirect(Url.Action("listado", "cuotas", new { area = "coadmin", Error = "No puede agregar cuotas. Usted no administra ninguna comunidad. Comuníquese con el Webmaster..." }));
                }                             
            }
            else
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
                        try
                        {
                            long userId = (long)Session["USER_ID"];                            
                            fee feeItem = entities.fees.Where(m => m.id == editID).FirstOrDefault();
                            if (feeItem != null)
                            {
                                user curUser = entities.users.Find(userId);
                                List<bank> bankList = entities.banks.Where(m => m.user_id == userId).ToList();
                                List<ShowMessage> pubMessageList = ep.GetChatMessages(userId);
                                agregarCuotasViewModel viewModel = new agregarCuotasViewModel();

                                communityList = ep.GetCommunityList(userId);
                                viewModel.communityList = communityList;

                                viewModel.side_menu = "cuotas";
                                viewModel.side_sub_menu = "cuotas_editar";
                                viewModel.document_category_list = entities.document_type.ToList();
                                viewModel.curUser = curUser;
                                viewModel.pubTaskList = ep.GetNotifiTaskList(userId);
                                viewModel.pubMessageList = pubMessageList;
                                viewModel.messageCount = ep.GetUnreadMessageCount(pubMessageList);
                                viewModel.feedId = Convert.ToInt64(editID);
                                viewModel.bankList = bankList;
                                viewModel.feeName = feeItem.name;
                                viewModel.cost = feeItem.cost;
                                viewModel.taxCharge = feeItem.tax_charge;
                                viewModel.penalty = feeItem.penalty;
                                viewModel.merchantAccount = feeItem.merchant_account;
                                viewModel.bankId = feeItem.bank_id;
                                return View(viewModel);
                            }
                            else
                            {
                                return Redirect(Url.Action("listado", "cuotas", new { area = "coadmin", Error = "No existe ese elemento" }));
                            }

                        }
                        catch (Exception ex)
                        {
                            return Redirect(Url.Action("Index", "Error"));
                        }
                    }
                    else
                    {
                        return Redirect(Url.Action("NotFound", "Error"));
                    }
                }
                else
                {
                    return Redirect(Url.Action("listado", "cuotas", new { area = "coadmin", Error = "No puede editar cuotas. Usted no administra ninguna comunidad. Comuníquese con el Webmaster..." }));
                }
                               
            } else
            {
                return Redirect(ep.GetLogoutUrl());
            }
        }

        public ActionResult agregarCuenta(long? nC, long? editC)
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
                        cuentaCuotasViewModel viewModel = new cuentaCuotasViewModel();

                        communityList = ep.GetCommunityList(userId);
                        viewModel.communityList = communityList;

                        viewModel.side_menu = "cuotas";
                        viewModel.side_sub_menu = "cuotas_cuenta";
                        viewModel.document_category_list = entities.document_type.ToList();
                        viewModel.curUser = curUser;
                        viewModel.pubTaskList = ep.GetNotifiTaskList(userId);
                        viewModel.pubMessageList = pubMessageList;
                        viewModel.messageCount = ep.GetUnreadMessageCount(pubMessageList);
                        ViewBag.newCuota = nC;
                        ViewBag.isEdit = editC;
                        return View(viewModel);
                    }
                    catch (Exception ex)
                    {
                        return Redirect(Url.Action("Index", "Error"));
                    }
                }
                else
                {
                    return Redirect(Url.Action("listadoCuentas", "cuotas", new { area = "coadmin", Error = "No puede agregar cuentas. Usted no administra ninguna comunidad. Comuníquese con el Webmaster..." }));
                }
                
            }
            else
            {
                return Redirect(ep.GetLogoutUrl());
            }
        }

        public ActionResult listadoCuentas(string Error)
        {
            if (Session["USER_ID"] != null)
            {
                try
                {
                    long userId = (long)Session["USER_ID"];                    
                    user curUser = entities.users.Find(userId);
                    BancosViewModel viewModel = new BancosViewModel();

                    communityList = ep.GetCommunityList(userId);
                    viewModel.communityList = communityList;
                    long communityAct = Convert.ToInt64(Session["CURRENT_COMU"]);

                    List<bank> bankList = new List<bank>();

                    if (Session["CURRENT_COMU"] != null)
                    {
                        bankList = entities.banks.ToList();
                    }
                    else
                    {
                        bankList.Clear();
                    }

                    List<ShowMessage> pubMessageList = ep.GetChatMessages(userId);
                    viewModel.banks = bankList;
                    viewModel.side_menu = "cuotas";
                    viewModel.side_sub_menu = "bancos_listado";
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
                    return Redirect(Url.Action("Index", "Error"));
                }
            }
            else
            {
                return Redirect(ep.GetLogoutUrl());
            }
        }

        public ActionResult editarCuenta(long? editID)
        {
            if (Session["USER_ID"] != null)
            {
                if (Session["CURRENT_COMU"] != null)
                {
                    if (editID != null)
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
                            bank banItem = entities.banks.Where(m => m.id == editID).FirstOrDefault();
                            if (banItem != null)
                            {
                                user curUser = entities.users.Find(userId);
                                List<ShowMessage> pubMessageList = ep.GetChatMessages(userId);
                                BancosViewModel viewModel = new BancosViewModel();

                                communityList = ep.GetCommunityList(userId);
                                viewModel.communityList = communityList;

                                viewModel.side_menu = "cuotas";
                                viewModel.side_sub_menu = "cuenta_editar";
                                viewModel.document_category_list = entities.document_type.ToList();
                                viewModel.curUser = curUser;
                                viewModel.pubTaskList = ep.GetNotifiTaskList(userId);
                                viewModel.pubMessageList = pubMessageList;
                                viewModel.messageCount = ep.GetUnreadMessageCount(pubMessageList);
                                viewModel.bankItem = banItem;
                                return View(viewModel);
                            }
                            else
                            {
                                return Redirect(Url.Action("listadoCuentas", "cuotas", new { area = "coadmin", Error = "No existe ese elemento" }));
                            }

                        }
                        catch (Exception ex)
                        {
                            return Redirect(Url.Action("Index", "Error"));
                        }
                    }
                    else
                    {
                        return Redirect(Url.Action("NotFound", "Error"));
                    }
                }
                else
                {
                    return Redirect(Url.Action("listadoCuentas", "cuotas", new { area = "coadmin", Error = "No puede editar cuentas. Usted no administra ninguna comunidad. Comuníquese con el Webmaster..." }));
                }                
            }
            else
            {
                return Redirect(ep.GetLogoutUrl());
            }
        }

        public ActionResult pagos(string searchString = "", int searchState = -1)
        {
            if (Session["USER_ID"] != null)
            {
                try
                {
                    bool state = false;

                    if (searchState == 1)
                    {
                        state = true;
                    }
                    else if (searchState == 0)
                    {
                        state = false;
                    }
                    long userId = (long)Session["USER_ID"];                  
                    user curUser = entities.users.Find(userId);
                    List<payment> paymentList = new List<payment>();

                    if (searchState != -1)
                    {
                        if (searchString != "")
                        {
                            paymentList = entities.payments.Where(m => m.first_name.Contains(searchString) == true
                                            && m.state == state).ToList();
                        }
                        else
                        {
                            paymentList = entities.payments.Where(m => m.state == state).ToList();
                        }
                    }
                    else
                    {
                        if (searchString != "")
                        {
                            paymentList = entities.payments.Where(m => m.first_name.Contains(searchString) == true).ToList();
                        }
                        else
                        {
                            paymentList = entities.payments.ToList();
                        }
                    }

                    List<ShowMessage> pubMessageList = ep.GetChatMessages(userId);
                    pagosCuotasViewModel viewModel = new pagosCuotasViewModel();

                    communityList = ep.GetCommunityList(userId);
                    viewModel.communityList = communityList;

                    viewModel.side_menu = "cuotas";
                    viewModel.side_sub_menu = "cuotas_pagos";
                    viewModel.document_category_list = entities.document_type.ToList();
                    viewModel.curUser = curUser;
                    viewModel.pubTaskList = ep.GetNotifiTaskList(userId);
                    viewModel.pubMessageList = pubMessageList;
                    viewModel.messageCount = ep.GetUnreadMessageCount(pubMessageList);                   
                    viewModel.paymentList = paymentList;
                    viewModel.searchString = searchString;
                    viewModel.searchState = searchState;
                    return View(viewModel);
                }
                catch(Exception ex)
                {
                    return Redirect(Url.Action("Index", "Error"));
                }                
            }
            else
            {
                return Redirect(ep.GetLogoutUrl());
            }
        }

        [HttpPost]
        public ActionResult editfee(long feedId, string fee_name, string cost, string taxCharge,
            string penalty, long bank_id, string merchant_account)
        {
            long userId = (long)Session["USER_ID"];
            fee feeItem = entities.fees.Find(feedId);

            if (feeItem != null)
            {
                feeItem.name = fee_name;
                feeItem.cost = Convert.ToDecimal(cost);
                feeItem.bank_id = bank_id;
                feeItem.merchant_account = merchant_account;
                feeItem.tax_charge = Convert.ToDecimal(taxCharge);
                feeItem.penalty = Convert.ToDecimal(penalty);
                entities.SaveChanges();
                return Redirect(Url.Action("listado", "cuotas", new { area = "coadmin" }));
            }
            else
            {
                return Redirect(ep.GetLogoutUrl());
            }
        }

        [HttpPost]
        public ActionResult addfee(string fee_name, string cost, string taxCharge,
            string penalty, long bank_id, string merchant_account)
        {
            long userId = (long)Session["USER_ID"];
            fee feeItem = new fee();
            feeItem.name = fee_name;
            feeItem.cost = Convert.ToDecimal(cost);
            feeItem.bank_id = bank_id;
            feeItem.merchant_account = merchant_account;
            feeItem.tax_charge = Convert.ToDecimal(taxCharge);
            feeItem.penalty = Convert.ToDecimal(penalty);
            feeItem.created_at = DateTime.Now;
            feeItem.user_id = userId;
            feeItem.community_id = Convert.ToInt64(Session["CURRENT_COMU"]);
            entities.fees.Add(feeItem);
            
            
            entities.SaveChanges();
            return Redirect(Url.Action("listado", "cuotas", new { area = "coadmin" }));
        }

        [HttpPost]
        public ActionResult addbank(string account_name, string account_number, 
            string bank_name, string route_number, long? nC, long? editC)
        {
            long userId = (long)Session["USER_ID"];
            bank bankItem = new bank();
            bankItem.user_id = userId;
            bankItem.account_name = account_name;
            bankItem.account_number = account_number;
            bankItem.bank_name = bank_name;
            bankItem.route_number = route_number;
            bankItem.created_at = DateTime.Now;
            bankItem.updated_at = DateTime.Now;
            entities.banks.Add(bankItem);

            entities.SaveChanges();
            if (nC == 1)
            {
                return Redirect(Url.Action("agregar", "cuotas", new { area = "coadmin" }));
            }
            else if(nC == 2)
            {
                return Redirect(Url.Action("editar", "cuotas", new { area = "coadmin", editID = editC }));
            }
            else
            {
                return Redirect(Url.Action("listadoCuentas", "cuotas", new { area = "coadmin" }));
            }
           
        }

        [HttpPost]
        public ActionResult editBank(long account_id, string account_name, string account_number,
            string bank_name, string route_number)
        {
            long userId = (long)Session["USER_ID"];
            bank bankItem = entities.banks.Find(account_id);

            if (bankItem != null)
            {
                bankItem.account_name = account_name;
                bankItem.account_number = account_number;
                bankItem.bank_name = bank_name;
                bankItem.route_number = route_number;
                bankItem.updated_at = DateTime.Now;
                entities.SaveChanges();
                return Redirect(Url.Action("listadoCuentas", "cuotas", new { area = "coadmin" }));
            }
            else
            {
                return Redirect(ep.GetLogoutUrl());
            }
        }

        public JsonResult DeleteFee(long feeId)
        {
            try
            {
                fee feeItem = entities.fees.Find(feeId);
                entities.fees.Remove(feeItem);
                entities.SaveChanges();
                return Json(new { result = "succes" });
            } catch(Exception ex)
            {
                return Json(new { result = "error", exception = ex.Message });
            }
        }

        public JsonResult DeleteBank(long bankId)
        {           
            try
            {
                List<fee> feeList = entities.fees.Where(x=> x.bank_id == bankId).ToList();
                if (feeList.Count == 0)
                {
                    bank bankItem = entities.banks.Find(bankId);
                    entities.banks.Remove(bankItem);
                    entities.SaveChanges();
                    return Json(new { result = "succes" });
                }
                else
                {
                    return Json(new { result = "NotAlowed"});
                }
                
            }
            catch (Exception ex)
            {
                return Json(new { result = "error", exception = ex.Message });
            }
        }

        [HttpPost]
        public ActionResult SeacrhResult(string searchString, int searchState)
        {
            return Redirect(Url.Action("pagos", "cuotas", new {
                searchString  = searchString,
                searchState = searchState }));
        }
    }
}