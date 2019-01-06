using System;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

namespace NoteLibrary.Migrations
{
    public partial class third : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "UserFileTable");

            migrationBuilder.AddColumn<string>(
                name: "AddedUserUserName",
                table: "FileTable",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "CategoryId",
                table: "FileTable",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "FilePath",
                table: "FileTable",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<DateTime>(
                name: "UploadDate",
                table: "FileTable",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.CreateIndex(
                name: "IX_FileTable_AddedUserUserName",
                table: "FileTable",
                column: "AddedUserUserName");

            migrationBuilder.CreateIndex(
                name: "IX_FileTable_CategoryId",
                table: "FileTable",
                column: "CategoryId");

            migrationBuilder.AddForeignKey(
                name: "FK_FileTable_UserTable_AddedUserUserName",
                table: "FileTable",
                column: "AddedUserUserName",
                principalTable: "UserTable",
                principalColumn: "UserName",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_FileTable_CategoryTable_CategoryId",
                table: "FileTable",
                column: "CategoryId",
                principalTable: "CategoryTable",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_FileTable_UserTable_AddedUserUserName",
                table: "FileTable");

            migrationBuilder.DropForeignKey(
                name: "FK_FileTable_CategoryTable_CategoryId",
                table: "FileTable");

            migrationBuilder.DropIndex(
                name: "IX_FileTable_AddedUserUserName",
                table: "FileTable");

            migrationBuilder.DropIndex(
                name: "IX_FileTable_CategoryId",
                table: "FileTable");

            migrationBuilder.DropColumn(
                name: "AddedUserUserName",
                table: "FileTable");

            migrationBuilder.DropColumn(
                name: "CategoryId",
                table: "FileTable");

            migrationBuilder.DropColumn(
                name: "FilePath",
                table: "FileTable");

            migrationBuilder.DropColumn(
                name: "UploadDate",
                table: "FileTable");

            migrationBuilder.CreateTable(
                name: "UserFileTable",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    AddedUserUserName = table.Column<string>(nullable: true),
                    CategoryId = table.Column<int>(nullable: true),
                    FilePath = table.Column<string>(nullable: false),
                    State = table.Column<bool>(nullable: false),
                    UploadDate = table.Column<DateTime>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserFileTable", x => x.Id);
                    table.ForeignKey(
                        name: "FK_UserFileTable_UserTable_AddedUserUserName",
                        column: x => x.AddedUserUserName,
                        principalTable: "UserTable",
                        principalColumn: "UserName",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_UserFileTable_CategoryTable_CategoryId",
                        column: x => x.CategoryId,
                        principalTable: "CategoryTable",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_UserFileTable_AddedUserUserName",
                table: "UserFileTable",
                column: "AddedUserUserName");

            migrationBuilder.CreateIndex(
                name: "IX_UserFileTable_CategoryId",
                table: "UserFileTable",
                column: "CategoryId");
        }
    }
}
