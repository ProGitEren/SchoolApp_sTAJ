using System.ComponentModel.DataAnnotations;

namespace SchoolApplication.Models.ViewModels
{
    public class EmailVerifyViewModel
    {
        [Required(ErrorMessage = "The email is required.")]
        [EmailAddress(ErrorMessage = "Please enter a valid email address.")]
        public string email { get; set; }

        public string userrole { get; set; }
    }
}
