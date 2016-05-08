using Microsoft.AspNet.Identity;
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
    [Authorize(Roles ="Active")]
    [Authorize(Roles = "Admin")]
    public class AdminController : Controller
    {
        public ActionResult Index()
        {
            return View();
        }

        public async Task<ActionResult> GetAdminPosts()
        {
            List<Post> posts = new List<Post>();
            using (var bs = new BlogService())
            {
                posts = await bs.GetAllPosts();
            }

            return Content(Newtonsoft.Json.JsonConvert.SerializeObject(posts, new Newtonsoft.Json.JsonSerializerSettings() { NullValueHandling = Newtonsoft.Json.NullValueHandling.Ignore, ReferenceLoopHandling = Newtonsoft.Json.ReferenceLoopHandling.Ignore }));
        }

        [HttpPost]
        [ValidateInput(false)]
        public async Task<ActionResult> SavePost(Post newPost)
        {
            newPost.UserId = User.Identity.GetUserId();
            Post blogPost = new Post();
            using (var bs = new BlogService())
            {
                blogPost = await bs.UpdatePost(newPost);
            }

            return Content(Newtonsoft.Json.JsonConvert.SerializeObject(blogPost, new Newtonsoft.Json.JsonSerializerSettings() { NullValueHandling = Newtonsoft.Json.NullValueHandling.Ignore, ReferenceLoopHandling = Newtonsoft.Json.ReferenceLoopHandling.Ignore }));
        }
    }
}