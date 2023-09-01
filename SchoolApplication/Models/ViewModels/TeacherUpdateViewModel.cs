using System.ComponentModel.DataAnnotations;

namespace SchoolApplication.Models.ViewModels
{
    public class TeacherUpdateViewModel
    {
        [Required]
        public string Id { get; set; }
        [Required]
        public string Name { get; set; }

        [Required]
        public string Phone { get; set; }

        [Required]
        public int Lecture { get; set; }

        [Required]
        public int Level { get; set; }
    }
}
