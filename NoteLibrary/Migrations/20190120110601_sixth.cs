using Microsoft.EntityFrameworkCore.Migrations;

namespace NoteLibrary.Migrations
{
    public partial class sixth : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ConfirmPassword",
                table: "UserTable");

            migrationBuilder.DropColumn(
                name: "Password",
                table: "UserTable");

            migrationBuilder.AddColumn<string>(
                name: "Hash",
                table: "UserTable",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Hash",
                table: "UserTable");

            migrationBuilder.AddColumn<string>(
                name: "ConfirmPassword",
                table: "UserTable",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "Password",
                table: "UserTable",
                nullable: false,
                defaultValue: "");
        }
    }
}
