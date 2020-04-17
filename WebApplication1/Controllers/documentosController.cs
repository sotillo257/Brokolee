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
    public class documentosController : Controller
    {
        pjrdev_condominiosEntities entities = new pjrdev_condominiosEntities();
        EFPublicRepository ep = new EFPublicRepository();

        List<Titulo> titulosList = new List<Titulo>();
        List<community> listComunities = new List<community>();

        // GET: documentos
        public ActionResult legales(int? type_id, string searchStr = "")
        {
            if (Session["USER_ID"] != null)
            {
                if (type_id != null)
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
                        document_type document_Type = entities.document_type.Find(type_id);
                        List<document> documentList = new List<document>();

                        long communityAct = Convert.ToInt64(Session["CURRENT_COMU"]);

                        if (searchStr == "")
                        {
                            var query = (from r in entities.documents
                                         where r.type_id == type_id && r.community_id == communityAct
                                         select r);
                            documentList = query.ToList();
                        }
                        else
                        {
                            var query = (from r in entities.documents
                                         where r.type_id == type_id && (r.first_name.Contains(searchStr) == true) && r.community_id == communityAct
                                         select r);
                            documentList = query.ToList();
                        }

                        documentosViewModel viewModel = new documentosViewModel();

                        titulosList = ep.GetTitulosByTitular(userId);
                        listComunities = ep.GetCommunityListByTitular(titulosList);
                        viewModel.communityList = listComunities;                     

                        viewModel.side_menu = "documentos";
                        viewModel.side_sub_menu = "documentos_" + document_Type.type_name;
                        viewModel.documentList = documentList;
                        viewModel.document_category_list = entities.document_type.ToList();
                        viewModel.curUser = curUser;
                        viewModel.searchStr = searchStr;
                        viewModel.typeId = Convert.ToInt32(type_id);
                        viewModel.pubTaskList = ep.GetNotifiTaskList(userId);
                        viewModel.pubMessageList = pubMessageList;
                        viewModel.messageCount = ep.GetUnreadMessageCount(pubMessageList);                       
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
                return Redirect(Url.Action("iniciar", "iniciar"));
            }
                
        }

        public ActionResult SeacrhResult(int type_id, string searchStr)
        {
            return Redirect(Url.Action("legales", "documentos",
                new
                {
                    type_id = type_id,
                    searchStr = searchStr
                }));
        }
    }
}