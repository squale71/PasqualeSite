using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Runtime.Caching;
using System.Text;
using System.Web.Http;

namespace PasqualeSite.Web.Controllers
{
    public class InstagramController : ApiController
    {
        public ObjectCache cache = MemoryCache.Default;
        public CacheItemPolicy policy = new CacheItemPolicy();

        // GET: api/instagram
        public HttpResponseMessage Get(string id)
        {
            var json = "";
            using (var wc = new WebClient())
            {
                json = wc.DownloadString("https://www.instagram.com/" + id + "/media");
            }

            return new HttpResponseMessage()
            {
                Content = new StringContent(
                        json,
                        Encoding.UTF8,
                       "application/json"
                    )
            };
        }
    }
}
