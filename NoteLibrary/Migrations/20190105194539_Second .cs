using Microsoft.EntityFrameworkCore.Migrations;

namespace NoteLibrary.Migrations
{
    public partial class Second : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "UpperId",
                table: "CategoryTable");

            migrationBuilder.AddColumn<int>(
                name: "CategoryIdId",
                table: "CategoryTable",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_CategoryTable_CategoryIdId",
                table: "CategoryTable",
                column: "CategoryIdId");

            migrationBuilder.AddForeignKey(
                name: "FK_CategoryTable_CategoryTable_CategoryIdId",
                table: "CategoryTable",
                column: "CategoryIdId",
                principalTable: "CategoryTable",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_CategoryTable_CategoryTable_CategoryIdId",
                table: "CategoryTable");

            migrationBuilder.DropIndex(
                name: "IX_CategoryTable_CategoryIdId",
                table: "CategoryTable");

            migrationBuilder.DropColumn(
                name: "CategoryIdId",
                table: "CategoryTable");

            migrationBuilder.AddColumn<int>(
                name: "UpperId",
                table: "CategoryTable",
                nullable: false,
                defaultValue: 0);
        }
    }
}
