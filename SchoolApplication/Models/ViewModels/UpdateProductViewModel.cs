using System.ComponentModel.DataAnnotations;

namespace SchoolApplication.Models.ViewModels
{
    public class UpdateProductViewModel
    {
        [Required]

        public int ProductId { get; set; }

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

        //This is stated as not required since the cover page may not change and the initial value of this is null unlike form the others

        [Display(Name = "TextBook Cover Page")]

        //it is also put nullable to be not required
        public IFormFile? ImageFile { get; set; }


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

        //this part is for showing the latest image to the user with read-only and the other input will take if another file needed to uploaded
        
        [Required]
        public string ImagePath { get; set; }
    }
}
