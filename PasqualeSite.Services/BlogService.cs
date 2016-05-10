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
    public class BlogService : DisposableService
    {
        public async Task<Post> GetBlogPost(int id)
        {
            var post = await db.Posts.Where(x => x.Id == id).FirstOrDefaultAsync();
            return post;
        }

        public async Task<List<Post>> GetAllPosts(bool includeInactive = true)
        {
            var posts = await db.Posts.Where(x => x.IsActive || includeInactive).ToListAsync();
            return posts;
        }

        public async Task<Post> UpdatePost(Post newPost)
        {
            newPost.PostContent = Sanitizer.GetSafeHtmlFragment(newPost.PostContent);
            var blogPost = await db.Posts.Where(x => x.Id == newPost.Id).FirstOrDefaultAsync();
            if (blogPost != null)
            {
                blogPost.DateModified = DateTime.Now.ToLocalTime();
                db.Entry(blogPost).CurrentValues.SetValues(newPost);
            }
            else
            {
                newPost.DateCreated = DateTime.Now.ToLocalTime();
                db.Posts.Add(newPost);                
            }

            await db.SaveChangesAsync();
            return await GetBlogPost(newPost.Id);
        }

        public async Task<Post> DeletePost(int id)
        {
            var post = db.Posts.Include(x => x.PostTags).Where(x => x.Id == id).FirstOrDefault();
            if (post != null)
            {
                // Remove all relationships between post and tag.
                foreach (var postTag in post.PostTags)
                {
                    db.PostTags.Remove(postTag);
                }
                db.Posts.Remove(post);
                await db.SaveChangesAsync();
                return post;
            }

            return null;
        }

    }
}