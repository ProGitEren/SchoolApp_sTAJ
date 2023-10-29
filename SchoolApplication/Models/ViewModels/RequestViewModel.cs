using System.ComponentModel.DataAnnotations;

namespace SchoolApplication.Models.ViewModels
{
    public class RequestViewModel
    {
        [Required]
        public string Name { get; set;}

        [Required]

        public string Id {  get; set; }

        [Required]
        public string Message { get; set; }

        [Required]
        public string Subject { get; set; }

    }
}
