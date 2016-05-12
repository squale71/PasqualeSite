using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Net;
using System.Xml;
using System.Data;
using System.Xml.Linq;
using System.Threading.Tasks;

namespace PasqualeSite.Web
{
    public class RSSFeed
    {
        public string Title { get; set; }
        public string Description { get; set; }
        public string Link { get; set; }
        public RSSImage Image { get; set; }
        public List<RSSItem> Items { get; set; }
    }

    public class RSSImage
    {
        public string Url { get; set; }
        public string Title { get; set; }
        public string Link { get; set; }
    }

    public class RSSItem
    {
        public string Title { get; set; }
        public string Link { get; set; }
        public string Description { get; set; }
    }

    public class RSSHelper
    {
        public RSSFeed RSSItem { get; set; }

        public RSSFeed GetRSS(string url)
        {
            XmlDocument rssXmlDoc = new XmlDocument();

            try
            {
                // Load the RSS file from the RSS URL
                rssXmlDoc.Load(url);

                // Create new RSS Object
                var newRSSItem = new RSSFeed();

                // Parse the Items in the RSS file
                XmlNode xmlTitle = rssXmlDoc.SelectSingleNode("rss/channel/title");
                newRSSItem.Title = xmlTitle != null ? xmlTitle.InnerText : "";

                XmlNode xmlDesc = rssXmlDoc.SelectSingleNode("rss/channel/description");
                newRSSItem.Description = xmlDesc != null ? xmlDesc.InnerText : "";

                XmlNode xmlLink = rssXmlDoc.SelectSingleNode("rss/channel/link");
                newRSSItem.Link = xmlLink != null ? xmlLink.InnerText : "";

                XmlNodeList rssItems = rssXmlDoc.SelectNodes("rss/channel/item");

                if (rssItems != null)
                {
                    newRSSItem.Items = new List<RSSItem>();
                    // Iterate through the items in the RSS file
                    foreach (XmlNode rssNode in rssItems)
                    {
                        var newItem = new RSSItem();
                        XmlNode rssSubNode = rssNode.SelectSingleNode("title");
                        newItem.Title = rssSubNode != null ? rssSubNode.InnerText : "";

                        rssSubNode = rssNode.SelectSingleNode("link");
                        newItem.Link = rssSubNode != null ? rssSubNode.InnerText : "";

                        rssSubNode = rssNode.SelectSingleNode("description");
                        newItem.Description = rssSubNode != null ? rssSubNode.InnerText : "";

                        newRSSItem.Items.Add(newItem);

                    }
                }

                return newRSSItem;
            }

            catch(Exception ex)
            {
                Console.WriteLine(url);
            }

            return null;      
        }
    }
}