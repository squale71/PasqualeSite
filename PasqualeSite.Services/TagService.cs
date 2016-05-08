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
    }
}