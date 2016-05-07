using PasqualeSite.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

namespace PasqualeSite.Web.Controllers
{
    [Authorize(Roles ="Admin")]
    [Authorize(Roles = "Active")]
    public class ImageController : Controller
    {
        // GET: Image
        public ActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public async Task<ActionResult> UploadImage(string imageName, HttpPostedFileBase file)
        {
            string urlPath = null;
            using (var imgService = new ImageService())
            {
                urlPath = await imgService.UploadImage(imageName, file);
            }

            return RedirectToAction("Index", "Admin");
            //return Content(Newtonsoft.Json.JsonConvert.SerializeObject(urlPath, new Newtonsoft.Json.JsonSerializerSettings() { NullValueHandling = Newtonsoft.Json.NullValueHandling.Ignore, ReferenceLoopHandling = Newtonsoft.Json.ReferenceLoopHandling.Ignore }));
        }
    }
}