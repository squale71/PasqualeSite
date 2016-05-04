using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace PasqualeSite.Data.Entities
{
    public class Tag
    {
        public int Id { get; set; }
        [MaxLength(100)]
        public string Name { get; set; }
    }
}