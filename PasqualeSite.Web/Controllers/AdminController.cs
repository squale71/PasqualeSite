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

        public async Task<ActionResult> GetAllTags()
        {
            List<Tag> tags = new List<Tag>();
            using (var ts = new TagService())
            {
                tags = await ts.GetAllTags();
            }

            return Content(Newtonsoft.Json.JsonConvert.SerializeObject(tags, new Newtonsoft.Json.JsonSerializerSettings() { NullValueHandling = Newtonsoft.Json.NullValueHandling.Ignore, ReferenceLoopHandling = Newtonsoft.Json.ReferenceLoopHandling.Ignore }));
        }

        [HttpPost]
        public async Task<ActionResult> AddTag(string name)
        {
            Tag tag = new Tag();
            tag.Name = name;
            using (var ts = new TagService())
            {
                tag = await ts.SaveTag(tag);
            }

            return Content(Newtonsoft.Json.JsonConvert.SerializeObject(tag, new Newtonsoft.Json.JsonSerializerSettings() { NullValueHandling = Newtonsoft.Json.NullValueHandling.Ignore, ReferenceLoopHandling = Newtonsoft.Json.ReferenceLoopHandling.Ignore }));
        }

        [HttpPost]
        public async Task<ActionResult> SaveTag(Tag newTag)
        {
            Tag tag = new Tag();
            using (var ts = new TagService())
            {
                tag = await ts.SaveTag(newTag);
            }

            return Content(Newtonsoft.Json.JsonConvert.SerializeObject(tag, new Newtonsoft.Json.JsonSerializerSettings() { NullValueHandling = Newtonsoft.Json.NullValueHandling.Ignore, ReferenceLoopHandling = Newtonsoft.Json.ReferenceLoopHandling.Ignore }));
        }

        [HttpPost]
        public async Task<ActionResult> DeleteTag(int id)
        {
            Tag tag = new Tag();
            using (var ts = new TagService())
            {
                tag = await ts.DeleteTag(id);
            }

            return Content(Newtonsoft.Json.JsonConvert.SerializeObject(tag, new Newtonsoft.Json.JsonSerializerSettings() { NullValueHandling = Newtonsoft.Json.NullValueHandling.Ignore, ReferenceLoopHandling = Newtonsoft.Json.ReferenceLoopHandling.Ignore }));
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

        [HttpPost]
        public async Task<ActionResult> DeletePost(int id)
        {
            Post deletedPost = new Post();
            using (var bs = new BlogService())
            {
                deletedPost = await bs.DeletePost(id);
            }

            return Content(Newtonsoft.Json.JsonConvert.SerializeObject(deletedPost, new Newtonsoft.Json.JsonSerializerSettings() { NullValueHandling = Newtonsoft.Json.NullValueHandling.Ignore, ReferenceLoopHandling = Newtonsoft.Json.ReferenceLoopHandling.Ignore }));
        }
    }
}