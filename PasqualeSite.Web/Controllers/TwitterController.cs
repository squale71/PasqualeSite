using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Runtime.Caching;
using System.Security.Cryptography;
using System.Text;
using System.Web.Http;
using System.Web.Script.Serialization;

namespace PasqualeSite.Web.Controllers
{
    public class TwitterController : ApiController
    {
        public ObjectCache cache = MemoryCache.Default;
        public CacheItemPolicy policy = new CacheItemPolicy();

        // GET: api/twitter
        public HttpResponseMessage Get(string id)
        {
            policy.AbsoluteExpiration = DateTimeOffset.Now.AddSeconds(300);

            string key = String.Format("Twitter-{0}", id);
            string res = (string)cache.Get(key);
            if (string.IsNullOrEmpty(res))
            {
                JavaScriptSerializer TheSerializer = new JavaScriptSerializer();

                // Get consumer keys
                string resource_url = ConfigurationManager.AppSettings["TwitterFeedAPIURL"];
                string oauth_consumer_key = ConfigurationManager.AppSettings["TwitterConsumerKey"];
                string oauth_consumer_secret = ConfigurationManager.AppSettings["TwitterConsumerSecret"];
                string oauth_token = ConfigurationManager.AppSettings["TwitterAccessToken"];
                string oauth_token_secret = ConfigurationManager.AppSettings["TwitterAccessSecret"];

                // oauth implementation details
                var oauth_version = "1.0";
                var oauth_signature_method = "HMAC-SHA1";

                // unique request details
                var oauth_nonce = Convert.ToBase64String(new ASCIIEncoding().GetBytes(DateTime.Now.Ticks.ToString()));
                var timeSpan = DateTime.UtcNow
                    - new DateTime(1970, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc);
                var oauth_timestamp = Convert.ToInt64(timeSpan.TotalSeconds).ToString();


                // create oauth signature
                var baseFormat = "oauth_consumer_key={0}&oauth_nonce={1}&oauth_signature_method={2}" +
                                "&oauth_timestamp={3}&oauth_token={4}&oauth_version={5}&screen_name={6}";

                var baseString = string.Format(baseFormat,
                                            oauth_consumer_key,
                                            oauth_nonce,
                                            oauth_signature_method,
                                            oauth_timestamp,
                                            oauth_token,
                                            oauth_version,
                                            Uri.EscapeDataString(id)
                                            );

                baseString = string.Concat("GET&", Uri.EscapeDataString(resource_url), "&", Uri.EscapeDataString(baseString));

                var compositeKey = string.Concat(Uri.EscapeDataString(oauth_consumer_secret),
                                        "&", Uri.EscapeDataString(oauth_token_secret));

                string oauth_signature;
                using (HMACSHA1 hasher = new HMACSHA1(ASCIIEncoding.ASCII.GetBytes(compositeKey)))
                {
                    oauth_signature = Convert.ToBase64String(
                        hasher.ComputeHash(ASCIIEncoding.ASCII.GetBytes(baseString)));
                }

                // create the request header
                var headerFormat = "OAuth oauth_nonce=\"{0}\", oauth_signature_method=\"{1}\", " +
                                   "oauth_timestamp=\"{2}\", oauth_consumer_key=\"{3}\", " +
                                   "oauth_token=\"{4}\", oauth_signature=\"{5}\", " +
                                   "oauth_version=\"{6}\"";

                var authHeader = string.Format(headerFormat,
                                        Uri.EscapeDataString(oauth_nonce),
                                        Uri.EscapeDataString(oauth_signature_method),
                                        Uri.EscapeDataString(oauth_timestamp),
                                        Uri.EscapeDataString(oauth_consumer_key),
                                        Uri.EscapeDataString(oauth_token),
                                        Uri.EscapeDataString(oauth_signature),
                                        Uri.EscapeDataString(oauth_version)
                                );



                ServicePointManager.Expect100Continue = false;

                // make the request
                var postBody = "screen_name=" + id;
                resource_url += "?" + postBody;
                HttpWebRequest request = (HttpWebRequest)WebRequest.Create(resource_url);
                request.Headers.Add("Authorization", authHeader);
                request.Method = "GET";
                request.ContentType = "application/x-www-form-urlencoded";
                var response = (HttpWebResponse)request.GetResponse();
                var reader = new StreamReader(response.GetResponseStream());
                res = reader.ReadToEnd();

                cache.Set(key, res, policy);
            }

            return new HttpResponseMessage()
            {
                Content = new StringContent(
                        res,
                        Encoding.UTF8,
                       "application/json"
                    )
            };

        }
    }
}
