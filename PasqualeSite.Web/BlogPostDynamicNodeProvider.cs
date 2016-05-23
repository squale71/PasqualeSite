using MvcSiteMapProvider;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using PasqualeSite.Data;
using PasqualeSite.Data.Database;

namespace PasqualeSite.Web
{
    public class BlogPostDynamicNodeProvider : DynamicNodeProviderBase
    {
        public override IEnumerable<DynamicNode> GetDynamicNodeCollection(ISiteMapNode node)
        {
            using (var db = new MyDbContext())
            {
                // Create a node for each album 
                foreach (var post in db.Posts.Where(x => x.IsActive))
                {
                    DynamicNode dynamicNode = new DynamicNode();
                    dynamicNode.Title = post.Title;
                    dynamicNode.RouteValues.Add("year", post.DateCreated.Year);
                    dynamicNode.RouteValues.Add("month", post.DateCreated.Month);
                    dynamicNode.RouteValues.Add("day", post.DateCreated.Day);
                    dynamicNode.RouteValues.Add("urlTitle", post.UrlTitle);

                    yield return dynamicNode;
                }
            }
        }
    }
}