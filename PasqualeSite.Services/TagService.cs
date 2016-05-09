using PasqualeSite.Data.Entities;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using System.Web;

namespace PasqualeSite.Services
{
    public class TagService : DisposableService
    {
        public async Task<List<Tag>> GetAllTags()
        {
            var tags = await db.Tags.ToListAsync();
            return tags;
        }

        public async Task<Tag> SaveTag(Tag newTag)
        {
            var tag = await db.Tags.Where(x => x.Id == newTag.Id).FirstOrDefaultAsync();
            if (tag != null)
            {
                db.Entry(tag).CurrentValues.SetValues(newTag);
            }
            else
            {
                db.Tags.Add(newTag);
            }

            await db.SaveChangesAsync();
            return newTag;
        }
    }
}