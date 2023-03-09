using System.ComponentModel.DataAnnotations;

namespace User_Identity_App.ViewModel
{
    public class ExternalLoginConfirmationVm
    {
        [Required]
        [EmailAddress]
        public string EmailAddress { get; set; }

        [Required]
        public string Name { get; set; }
    }
}
