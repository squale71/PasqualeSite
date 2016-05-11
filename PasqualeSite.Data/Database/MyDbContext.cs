using Microsoft.AspNet.Identity.EntityFramework;
using PasqualeSite.Data.Entities;
using PasqualeSite.Data.Identity;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;

namespace PasqualeSite.Data.Database
{
    public class MyDbContext : IdentityDbContext<AppUser>
    {
        //TODO: Add other entities here...
        public DbSet<Post> Posts { get; set; }
        public DbSet<PostImage> PostImages { get; set; }
        public DbSet<PostTag> PostTags { get; set; }
        public DbSet<Tag> Tags { get; set; }
        public DbSet<Comment> Commments { get; set; }
        public DbSet<UserLikes> Likes { get; set; }
        public DbSet<RSSFeeds> Feeds { get; set; }

        public MyDbContext() : base("MyDbConnection")
        {
        }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
        }
    }
}