using PasqualeSite.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

namespace PasqualeSite.Web.Controllers
{
    public class RSSController : Controller
    {
        public async Task<ActionResult> GetRSSFeed()
        {
            var RSS = new RSSHelper();
            using (var fs = new FeedService())
            {
                var feed = await fs.GetRandomFeed();
                RSS.RSSItem = RSS.GetRSS(feed.FeedUrl);
            }

            return Content(Newtonsoft.Json.JsonConvert.SerializeObject(RSS.RSSItem, new Newtonsoft.Json.JsonSerializerSettings() { NullValueHandling = Newtonsoft.Json.NullValueHandling.Ignore, ReferenceLoopHandling = Newtonsoft.Json.ReferenceLoopHandling.Ignore }));
        }
    }
}