using FisherManager.Business.Managers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;
using System.Net.Mail;
using System.Configuration;
using FisherManager.Data;
using FisherManager.Business.Models;
using System.IO;

namespace FisherManager.Web.Controllers
{
    public class BaseController : Controller
    {
        public MenuManager _menuManager { get; set; }
        public MessageManager messageManager = new MessageManager();
        public BaseController()
        {
            _menuManager = new MenuManager();
        }
        public string LANGUAGE_CODE()
        {
            return Thread.CurrentThread.CurrentCulture.Name;
        }

        protected override IAsyncResult BeginExecute(RequestContext requestContext, AsyncCallback callback, object state)
        {
            var cultures = new List<string> { "tr-TR", "en-US" };
            var culture = requestContext.RouteData.Values["culture"] + "";

            //// generic seo support
            string seoUrl = requestContext.RouteData.Values["SeoUrl"] + "";
            

            if (cultures.Contains(culture))
            {
                Thread.CurrentThread.CurrentCulture = new System.Globalization.CultureInfo(culture);
                Thread.CurrentThread.CurrentUICulture = new System.Globalization.CultureInfo(culture);
            }
            else if (string.IsNullOrEmpty(seoUrl))
            {
                var firstMenuResult =  _menuManager.GetMenuBySeoUrl(culture.ToLower());
                
                if (firstMenuResult.RESULT_DATA != null)
                {
                    var firstMenu = (MENU)firstMenuResult.RESULT_DATA;
                    culture = firstMenu.LANGUAGE_CODE;
                    seoUrl = firstMenu.SEO_URL;
                    Thread.CurrentThread.CurrentCulture = new System.Globalization.CultureInfo(culture);
                    Thread.CurrentThread.CurrentUICulture = new System.Globalization.CultureInfo(culture);
                }
            }

            var menuList = _menuManager.GetMenuList(new Business.Models.ApiRequestModel()
            {
                LANGUAGE_CODE = culture,
                HEADER_MENU = true,
                FOOTER_MENU = true
            });

            string controllerName = requestContext.RouteData.Values["controller"] + "";
            string actionName = requestContext.RouteData.Values["action"] + "";
            //Eski FisherManager web sitesi için var olan SEO'yu kaybetmemek adına bu kontrolü yapıyoruz.
            if (actionName == "Redirect")
            {
                requestContext.HttpContext.Response.StatusCode = 301;
                requestContext.HttpContext.Response.Redirect("/tr-TR/" + seoUrl);
            }


            if (!string.IsNullOrEmpty(seoUrl) && controllerName == "Home" && actionName == "Index")
            {
                var currentMenu = menuList.MENU_LIST.FirstOrDefault(x => x.SEO_URL == seoUrl.ToLower());
                if (currentMenu != null)
                {
                    requestContext.RouteData.Values["controller"] = "Home";
                    requestContext.RouteData.Values["action"] = "MenuDetail";// INDEX

                    if (!string.IsNullOrEmpty(currentMenu.CONTROLLER_NAME))
                        requestContext.RouteData.Values["controller"] = currentMenu.CONTROLLER_NAME;
                    if (!string.IsNullOrEmpty(currentMenu.ACTION_NAME))
                        requestContext.RouteData.Values["action"] = currentMenu.ACTION_NAME;

                }
            }

            return base.BeginExecute(requestContext, callback, state);
        }

        public MessageResponseModel SendGetOfferOrInfoToSystem(MESSAGE message, string customerBody, string culture)
        {
            return messageManager.SendGetOfferOrInfoToSystem(message, customerBody, culture);
        }

        public string CreateEmailBodyGetOfferOrInfoForCustomer(MESSAGE message, string culture)
        {

            string body = string.Empty;
            //using streamreader for reading my htmltemplate   

            using (StreamReader reader = new StreamReader(Server.MapPath("~/MessageTemplate/GetOfferOrInfoToCustomerTemplate." + culture + ".html")))
            {
                body = reader.ReadToEnd();
            }

            body = body.Replace("{COMPANY_NAME}", message.COMPANY_NAME); //replacing the required things  
            body = body.Replace("{FARM_TYPE}", message.FARM_TYPE);
            body = body.Replace("{E_MAIL}", message.E_MAIL);
            body = body.Replace("{FARM_CAPACITY}", message.FARM_CAPACITY);
            body = body.Replace("{NUMBER_OF_EMPLOYEE}", message.NUMBER_OF_EMPLOYEE);
            body = body.Replace("{PHONE_NUMBER}", message.PHONE_NUMBER);
            body = body.Replace("{SUBJECT}", message.SUBJECT);
            body = body.Replace("{CITY}", message.CITY);
            body = body.Replace("{APPOINTMENT_TIME}", message.APPOINTMENT_TIME.ToString());
            body = body.Replace("{COUNTRY}", message.COUNTRY);


            return body;

        }
        

    }
}