using System.ComponentModel.DataAnnotations;

namespace Management.ViewModels.Account
{
    public class TwoFactorLoginViewModel
    {
        [Required(ErrorMessage = "Enter the Code sent")]
        public string Code { get; set; }
        public bool IsPersistent { get; set; }
        public string Provider { get; set; }
    }
}
