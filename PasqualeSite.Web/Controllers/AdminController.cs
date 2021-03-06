﻿using Microsoft.AspNet.Identity;
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

        public async Task<ActionResult> GetAllFeeds()
        {
            List<RSSFeeds> feeds = new List<RSSFeeds>();
            using (var fs = new FeedService())
            {
                feeds = await fs.GetFeeds();
            }

            return Content(Newtonsoft.Json.JsonConvert.SerializeObject(feeds, new Newtonsoft.Json.JsonSerializerSettings() { NullValueHandling = Newtonsoft.Json.NullValueHandling.Ignore, ReferenceLoopHandling = Newtonsoft.Json.ReferenceLoopHandling.Ignore }));
        }

        public async Task<ActionResult> GetAllImages()
        {
            List<PostImage> images = new List<PostImage>();
            using (var imgService = new ImageService())
            {
                images = await imgService.GetImages();
            }

            return Content(Newtonsoft.Json.JsonConvert.SerializeObject(images, new Newtonsoft.Json.JsonSerializerSettings() { NullValueHandling = Newtonsoft.Json.NullValueHandling.Ignore, ReferenceLoopHandling = Newtonsoft.Json.ReferenceLoopHandling.Ignore }));
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
        public async Task<ActionResult> AddFeed(string name, string url)
        {
            RSSFeeds feed = new RSSFeeds();
            feed.Name = name;
            feed.FeedUrl = url;
            using (var fs = new FeedService())
            {
                feed = await fs.SaveFeed(feed);
            }

            return Content(Newtonsoft.Json.JsonConvert.SerializeObject(feed, new Newtonsoft.Json.JsonSerializerSettings() { NullValueHandling = Newtonsoft.Json.NullValueHandling.Ignore, ReferenceLoopHandling = Newtonsoft.Json.ReferenceLoopHandling.Ignore }));
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
        public async Task<ActionResult> SaveFeed(RSSFeeds newFeed)
        {
            RSSFeeds feed = new RSSFeeds();
            using (var fs = new FeedService())
            {
                feed = await fs.SaveFeed(newFeed);
            }

            return Content(Newtonsoft.Json.JsonConvert.SerializeObject(feed, new Newtonsoft.Json.JsonSerializerSettings() { NullValueHandling = Newtonsoft.Json.NullValueHandling.Ignore, ReferenceLoopHandling = Newtonsoft.Json.ReferenceLoopHandling.Ignore }));
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
        public async Task<ActionResult> DeleteFeed(int id)
        {
            RSSFeeds feed = new RSSFeeds();
            using (var fs = new FeedService())
            {
                feed = await fs.DeleteFeed(id);
            }

            return Content(Newtonsoft.Json.JsonConvert.SerializeObject(feed, new Newtonsoft.Json.JsonSerializerSettings() { NullValueHandling = Newtonsoft.Json.NullValueHandling.Ignore, ReferenceLoopHandling = Newtonsoft.Json.ReferenceLoopHandling.Ignore }));
        }

        [HttpPost]
        [ValidateInput(false)]
        public async Task<ActionResult> SavePost(Post newPost)
        {
            newPost.UserId = User.Identity.GetUserId();
            newPost.Author = User.Identity.Name;
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