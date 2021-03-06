﻿using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity.Infrastructure;
using System.Data.Entity.Validation;
using System.IO;
using System.Linq;
using System.Xml.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using WebApplication1.Areas.webmaster.ViewModels;
using WebApplication1.Concrete;
using WebApplication1.Models;
using Westwind.Web.Mvc;
using System.Net;
using System.Net.Mail;


namespace WebApplication1.Areas.webmaster.Controllers
{
    public class titularesController : Controller
    {
        pjrdev_condominiosEntities entities = new pjrdev_condominiosEntities();
        EFPublicRepository ep = new EFPublicRepository();        
        // GET: webmaster/titulares
      

        #region VEHICULO
        public ActionResult listadoVehiculos(long? Id)
        {
            if (Session["USER_ID"] != null)
            {
                if (Id != null)
                {
                    Titulo tituls = entities.Titulos.Find(Id);
                    if (tituls != null)
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

                        viewModel.vehiculosList = vehiculosLists;
                        viewModel.curUser = curUser;
                        viewModel.CantidadDeVehiculos = vehiculosLists.Count();
                        viewModel.pubTaskList = ep.GetNotifiTaskList(userId);
                        viewModel.pubMessageList = pubMessageList;
                        viewModel.messageCount = ep.GetUnreadMessageCount(pubMessageList);
                        return View(viewModel);
                    }
                    else
                    {
                        return Redirect(Url.Action("listado", "titulares", new { area = "webmaster", Error = "No existe el titulo" }));
                    }
                }
                else
                {
                    return Redirect(Url.Action("listado", "titulares", new { area = "webmaster" }));
                }                
            }
            else
            {
                return Redirect(ep.GetLogoutUrl());
            }
        }
        public ActionResult agregarVehiculo(long? Id, string Error)
        {

            if (Session["USER_ID"] != null)
            {
                if (Id != null)
                {
                    Titulo tituls = entities.Titulos.Find(Id);
                    if (tituls != null)
                    {
                        long userId = (long)Session["USER_ID"];
                        user curUser = entities.users.Find(userId);
                        List<ShowMessage> pubMessageList = ep.GetChatMessages(userId);
                        Titulo titulo = entities.Titulos.Find(Id);
                        listadoVehiculosViewModel viewModel = new listadoVehiculosViewModel();
                        viewModel.titulo = titulo;
                        viewModel.side_menu = "titulares";
                        viewModel.side_sub_menu = "titulares_agregar";

                        viewModel.curUser = curUser;
                        viewModel.pubTaskList = ep.GetNotifiTaskList(userId);
                        viewModel.pubMessageList = pubMessageList;
                        viewModel.messageCount = ep.GetUnreadMessageCount(pubMessageList);
                        viewModel.communityList = entities.communities.ToList();
                        ViewBag.msgError = Error;
                        return View(viewModel);
                    }
                    else
                    {
                        return Redirect(Url.Action("listado", "titulares", new { area = "webmaster", Error = "No existe el titulo" }));
                    }
                }
                else
                {
                    return Redirect(Url.Action("listado", "titulares", new { area = "webmaster" }));
                }               
            }
            else
            {
                return Redirect(ep.GetLogoutUrl());
            }
        }
        public ActionResult editarVehiculo(long? id, string Error)
        {

            if (Session["USER_ID"] != null)
            {
                if (id != null)
                {
                    Vehiculo vehiculo = entities.Vehiculos.Find(id);
                    if (vehiculo != null)
                    {
                        long userId = (long)Session["USER_ID"];
                        user curUser = entities.users.Find(userId);
                        List<ShowMessage> pubMessageList = ep.GetChatMessages(userId);                        
                        editarVehiculoViewModel viewModel = new editarVehiculoViewModel();
                        viewModel.side_menu = "titulares";
                        viewModel.side_sub_menu = "manage_edit_headlines";

                        viewModel.vehiculo = vehiculo;
                        viewModel.curUser = curUser;
                        viewModel.pubTaskList = ep.GetNotifiTaskList(userId);
                        viewModel.pubMessageList = pubMessageList;
                        viewModel.messageCount = ep.GetUnreadMessageCount(pubMessageList);
                        ViewBag.msgError = Error;
                        return View(viewModel);
                    }
                    else
                    {
                        return Redirect(Url.Action("listado", "titulares", new { area = "webmaster", Error = "No existe ese elemento" }));
                    }                   
                }
                else
                {
                    return Redirect(Url.Action("listado", "titulares", new { area = "webmaster" }));
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
                    Vehiculo vehiculo = entities.Vehiculos.Find(id);
                    if (vehiculo != null)
                    {
                        long userId = (long)Session["USER_ID"];
                        user curUser = entities.users.Find(userId);
                        List<ShowMessage> pubMessageList = ep.GetChatMessages(userId);
                       
                        editarVehiculoViewModel viewModel = new editarVehiculoViewModel();
                        viewModel.side_menu = "titulares";
                        viewModel.side_sub_menu = "manage_edit_headlines";

                        viewModel.vehiculo = vehiculo;
                        viewModel.curUser = curUser;
                        viewModel.pubTaskList = ep.GetNotifiTaskList(userId);
                        viewModel.pubMessageList = pubMessageList;
                        viewModel.messageCount = ep.GetUnreadMessageCount(pubMessageList);
                        return View(viewModel);
                    }
                    else
                    {
                        return Redirect(Url.Action("listado", "titulares", new { area = "webmaster", Error = "No existe ese elemento" }));
                    }                   
                }
                else
                {
                    return Redirect(Url.Action("listado", "titulares", new { area = "webmaster" }));
                }
            }
            else
            {
                return Redirect(ep.GetLogoutUrl());
            }
        }

