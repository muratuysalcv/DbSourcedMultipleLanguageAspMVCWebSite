using FisherManager.Business.Managers;
using FisherManager.Business.Models;
using FisherManager.Web._Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Web;
using System.Web.Mvc;

namespace FisherManager.Web.Controllers
{
    public class LoginController : BaseController
    {
        LoginManager loginManager = new LoginManager();
        LocalizationManager localizationManager = new LocalizationManager();
        LoginResponseModel loginResponseModel = new LoginResponseModel();
        // GET: Login
        public ActionResult Login()
        {
            return View();
        }

        [HttpPost]
        public JsonResult Login(LoginRequestModel model)
        {
            var loginInfo = loginManager.Login(model, this.LANGUAGE_CODE());
            if (loginInfo.USER_ACCOUNT != null)
            {
                SessionHelper.SetSession(loginInfo.USER_ACCOUNT);
                return Json(new LoginResponseModel()
                {
                    RESOURCE_KEY=loginInfo.RESOURCE_KEY,
                    RESOURCE_VALUE = loginInfo.RESOURCE_VALUE
                }, JsonRequestBehavior.AllowGet);
            }
            else
            {
                return Json(new LoginResponseModel()
                {
                    RESOURCE_KEY = loginInfo.RESOURCE_KEY,
                    RESOURCE_VALUE = loginInfo.RESOURCE_VALUE
                }, JsonRequestBehavior.AllowGet);
            }

        }

        public ActionResult Logout()
        {
            var culture = Thread.CurrentThread.CurrentCulture;
            SessionHelper.KillSession();
            return View("Login");
        }

    }
}