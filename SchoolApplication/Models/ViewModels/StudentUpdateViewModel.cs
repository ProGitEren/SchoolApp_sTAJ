using System.ComponentModel.DataAnnotations;
using System.Xml.Linq;

namespace SchoolApplication.Models.ViewModels
{
    public class StudentUpdateViewModel
    {
        [Required]
        public string Id { get; set; }

        [Required]
        

        public string Department { get; set; }

        [Required]
        
        public string Name { get; set; }

        [Required]
        
        public string Phone { get; set; }

        [Required]
        
        public int Discount { get; set; }
    }
}
