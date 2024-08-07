using System.ComponentModel.DataAnnotations;

namespace Management.ViewModels.Account
{
    public class ResetPasswordViewModel
    {
        [Required(ErrorMessage = "Enter your Password!")]
        [DataType(DataType.Password)]
        public string Password { get; set; }

        [Required(ErrorMessage = "Enter your Repeat password!")]
        [DataType(DataType.Password)]
        [Compare(nameof(Password), ErrorMessage = "The password and its repetition must be equal!")]
        [Display(Name = "Repeat password")]
        public string RePassword { get; set; }

        public string UserId { get; set; }
        public string Token { get; set; }
    }
}
