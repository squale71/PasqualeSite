using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace PasqualeSite.Data.Entities
{
    public class PostImage
    {
        public int Id { get; set; }
        public string Path { get; set; }
        [MaxLength(200)]
        public string Description { get; set; }
    }
}