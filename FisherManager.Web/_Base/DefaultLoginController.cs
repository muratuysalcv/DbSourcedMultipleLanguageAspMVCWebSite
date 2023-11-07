using FisherManager.Web.Controllers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Web;
using System.Web.Mvc;

namespace FisherManager.Web._Base
{
    public class DefaultLoginController : BaseController
    {
        protected override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            var culture = Thread.CurrentThread.CurrentCulture.Name;
            if (SessionHelper.GetSession() == null)
            {
                filterContext.HttpContext.Response.Redirect("~/"+culture+"/Login/Login");
            }
            else
                base.OnActionExecuting(filterContext);
        }
    }
}