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
        // GET: coadmin/cuotas
        public ActionResult listado()
        {
            if (Session["USER_ID"] != null)
            {
                try
                {
                    long userId = 0;
                    if (Convert.ToInt32(Session["USER_ROLE"])>= 1)
                    {
                        userId = (long)Session["USER_ID"];
                    }
                    else if (Convert.ToInt32(Session["USER_ROLE"]) > 1
                    && Session["ACC_USER_ID"] != null)
                    {
                        userId = (long)Session["ACC_USER_ID"];
                    }

                    user curUser = entities.users.Find(userId);
                    listadoCuotasViewModel viewModel = new listadoCuotasViewModel();
                    Dictionary<long, string> bankDict = new Dictionary<long, string>();

                    long communityAct = Convert.ToInt64(Session["CURRENT_COMU"]);
                    List<fee> feeList = entities.fees.Where(m => m.user_id == userId && m.community_id == communityAct).ToList();
                    foreach (var item in feeList)
                    {
                        long bankID = item.bank_id;
                        bank bankItem = entities.banks.Find(bankID);
                      
                            bankDict.Add(bankID, bankItem.bank_name);
                        
                    }
                    List<ShowMessage> pubMessageList = ep.GetChatMessages(userId);
                    viewModel.feeList = feeList;
                    viewModel.side_menu = "cuotas";
                    viewModel.side_sub_menu = "cuotas_listado";
                    viewModel.document_category_list = entities.document_type.ToList();
                    viewModel.curUser = curUser;
                    viewModel.pubTaskList = ep.GetNotifiTaskList(userId);
                    viewModel.pubMessageList = pubMessageList;
                    viewModel.messageCount = ep.GetUnreadMessageCount(pubMessageList);                   
                    viewModel.bankDict = bankDict;
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
                    List<bank> bankList = entities.banks.Where(m => m.user_id == userId).ToList();
                    List<ShowMessage> pubMessageList = ep.GetChatMessages(userId);
                    agregarCuotasViewModel viewModel = new agregarCuotasViewModel();
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
        public ActionResult editar(long? editID)
        {
            if (Session["USER_ID"] != null)
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
                        fee feeItem = entities.fees.Where(m => m.id == editID).FirstOrDefault();
                        user curUser = entities.users.Find(userId);
                        List<bank> bankList = entities.banks.Where(m => m.user_id == userId).ToList();
                        List<ShowMessage> pubMessageList = ep.GetChatMessages(userId);
                        agregarCuotasViewModel viewModel = new agregarCuotasViewModel();
                        viewModel.side_menu = "cuotas";
                        viewModel.side_sub_menu = "cuotas_editar";
                        viewModel.document_category_list = entities.document_type.ToList();
                        viewModel.curUser = curUser;
                        viewModel.pubTaskList = ep.GetNotifiTaskList(userId);
                        viewModel.pubMessageList = pubMessageList;
                        viewModel.messageCount = ep.GetUnreadMessageCount(pubMessageList);
                        viewModel.communityName = ep.GetCommunityCoInfo(userId)[0];
                        viewModel.communityApart = ep.GetCommunityCoInfo(userId)[1];
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

        public ActionResult agregarcuenta()
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
                    cuentaCuotasViewModel viewModel = new cuentaCuotasViewModel();
                    viewModel.side_menu = "cuotas";
                    viewModel.side_sub_menu = "cuotas_cuenta";
                    viewModel.document_category_list = entities.document_type.ToList();
                    viewModel.curUser = curUser;
                    viewModel.pubTaskList = ep.GetNotifiTaskList(userId);
                    viewModel.pubMessageList = pubMessageList;
                    viewModel.messageCount = ep.GetUnreadMessageCount(pubMessageList);
                    viewModel.communityName = ep.GetCommunityCoInfo(userId)[0];
                    viewModel.communityApart = ep.GetCommunityCoInfo(userId)[1];
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
        public ActionResult editfee(long feedId, string fee_name, decimal cost, decimal taxCharge,
            decimal penalty, long bank_id, string merchant_account)
        {
            long userId = (long)Session["USER_ID"];
            fee feeItem = entities.fees.Find(feedId);

            if (feeItem != null)
            {
                feeItem.name = fee_name;
                feeItem.cost = cost;
                feeItem.bank_id = bank_id;
                feeItem.merchant_account = merchant_account;
                feeItem.tax_charge = taxCharge;
                feeItem.penalty = penalty;
                entities.SaveChanges();
                return Redirect(Url.Action("listado", "cuotas", new { area = "coadmin" }));
            }
            else
            {
                return Redirect(ep.GetLogoutUrl());
            }
        }

        [HttpPost]
        public ActionResult addfee(string fee_name, decimal cost, decimal taxCharge, 
            decimal penalty, long bank_id, string merchant_account)
        {
            long userId = (long)Session["USER_ID"];
            fee feeItem = new fee();
            feeItem.name = fee_name;
            feeItem.cost = cost;
            feeItem.bank_id = bank_id;
            feeItem.merchant_account = merchant_account;
            feeItem.tax_charge = taxCharge;
            feeItem.penalty = penalty;
            feeItem.created_at = DateTime.Now;
            feeItem.user_id = userId;
            feeItem.community_id = Convert.ToInt64(Session["CURRENT_COMU"]);
            entities.fees.Add(feeItem);
            
            
            entities.SaveChanges();
            return Redirect(Url.Action("listado", "cuotas", new { area = "coadmin" }));
        }

        [HttpPost]
        public ActionResult addbank(string account_name, string account_number, 
            string bank_name, string route_number)
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
            return Redirect(Url.Action("agregar", "cuotas", new { area = "coadmin" }));
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

        [HttpPost]
        public ActionResult SeacrhResult(string searchString, int searchState)
        {
            return Redirect(Url.Action("pagos", "cuotas", new {
                searchString  = searchString,
                searchState = searchState }));
        }
    }
}