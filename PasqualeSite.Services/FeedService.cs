using PasqualeSite.Data.Entities;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using System.Web;

namespace PasqualeSite.Services
{
    public class FeedService : DisposableService
    {
        public async Task<List<RSSFeeds>> GetFeeds()
        {
            var feeds = await db.Feeds.ToListAsync();
            return feeds;
        }

        public async Task<RSSFeeds> SaveFeed(RSSFeeds feed)
        {
            var existingFeed = await db.Feeds.Where(x => x.Id == feed.Id).FirstOrDefaultAsync();
            if (existingFeed != null)
            {
                db.Entry(existingFeed).CurrentValues.SetValues(feed);
            }
            else
            {
                db.Feeds.Add(feed);
            }

            await db.SaveChangesAsync();
            return feed;
        }

        public async Task<RSSFeeds> DeleteFeed(int id)
        {
            var feed = await db.Feeds.Where(x => x.Id == id).FirstOrDefaultAsync();
            if (feed != null)
            {
                db.Feeds.Remove(feed);
                await db.SaveChangesAsync();
                return feed;
            }
            return null;
        }
    }
}