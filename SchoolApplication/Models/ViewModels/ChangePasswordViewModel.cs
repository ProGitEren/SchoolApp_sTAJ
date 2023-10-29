using System.ComponentModel.DataAnnotations;

namespace SchoolApplication.Models.ViewModels
{
    public class ChangePasswordViewModel
    {
        [Required(ErrorMessage = "Please enter your old password.")]
        [DataType(DataType.Password)]
        public string oldPassword { get; set; }

        [Required]
        public string Id { get; set; }

        [Required]
        [DataType(DataType.Password)]
        public string newPassword { get; set; }

        [Required]
        [DataType(DataType.Password)]
        [Compare("newPassword", ErrorMessage = "The password and confirmation password do not match.")]
        public string newConfirmPassword { get; set; }
    }
}
