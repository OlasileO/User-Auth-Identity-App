using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;

namespace User_Identity_App.ViewModel
{
    public class ResetPasswordVm
    {
        [Required]
        [EmailAddress]
        [Display(Name = "Email Address")]
        public string  EmailAddress { get; set; }

        [Required]
        [DataType(DataType.Password)]
        [Display(Name = "Password")]
        public string Password { get; set; }
        [Required]
        [DataType(DataType.Password)]
        [Display(Name = "Confrim Password")]
        [Compare("Password", ErrorMessage = "The Password And Confrim Password Do not Match")]
        public string ConfrimPassword { get; set; }
        
    }
}
