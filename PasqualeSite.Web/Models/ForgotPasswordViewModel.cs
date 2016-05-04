using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace PasqualeSite.Web.Models
{
    public class ForgotPasswordViewModel
    {
        [DisplayName("User Name")]
        [Required]
        public string User { get; set; }

        [DisplayName("Email Address")]
        [Required]
        public string EmailAddress { get; set; }

        [DisplayName("Last Name")]
        [Required]
        public string LastName { get; set; }
    }
}