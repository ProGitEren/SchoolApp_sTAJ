using Microsoft.EntityFrameworkCore;
using SchoolApplication.Messages;
using SchoolApplication.Models;


namespace SchoolApplication.Data
{
    public class ObjectDbContext : DbContext
    {
        public ObjectDbContext(DbContextOptions<ObjectDbContext> options) : base(options)
        {
        }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {


            modelBuilder.Entity<Product>().HasData(
                new Product 
                {
                    Id= 1,
                    Title = "Calculus II",
                    Author = "Eren Gökmenler",
                    Lecture = "Math",
                    Quantity = 600,
                    ISBN = "SWD999901",
                    ListPrice = 99,
                    Price = 90,
                    Price50 = 85,
                    Price100 = 80,
                    Description = "The calculus book represents the integrals and differentiations in advanced level.",
                    ImagePath = "/Images/Image_1.jpg"

                },
                new Product
                {
                    Id = 2,
                    Title = "Calculus III",
                    Author = "Emre Gökmenler",
                    Lecture = "Math",
                    Quantity = 400,
                    ISBN = "SWD999802",
                    ListPrice = 102,
                    Price = 95,
                    Price50 = 90,
                    Price100 = 85,
                    Description = "The calculus book represents the multilevel integrals,partial differentiations and vectors in advanced level.",
                    ImagePath = "/Images/image2.png"

                }

                );
                

            // Other configurations
        }

        // Define your custom data models as DbSet properties here
        public DbSet<MessageContainer> MessageContainer { get; set; }
        public DbSet<Product> Products { get; set; }

        // Add other custom configurations as needed
    }
}
