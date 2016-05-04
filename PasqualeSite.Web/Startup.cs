using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using Microsoft.AspNet.Identity.Owin;
using Microsoft.Owin;
using Microsoft.Owin.Security.Cookies;
using Microsoft.Owin.Security.DataProtection;
using Owin;
using PasqualeSite.Data.Database;
using PasqualeSite.Data.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace PasqualeSite.Web
{
    public class Startup
    {
        public static Func<UserManager<AppUser>> UserManagerFactory { get; private set; }
        internal static IDataProtectionProvider DataProtectionProvider { get; private set; }

        public void Configuration(IAppBuilder app)
        {
            DataProtectionProvider = app.GetDataProtectionProvider();
            // We will be using cookie based authentication
            app.UseCookieAuthentication(new CookieAuthenticationOptions
            {
                AuthenticationType = DefaultAuthenticationTypes.ApplicationCookie, // string value that identifies the cookie. -PC
                LoginPath = new PathString("/Account/Unauthorized"), // Where user will redirect when there is an unauthorized response (401). -PC
            });

            // configure the user manager
            UserManagerFactory = () =>
            {
                var usermanager = new UserManager<AppUser>(
                    new UserStore<AppUser>(new MyDbContext()));
                // allow alphanumeric characters in username
                usermanager.UserValidator = new UserValidator<AppUser>(usermanager)
                {
                    AllowOnlyAlphanumericUserNames = false,
                    RequireUniqueEmail = true,

                };

                usermanager.EmailService = new EmailService();
                usermanager.UserLockoutEnabledByDefault = true;
                usermanager.DefaultAccountLockoutTimeSpan = TimeSpan.FromMinutes(15); //Lock users out for 20 minutes
                usermanager.MaxFailedAccessAttemptsBeforeLockout = 5; // 5 attempts before lockout

                var dataProtectionProvider = Startup.DataProtectionProvider;
                usermanager.UserTokenProvider = new DataProtectorTokenProvider<AppUser>(dataProtectionProvider.Create("ASP.NET Identity"));

                return usermanager;
            };
        }
    }
}