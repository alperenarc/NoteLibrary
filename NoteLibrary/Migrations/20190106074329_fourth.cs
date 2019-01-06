using Microsoft.EntityFrameworkCore.Migrations;

namespace NoteLibrary.Migrations
{
    public partial class fourth : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_FileTable_UserTable_AddedUserUserName",
                table: "FileTable");

            migrationBuilder.DropPrimaryKey(
                name: "PK_UserTable",
                table: "UserTable");

            migrationBuilder.DropIndex(
                name: "IX_FileTable_AddedUserUserName",
                table: "FileTable");

            migrationBuilder.AddColumn<int>(
                name: "AddedUserId",
                table: "FileTable",
                nullable: true);

            migrationBuilder.AddUniqueConstraint(
                name: "AK_UserTable_UserName",
                table: "UserTable",
                column: "UserName");

            migrationBuilder.AddPrimaryKey(
                name: "PK_UserTable",
                table: "UserTable",
                columns: new[] { "Id", "UserName" });

            migrationBuilder.CreateIndex(
                name: "IX_FileTable_AddedUserId_AddedUserUserName",
                table: "FileTable",
                columns: new[] { "AddedUserId", "AddedUserUserName" });

            migrationBuilder.AddForeignKey(
                name: "FK_FileTable_UserTable_AddedUserId_AddedUserUserName",
                table: "FileTable",
                columns: new[] { "AddedUserId", "AddedUserUserName" },
                principalTable: "UserTable",
                principalColumns: new[] { "Id", "UserName" },
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_FileTable_UserTable_AddedUserId_AddedUserUserName",
                table: "FileTable");

            migrationBuilder.DropUniqueConstraint(
                name: "AK_UserTable_UserName",
                table: "UserTable");

            migrationBuilder.DropPrimaryKey(
                name: "PK_UserTable",
                table: "UserTable");

            migrationBuilder.DropIndex(
                name: "IX_FileTable_AddedUserId_AddedUserUserName",
                table: "FileTable");

            migrationBuilder.DropColumn(
                name: "AddedUserId",
                table: "FileTable");

            migrationBuilder.AddPrimaryKey(
                name: "PK_UserTable",
                table: "UserTable",
                column: "UserName");

            migrationBuilder.CreateIndex(
                name: "IX_FileTable_AddedUserUserName",
                table: "FileTable",
                column: "AddedUserUserName");

            migrationBuilder.AddForeignKey(
                name: "FK_FileTable_UserTable_AddedUserUserName",
                table: "FileTable",
                column: "AddedUserUserName",
                principalTable: "UserTable",
                principalColumn: "UserName",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
