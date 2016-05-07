using PasqualeSite.Data.Identity;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace PasqualeSite.Data.Entities
{
    public class Comment
    {
        public string Id { get; set; }
        [MaxLength(500)]
        public string Text { get; set; }
        public DateTime DatePosted { get; set; }
        public int PostId { get; set; }
        public string UserId { get; set; }
        public bool Enabled { get; set; }

        [ForeignKey("UserId")]
        public virtual AppUser User { get; set; }

        [ForeignKey("PostId")]
        public virtual Post Post { get; set; }

        public ICollection<UserLikes> Likes { get; set; }
    }
}