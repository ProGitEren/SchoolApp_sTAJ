using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;

namespace SchoolApplication.Models
{
    public class ApplicationUser :IdentityUser
    {
        [Required]
        public string Name { get; set; }

        [Required]
        public DateTime CreatedTime { get; set; } = DateTime.Now;

        [Required]
        public string Phone { get; set; }

    
    }
}
