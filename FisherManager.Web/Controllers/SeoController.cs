using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace FisherManager.Web.Controllers
{
    public class SeoController : BaseController
    {
        // GET: Seo
        public ActionResult Index(string SeoUrl)
        {    
            return HttpNotFound();
        }
    }
}