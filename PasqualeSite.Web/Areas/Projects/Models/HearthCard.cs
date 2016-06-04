using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace PasqualeSite.Web.Areas.Projects.Models
{
    public class HearthCard
    {
        public int Cost { get; set; }
        public string Name { get; set; }
        public int Attack { get; set; }
        public int Health { get; set; }
        public string Rarity { get; set; }
    }
}