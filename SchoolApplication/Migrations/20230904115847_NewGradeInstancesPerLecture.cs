using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SchoolApplication.Migrations
{
    public partial class NewGradeInstancesPerLecture : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "changingLecture",
                table: "AspNetUsers");

            migrationBuilder.RenameColumn(
                name: "changingGrade",
                table: "AspNetUsers",
                newName: "Sports");

            migrationBuilder.AddColumn<decimal>(
                name: "History",
                table: "AspNetUsers",
                type: "decimal(18,2)",
                nullable: true);

            migrationBuilder.AddColumn<decimal>(
                name: "Language",
                table: "AspNetUsers",
                type: "decimal(18,2)",
                nullable: true);

            migrationBuilder.AddColumn<decimal>(
                name: "Math",
                table: "AspNetUsers",
                type: "decimal(18,2)",
                nullable: true);

            migrationBuilder.AddColumn<decimal>(
                name: "Science",
                table: "AspNetUsers",
                type: "decimal(18,2)",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "History",
                table: "AspNetUsers");

            migrationBuilder.DropColumn(
                name: "Language",
                table: "AspNetUsers");

            migrationBuilder.DropColumn(
                name: "Math",
                table: "AspNetUsers");

            migrationBuilder.DropColumn(
                name: "Science",
                table: "AspNetUsers");

            migrationBuilder.RenameColumn(
                name: "Sports",
                table: "AspNetUsers",
                newName: "changingGrade");

            migrationBuilder.AddColumn<string>(
                name: "changingLecture",
                table: "AspNetUsers",
                type: "nvarchar(max)",
                nullable: true);
        }
    }
}
