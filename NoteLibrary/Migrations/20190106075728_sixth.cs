using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

namespace NoteLibrary.Migrations
{
    public partial class sixth : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
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
                oldClrType: typeof(int))
                .OldAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropSequence(
                name: "UserTable",
                schema: "dbo");

            migrationBuilder.AlterColumn<int>(
                name: "Id",
                table: "UserTable",
                nullable: false,
                oldClrType: typeof(int),
                oldDefaultValueSql: "NEXT VALUE FOR dbo.UserTable")
                .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);
        }
    }
}
