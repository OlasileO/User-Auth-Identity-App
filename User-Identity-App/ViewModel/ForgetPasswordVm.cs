using System.ComponentModel.DataAnnotations;

namespace User_Identity_App.ViewModel
{
    public class ForgetPasswordVm
    {
        [Required]
        [EmailAddress]
        public string EmailAddress { get; set; }
    }
}
