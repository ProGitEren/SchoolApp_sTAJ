using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SchoolApplication.Migrations.ObjectDb
{
    public partial class ProductToObjectDb : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Products",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Title = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Lecture = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Quantity = table.Column<int>(type: "int", nullable: false),
                    Author = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    ISBN = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    ImagePath = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    ListPrice = table.Column<double>(type: "float", nullable: false),
                    Price = table.Column<double>(type: "float", nullable: false),
                    Price50 = table.Column<double>(type: "float", nullable: false),
                    Price100 = table.Column<double>(type: "float", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Products", x => x.Id);
                });

            migrationBuilder.InsertData(
                table: "Products",
                columns: new[] { "Id", "Author", "Description", "ISBN", "ImagePath", "Lecture", "ListPrice", "Price", "Price100", "Price50", "Quantity", "Title" },
                values: new object[] { 1, "Eren Gökmenler", "The calculus book represents the integrals and differentiations in advanced level.", "SWD999901", "Images/Image_1", "Math", 99.0, 90.0, 80.0, 85.0, 600, "Calculus II" });

            migrationBuilder.InsertData(
                table: "Products",
                columns: new[] { "Id", "Author", "Description", "ISBN", "ImagePath", "Lecture", "ListPrice", "Price", "Price100", "Price50", "Quantity", "Title" },
                values: new object[] { 2, "Emre Gökmenler", "The calculus book represents the multilevel integrals,partial differentiations and vectors in advanced level.", "SWD999802", "Images/Image_2", "Math", 102.0, 95.0, 85.0, 90.0, 400, "Calculus III" });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Products");
        }
    }
}
