using System.ComponentModel.DataAnnotations;
using System.Xml.Linq;

namespace SchoolApplication.Models.ViewModels
{
    public class TeacherLoginViewModel
    {
        [Required]
        [Display(Name = "Username")]
        public string UserName { get; set; }
        [Required]
        [DataType(DataType.Password)]
        public string Password { get; set; }
    }
}
