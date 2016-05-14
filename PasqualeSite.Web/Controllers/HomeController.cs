using PasqualeSite.Data.Entities;
using PasqualeSite.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

namespace PasqualeSite.Web.Controllers
{
    public class HomeController : Controller
    {
        public async Task<ActionResult> Index()
        {
            #if !DEBUG
                if (Request.IsSecureConnection)
                {
                    Response.Redirect(Request.Url.ToString().Replace("https:", "http:"));
                }
            #endif
            List<Post> FeaturedPosts = new List<Post>();
            using (var bs = new BlogService())
            {
                FeaturedPosts = await bs.GetFeaturedPosts();
            }
            return View(FeaturedPosts);
        }
    }
}