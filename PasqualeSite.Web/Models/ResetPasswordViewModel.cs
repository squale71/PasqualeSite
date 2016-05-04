using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace PasqualeSite.Web.Models
{
    public class ResetPasswordViewModel
    {
        public string userId { get; set; }
        public string resetToken { get; set; }

        [Required(ErrorMessage = "Password is required")]
        [StringLength(255, ErrorMessage = "Must be between 6 and 255 characters", MinimumLength = 6)]
        [DataType(DataType.Password)]
        [DisplayName("New Password")]
        public string newPassword { get; set; }

        [Required(ErrorMessage = "Confirm Password is required")]
        [StringLength(255, ErrorMessage = "Must be between 6 and 255 characters", MinimumLength = 6)]
        [DataType(DataType.Password)]
        [Compare("newPassword")]
        [DisplayName("Confirm Password")]
        public string confirmPassword { get; set; }
    }
}