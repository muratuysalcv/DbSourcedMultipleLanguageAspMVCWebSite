using FisherManager.Business.Managers;
using FisherManager.Data;
using System.Web.Mvc;
using FisherManager.Business.Models;
using System.IO;

namespace FisherManager.Web.Controllers
{
    public class HomeController : BaseController
    {
        public LocalizationManager _localizationManager = new LocalizationManager();
        public MenuManager _menuManager = new MenuManager();
        MessageResponseModel messageResponseModel = new MessageResponseModel();
        // menu seo url
        public ActionResult Index()
        {
            return View();
        }
        //Eski FisherManager web sitesi için var olan SEO'yu kaybetmemek adına bu yönlendirmeyi yapıyoruz.
        public ActionResult Redirect()
        {
            return View();
        }
        public ActionResult Menu()
        {
            var responseModel = _menuManager.GetMenuList(new ApiRequestModel()
            {
                LANGUAGE_CODE = this.LANGUAGE_CODE(),
                HEADER_MENU=true,
                FOOTER_MENU=false
            });

            return View(responseModel.MENU_LIST);
        }
        public ActionResult FooterMenu()
        {
            var responseModel = _menuManager.GetMenuList(new ApiRequestModel()

            {
                LANGUAGE_CODE = this.LANGUAGE_CODE(),
                HEADER_MENU = false,
                FOOTER_MENU = true
            });

            return View(responseModel.MENU_LIST);
        }
        public ActionResult MenuDetail()
        {
            var seoUrl = this.RouteData.Values["SeoUrl"] + "";
            var menu = _menuManager.GetMenuBySeoUrl(seoUrl);
            if (menu.RESULT_DATA != null)
                return View((MENU)menu.RESULT_DATA);

            return HttpNotFound();
        }

        [HttpPost]
        public JsonResult SendMessage(MESSAGE message)
        {
            string customerBody = this.CreateEmailBodyGetOfferOrInfoForCustomer(message,this.LANGUAGE_CODE());
            messageResponseModel = this.SendGetOfferOrInfoToSystem(message,customerBody,this.LANGUAGE_CODE());

            return Json(messageResponseModel, JsonRequestBehavior.AllowGet);
        }
        


    }
}