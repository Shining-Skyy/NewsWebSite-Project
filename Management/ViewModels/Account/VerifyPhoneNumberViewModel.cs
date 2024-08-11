using System.ComponentModel.DataAnnotations;

namespace Management.ViewModels.Account
{
    public class VerifyPhoneNumberViewModel
    {
        [Required(ErrorMessage = "Enter your Code!")]
        [MinLength(6)]
        [MaxLength(6)]
        [Display(Name = ("Code"))]
        public string Token { get; set; }
    }
}
