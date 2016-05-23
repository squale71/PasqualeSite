using Microsoft.Security.Application;
using PasqualeSite.Web.Models;
using SendGrid;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Diagnostics;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

namespace PasqualeSite.Web.Controllers
{
    public class ContactController : Controller
    {
        public ActionResult Index()
        {
            return View();
        }

        [CaptchaValidate]
        [HttpPost]
        public async Task<ActionResult> Index(ContactModel contact, bool CaptchaValid)
        {
            if (!CaptchaValid)
            {
                ViewBag.Error = true;
                ViewBag.Message = "I need proof that you're not a robot before this form will submit.";
                return View();
            }

            else if (!ModelState.IsValid)
            {
                ViewBag.Error = true;
                ViewBag.Message = "Something went wrong when validating the form fields. Please double check to make sure everything is correct.";
                return View();
            }

            // Everything passed. Let's send the email
            else
            {
                ViewBag.SentMessage = true;
                var messageString = String.Format("<p>You've received a new message from {0} via the squalls contact page!</p><p>{1}</p>", contact.Email != null ? contact.Email : "(No Email Left)", Sanitizer.GetSafeHtmlFragment(contact.Comment));

                var myMessage = new SendGridMessage();
                myMessage.AddTo("plcaiazzo@gmail.com");
                myMessage.From = !String.IsNullOrEmpty(contact.Email) ? new System.Net.Mail.MailAddress(contact.Email) : new System.Net.Mail.MailAddress(ConfigurationManager.AppSettings["FromAddress"]);
                myMessage.Subject = Sanitizer.GetSafeHtmlFragment(contact.Subject);
                myMessage.Text = messageString;
                myMessage.Html = messageString;

                var credentials = new NetworkCredential(
                           ConfigurationManager.AppSettings["SendGridUsername"],
                           ConfigurationManager.AppSettings["SendGridPassword"]
                           );

                // Create a Web transport for sending email.
                var transportWeb = new SendGrid.Web(credentials);

                // Send the email.
                if (transportWeb != null)
                {
                    await transportWeb.DeliverAsync(myMessage);
                }
                else
                {
                    Trace.TraceError("Failed to create Web transport.");
                    await Task.FromResult(0);
                }

                ViewBag.Message = "Message has been sent! Thanks for contacting me. If you provided contact info and have requested me to contact you back, I will be in touch.";
                return View();
            }
        }
    }
}