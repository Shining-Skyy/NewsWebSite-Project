using System.ComponentModel.DataAnnotations;

namespace Management.ViewModels.Account
{
    public class LoginViewModel
    {
        [Required(ErrorMessage = "Enter your Email!")]
        [EmailAddress]
        public string Email { get; set; }

        [Required(ErrorMessage = "Enter your Password!")]
        [DataType(DataType.Password)]
        public string Password { get; set; }

        [Display(Name = ("Remember Me"))]
        public bool IsPersistent { get; set; } = false;

        public string ReturnUrl { get; set; }
    }
}
