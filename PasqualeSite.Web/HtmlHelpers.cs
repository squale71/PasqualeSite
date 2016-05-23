using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.Mvc;

namespace PasqualeSite.Web
{
    public static class HtmlHelpers
    {
        public static IHtmlString reCaptcha(this HtmlHelper helper)
        {
            StringBuilder sb = new StringBuilder();
            string publickey = System.Web.Configuration.WebConfigurationManager.AppSettings["RecaptchaPublicKey"];
            sb.AppendLine("<script type=\"text/javascript\" src = 'https://www.google.com/recaptcha/api.js' ></script>");
            sb.AppendLine("");
            sb.AppendLine("<div class=\"g-recaptcha\" data-sitekey =\"" + publickey + "\"></div>");
            return MvcHtmlString.Create(sb.ToString());
        }
    }
}