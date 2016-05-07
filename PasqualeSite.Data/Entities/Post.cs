using PasqualeSite.Data.Identity;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace PasqualeSite.Data.Entities
{
    public class Post
    {
        public int Id { get; set; }
        [MaxLength(100)]
        public string Title { get; set; }
        [MaxLength(500)]
        public string Teaser { get; set; }
        public string PostContent { get; set; }
        public int ImageId { get; set; }
        public string UserId { get; set; }
        public DateTime DateCreated { get; set; }
        public DateTime? DateModified { get; set; }
        public bool IsFeatured { get; set; }
        public int Priority { get; set; }

        [ForeignKey("UserId")]
        public virtual AppUser User { get; set; }
        [ForeignKey("ImageId")]
        public virtual PostImage Image { get; set; }

        public ICollection<Comment> Comments { get; set; }
    }
}