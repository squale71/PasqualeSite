using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace PasqualeSite.Web.Models
{
    public class LogInModel
    {
        [Required(ErrorMessage = "The username is a required field")]
        [Display(Name = "User Name")]
        public string UserName { get; set; }

        [Required(ErrorMessage = "The password is required field")]
        [DataType(DataType.Password)]
        public string Password { get; set; }
    }
}