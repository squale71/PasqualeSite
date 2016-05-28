using PasqualeSite.Data.Database;
using PasqualeSite.Web.App_Start;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Web.Http;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;

namespace PasqualeSite.Web
{
    public class MvcApplication : System.Web.HttpApplication
    {
        protected void Application_Start()
        {
            AreaRegistration.RegisterAllAreas();
            GlobalConfiguration.Configure(WebApiConfig.Register);
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            BundleConfig.RegisterBundles(BundleTable.Bundles);
            

            // Fill database using EF Code First.
            //TODO: Comment in Production
            Database.SetInitializer(new CreateDatabaseIfNotExists<MyDbContext>());
            var userContext = new MyDbContext();
            userContext.Database.Initialize(true);
        }
    }
}
