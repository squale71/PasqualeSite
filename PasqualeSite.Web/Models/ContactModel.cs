using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;

namespace PasqualeSite.Web.Models
{
    public class ContactModel
    {
        [Required]
        [MaxLength(150, ErrorMessage = "Please keep this field under 150 characters.")]
        public string Subject { get; set; }

        [EmailAddress(ErrorMessage = "That doesn't look like a valid email address. Try again.")]
        [MaxLength(254, ErrorMessage = "No email is that long! Please limit to 254 characters.")]
        public string Email { get; set; }

        [DataType(DataType.MultilineText)]
        [Required(ErrorMessage = "What's the point of contacting me without typing anything?")]
        [MaxLength(1000, ErrorMessage = "Sorry! Your comment exceeded the max length. Please be a bit more concise. Under 1000 characters please.")]
        public string Comment { get; set; }
    }
}