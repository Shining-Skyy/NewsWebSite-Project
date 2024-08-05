using System.ComponentModel.DataAnnotations;

namespace Management.ViewModels.Account
{
    public class RegisterViewModel
    {
        [Required(ErrorMessage = "Enter your Full Name!")]
        [MaxLength(30, ErrorMessage = "Full Name should not be more than 30 characters")]
        public string FullName { get; set; }

        [Required(ErrorMessage = "Enter your Email!")]
        [EmailAddress]
        public string Email { get; set; }

        [Required(ErrorMessage = "Enter your Password!")]
        [DataType(DataType.Password)]
        public string Password { get; set; }

        [Required(ErrorMessage = "Enter your Repeat password!")]
        [DataType(DataType.Password)]
        [Compare(nameof(Password), ErrorMessage = "The password and its repetition must be equal!")]
        [Display(Name = "Repeat password")]
        public string RePassword { get; set; }

        [Required(ErrorMessage ="Enter your Phone Number!")]
        [RegularExpression(@"^([0-9]{11})$", ErrorMessage = "The mobile number entered is not valid")]
        public string PhoneNumber { get; set; }
    }
}
