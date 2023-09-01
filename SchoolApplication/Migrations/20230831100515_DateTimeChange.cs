using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SchoolApplication.Migrations
{
    public partial class DateTimeChange : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Student_Phone",
                table: "AspNetUsers");

            migrationBuilder.DropColumn(
                name: "Teacher_Phone",
                table: "AspNetUsers");

            migrationBuilder.RenameColumn(
                name: "Teacher_LastModified",
                table: "AspNetUsers",
                newName: "expiredate");

            migrationBuilder.RenameColumn(
                name: "LastModified",
                table: "AspNetUsers",
                newName: "CreatedTime");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "expiredate",
                table: "AspNetUsers",
                newName: "Teacher_LastModified");

            migrationBuilder.RenameColumn(
                name: "CreatedTime",
                table: "AspNetUsers",
                newName: "LastModified");

            migrationBuilder.AddColumn<string>(
                name: "Student_Phone",
                table: "AspNetUsers",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Teacher_Phone",
                table: "AspNetUsers",
                type: "nvarchar(max)",
                nullable: true);
        }
    }
}
