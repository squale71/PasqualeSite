using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace PasqualeSite.Web.Controllers
{
    public class ErrorController : Controller
    {
        [ActionName("404")]
        public ActionResult NotFound()
        {
            return View("NotFound");
        }

        [ActionName("500")]
        public ActionResult ServerError()
        {
            return View("ServerError");
        }
    }
}