        public ActionResult InsertarVehiculo(long IdTitulo, string brand, string model, string colour, string year, string clapboard,
           string stamp_number)
        {
            if (Session["USER_ID"] != null)
            {
                try
                {
                    Vehiculo vehiculo = new Vehiculo();
                        vehiculo = CrearObjetoVehiculo(IdTitulo, brand, model, colour, year, clapboard, stamp_number, vehiculo);
                        entities.Vehiculos.Add(vehiculo);
                        entities.SaveChanges();
                        return Redirect(Url.Action("listadoVehiculos", "titulares", new { area = "webmaster", Id = IdTitulo }));
                }
                catch (Exception ex)
                {
                    return Redirect(Url.Action("agregarVehiculo", "titulares", new { area = "webmaster", Id = IdTitulo, Error = "Intentando guardar Vehiculo " + ex.Message }));
                }


            }
            else
            {
                return Redirect(ep.GetLogoutUrl());
            }
        }

        public ActionResult ModificarVehiculo(long IdVehiculo, long IdTitulo, string brand, string model, string colour, string year, string clapboard,
          string stamp_number)
        {
            if (Session["USER_ID"] != null)
            {
                try
                {
                    Vehiculo vehiculo = entities.Vehiculos.Find(IdVehiculo);
                    vehiculo = CrearObjetoVehiculo(IdTitulo, brand, model, colour, year, clapboard, stamp_number, vehiculo);
                    entities.SaveChanges();
                    return Redirect(Url.Action("listadoVehiculos", "titulares", new { area = "webmaster", Id = IdTitulo }));
                }
                catch (Exception ex)
                {
                    return Redirect(Url.Action("editarVehiculo", "titulares", new { area = "webmaster", Id = IdVehiculo, Error = "Intentando guardar Vehiculo " + ex.Message }));
                }


            }
            else
            {
                return Redirect(ep.GetLogoutUrl());
            }
        }
        public ActionResult DeleteVehiculo(long delID)
        {
            try
            {
                Vehiculo titulo = entities.Vehiculos.Find(delID);
               
                entities.Vehiculos.Remove(titulo);
                entities.SaveChanges();
                return Json(new { result = "success" });
            }
            catch (Exception ex)
            {
                return Json(new
                {
                    result = "error",
                    exception = ex.HResult
                });
            }
        }
        private Vehiculo CrearObjetoVehiculo(long IdTitulo, string brand, string model, string colour, string year, string clapboard,
           string stamp_number, Vehiculo newVehiculo)
        {
            newVehiculo.Model = model;
            newVehiculo.Brand = brand;
            newVehiculo.Color = colour;
            newVehiculo.IdTitulo = IdTitulo;
            newVehiculo.StampNumber = stamp_number;
            newVehiculo.Year = (year == null) ? 0 : int.Parse(year);
            newVehiculo.ClapBoard = clapboard;





            return newVehiculo;
        }

        #endregion VEHICULO

        #region TITULO
        public ActionResult listadoTitulos(long? Id)
        {
            if (Session["USER_ID"] != null)
            {
                if (Id != null)
                {
                    user userT = entities.users.Find(Id);
                    if (userT != null)
                    {
                        Session["WM_selectedComm"] = null;
                        long userId = (long)Session["USER_ID"];
                        user curUser = entities.users.Find(userId);
                        Dictionary<long, string> communityDict = new Dictionary<long, string>();
                        List<ShowMessage> pubMessageList = ep.GetChatMessages(userId);
                        List<Titulo> titulosList = new List<Titulo>();
                        titulosList = entities.Titulos.Where(x => x.is_del != true && x.IdUser == Id).ToList();

                        listadoTitulosViewModel viewModel = new listadoTitulosViewModel();
                        viewModel.side_menu = "titulares";
                        viewModel.side_sub_menu = "titulares_listado";

                        viewModel.titulosList = titulosList;
                        viewModel.IdUserTitular = (int)Id;
                        viewModel.curUser = curUser;
                        viewModel.pubTaskList = ep.GetNotifiTaskList(userId);
                        viewModel.pubMessageList = pubMessageList;
                        viewModel.messageCount = ep.GetUnreadMessageCount(pubMessageList);
                        return View(viewModel);
                    }
                    else
                    {
                        return Redirect(Url.Action("listado", "titulares", new { area = "webmaster", Error = "No existe el titular" }));
                    }
                }
                else
                {
                    return Redirect(Url.Action("listado", "titulares", new { area = "webmaster" }));
                }                
            }
            else
            {
                return Redirect(ep.GetLogoutUrl());
            }
        }

        public ActionResult agregarTitulo(long? Id, string Error)
        {

            if (Session["USER_ID"] != null)
            {
                if (Id != null)
                {
                    user userT = entities.users.Find(Id);
                    if (userT != null)
                    {
                        long userId = (long)Session["USER_ID"];
                        user curUser = entities.users.Find(userId);
                        List<ShowMessage> pubMessageList = ep.GetChatMessages(userId);
                        listadoTitulosViewModel viewModel = new listadoTitulosViewModel();
                        viewModel.side_menu = "titulares";
                        viewModel.side_sub_menu = "titulares_agregar";

                        viewModel.curUser = curUser;
                        viewModel.IdUserTitular = (int)Id;
                        viewModel.pubTaskList = ep.GetNotifiTaskList(userId);
                        viewModel.pubMessageList = pubMessageList;
                        viewModel.messageCount = ep.GetUnreadMessageCount(pubMessageList);

                        List<community> community = entities.communities.ToList();
                        List<community> communityAllow = new List<community>();
                        foreach (var item in community)
                        {
                            communuser commuxu = entities.communusers.Where(x => x.commun_id == item.id).FirstOrDefault();
                            if (commuxu != null)
                            {
                                communityAllow.Add(item);
                            }
                        }

                        viewModel.communityList = communityAllow;
                        ViewBag.msgError = Error;
                        return View(viewModel);
                    }
                    else
                    {
                        return Redirect(Url.Action("listado", "titulares", new { area = "webmaster", Error = "No existe el titular" }));
                    }
                }
                else
                {
                    return Redirect(Url.Action("listado", "titulares", new { area = "webmaster" }));
                }                
            }
            else
            {
                return Redirect(ep.GetLogoutUrl());
            }
        }

