using Microsoft.AspNet.Identity.EntityFramework;
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

        public MyDbContext() : base("MyDbConnection")
        {
        }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
        }
    }
}