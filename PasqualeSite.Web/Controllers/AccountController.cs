using Microsoft.AspNet.Identity;
using Microsoft.Owin.Security;
using PasqualeSite.Data.Identity;
using PasqualeSite.Services;
using PasqualeSite.Web.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

namespace PasqualeSite.Web.Controllers
{
    public class AccountController : Controller
    {
        private readonly UserManager<AppUser> userManager;

        public AccountController()
            : this(Startup.UserManagerFactory.Invoke())
        {
        }

        public AccountController(UserManager<AppUser> userManager)
        {
            this.userManager = userManager;
        }

        [AllowAnonymous]
        [HttpGet]
        public ActionResult Login(string returnUrl)
        {
            ViewBag.ReturnUrl = returnUrl;
            return View();
        }

        [AllowAnonymous]
        [HttpPost]
        public async Task<ActionResult> LogIn(LogInModel model, string returnUrl)
        {
            if (!ModelState.IsValid)
            {
                return View();
            }

            var user = await userManager.FindAsync(model.UserName, model.Password);

            // If the user exists
            if (user != null)
            {
                // First let's check if user is locked out, if so, send them to error screen.
                if (await userManager.IsLockedOutAsync(user.Id))
                {
                    ViewBag.errorMessage = "Your account has been locked out temporarily.";
                    return View("Error");
                }

                // Confirm that they have confirmed their email, if not, send them to error screen.
                if (!await userManager.IsEmailConfirmedAsync(user.Id))
                {
                    ViewBag.Information = "The email provided when the system administrator created the account has not yet been verified. An email containing a link has been sent to your account on file with further instructions.";
                    await SendEmailConfirmationTokenAsync(user.Id, "");
                    return View("Info");
                }

                var identity = await userManager.CreateIdentityAsync(
                    user, DefaultAuthenticationTypes.ApplicationCookie);

                // Sign user in
                GetAuthenticationManager().SignIn(identity);

                // Reset access failed count to zero upon successful authentication
                userManager.ResetAccessFailedCount(user.Id);
                return Redirect(GetRedirectUrl(returnUrl));
            }


            var failedUser = await userManager.FindByNameAsync(model.UserName);
            // If user exists but password doesn't match, increment failed access count.
            if (failedUser != null)
            {
                await userManager.AccessFailedAsync(failedUser.Id);
            }

            // If we reach down here, authentication failed.
            ModelState.AddModelError("", "Invalid email or password");
            return View();
        }

        [AllowAnonymous]
        public ActionResult LogOut()
        {
            var ctx = Request.GetOwinContext();
            var authManager = ctx.Authentication;

            authManager.SignOut(DefaultAuthenticationTypes.ApplicationCookie);
            return RedirectToAction("Login", "Account");
        }

        [AllowAnonymous]
        public ActionResult Unauthorized(string returnUrl)
        {
            if (!User.Identity.IsAuthenticated)
            {
                return RedirectToAction("Login", new { returnUrl = returnUrl });
            }

            ViewBag.Message = "You are not authorized to view this page.";
            return View();
        }

        [Authorize]
        [HttpGet]
        public ActionResult Register()
        {
            return View();
        }

        //TODO: Change back to Allow Anon when we are ready to allow other users to register.
        [Authorize]
        [HttpPost]
        public async Task<ActionResult> Register(RegisterModel model)
        {
            if (!ModelState.IsValid)
            {
                return View();
            }

            var user = new AppUser
            {
                UserName = model.UserName,
                FirstName = model.FirstName,
                LastName = model.LastName,
                Email = model.EmailAddress,
                //TODO: Collect more data from user upon registration. Add to AppUser model, migrate to DB, and then add fields to Register model to assign here.
            };

            var result = await userManager.CreateAsync(user, model.Password);

            if (result.Succeeded && model.Password == model.PasswordConfirm)
            {
                // Adds user to Active role upon creation of account.
                using (var us = new UserService())
                {
                    await us.AddRoleToUser(user.Id, "Active");
                }

                await SendEmailConfirmationTokenAsync(user.Id, "");
                await SignIn(user);
                return RedirectToAction("Index", "Home");
            }

            foreach (var error in result.Errors)
            {
                ModelState.AddModelError("", error);
            }

            return View();
        }

