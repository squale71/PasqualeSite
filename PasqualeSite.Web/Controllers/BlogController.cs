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
        public async Task<ActionResult> Index(int page = 1, string year = null, string month = null, string tag = null)
        {
            #if !DEBUG
                if (Request.IsSecureConnection)
                {
                    Response.Redirect(Request.Url.ToString().Replace("https:", "http:"));
                }
            #endif
            try
            {
                var pagingModel = new PostPagingModel();
                int tagId = 0;

                if (tag != null)
                {
                    using (var ts = new TagService())
                    {
                        int? id = await ts.GetTagId(tag);
                        if (id != null)
                        {
                            tagId = id.Value;
                        }

                        else
                        {
                            ViewBag.Information = "That tag you have entered doesn't exist.";
                            return View("Info");
                        }
                    }
                }

                using (var bs = new BlogService())
                {
                    pagingModel = await bs.GetFilteredPosts(tagId, year, month, 6, page);
                }

                int theYear, theMonth;

                if (int.TryParse(month, out theMonth) && int.TryParse(year, out theYear))
                {
                    pagingModel.Year = theYear;
                    pagingModel.Month = theMonth;
                }
                
                pagingModel.Tag = tag;
                return View(pagingModel);
            }

            catch (FormatException ex)
            {
                ViewBag.Error = "Uh oh. There was a problem with the URL you entered, as the page could not be found. How unfortunate";
                return View("PageNotFound");
            }
            
            catch (Exception ex)
            {
                ViewBag.Error = "Oops. Looks like something broke. Well, I'm not perfect. Try again later.";
                return View("Error");
            }
            
        }

        public async Task<ActionResult> Post(int year, int month, int day, string title)
        {
            #if !DEBUG
                if (Request.IsSecureConnection)
                {
                    Response.Redirect(Request.Url.ToString().Replace("https:", "http:"));
                }
            #endif
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