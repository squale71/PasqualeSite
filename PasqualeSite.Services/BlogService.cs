using Microsoft.Security.Application;
using PasqualeSite.Data.Entities;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using System.Web;

namespace PasqualeSite.Services
{
    public class PostPagingModel
    {
        public int CurrentPage { get; set; }
        public int PerPage { get; set; }
        public int Total { get; set; }
        public int Year { get; set; }
        public int Month { get; set; }
        public string Tag { get; set; }
        public List<Post> CurrentPosts { get; set; }
        public List<Tag> Tags { get; set; }
        public List<DateTime> BlogMonths { get; set; }
    }

    public class BlogService : DisposableService
    {
        public async Task<Post> GetBlogPost(int id)
        {
            var post = await db.Posts.Where(x => x.Id == id).FirstOrDefaultAsync();
            return post;
        }

        public async Task<Post> GetBlogPost(int year, int month, int day, string title)
        {
            var post = await db.Posts.Include(x => x.PostTags.Select(y => y.Tag)).Where(x => x.UrlTitle == title && x.DateCreated.Year == year && x.DateCreated.Month == month && x.DateCreated.Day == day).FirstOrDefaultAsync();
            return post;
        }

        public async Task<List<Post>> GetFeaturedPosts()
        {
            var posts = await db.Posts.Include(x => x.Image).Where(x => x.IsFeatured && x.IsFeatured).OrderByDescending(x => x.Priority).Take(6).ToListAsync();
            return posts;
        }

        public async Task<PostPagingModel> GetFilteredPosts(int tagId, string year = null, string month = null, int perPage = 5, int page = 1)
        {
            int theMonth, theYear;

            if (!int.TryParse(month, out theMonth))
                theMonth = 0;

            if (!int.TryParse(year, out theYear))
                theYear = 0;

            var pagingModel = new PostPagingModel();
            pagingModel.CurrentPage = page;
            pagingModel.PerPage = perPage;
            pagingModel.Total = await db.Posts
                .Include(x => x.PostTags.Select(y => y.Tag))
                .Where(x => x.IsActive && x.PostTags.Any(y => y.TagId == tagId) || tagId == 0)
                .Where(x => (x.DateCreated.Year == theYear && x.DateCreated.Month == theMonth) || (theYear == 0 || theMonth == 0))
                .CountAsync();
            pagingModel.CurrentPosts = await db.Posts
                .Include(x => x.Image)
                .Include(x => x.PostTags.Select(y => y.Tag))
                .Where(x => x.IsActive && x.PostTags.Any(y => y.TagId == tagId) || tagId == 0)
                .Where(x => (x.DateCreated.Year == theYear && x.DateCreated.Month == theMonth) || (theYear == 0 || theMonth == 0))
                .OrderByDescending(x => x.DateCreated)
                .Skip(perPage * (page - 1))
                .Take(perPage)
                .ToListAsync();

            pagingModel.BlogMonths = GetBlogMonths();

            using (var ts = new TagService())
            {
                pagingModel.Tags = await ts.GetAllTags();
            }

            return pagingModel;
        }

        public async Task<List<Post>> GetAllPosts(bool includeInactive = true)
        {
            var posts = await db.Posts.Where(x => x.IsActive || includeInactive).Include(x => x.PostTags).Include(x => x.Image).ToListAsync();
            foreach (var post in posts)
            {
                post.TagIds = new List<int>();
                foreach (var tag in post.PostTags)
                {
                    post.TagIds.Add(tag.TagId);
                }
            }
            return posts;
        }

        public async Task<Post> UpdatePost(Post newPost)
        {
            //newPost.PostContent = Sanitizer.GetSafeHtml(newPost.PostContent); // TODO: Find better solution. This was stripping out inline styles.
            newPost.UrlTitle = Slugify(newPost.Title);
            var blogPost = await db.Posts.Where(x => x.Id == newPost.Id).FirstOrDefaultAsync();
            if (blogPost != null)
            {
                newPost.DateModified = DateTime.Now.ToLocalTime();
                db.Entry(blogPost).CurrentValues.SetValues(newPost);
            }
            else
            {
                newPost.DateCreated = DateTime.Now.ToLocalTime();
                db.Posts.Add(newPost);                
            }

            await db.SaveChangesAsync();

            await UpdateTags(newPost, newPost.TagIds);
            return await GetBlogPost(newPost.Id);
        }

        public async Task<List<int>> UpdateTags(Post newPost, List<int> TagIds)
        {
            var currentTags = await db.PostTags.Where(x => x.PostId == newPost.Id).ToListAsync();
            db.PostTags.RemoveRange(currentTags); // Lets just refresh all the relationships.
            foreach (var id in TagIds)
            {
                db.PostTags.Add(new PostTag() { PostId = newPost.Id, TagId = id }); // Add the refreshed relationships.
            }
            await db.SaveChangesAsync();
            return TagIds;
        }

        public async Task<Post> DeletePost(int id)
        {
            var post = db.Posts.Include(x => x.PostTags).Where(x => x.Id == id).FirstOrDefault();
            if (post != null)
            {
                // Remove all relationships between post and tag.
                db.PostTags.RemoveRange(post.PostTags);
                db.Posts.Remove(post);
                await db.SaveChangesAsync();
                return post;
            }

            return null;
        }

        public List<DateTime> GetBlogMonths()
        {
            var yearMonths = db.Posts
                .Select(p => new { p.DateCreated.Year, p.DateCreated.Month })
                    .Distinct()
                    .ToList()
                    .Select(x => new DateTime(x.Year, x.Month, 1));

            return yearMonths.ToList();
        }

        private string RemoveAccent(string txt)
        {
            byte[] bytes = System.Text.Encoding.GetEncoding("Cyrillic").GetBytes(txt);
            return System.Text.Encoding.ASCII.GetString(bytes);
        }

        private string Slugify(string phrase)
        {
            string str = RemoveAccent(phrase).ToLower();
            str = System.Text.RegularExpressions.Regex.Replace(str, @"[^a-z0-9\s-]", ""); // Remove all non valid chars          
            str = System.Text.RegularExpressions.Regex.Replace(str, @"\s+", " ").Trim(); // convert multiple spaces into one space  
            str = System.Text.RegularExpressions.Regex.Replace(str, @"\s", "-"); // //Replace spaces by dashes
            return str;
        }

    }
}