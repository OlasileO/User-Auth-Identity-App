using System.ComponentModel.DataAnnotations;

namespace User_Identity_App.ViewModel
{
    public class RegisterVm
    {
        [Required]
        [EmailAddress] 
        [Display (Name ="Email Address")]
        public string EmailAddress { get; set; }

        [Required]
        [Display(Name = "Username")]
        public string Username { get; set; }

        [Required]
        [StringLength(100, ErrorMessage = "The {0} must be at least {2} characters long.", MinimumLength = 6)]
        [DataType(DataType.Password)]
        [Display(Name = "Password")]
        public string Password { get; set; }

        [Required]
        [DataType(DataType.Password)]
        [Display(Name = "Confrim Password")]
        [Compare("Password", ErrorMessage ="The Password And Confrim Password Do not Match")]
        public string ConfirmPassword { get; set; }

        public string?  ReturnUrl { get; set; }
    }
}
