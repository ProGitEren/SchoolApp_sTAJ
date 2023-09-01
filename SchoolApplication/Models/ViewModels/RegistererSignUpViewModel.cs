using System.ComponentModel.DataAnnotations;
using System.Xml.Linq;

namespace SchoolApplication.Models.ViewModels
{
    public class RegistererSignUpViewModel
    {

            //[Required]
            //[Display(Name = "Name")]
            //public string Name { get; set; }

            [Required]
            [Display(Name = "Username")]
            public string UserName { get; set; }

            [Required]
            [DataType(DataType.Password)]
            public string Password { get; set; }

            [Required]
            [DataType(DataType.Password)]
            [Compare("Password", ErrorMessage = "The password and confirmation password do not match.")]
            public string ConfirmPassword { get; set; }

            [Required]
            [EmailAddress]
            public string Email { get; set; }

            [Required]
            [Display(Name ="Phone")]
            public string Phone { get; set; }

            

    }

}
