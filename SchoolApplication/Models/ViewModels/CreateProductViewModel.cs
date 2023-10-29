using System.ComponentModel.DataAnnotations;

namespace SchoolApplication.Models.ViewModels
{
    public class CreateProductViewModel
    {
        [Required]
        public string Title { get; set; }

        [Required]
        public string Description { get; set; }

        [Required]

        public string Lecture { get; set; }

        [Required]
        public int Quantity { get; set; }


        [Required]

        public string Author { get; set; }

        [Required]

        public string ISBN { get; set; }

        [Required]

        [Display(Name = "TextBook Cover Page")]
        public IFormFile ImageFile { get; set; }


        [Required]
        [Display(Name = "List Price")]
        [Range(1, 1000)]
        public double ListPrice { get; set; }


        [Required]
        [Display(Name = "Price for 1-50")]
        [Range(1, 1000)]
        public double Price { get; set; }


        [Required]
        [Display(Name = "Price for 50+")]
        [Range(1, 1000)]
        public double Price50 { get; set; }


        [Required]
        [Display(Name = "Price for 100+")]
        [Range(1, 1000)]
        public double Price100 { get; set; }

      
    }
}
