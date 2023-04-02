using Microsoft.EntityFrameworkCore.Migrations;

namespace DatingApp_API.Migrations
{
    public partial class UpdateTableAppUser : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Introducation",
                table: "AppUsers",
                newName: "Introduction");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Introduction",
                table: "AppUsers",
                newName: "Introducation");
        }
    }
}
