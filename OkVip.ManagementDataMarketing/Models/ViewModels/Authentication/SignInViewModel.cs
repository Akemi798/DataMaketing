using System.ComponentModel.DataAnnotations;

namespace OkVip.ManagementDataMarketing.Models.ViewModels.Authentication
{
    public class SignInViewModel
    {
        [Required]
        [Display(Name = "UserName")]
        public string Email { get; set; }

        [Required]
        [DataType(DataType.Password)]
        public string Password { get; set; }

        [Display(Name = "Google Authenticator Pin")]
        public string Pin { get; set; }


        [Display(Name = "Remember me?")]
        public bool RememberMe { get; set; }
    }
}
