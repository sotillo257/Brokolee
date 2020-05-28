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
    public class controlController : Controller
    {
        pjrdev_condominiosEntities entities = new pjrdev_condominiosEntities();
        EFPublicRepository ep = new EFPublicRepository();

        List<Titulo> titulosList = new List<Titulo>();
        List<community> listComunities = new List<community>();       

        // GET: community          
        public ActionResult community()
        {           
            if (Session["USER_ID"] != null)
            {
                try
                {
                    long userId = (long)Session["USER_ID"];
                    long communityAct = Convert.ToInt64(Session["CURRENT_COMU"]);
                    user curUser = entities.users.Find(userId);
                    List<ShowMessage> pubMessageList = ep.GetChatMessages(userId);
                    communityControlViewModel viewModel = new communityControlViewModel();
                    titulosList = ep.GetTitulosByTitular(userId);
                    listComunities = ep.GetCommunityListByTitular(titulosList);                                          
                    if (listComunities.Count == 0)
                    {                        
                        viewModel.side_menu = "control";
                        viewModel.side_sub_menu = "control";
                         viewModel.document_category_list = entities.document_type.Where(x => x.community_id == communityAct).ToList();
                        viewModel.curUser = curUser;
                        viewModel.pubTaskList = ep.GetNotifiTaskList(userId);
                        viewModel.pubMessageList = pubMessageList;
                        viewModel.messageCount = ep.GetUnreadMessageCount(pubMessageList);
                        viewModel.titulosList = titulosList;
                        viewModel.communityList = listComunities;
                        return View(viewModel);
                    }
                    else
                    {
                        if (Session["CURRENT_COMU"] == null)
                        {
                            long selectedCommu = 0;
                            community firstCommu = listComunities.FirstOrDefault();
                            selectedCommu = firstCommu.id;
                            Session["CURRENT_COMU"] = selectedCommu;
                            viewModel.communityID1 = selectedCommu;
                        }
                        else
                        {
                            long Id = Convert.ToInt64(Session["CURRENT_COMU"]);
                            List<community> ordenCommunity = new List<community>();
                            community selectedCommu = entities.communities.Find(Id);
                            ordenCommunity.Add(selectedCommu);
                            foreach (community item in listComunities)
                            {
                                if (item.id != selectedCommu.id)
                                {
                                    ordenCommunity.Add(item);
                                }
                            }

                            listComunities.Clear();

                            foreach (community item2 in ordenCommunity)
                            {
                                listComunities.Add(item2);
                            }
                        }
                        viewModel.communityList = listComunities;
                        viewModel.side_menu = "control";
                        viewModel.side_sub_menu = "control";                     
                        viewModel.document_category_list = entities.document_type.Where(x => x.community_id == communityAct).ToList();
                        viewModel.curUser = curUser;
                        viewModel.pubTaskList = ep.GetNotifiTaskList(userId);
                        viewModel.pubMessageList = pubMessageList;
                        viewModel.messageCount = ep.GetUnreadMessageCount(pubMessageList);
                        viewModel.titulosList = titulosList;                    
                        return View(viewModel);
                    }                    
                }
                catch(Exception ex)
                {
                    return Redirect(Url.Action("error", "control", new { Error = "Community: " + ex.Message }));
                }                
            }
            else
            {
                return Redirect(ep.GetLogoutUrl());
            }
        }
        public ActionResult panel(long? id)
        {
            if (Session["USER_ID"] != null)
            {
                if (id != null)
                {
                    try
                    {
                        long userId = (long)Session["USER_ID"];
                        long comId = Convert.ToInt64(id);
                        long communityAct = Convert.ToInt64(Session["CURRENT_COMU"]);
                        List<blog> blogList = new List<blog>();
                        user curUser = entities.users.Find(userId);
                        List<ShowMessage> pubMessageList = ep.GetChatMessages(userId);
                        communuser communuser = entities.communusers.Where(m => m.commun_id == comId).FirstOrDefault();
                        if (communuser != null)
                        {
                            long comUserId = communuser.user_id;
                            blogList = entities.blogs.Where(m => m.user_id == comUserId).ToList();
                        }
                        controlViewModel viewModel = new controlViewModel();
                        viewModel.ComID = comId;
                        viewModel.communityID1 = Convert.ToInt64(Session["CURRENT_COMU"]);
                        viewModel.side_menu = "control";
                        viewModel.document_category_list = entities.document_type.Where(x => x.community_id == communityAct).ToList();
                        viewModel.curUser = curUser;
                        viewModel.pubTaskList = ep.GetNotifiTaskList(userId);
                        viewModel.pubMessageList = pubMessageList;
                        viewModel.messageCount = ep.GetUnreadMessageCount(pubMessageList);
                        viewModel.blogList = blogList;
                        return View(viewModel);
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
                return Redirect(ep.GetLogoutUrl());
            }
        }

        #region Titulos
        public ActionResult listadoTitulos(long? Id)
        {
            if (Session["USER_ID"] != null)
            {
                if (Session["CURRENT_COMU"] == null)
                {
                    if (Id != null)
                    {
                        try
                        {
                            long communityAct = Convert.ToInt64(Session["CURRENT_COMU"]);
                            long userId = (long)Session["USER_ID"];
                            user curUser = entities.users.Find(userId);
                            Dictionary<long, string> communityDict = new Dictionary<long, string>();
                            List<ShowMessage> pubMessageList = ep.GetChatMessages(userId);
                            List<Titulo> titulosList = new List<Titulo>();
                            listadoTitulosViewModel viewModel = new listadoTitulosViewModel();
                            titulosList = ep.GetTitulosByTitular(userId);
                            listComunities = ep.GetCommunityListByTitular(titulosList);
                            viewModel.communityList = listComunities;
                            titulosList = entities.Titulos.Where(x => x.is_del != true && x.IdUser == userId && x.community.id == Id).ToList();
                            Session["CURRENT_COMU"] = Id;
                            viewModel.communityID1 = Convert.ToInt64(Id);
                            viewModel.side_menu = "control";
                            viewModel.side_sub_menu = "control";
                             viewModel.document_category_list = entities.document_type.Where(x => x.community_id == communityAct).ToList();
                            viewModel.titulosList = titulosList;
                            viewModel.IdUserTitular = (int)Id;
                            viewModel.curUser = curUser;
                            viewModel.messageCount = ep.GetUnreadMessageCount(pubMessageList);
                            viewModel.pubTaskList = ep.GetNotifiTaskList(userId);
                            viewModel.pubMessageList = pubMessageList;
                            return View(viewModel);
                        }
                        catch (Exception ex)
                        {
                            return Redirect(Url.Action("error", "control", new { Error = "Títulos: " + ex.Message }));
                        }                        
                    }
                    else
                    {
                        return Redirect(Url.Action("community", "control"));
                    }

                }
                else
                {
                    try
                    {
                        long communityAct = Convert.ToInt64(Session["CURRENT_COMU"]);
                        long userId = (long)Session["USER_ID"];
                        user curUser = entities.users.Find(userId);
                        Dictionary<long, string> communityDict = new Dictionary<long, string>();
                        List<ShowMessage> pubMessageList = ep.GetChatMessages(userId);
                        List<Titulo> titulosList = new List<Titulo>();
                        listadoTitulosViewModel viewModel = new listadoTitulosViewModel();
                        titulosList = ep.GetTitulosByTitular(userId);
                        listComunities = ep.GetCommunityListByTitular(titulosList);
                        viewModel.communityList = listComunities;
                        titulosList = entities.Titulos.Where(x => x.is_del != true && x.IdUser == userId && x.community.id == communityAct).ToList();
                        viewModel.communityID1 = Convert.ToInt64(communityAct);
                        viewModel.side_menu = "control";
                        viewModel.side_sub_menu = "control";
                         viewModel.document_category_list = entities.document_type.Where(x => x.community_id == communityAct).ToList();
                        viewModel.titulosList = titulosList;
                        viewModel.IdUserTitular = (int)Id;
                        viewModel.curUser = curUser;
                        viewModel.messageCount = ep.GetUnreadMessageCount(pubMessageList);
                        viewModel.pubTaskList = ep.GetNotifiTaskList(userId);
                        viewModel.pubMessageList = pubMessageList;
                        return View(viewModel);
                    }
                    catch (Exception ex)
                    {
                        return Redirect(Url.Action("error", "control", new { Error = "Títulos: " + ex.Message }));
                    }                    
                }                
            }
            else
            {
                return Redirect(ep.GetLogoutUrl());
            }
        }

        public ActionResult verTitulo(long? id)
        {

            if (Session["USER_ID"] != null)
            {
                if (Session["CURRENT_COMU"] != null)
                {
                    if (id != null)
                    {
                        long userId = (long)Session["USER_ID"];
                        long communityAct = Convert.ToInt64(Session["CURRENT_COMU"]);
                        Titulo titulo = entities.Titulos.Where(x => x.Id == id && x.IdCommunity == communityAct && x.IdUser == userId).FirstOrDefault();
                        if (titulo != null)
                        {                            
                            List<ShowMessage> pubMessageList = ep.GetChatMessages(userId);
                            user curUser = entities.users.Find(userId);
                            editarTituloViewModel viewModel = new editarTituloViewModel();
                            viewModel.side_menu = "control";
                            viewModel.side_sub_menu = "control";
                             viewModel.document_category_list = entities.document_type.Where(x => x.community_id == communityAct).ToList();
                            viewModel.curUser = curUser;
                            viewModel.messageCount = ep.GetUnreadMessageCount(pubMessageList);
                            viewModel.titulo = titulo;
                            viewModel.pubTaskList = ep.GetNotifiTaskList(userId);
                            viewModel.pubMessageList = pubMessageList;
                            titulosList = ep.GetTitulosByTitular(userId);
                            listComunities = ep.GetCommunityListByTitular(titulosList);
                            viewModel.communityList = listComunities;
                            return View(viewModel);
                        }
                        else
                        {
                            return Redirect(Url.Action("listadoTitulos", "control", new { Error = "No existe ese elemento: " }));
                        }
                    }
                    else
                    {
                        return Redirect(Url.Action("listadoTitulos", "control"));
                    }
                }
                else
                {
                    return Redirect(Url.Action("community", "control"));
                }                
            }
            else
            {
                return Redirect(ep.GetLogoutUrl());
            }
        }
        #endregion

        #region Vehiculos
        public ActionResult listadoVehiculos(long? Id)
        {
            if (Session["USER_ID"] != null)
            {
                if (Id != null)
                {
                    try
                    {
                        long communityAct = Convert.ToInt64(Session["CURRENT_COMU"]);
                        long userId = (long)Session["USER_ID"];
                        user curUser = entities.users.Find(userId);
                        Dictionary<long, string> communityDict = new Dictionary<long, string>();
                        List<ShowMessage> pubMessageList = ep.GetChatMessages(userId);
                        List<Vehiculo> vehiculosLists = new List<Vehiculo>();
                        vehiculosLists = entities.Vehiculos.Where(x => x.is_del != true && x.IdTitulo == Id && x.Titulo.IdUser == userId).ToList();
                        Titulo titulo = entities.Titulos.Find(Id);
                        listadoVehiculosViewModel viewModel = new listadoVehiculosViewModel();
                        viewModel.titulo = titulo;
                        viewModel.side_menu = "control";
                        viewModel.side_sub_menu = "control";
                         viewModel.document_category_list = entities.document_type.Where(x => x.community_id == communityAct).ToList();
                        viewModel.vehiculosList = vehiculosLists;
                        viewModel.curUser = curUser;
                        viewModel.messageCount = ep.GetUnreadMessageCount(pubMessageList);
                        viewModel.CantidadDeVehiculos = vehiculosLists.Count();
                        viewModel.pubTaskList = ep.GetNotifiTaskList(userId);
                        viewModel.pubMessageList = pubMessageList;
                        titulosList = ep.GetTitulosByTitular(userId);
                        listComunities = ep.GetCommunityListByTitular(titulosList);
                        viewModel.communityList = listComunities;
                        return View(viewModel);
                    }
                    catch (Exception ex)
                    {
                        return Redirect(Url.Action("error", "control", new { Error = "Vehículos: " + ex.Message }));
                    }
                }
                else
                {
                    return Redirect(Url.Action("community", "control"));
                }
            }
            else
            {
                return Redirect(ep.GetLogoutUrl());
            }
        }

        public ActionResult verVehiculo(long? id)
        {

            if (Session["USER_ID"] != null)
            {
                if (id != null)
                {
                    long userId = (long)Session["USER_ID"];               
                    Vehiculo vehiculo = entities.Vehiculos.Where(x=> x.IdVehiculo == id && x.Titulo.IdUser == userId).FirstOrDefault();
                    if (vehiculo != null)
                    {
                        long communityAct = Convert.ToInt64(Session["CURRENT_COMU"]);
                        List<ShowMessage> pubMessageList = ep.GetChatMessages(userId);
                        user curUser = entities.users.Find(userId);
                        editarVehiculoViewModel viewModel = new editarVehiculoViewModel();
                        viewModel.side_menu = "control";
                        viewModel.side_sub_menu = "control";
                         viewModel.document_category_list = entities.document_type.Where(x => x.community_id == communityAct).ToList();
                        viewModel.vehiculo = vehiculo;
                        viewModel.curUser = curUser;
                        viewModel.messageCount = ep.GetUnreadMessageCount(pubMessageList);
                        viewModel.pubTaskList = ep.GetNotifiTaskList(userId);
                        viewModel.pubMessageList = pubMessageList;
                        viewModel.messageCount = ep.GetUnreadMessageCount(pubMessageList);
                        titulosList = ep.GetTitulosByTitular(userId);
                        listComunities = ep.GetCommunityListByTitular(titulosList);
                        viewModel.communityList = listComunities;
                        return View(viewModel);
                    }
                    else
                    {
                        return Redirect(Url.Action("listadoTitulos", "control", new { Error = "No existe ese elemento: " }));
                    }                                        
                }
                else
                {
                    return Redirect(Url.Action("community", "control"));
                }
            }
            else
            {
                return Redirect(ep.GetLogoutUrl());
            }
        }

        #endregion

        public JsonResult ChangeCommunity(long id)
        {
            try
            {

                Session["CURRENT_COMU"] = id;
                return Json(new { result = "success" });
            }
            catch (Exception ex)
            {
                return Json(new
                {
                    result = "error",
                    exception = ex.Message
                });
            }
        }

        public ActionResult error(string Error)
        {
            communityControlViewModel viewModel = new communityControlViewModel();
            if (Session["USER_ID"] != null)
            {
                if (Error != "")
                {
                    long userId = (long)Session["SUS_USER_ID"];
                    user curUser = entities.users.Find(userId);
                    List<ShowMessage> pubMessageList = ep.GetChatMessages(userId);
                    List<document_type> document_category_list = entities.document_type.ToList();
                    titulosList = ep.GetTitulosByTitular(userId);
                    listComunities = ep.GetCommunityListByTitular(titulosList);
                    viewModel.communityList = listComunities;
                    viewModel.side_menu = "control_panel";
                    viewModel.side_sub_menu = "";
                    viewModel.document_category_list = document_category_list;
                    viewModel.curUser = curUser;
                    viewModel.pubTaskList = ep.GetNotifiTaskList(userId);
                    viewModel.pubMessageList = pubMessageList;
                    viewModel.messageCount = ep.GetUnreadMessageCount(pubMessageList);
                    ViewBag.msgError = Error;
                    return View(viewModel);
                }
                else
                {
                    return Redirect(Url.Action("community", "control"));
                }
            }
            else
            {
                return Redirect(ep.GetLogoutUrl());
            }
        }
    }
}