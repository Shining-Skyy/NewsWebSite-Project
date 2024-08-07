using System.ComponentModel.DataAnnotations;

namespace Management.ViewModels.Account
{
    public class ForgotPasswordViewModel
    {
        [Required(ErrorMessage = "Enter your Email!")]
        [EmailAddress]
        public string Email { get; set; }
    }
}
