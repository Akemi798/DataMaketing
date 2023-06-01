using System;
using System.ComponentModel.DataAnnotations;

namespace OkVip.ManagementDataMarketing.Models.ViewModels.Account
{
    public class CreateAccountViewModel
    {
        public string UserName { get; set; }

        [Required]
        [StringLength(100, ErrorMessage = "The {0} must be at least {2} and at max {1} characters long.", MinimumLength = 6)]
        [DataType(DataType.Password)]
        [Display(Name = "Password")]
        public string Password { get; set; }

        [DataType(DataType.Password)]
        [Display(Name = "Confirm password")]
        [Compare("Password", ErrorMessage = "Xác nhận password không khớp")]
        public string ConfirmPassword { get; set; }

        public bool IsAdmin { get; set; }

    }
}
