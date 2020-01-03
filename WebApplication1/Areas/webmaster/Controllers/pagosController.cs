using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WebApplication1.Areas.webmaster.ViewModels;
using WebApplication1.Concrete;
using WebApplication1.Models;

namespace WebApplication1.Areas.webmaster.Controllers
{
    public class pagosController : Controller
    {
        pjrdev_condominiosEntities entities = new pjrdev_condominiosEntities();
        EFPublicRepository ep = new EFPublicRepository();
        // GET: webmaster/pagos
        public ActionResult recibidos(string searchString = "")
        {
            if (Session["USER_ID"] != null)
            {
                long userId = (long)Session["USER_ID"];
                user curUser = entities.users.Find(userId);
                List<PlanViewModel> paymentList = new List<PlanViewModel>();
                List<package> packageList = entities.packages.ToList();

                foreach (var item in packageList)
                {
                    List<community> communityList = entities.communities.Where(m => m.package_id == item.id).ToList();

                    foreach (var com in communityList)
                    {
                        communuser communuser = entities.communusers.Where(m => m.commun_id == com.id).FirstOrDefault();
                        if (communuser != null)
                        {
                            user adminUser = entities.users.Find(communuser.user_id);
                            PlanViewModel vm = new PlanViewModel();
                            vm.packageName = item.first_name;
                            vm.adminEmail = adminUser.email;
                            vm.adminName = adminUser.first_name1 + " " + adminUser.last_name1;
                            List<payment> payList = entities.payments.Where(m => m.to_user_id == adminUser.id).ToList();
                            decimal p = 0;
                            foreach (var payItem in payList)
                            {
                                if (payItem.quantity != null)
                                {
                                    p += (decimal)payItem.quantity;
                                }
                            }

                            vm.payment = p;
                            paymentList.Add(vm);
                        }                       
                    }

                }
                List<ShowMessage> pubMessageList = ep.GetChatMessages(userId);
                List<contactinfo> contactinfoList = entities.contactinfoes.Where(m => m.user_id == userId).ToList();
                List<document_type> document_category_list = entities.document_type.ToList();
                recibidosPagosViewModel viewModel = new recibidosPagosViewModel();
                viewModel.side_menu = "pagos";
                viewModel.side_sub_menu = "recibidos";
                viewModel.curUser = curUser;
                viewModel.document_category_list = document_category_list;
                viewModel.pubTaskList = ep.GetNotifiTaskList(userId);
                viewModel.pubMessageList = pubMessageList;
                viewModel.messageCount = ep.GetUnreadMessageCount(pubMessageList);
                viewModel.paymentList = paymentList;
                viewModel.searchString = searchString;
                return View(viewModel);
            }
            else
            {
                return Redirect(ep.GetLogoutUrl());
            }

        }

        [HttpPost]
        public ActionResult SeacrhResult(string searchString)
        {
            return Redirect(Url.Action("recibidos", "pagos", new { area = "webmaster" }));
        }
    }
}