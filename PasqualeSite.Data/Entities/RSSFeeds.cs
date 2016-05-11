using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace PasqualeSite.Data.Entities
{
    public class RSSFeeds
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string FeedUrl { get; set; }
    }
}