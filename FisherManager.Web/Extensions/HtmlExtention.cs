using FisherManager.Business.Managers;
using FisherManager.Business.Models;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Web;
using System.Web.Mvc;
using System.Data.Sql;
using System.Linq;
using System.Linq.Expressions;
using System.Collections;
namespace FisherManager.Web.Extensions
{
    public static class HtmlExtension
    {
        public static IHtmlString T(this HtmlHelper htmlHelper, string resourceKey)
        {
            LocalizationManager localizationManager = new LocalizationManager();

            var response = localizationManager.GetResource(new LocalizationRequestModel()
            {
                RESOURCE_KEY = resourceKey,
                LANGUAGE_CODE = Thread.CurrentThread.CurrentCulture.Name
            });
            return new HtmlString(response.RESOURCE_VALUE);
        }
        public static IHtmlString ActionUrl(this HtmlHelper htmlHelper, string ActionName, string controllerName)
        {
            var cultureName = Thread.CurrentThread.CurrentCulture.Name;
            var mm = new MenuManager();
            var menu = mm.GetMenuByControlerActionName(ActionName, controllerName, cultureName);
            if (menu != null)
                return new HtmlString("/" + cultureName + "/" + menu.SEO_URL);
            else
                return new HtmlString("#" + menu.SEO_URL);
        }
    }
}