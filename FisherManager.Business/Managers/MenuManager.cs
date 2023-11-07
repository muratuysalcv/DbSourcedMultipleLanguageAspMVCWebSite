using FisherManager.Business.Models;
using FisherManager.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FisherManager.Business.Managers
{
    public class MenuManager : BaseManager
    {
        public MenuResponseModel GetMenuList(ApiRequestModel model)
        {
            var responseModel = new MenuResponseModel();
            if (model.FOOTER_MENU && model.HEADER_MENU)
                responseModel.MENU_LIST = db.MENU.Where(x => x.LANGUAGE_CODE == model.LANGUAGE_CODE && x.ACTIVE).OrderBy(x => x.DISPLAY_ORDER).ToList();
            else
            if (model.FOOTER_MENU)
                responseModel.MENU_LIST = db.MENU.Where(x => x.LANGUAGE_CODE == model.LANGUAGE_CODE && x.ACTIVE && x.SHOW_ON_FOOTER_MENU == model.FOOTER_MENU).OrderBy(x => x.DISPLAY_ORDER).ToList();
            else
                responseModel.MENU_LIST = db.MENU.Where(x => x.LANGUAGE_CODE == model.LANGUAGE_CODE && x.ACTIVE && x.SHOW_ON_HEADER_MENU == model.HEADER_MENU).OrderBy(x => x.DISPLAY_ORDER).ToList();

            return responseModel;
        }

        public MenuResponseModel GetMenuBySeoUrl(string seoUrl)
        {
            var responseModel = new MenuResponseModel();
            responseModel.RESULT_DATA = db.MENU.FirstOrDefault(x => x.SEO_URL == seoUrl && x.ACTIVE);

            return responseModel;
        }

        public MENU GetMenuByControlerActionName(string actionName, string controllerName,string languageCode)
        {
            var currentMenu = db.MENU.FirstOrDefault(x => x.CONTROLLER_NAME.ToLower() == controllerName.ToLower() && x.ACTION_NAME.ToLower() == actionName.ToLower() && x.LANGUAGE_CODE==languageCode);

            return currentMenu;
        }

    }
}