        [AllowAnonymous]
        public ActionResult ConfirmEmail(string userId, string code)
        {
            var success = ConfirmAccount(userId, code);
            if (success)
            {
                ViewBag.Success = true;
                return View();
            }
            ViewBag.Success = false;
            return View();
        }

        [HttpGet]
        [AllowAnonymous]
        public ActionResult ForgotPassword()
        {
            return View();
        }

        [HttpPost]
        [AllowAnonymous]
        public async Task<ActionResult> ForgotPassword(ForgotPasswordViewModel model)
        {
            if (ModelState.IsValid)
            {
                var user = await userManager.FindByNameAsync(model.User);

                if (user == null || user.LastName.ToLower() != model.LastName.ToLower() || user.Email.ToLower() != model.EmailAddress.ToLower() || !(await userManager.IsEmailConfirmedAsync(user.Id)))
                {
                    // Don't reveal that the user does not exist or is not confirmed
                    return View("ForgotPasswordConfirmation");
                }

                var code = await userManager.GeneratePasswordResetTokenAsync(user.Id);
                var callbackUrl = Url.Action("ResetPassword", "Account", new { UserId = user.Id, code = code }, protocol: Request.Url.Scheme);
                await userManager.SendEmailAsync(user.Id, "Reset Password",
                "<p>We received a request to reset your password.</p><p>If you didn't request a password update, you can ignore this email and your password will not be changed. Otherwise, you can go <a href=\"" + callbackUrl + "\">here</a> to update your password.</p>");
                return View("ForgotPasswordConfirmation");
            }

            // If we got this far, something failed, redisplay form
            return View(model);
        }

        [HttpGet]
        [AllowAnonymous]
        public ActionResult ResetPassword(string userId, string code)
        {
            var resetModel = new ResetPasswordViewModel();
            resetModel.userId = userId;
            resetModel.resetToken = code;
            return View(resetModel);
        }

        [HttpPost]
        [AllowAnonymous]
        public async Task<ActionResult> ResetPassword(ResetPasswordViewModel resetModel)
        {
            if (!ModelState.IsValid && resetModel.newPassword != resetModel.confirmPassword)
            {
                return View();
            }

            var result = await userManager.ResetPasswordAsync(resetModel.userId, resetModel.resetToken, resetModel.newPassword);

            if (result.Succeeded)
            {
                ViewBag.Success = true;
                return View("PasswordChanged");
            }

            return View();
        }


        #region Private Methods

        private async Task<string> SendEmailConfirmationTokenAsync(string userID, string subject)
        {
            string code = await userManager.GenerateEmailConfirmationTokenAsync(userID);
            var callbackUrl = Url.Action("ConfirmEmail", "Account",
               new { userId = userID, code = code }, protocol: Request.Url.Scheme);
            await userManager.SendEmailAsync(userID, "Confirm your account", "<p>In order to completed your registration to thesqualls.com, you will need to confirm your email. Please click <a href=\"" + callbackUrl + "\">here</a> to confirm your account.</p>");

            return callbackUrl;
        }

        private string GetRedirectUrl(string returnUrl)
        {
            if (string.IsNullOrEmpty(returnUrl) || !Url.IsLocalUrl(returnUrl))
            {
                return Url.Action("Index", "Home");
            }

            return returnUrl;
        }

        private IAuthenticationManager GetAuthenticationManager()
        {
            var ctx = Request.GetOwinContext();
            return ctx.Authentication;
        }

        private async Task SignIn(AppUser user)
        {
            var identity = await userManager.CreateIdentityAsync(
                user, DefaultAuthenticationTypes.ApplicationCookie);

            GetAuthenticationManager().SignIn(identity);
        }

        private bool ConfirmAccount(string userId, string code)
        {
            if (String.IsNullOrEmpty(userId) || String.IsNullOrEmpty(code))
            {
                return false;
            }
            var item = userManager.ConfirmEmail(userId, code);
            return item.Succeeded;
        }
        #endregion


    }
}