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

                List<document_type> document_category_list = entities.document_type.Where(x => x.community_id == communityAct).ToList();
                documentosViewModel viewModel = new documentosViewModel();

                titulosList = ep.GetTitulosByTitular(userId);
                listComunities = ep.GetCommunityListByTitular(titulosList);
                viewModel.communityList = listComunities;

                document_type document_type = entities.document_type.Find(idCategory);
                viewModel.side_menu = "documentos";
                if (idCategory != 0)
                {
                    viewModel.side_sub_menu = "documentos_" + document_type.type_name;
                }
                else
                {
                    viewModel.side_sub_menu = "documentos_listado";
                }
                viewModel.document_category_list = document_category_list;
                viewModel.documentList = document_list;
                viewModel.searchStr = searchStr;
                viewModel.typeID = idCategory;
                viewModel.curUser = curUser;
                viewModel.pubTaskList = ep.GetNotifiTaskList(userId);
                viewModel.pubMessageList = pubMessageList;
                viewModel.messageCount = ep.GetUnreadMessageCount(pubMessageList);
                ViewBag.msgError = Error;
                return View(viewModel);
            }
            else
            {
                return Redirect(ep.GetLogoutUrl());
            }
        }

        [HttpPost]
        public ActionResult SeacrhResult(string searchStr, int idDocumentT)
        {
            return Redirect(Url.Action("listado", "documentos", new { searchStr = searchStr, idCategory = idDocumentT }));
        }
    }
}