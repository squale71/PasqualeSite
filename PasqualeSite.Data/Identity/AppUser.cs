using Microsoft.AspNet.Identity.EntityFramework;
using PasqualeSite.Data.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace PasqualeSite.Data.Identity
{
    public class AppUser : IdentityUser
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }

        public ICollection<Post> Posts { get; set; }
        public ICollection<Comment> Comments { get; set; }
        public ICollection<UserLikes> Likes { get; set; }
    }
}