using System.ComponentModel.DataAnnotations;

namespace SchoolApplication.Models.ViewModels
{
    public class RegistererLoginViewModel
    {
        [Required]
        [Display(Name ="Username")]
        public string UserName { get; set; }
        [Required]
        [DataType(DataType.Password)]
        public string Password { get; set; }

        [Required]
        public int logintrial { get; set; }
    }
}
