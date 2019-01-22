using Microsoft.EntityFrameworkCore.Migrations;

namespace NoteLibrary.Migrations
{
    public partial class seventh : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Department",
                table: "FileTable",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "University",
                table: "FileTable",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Department",
                table: "FileTable");

            migrationBuilder.DropColumn(
                name: "University",
                table: "FileTable");
        }
    }
}