        public ActionResult editarTitulo(long? id,string Error)
        {

            if (Session["USER_ID"] != null)
            {
                if (id != null)
                {
                    Titulo titulo = entities.Titulos.Find(id);
                    if (titulo != null)
                    {
                        long userId = (long)Session["USER_ID"];                       
                        List<ShowMessage> pubMessageList = ep.GetChatMessages(userId);
                        user curUser = entities.users.Find(userId);
                        editarTituloViewModel viewModel = new editarTituloViewModel();
                        viewModel.side_menu = "titulares";
                        viewModel.side_sub_menu = "manage_edit_headlines";

                        viewModel.curUser = curUser;
                        viewModel.titulo = titulo;
                        viewModel.pubTaskList = ep.GetNotifiTaskList(userId);
                        viewModel.pubMessageList = pubMessageList;
                        List<community> community = entities.communities.ToList();
                        List<community> communityAllow = new List<community>();
                        foreach (var item in community)
                        {
                            communuser commuxu = entities.communusers.Where(x => x.commun_id == item.id).FirstOrDefault();
                            if (commuxu != null)
                            {
                                communityAllow.Add(item);
                            }
                        }
                        viewModel.communityList = communityAllow;
                        viewModel.messageCount = ep.GetUnreadMessageCount(pubMessageList);
                        ViewBag.msgError = Error;
                        return View(viewModel);
                    }
                    else
                    {
                        return Redirect(Url.Action("listado", "titulares", new { area = "webmaster", Error = "No existe ese elemento" }));
                    }                        
                }
                else
                {
                    return Redirect(Url.Action("listado", "titulares", new { area = "webmaster" }));
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
                if (id != null)
                {
                    Titulo titulo = entities.Titulos.Find(id);
                    if (titulo != null)
                    {
                        long userId = (long)Session["USER_ID"];
                        List<ShowMessage> pubMessageList = ep.GetChatMessages(userId);
                        user curUser = entities.users.Find(userId);
                        editarTituloViewModel viewModel = new editarTituloViewModel();
                        viewModel.side_menu = "titulares";
                        viewModel.side_sub_menu = "manage_edit_headlines";

                        viewModel.curUser = curUser;
                        viewModel.titulo = titulo;
                        viewModel.pubTaskList = ep.GetNotifiTaskList(userId);
                        viewModel.pubMessageList = pubMessageList;
                        viewModel.communityList = entities.communities.ToList();
                        viewModel.messageCount = ep.GetUnreadMessageCount(pubMessageList);
                        return View(viewModel);
                    }
                    else
                    {
                        return Redirect(Url.Action("listado", "titulares", new { area = "webmaster", Error = "No existe ese elemento" }));
                    }                    
                }
                else
                {
                    return Redirect(Url.Action("listado", "titulares", new { area = "webmaster" }));
                }
            }
            else
            {
                return Redirect(ep.GetLogoutUrl());
            }
        }

        [HttpPost]
        public ActionResult InsertarTitulo(long IdUser, string siono, string since, string until, int IdCommunity,
            string tenant_first_name1, string tenant_last_name1, string tenant_mother_last_name1, string apartment,
            HttpPostedFileBase script_file, string leased_postal_address, string leased_residential_address, string acq_date, HttpPostedFileBase writing_script)
        {
            if (Session["USER_ID"] != null)
            {
                try
                {
                    if (writing_script != null)
                    {
                        Titulo titulo = new Titulo();
                         titulo = CrearObjetoTitulo(IdUser, siono, since, until, IdCommunity,
                                          tenant_first_name1, tenant_last_name1, tenant_mother_last_name1, apartment,
                                          script_file, leased_postal_address, leased_residential_address, acq_date, writing_script, titulo);
                        entities.Titulos.Add(titulo);
                        entities.SaveChanges();
                        Session["WM_selectedComm"] = null;
                        return Redirect(Url.Action("listadoTitulos", "titulares", new { area = "webmaster", Id = IdUser, }));
                    }
                    else
                    {
                        return Redirect(Url.Action("agregarTitulo", "titulares", new { area = "webmaster", Id = IdUser, Error = "Debe cargar el titulo de propiedad" }));
                    }                                      
                }
                catch (Exception ex)
                {
                    return Redirect(Url.Action("agregarTitulo", "titulares", new { area = "webmaster", Id = IdUser, Error = "Intentando guardar Titulo "+ ex.Message }));
                }


            }
            else
            {
                return Redirect(ep.GetLogoutUrl());
            }
        }

        [HttpPost]
        public ActionResult EditarTitulo(long IdTitulo, long IdUser, string siono, string since, string until, int IdCommunity,
           string tenant_first_name1, string tenant_last_name1, string tenant_mother_last_name1, string apartment,
           HttpPostedFileBase script_file, string leased_postal_address, string leased_residential_address, string acq_date, HttpPostedFileBase writing_script)
        {
            if (Session["USER_ID"] != null)
            {
                try
                {
                    Titulo titulo = entities.Titulos.Find(IdTitulo);
                   
                        titulo = CrearObjetoTitulo(0, siono, since, until, IdCommunity,
                                         tenant_first_name1, tenant_last_name1, tenant_mother_last_name1, apartment,
                                         script_file, leased_postal_address, leased_residential_address, acq_date, writing_script, titulo);
                       
                        entities.SaveChanges();
                        return Redirect(Url.Action("listadoTitulos", "titulares", new { area = "webmaster", Id = IdUser }));



                }
                catch (Exception ex)
                {
                    return Redirect(Url.Action("editarTitulo", "titulares", new { area = "webmaster", Id = IdTitulo, Error = "Intentando guardar Titulo" + ex.Message }));
                }


            }
            else
            {
                return Redirect(ep.GetLogoutUrl());
            }
        }

        private Titulo CrearObjetoTitulo(long IdUser, string siono, string since, string until, int IdCommunity,
            string tenant_first_name1, string tenant_last_name1, string tenant_mother_last_name1, string apartment,
            HttpPostedFileBase script_file, string leased_postal_address, string leased_residential_address, string acq_date, HttpPostedFileBase writing_script, Titulo newTitulo)
        {
            if (IdUser > 0)
            {
                newTitulo.IdUser = IdUser;
            }            
            newTitulo.apartment = apartment;
            string urlW = GuardarArchivos(writing_script, "~/Upload/Upload_Writing");
            if (urlW != null)
            {
                newTitulo.upload_writing = urlW;
            }
            string urlF = GuardarArchivos(script_file, "~/Upload/Upload_Contract");
            if (urlF != null)
            {
                newTitulo.leased_upload_file = urlF;
            }
           
            newTitulo.is_leased = (siono == "boxyes") ? true : false;
            newTitulo.acq_date = (ValidarFecha(acq_date) == null) ? DateTime.Now : (DateTime)ValidarFecha(acq_date);
            newTitulo.since = ValidarFecha(since);
            newTitulo.until = ValidarFecha(until);
            newTitulo.tenant_first_name1 = tenant_first_name1;
            newTitulo.tenant_last_name1 = tenant_last_name1;
            newTitulo.tenant_mother_last_name1 = tenant_mother_last_name1;
            newTitulo.leased_postal_address = leased_postal_address;
            newTitulo.leased_residential_address = leased_residential_address;
            newTitulo.IdCommunity = IdCommunity;


            return newTitulo;
        }
        #endregion TITULO

        #region TITULAR

        public ActionResult listado(string Error, int? iN, string searchStr = "", int CommunityId = -1)
        {
            if (Session["USER_ID"] != null)
            {
                try
                {
                    long userId = (long)Session["USER_ID"];
                    user curUser = entities.users.Find(userId);
                    Dictionary<long, string> communityDict = new Dictionary<long, string>();
                    List<ShowMessage> pubMessageList = ep.GetChatMessages(userId);
                    List<user> titularList = new List<user>();
                    List<community> communityList = entities.communities.ToList();

                    long commId = Convert.ToInt64(CommunityId);                   
                    if (Session["WM_selectedComm"] != null)
                    {
                        if(commId != -1)
                        {
                            Session["WM_selectedComm"] = commId;
                        }
                        else
                        {
                            Session["WM_selectedComm"] = -1;
                        }                        
                    }
                    else
                    {
                        Session["WM_selectedComm"] = commId;
                    }              
                    
                    if (searchStr == "" && commId == -1)
                    {
                        var query1 = (from r in entities.users
                                      where
                                    r.role == 1 && r.is_del != true
                                      select r);
                        titularList = query1.ToList();
                    }
                    else if (searchStr != "" && commId == -1)
                    {
                        var query = (from r in entities.users
                                     where r.role == 1 &&
                                     (r.first_name1.Contains(searchStr) == true
                                     || r.last_name1.Contains(searchStr) == true)
                                     && r.is_del != true
                                     select r);
                        titularList = query.ToList();
                    }
                    else if (searchStr == "" && commId != -1)
                    {
                        var query2 = (from r in entities.users
                                      where r.role == 1 && r.is_del != true && r.Titulos.Any(x => x.IdCommunity == commId)
                                      select r
                                      );
                        titularList = query2.ToList();
                    }
                    else
                    {
                        var query3 = (from r in entities.users
                                      where r.role == 1 &&
                                     (r.first_name1.Contains(searchStr) == true
                                     || r.last_name1.Contains(searchStr) == true)
                                     && r.is_del != true && r.Titulos.Any(x => x.IdCommunity == commId)
                                      select r);
                        titularList = query3.ToList();
                    }

                    foreach (var item in titularList)
                    {
                        string communityName = ep.GetCommunityCoInfo(item.id)[0];                             
                        communityDict.Add(item.id, communityName);                       
                    }
                   
                    listadoTitularesViewModel viewModel = new listadoTitularesViewModel();
                    viewModel.communityList = communityList;
                    viewModel.side_menu = "titulares";
                    viewModel.side_sub_menu = "titulares_listado";
                    viewModel.titularList = titularList;
                    viewModel.searchStr = searchStr;
                    viewModel.curUser = curUser;
                    viewModel.communityDict = communityDict;
                    viewModel.pubTaskList = ep.GetNotifiTaskList(userId);
                    viewModel.pubMessageList = pubMessageList;
                    viewModel.messageCount = ep.GetUnreadMessageCount(pubMessageList);
                    ViewBag.msgError = Error;
                    ViewBag.selectForm = iN;
                    return View(viewModel);
                }
                catch (Exception ex)
                {
                    return Redirect(Url.Action("error", "control", new { area = "webmaster", Error = "Error al obtener listado de titulares: " + ex.Message }));
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
                if (Session["WM_selectedComm"] != null)
                {
                    long selectedCommu = Convert.ToInt64(Session["WM_selectedComm"]);
                    if (selectedCommu != -1)
                    {
                        List<communuser> comUer = entities.communusers.Where(x => x.commun_id == selectedCommu).ToList();
                        if (comUer.Count > 0)
                        {
                            long userId = (long)Session["USER_ID"];
                            user curUser = entities.users.Find(userId);
                            List<ShowMessage> pubMessageList = ep.GetChatMessages(userId);
                            agregarTitularesViewModel viewModel = new agregarTitularesViewModel();
                            viewModel.side_menu = "titulares";
                            viewModel.side_sub_menu = "titulares_agregar";
                            
                            viewModel.curUser = curUser;
                            viewModel.pubTaskList = ep.GetNotifiTaskList(userId);
                            viewModel.pubMessageList = pubMessageList;
                            viewModel.messageCount = ep.GetUnreadMessageCount(pubMessageList);
                            viewModel.communityList = entities.communities.ToList();
                            ViewBag.msgError = Error;
                            return View(viewModel);
                        }
                        else
                        {
                            return Redirect(Url.Action("listado", "titulares", new { area = "webmaster", Error = "No se puede crear titulares en una comunidad sin administradores" }));
                        }
                    }
                    else
                    {
                        return Redirect(Url.Action("listado", "titulares", new { area = "webmaster", Error = "Debe tener seleccionada una comunidad para crear titulares", iN = 1 }));
                    }
                }
                else
                {
                    return Redirect(Url.Action("listado", "titulares", new { area = "webmaster" }));
                }                                              
            }
            else
            {
                return Redirect(ep.GetLogoutUrl());
            }
        }

        public ActionResult ver(long? id)
        {
            if (Session["USER_ID"] != null)
            {
                if (id != null)
                {
                    long userId = (long)Session["USER_ID"];
                    user curUser = entities.users.Find(userId);
                    List<ShowMessage> pubMessageList = ep.GetChatMessages(userId);
                    user editUser = entities.users.Find(id);
                    verTitularesViewModel viewModel = new verTitularesViewModel();
                    viewModel.side_menu = "titulares";
                    viewModel.side_sub_menu = "manage_edit_headlines";
                    
                    viewModel.editUser = editUser;
                    viewModel.view_resident_logo = "~/App_Data/User_Logo/" + editUser.user_img;
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
            }
            else
            {
                return Redirect(ep.GetLogoutUrl());
            }
        }
        public ActionResult editar(long? id, string Error)
        {

            if (Session["USER_ID"] != null)
            {
                if (id != null)
                {
                    long userId = (long)Session["USER_ID"];
                    user curUser = entities.users.Find(userId);
                    List<ShowMessage> pubMessageList = ep.GetChatMessages(userId);
                    user editUser = entities.users.Find(id);
                    editarTitularesViewModel viewModel = new editarTitularesViewModel();
                    viewModel.side_menu = "titulares";
                    viewModel.side_sub_menu = "manage_edit_headlines";
                    
                    viewModel.editUser = editUser;
                    viewModel.curUser = curUser;
                    viewModel.password = ep.Decrypt(editUser.password);
                    viewModel.pubTaskList = ep.GetNotifiTaskList(userId);
                    viewModel.pubMessageList = pubMessageList;
                    viewModel.messageCount = ep.GetUnreadMessageCount(pubMessageList);
                    ViewBag.msgError = Error;
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

        [HttpPost]
        public ActionResult InsertarTitular(HttpPostedFileBase user_logo, string email, string password, string first_name1,
                string last_name1, string mother_last_name1, string phone_number1, string postal_address, string residential_address)
        {
            if (Session["USER_ID"] != null)
            {
                try
                {
                    user newResident = new user();
                    //Metodo que crea el objeto user
                     newResident = CrearObjetoUser(user_logo, email, password, first_name1, last_name1, mother_last_name1,
                                                  phone_number1, postal_address, residential_address, newResident);
                    //Condicion para validadr si el email ya existe en la base de datos
                    if (entities.users.Where(x => x.email == newResident.email).ToList().Count > 0)
                    {
                        return Redirect(Url.Action("agregar", "titulares", new { area = "webmaster", Error = "El email ya existe" }));
                    }
                    else
                    {
                        entities.users.Add(newResident);
                        entities.SaveChanges();                      
                        return Redirect(Url.Action("agregarTitulo", "titulares", new { area = "webmaster", Id = newResident.id }));
                    }
                }
                catch (Exception ex)
                {
                    return Redirect(Url.Action("agregar", "titulares", new { area = "webmaster", Error = "Intentando guardar Titular" }));
                }


            }
            else
            {
                return Redirect(ep.GetLogoutUrl());
            }
        }

        public ActionResult EditarTitular(long editID, HttpPostedFileBase user_logo, string email, string password, string first_name1,
               string last_name1, string mother_last_name1, string phone_number1, string postal_address, string residential_address)
        {
            if (Session["USER_ID"] != null)
            {
                try
                {
                    user editResident = entities.users.Find(editID);
                    //Metodo que crea el objeto user
                    editResident = CrearObjetoUser(user_logo, email, password, first_name1, last_name1, mother_last_name1,
                                                  phone_number1, postal_address, residential_address, editResident);
                  //Se Guardan los cambias realizados a la entidad
                        entities.SaveChanges();

                        return Redirect(Url.Action("listado", "titulares", new { area = "webmaster" }));
                }
                catch (Exception ex)
                {
                    return Redirect(Url.Action("editar", "titulares", new { area = "webmaster",id = editID, Error = "Intentando guardar Titular"+ ex.Message }));
                }


            }
            else
            {
                return Redirect(ep.GetLogoutUrl());
            }
        }

        private user CrearObjetoUser(HttpPostedFileBase user_logo, string email, string password, string first_name1,
          string last_name1, string mother_last_name1, string phone_number1, string postal_address, string residential_address, user Titular)
        {
          
            long userId = (long)Session["USER_ID"];
            if (Titular.user_img != null)
            {
                string url = GuardarArchivos(user_logo, "~/Upload/User_Logo");
                Titular.user_img = (url != null)? url: Titular.user_img;
            }
            else
            {
                Titular.user_img = GuardarArchivos(user_logo, "~/Upload/User_Logo");
            }            
            Titular.email = email;
            Titular.password = ep.Encrypt(password);
            Titular.first_name1 = first_name1;
            Titular.last_name1 = last_name1;
            Titular.mother_last_name1 = mother_last_name1;
            Titular.phone_number1 = phone_number1;
            Titular.postal_address = postal_address;
            Titular.residential_address = residential_address;
            Titular.role = 1;
            Titular.create_userid = userId;
            Titular.acq_date = DateTime.Now;           
            return Titular;
        }

        #endregion TITULAR

        private void GuardarComunidadxUsuario(long newResidentID, List<string> communityID)
        {
            communuser communuser = new communuser();
            communuser.user_id = newResidentID;
            if (communityID.Count > 0)
            {
                foreach (var item in communityID)
                {
                    communuser.commun_id = int.Parse(item);
                    entities.communusers.Add(communuser);
                    entities.SaveChanges();
                }
            }

        }

       

        private DateTime? ValidarFecha(string fecha) {
            if (fecha == "" || fecha == "0001-01-01" || fecha == null)
            {
                return null;
            }
            else
            {
                return DateTime.Parse(fecha);
            }

        }

        private string GuardarArchivos(HttpPostedFileBase Archivo, string p_Path)
        {
            if (Archivo != null && Archivo.ContentLength > 0)
            {
                var fileName = Path.GetFileName(Archivo.FileName);
                // store the file inside ~/App_Data/uploads folder
                var path = Path.Combine(Server.MapPath(p_Path), fileName);
                Archivo.SaveAs(path);
                return fileName;
            }
            else
            {
                return null;
            }
        }

        public JsonResult DeleteUser(long delID)
        {
            try
            {
                List<chatmessage> chatmessages = entities.chatmessages.Where(m => m.from_user_id == delID
                || m.to_user_id == delID).ToList();
                entities.chatmessages.RemoveRange(chatmessages);
                // Delete blogs
                List<blog> blogs = entities.blogs.Where(m => m.user_id == delID).ToList();
                foreach (var item in blogs)
                {
                    List<blogcomment> blogcomments = entities.blogcomments.Where(m => m.blog_id == item.id).ToList();
                    entities.blogcomments.RemoveRange(blogcomments);
                }
                entities.blogs.RemoveRange(blogs);
                // Delete bank info
                List<bank> banks = entities.banks.Where(m => m.user_id == delID).ToList();
                foreach (var item in banks)
                {
                    List<fee> fees = entities.fees.Where(m => m.bank_id == item.id).ToList();
                    entities.fees.RemoveRange(fees);
                }
                entities.banks.RemoveRange(banks);
                // Delete CreditCards
                List<creditcard> creditcards = entities.creditcards.Where(m => m.user_id == delID).ToList();
                entities.creditcards.RemoveRange(creditcards);
                List<taskuser> taskusers = entities.taskusers.Where(m => m.user_id == delID).ToList();
                entities.taskusers.RemoveRange(taskusers);
                List<taskcomment> taskcomments = entities.taskcomments.Where(m => m.user_id == delID).ToList();
                entities.taskcomments.RemoveRange(taskcomments);
                List<uso> usos = entities.usoes.Where(m => m.create_userid == delID).ToList();
                entities.usoes.RemoveRange(usos);
                List<emailtheme> emailthemes = entities.emailthemes.Where(m => m.user_id == delID).ToList();
                entities.emailthemes.RemoveRange(emailthemes);

                List<Vehiculo> vehiculos = entities.Vehiculos.Where(x => x.Titulo.IdUser == delID).ToList();
                entities.Vehiculos.RemoveRange(vehiculos);
                List<Titulo> titulos = entities.Titulos.Where(x => x.IdUser == delID).ToList();
                entities.Titulos.RemoveRange(titulos);
                List<comment> comentarios = entities.comments.Where(x=> x.user_id == delID).ToList();
                entities.comments.RemoveRange(comentarios);

                List<book> reservas = entities.books.Where(x => x.idUser == delID).ToList();
                entities.books.RemoveRange(reservas);

                user delUser = entities.users.Find(delID);                
                entities.users.Remove(delUser);
                return Json(new { result = "success" });
            }
            catch (Exception ex)
            {
                return Json(new { result = "error", exception = ex.HResult });
            }
        }

        public JsonResult ActiveUser(long delID)
        {
            try
            {
                user user = entities.users.Find(delID);
                user.is_active = true;
                entities.SaveChanges();
                return Json(new { result = "success" });
            }
            catch (Exception ex)
            {
                return Json(new { result = "error", exception = ex.HResult });
            }
        }

        public JsonResult DeactiveUser(long delID)
        {
            try
            {
                user user = entities.users.Find(delID);
                user.is_active = false;
                entities.SaveChanges();
                return Json(new { result = "success" });
            }
            catch (Exception ex)
            {
                return Json(new { result = "error", exception = ex.HResult });
            }
        }

        [HttpPost]
        public FileResult ExportCSV(string searchStr = "")
        {
            List<user> userList = new List<user>();
            long userId = (long)Session["USER_ID"];

            if (searchStr == "")
            {               
                var query1 = (from r in entities.users
                              where r.role == 1 && r.is_del != true                
                              select r);
                userList = query1.ToList();

            }
            else
            {
                var query1 = (from r in entities.users
                              where r.role == 1 && (r.first_name1.Contains(searchStr) || r.last_name1.Contains(searchStr)) && r.is_del != true                            
                              select r);
                userList = query1.ToList();
            }

            //string[] columnNames = new string[] { "First Name", "Last Name", "Acquired Date",
            //"Email", "Password", "Phone Number", "Postal Address", "Residential Address",
            //"Siono"};
            string csv = string.Empty;
            StringBuilder csvContent = new StringBuilder();

            //foreach (string columnName in columnNames)
            //{
                //Add the Header row for CSV file.
            csv += csvContent.AppendLine("First Name,Last Name,Acquired Date,Email,Password,Phone Number,Postal Address,Residential Address,Siono");
            //}

            //Add new line.
            //csv += "\r\n";

            foreach (var item in userList)
            {
                csv = string.Empty;
                string phoneNumber1, postalAddress, resAddress, siNo = "";
                if (item.phone_number1 != null)
                {
                    phoneNumber1 = item.phone_number1.ToString();
                }
                else
                {
                    phoneNumber1 = "".ToString();
                }

                if (item.postal_address != null)
                {
                    postalAddress = item.postal_address.ToString();
                }
                else
                {
                    postalAddress = "".ToString();
                }

                if (item.residential_address != null)
                {
                    resAddress = item.residential_address.ToString();
                }
                else
                {
                    resAddress = "".ToString();
                }

                if (item.siono != null)
                {
                    siNo = item.siono.ToString();
                }
                else
                {
                    siNo = "".ToString();
                }

                csv += csvContent.AppendLine(item.first_name1.ToString() + "," + item.last_name1.ToString() + "," + item.acq_date.ToString("yyyy-MM-dd")
                    + "," + item.email.ToString() + "," + item.password.ToString() + "," + phoneNumber1 + "," + postalAddress
                    + "," + resAddress + "," + siNo);

                //csv += item.first_name1.ToString().Replace(",", ";") + ",";
                //csv += item.last_name1.ToString().Replace(",", ";") + ",";
                //csv += item.acq_date.ToString("yyyy-MM-dd").Replace(",", ";") + ",";
                //csv += item.email.ToString().Replace(",", ";") + ",";
                //csv += item.password.ToString().Replace(",", ";") + ",";
                //if (item.phone_number1 != null)
                //{
                //    csv += item.phone_number1.ToString().Replace(",", ";") + ",";
                //}
                //else
                //{
                //    csv += "".Replace(",", ";") + ",";
                //}

                //if (item.postal_address != null)
                //{
                //    csv += item.postal_address.ToString().Replace(",", ";") + ",";
                //}
                //else
                //{
                //    csv += "".ToString().Replace(",", ";") + ",";
                //}

                //if (item.residential_address != null)
                //{
                //    csv += item.residential_address.ToString().Replace(",", ";") + ",";
                //}
                //else
                //{
                //    csv += "".ToString().Replace(",", ";") + ",";
                //}

                //if (item.siono != null)
                //{
                //    csv += item.siono.ToString().Replace(",", ";") + ",";
                //}
                //else
                //{
                //    csv += "".ToString().Replace(",", ";") + ",";
                //}

                ////Add new line.
                //csv += "\r\n";
            }

            //Download the CSV file.
            byte[] bytes = Encoding.ASCII.GetBytes(csv);
            return File(bytes, "application/text", "titular.csv");

        }

        [HttpPost]
        public async Task<ActionResult> ImportCsv(HttpPostedFileBase csv_file)
        {
            try
            {
                string emailTemplate = "";
                long userId = (long)Session["USER_ID"];
                user curUser = entities.users.Find(userId);
                if (csv_file != null && csv_file.ContentLength > 0)
                {
                    string path1 = string.Format("{0}/{1}",
                        Server.MapPath("~/Upload/Upload_Import"), csv_file.FileName);
                    string[] validFileTypes = { ".xls", ".xlsx", ".csv" };
                    string extension = System.IO.Path.GetExtension(csv_file.FileName).ToLower();

                    if (!Directory.Exists(path1))
                    {
                        Directory.CreateDirectory(Server.MapPath("~/Upload/Upload_Import"));
                    }

                    if (validFileTypes.Contains(extension))
                    {
                        if (System.IO.File.Exists(path1))
                        {
                            System.IO.File.Delete(path1);
                        }
                        csv_file.SaveAs(path1);

                        if (extension == ".csv")
                        {
                            DataTable dt = Utility.ConvertCSVtoDataTable(path1);
                            for (int i = 0; i < dt.Rows.Count; i++)
                            {
                                emailTemplate = "";
                                user user = new user();
                                string first_name = (string)dt.Rows[i].ItemArray.GetValue(0);
                                string last_name = (string)dt.Rows[i].ItemArray.GetValue(1);
                                string acqDateStr = (string)dt.Rows[i].ItemArray.GetValue(2);
                                DateTime acq_date = DateTime.ParseExact(acqDateStr, "yyyy-MM-dd",
                                                    System.Globalization.CultureInfo.InvariantCulture);
                                string email = (string)dt.Rows[i].ItemArray.GetValue(3);
                                string password = ep.Encrypt((string)dt.Rows[i].ItemArray.GetValue(4));
                                string phone_number = (string)dt.Rows[i].ItemArray.GetValue(5);
                                string postal_address = (string)dt.Rows[i].ItemArray.GetValue(6);
                                string residential_address = (string)dt.Rows[i].ItemArray.GetValue(7);
                                string sinno = (string)dt.Rows[i].ItemArray.GetValue(8);
                                user.first_name1 = first_name;
                                user.last_name1 = last_name;
                                user.email = email;
                                user.password = password;
                                user.phone_number1 = phone_number;
                                user.postal_address = postal_address;
                                user.residential_address = residential_address;
                                user.role = 1;
                                user.acq_date = acq_date;
                                if (sinno == "boxyes")
                                {
                                    user.siono = sinno;
                                }
                                else
                                {
                                    user.siono = "boxno";
                                }

                                user.create_userid = userId;
                                user.is_active = true;
                                user.is_admin = false;
                                user.is_logged = false;
                                entities.users.Add(user);
                                entities.SaveChanges();
                                emailtheme emailtheme = entities.emailthemes.Where(m => m.user_id == userId &&
                                                        m.type_id == 1).FirstOrDefault();
                                //if (emailtheme != null)
                                //{
                                //    emailTemplate = emailtheme.htmcontent;
                                //} else
                                //{
                                //    string patialView = "~/Areas/webmaster/Views/titulares/emailing.cshtml";
                                //    emailingViewModel viewModel = new emailingViewModel();
                                //    emailTemplate = ViewRenderer.RenderPartialView(patialView, viewModel);
                                //}

                                string patialView = "~/Views/iniciar/emailing.cshtml";
                                emailingViewModel viewModel = new emailingViewModel();
                                viewModel.firstName = first_name;
                                viewModel.lastName = last_name;
                                viewModel.fromEmail = curUser.email;
                                viewModel.toEmail = email;
                                emailTemplate = ViewRenderer.RenderPartialView(patialView, viewModel);

                                int a = await ep.SendAlertEmail(
                                   curUser.email, email,
                                   curUser.first_name1 + " " + curUser.last_name1,
                                   "añadir título",
                                   "has sido añadido como titular\n password: " + password,
                                   emailTemplate
                                   );
                            }
                        }
                    }
                }
                return Redirect(Url.Action("listado", "titulares", new { area = "webmaster" }));
            }
            catch (DbEntityValidationException ex)
            {
                foreach (DbEntityValidationResult item in ex.EntityValidationErrors)
                {
                    // Get entry

                    DbEntityEntry entry = item.Entry;
                    string entityTypeName = entry.Entity.GetType().Name;

                    // Display or log error messages

                    foreach (DbValidationError subItem in item.ValidationErrors)
                    {
                        string message = string.Format("Error '{0}' occurred in {1} at {2}",
                                 subItem.ErrorMessage, entityTypeName, subItem.PropertyName);
                        Console.WriteLine(message);
                    }
                }
                return Redirect(Url.Action("listado", "titulares", new { area = "webmaster" }));
            }

        }

        public JsonResult DeleteTitulares(long delID)
        {
            try
            {
                // Delete Forign records
                // Delete USO
                // Delete related records in other tables
                List<chatmessage> chatmessages = entities.chatmessages.Where(m => m.from_user_id == delID
                || m.to_user_id == delID).ToList();
                entities.chatmessages.RemoveRange(chatmessages);
                // Delete blogs
                List<blog> blogs = entities.blogs.Where(m => m.user_id == delID).ToList();
                foreach (var item in blogs)
                {
                    List<blogcomment> blogcomments = entities.blogcomments.Where(m => m.blog_id == item.id).ToList();
                    entities.blogcomments.RemoveRange(blogcomments);
                }
                entities.blogs.RemoveRange(blogs);
                // Delete bank info
                List<bank> banks = entities.banks.Where(m => m.user_id == delID).ToList();
                foreach (var item in banks)
                {
                    List<fee> fees = entities.fees.Where(m => m.bank_id == item.id).ToList();
                    entities.fees.RemoveRange(fees);
                }
                entities.banks.RemoveRange(banks);
                // Delete CreditCards
                List<creditcard> creditcards = entities.creditcards.Where(m => m.user_id == delID).ToList();
                entities.creditcards.RemoveRange(creditcards);
                List<taskuser> taskusers = entities.taskusers.Where(m => m.user_id == delID).ToList();
                entities.taskusers.RemoveRange(taskusers);
                List<uso> usos = entities.usoes.Where(m => m.create_userid == delID).ToList();
                entities.usoes.RemoveRange(usos);
                List<emailtheme> emailthemes = entities.emailthemes.Where(m => m.user_id == delID).ToList();
                entities.emailthemes.RemoveRange(emailthemes);
                user delUser = entities.users.Find(delID);
                delUser.is_del = true;

                //Delete Titulos
                List<Titulo> titulosd = entities.Titulos.Where(m => m.user.id == delID).ToList();
                foreach (var item2 in titulosd)
                {
                    List<Vehiculo> vehiculosd = entities.Vehiculos.Where(m => m.Titulo.Id == item2.Id).ToList();
                    entities.Vehiculos.RemoveRange(vehiculosd);
                }
                entities.Titulos.RemoveRange(titulosd);

                entities.SaveChanges();
                return Json(new { result = "success" });
            }
            catch (Exception ex)
            {
                return Json(new
                {
                    result = "error",
                    exception = ex.HResult
                });
            }
        }

        public JsonResult DeleteTitulo(long delID) {
            try {
                Titulo titulo = entities.Titulos.Find(delID);
                List<Vehiculo> vehiculosList = entities.Vehiculos.Where(x => x.IdTitulo == titulo.Id).ToList();
                entities.Vehiculos.RemoveRange(vehiculosList);
                entities.Titulos.Remove(titulo);
                entities.SaveChanges();
                return Json(new { result = "success" });
            }
            catch (Exception ex)
            {
                return Json(new
                {
                    result = "error",
                    exception = ex.HResult
                });
            }
        }
       
        //public JsonResult almacenarID(long id)
        //{
        //    try
        //    {
        //        Session["idTitular"] = id;
        //        return Json(new { result = "success" });
        //    }
        //    catch (Exception ex)
        //    {
        //        return Json(new
        //        {
        //            result = "error",
        //            exception = ex.HResult
        //        });
        //    }
        //}
        
        //public JsonResult listaComunidades()
        //{
        //    try
        //    {
        //        long userId = (long)Session["idTitular"];
        //        List<Titulo> titulosList = new List<Titulo>();
        //        List<community> listComunities = new List<community>();
        //        titulosList = ep.GetTitulosByTitular(userId);
        //        listComunities = ep.GetCommunityListByTitular(titulosList);
        //        return Json(new { result = listComunities });
        //    }
        //    catch (Exception ex)
        //    {
        //        return Json(new
        //        {
        //            result = "error",
        //            exception = ex.HResult
        //        });
        //    }
        //}



        [HttpPost]
        public ActionResult SeacrhResult(string searchStr, int searchCommunityId)
        {
            return Redirect(Url.Action("listado", "titulares", new { area = "webmaster", searchStr = searchStr, CommunityId = searchCommunityId }));
        }
    }
}