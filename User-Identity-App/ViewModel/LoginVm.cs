using System.ComponentModel.DataAnnotations;

namespace User_Identity_App.ViewModel
{
    public class LoginVm
    {
        [Required]
        [Display(Name = "Email")]
        public string EmailAddress { get; set; }

        [Required]
        [DataType(DataType.Password)]
        public string Password { get; set; }

        //[Display(Name = "Remember Me?")]
        //public bool RememberMe { get; set; }
        //public string? ReturnUrl { get; set; }
    }
}
