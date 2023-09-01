using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SchoolApplication.Migrations
{
    public partial class ChangeInAllProperties : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Price",
                table: "AspNetUsers");

            migrationBuilder.AddColumn<int>(
                name: "Discount",
                table: "AspNetUsers",
                type: "int",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Discount",
                table: "AspNetUsers");

            migrationBuilder.AddColumn<decimal>(
                name: "Price",
                table: "AspNetUsers",
                type: "decimal(18,2)",
                nullable: true);
        }
    }
}
