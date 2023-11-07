using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace FisherManager.Web.Controllers
{
    public class WhatAreWeDoingController : BaseController
    {
        // GET: WhatAreWeDoing
        public ActionResult Index()
        {
            return View();
        }
    }
}