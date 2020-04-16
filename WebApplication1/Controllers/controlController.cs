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
        public ActionResult community(long? Id)
        {           
            if (Session["USER_ID"] != null)
            {
                try
                {
                    long userId = 0;
                    if (Convert.ToInt32(Session["USER_ROLE"]) == 1)
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
                    communityControlViewModel viewModel = new communityControlViewModel();

                    titulosList = ep.GetTitulosByTitular(userId);
                    listComunities = ep.GetCommunityListByTitular(titulosList);                      
                    viewModel.communityList = listComunities;
                    if (listComunities.Count == 0)
                    {                        
                        viewModel.side_menu = "control";
                        viewModel.side_sub_menu = "control";
                        viewModel.document_category_list = entities.document_type.ToList();
                        viewModel.curUser = curUser;
                        viewModel.pubTaskList = ep.GetNotifiTaskList(userId);
                        viewModel.pubMessageList = pubMessageList;
                        viewModel.messageCount = ep.GetUnreadMessageCount(pubMessageList);
                        viewModel.titulosList = titulosList;                        
                        return View(viewModel);
                    }
                    else
                    {
                        if (Session["CURRENT_COMU"] == null)
                        {
                            long selectedCommu = 0;
                            community firstCommu = listComunities.LastOrDefault();
                            selectedCommu = firstCommu.id;
                            Session["CURRENT_COMU"] = selectedCommu;
                            viewModel.communityID1 = selectedCommu;
                        }
                        else
                        {
                            if (Id == null)
                            {
                                long comId = Convert.ToInt64(Session["CURRENT_COMU"]);
                                viewModel.communityID1 = comId;
                            }
                            else
                            {
                                Session["CURRENT_COMU"] = Id;
                                viewModel.communityID1 = Convert.ToInt64(Id);
                            }
                        }

                        viewModel.side_menu = "control";
                        viewModel.side_sub_menu = "control";
                        viewModel.document_category_list = entities.document_type.ToList();
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
                    return Redirect(Url.Action("Index", "Error"));
                }                
            }
            else
            {
                return Redirect(Url.Action("iniciar", "iniciar"));
            }
        }
        // GET: control
        public ActionResult panel(long? id)
        {
            if (Session["USER_ID"] != null)
            {
                if (id != null)
                {
                    try
                    {
                        long userId = 0;
                        if (Convert.ToInt32(Session["USER_ROLE"]) == 1)
                        {
                            userId = (long)Session["USER_ID"];
                        }
                        else if (Convert.ToInt32(Session["USER_ROLE"]) > 1
                        && Session["ACC_USER_ID"] != null)
                        {
                            userId = (long)Session["ACC_USER_ID"];
                        }

                        long comId = Convert.ToInt64(id);
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
                        viewModel.document_category_list = entities.document_type.ToList();
                        viewModel.curUser = curUser;
                        viewModel.pubTaskList = ep.GetNotifiTaskList(userId);
                        viewModel.pubMessageList = pubMessageList;
                        viewModel.messageCount = ep.GetUnreadMessageCount(pubMessageList);                        
                        viewModel.blogList = blogList;
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
            }
            else
            {
                return Redirect(Url.Action("iniciar", "iniciar"));
            } 
        }

        #region Titulos
        public ActionResult listadoTitulos(long? Id)
        {
            if (Session["USER_ID"] != null && Id != null)
            {
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
                     
                viewModel.side_menu = "titulares";
                viewModel.side_sub_menu = "titulares_listado";
                viewModel.document_category_list = entities.document_type.ToList();
                viewModel.titulosList = titulosList;
                viewModel.IdUserTitular = (int)Id;
                viewModel.curUser = curUser;
                viewModel.messageCount = ep.GetUnreadMessageCount(pubMessageList);                
                viewModel.pubTaskList = ep.GetNotifiTaskList(userId);
                viewModel.pubMessageList = pubMessageList;              
                return View(viewModel);
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
                if (id != null)
                {

                    long userId = (long)Session["USER_ID"];
                    Titulo titulo = entities.Titulos.Find(id);
                    List<ShowMessage> pubMessageList = ep.GetChatMessages(userId);
                    user curUser = entities.users.Find(userId);
                    editarTituloViewModel viewModel = new editarTituloViewModel();
                    viewModel.side_menu = "titulares";
                    viewModel.side_sub_menu = "manage_edit_headlines";
                    viewModel.document_category_list = entities.document_type.ToList();
                    viewModel.curUser = curUser;
                    viewModel.messageCount = ep.GetUnreadMessageCount(pubMessageList);                    
                    viewModel.titulo = titulo;
                    viewModel.pubTaskList = ep.GetNotifiTaskList(userId);
                    viewModel.pubMessageList = pubMessageList;
                    viewModel.communityID1 = Convert.ToInt64(Session["CURRENT_COMU"]);

                    List<Titulo> titulosList = new List<Titulo>();
                    titulosList = ep.GetTitulosByTitular(userId);
                    listComunities = ep.GetCommunityListByTitular(titulosList);
                    viewModel.communityList = listComunities;
                                  
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
        #endregion

        #region Vehiculos
        public ActionResult listadoVehiculos(long? Id)
        {
            if (Session["USER_ID"] != null && Id != null)
            {
                long userId = (long)Session["USER_ID"];
                user curUser = entities.users.Find(userId);
                Dictionary<long, string> communityDict = new Dictionary<long, string>();
                List<ShowMessage> pubMessageList = ep.GetChatMessages(userId);
                List<Vehiculo> vehiculosLists = new List<Vehiculo>();
                vehiculosLists = entities.Vehiculos.Where(x => x.is_del != true && x.IdTitulo == Id).ToList();
                Titulo titulo = entities.Titulos.Find(Id);
                listadoVehiculosViewModel viewModel = new listadoVehiculosViewModel();
                viewModel.titulo = titulo;
                viewModel.side_menu = "titulares";
                viewModel.side_sub_menu = "titulares_listado";
                viewModel.document_category_list = entities.document_type.ToList();
                viewModel.vehiculosList = vehiculosLists;
                viewModel.curUser = curUser;
                viewModel.messageCount = ep.GetUnreadMessageCount(pubMessageList);                
                viewModel.CantidadDeVehiculos = vehiculosLists.Count();
                viewModel.pubTaskList = ep.GetNotifiTaskList(userId);
                viewModel.pubMessageList = pubMessageList;
                viewModel.messageCount = ep.GetUnreadMessageCount(pubMessageList);

                viewModel.communityID1 = Convert.ToInt64(Session["CURRENT_COMU"]);
                List<Titulo> titulosList = new List<Titulo>();
                titulosList = ep.GetTitulosByTitular(userId);
                listComunities = ep.GetCommunityListByTitular(titulosList);
                viewModel.communityList = listComunities;

                return View(viewModel);
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
                    user curUser = entities.users.Find(userId);
                    List<ShowMessage> pubMessageList = ep.GetChatMessages(userId);
                    Vehiculo vehiculo = entities.Vehiculos.Find(id);
                    editarVehiculoViewModel viewModel = new editarVehiculoViewModel();
                    viewModel.side_menu = "titulares";
                    viewModel.side_sub_menu = "manage_edit_headlines";
                    viewModel.document_category_list = entities.document_type.ToList();
                    viewModel.vehiculo = vehiculo;
                    viewModel.curUser = curUser;
                    viewModel.messageCount = ep.GetUnreadMessageCount(pubMessageList);                    
                    viewModel.pubTaskList = ep.GetNotifiTaskList(userId);
                    viewModel.pubMessageList = pubMessageList;
                    viewModel.messageCount = ep.GetUnreadMessageCount(pubMessageList);

                    List<Titulo> titulosList = new List<Titulo>();
                    titulosList = ep.GetTitulosByTitular(userId);
                    listComunities = ep.GetCommunityListByTitular(titulosList);
                    viewModel.communityList = listComunities;
                    viewModel.communityID1 = Convert.ToInt64(Session["CURRENT_COMU"]);

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
    }
}