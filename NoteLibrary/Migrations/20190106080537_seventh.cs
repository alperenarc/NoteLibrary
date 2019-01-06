using Microsoft.EntityFrameworkCore.Migrations;

namespace NoteLibrary.Migrations
{
    public partial class seventh : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropSequence(
                name: "UserTable",
                schema: "dbo");

            migrationBuilder.AlterColumn<int>(
                name: "Id",
                table: "UserTable",
                nullable: false,
                oldClrType: typeof(int),
                oldDefaultValueSql: "NEXT VALUE FOR dbo.UserTable");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.EnsureSchema(
                name: "dbo");

            migrationBuilder.CreateSequence<int>(
                name: "UserTable",
                schema: "dbo");

            migrationBuilder.AlterColumn<int>(
                name: "Id",
                table: "UserTable",
                nullable: false,
                defaultValueSql: "NEXT VALUE FOR dbo.UserTable",
                oldClrType: typeof(int));
        }
    }
}
