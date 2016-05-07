using PasqualeSite.Data.Identity;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace PasqualeSite.Data.Entities
{
    public class UserLikes
    {
        public string Id { get; set; }
        public string UserId { get; set; }
        public string CommentId { get; set; }
        public bool DidLike { get; set; }

        [ForeignKey("UserId")]
        public virtual AppUser User { get; set; }

        [ForeignKey("CommentId")]
        public virtual Comment Comment { get; set; }
    }
}