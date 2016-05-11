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
    public class BlogController : Controller
    {
        // GET: Blog
        public ActionResult Index()
        {
            return View();
        }

        public async Task<ActionResult> Post(int year, int month, int day, string title)
        {
            Post post = new Post();
            using (var bs = new BlogService())
            {
                post = await bs.GetBlogPost(year, month, day, title);
            }

            if (post != null)
            {
                return View(post);
            }

            ViewBag.Information = "The blog post you are attempting to see doesn't exist. Sorry about that.";
            return View("Info");
        }
    }
}