using System;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

namespace NoteLibrary.Migrations
{
    public partial class Database_Added : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Category");

            migrationBuilder.CreateTable(
                name: "CategoryTable",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    State = table.Column<bool>(nullable: false),
                    Name = table.Column<string>(nullable: false),
                    UpperId = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CategoryTable", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "FileTable",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    State = table.Column<bool>(nullable: false),
                    CourseName = table.Column<string>(nullable: false),
                    Title = table.Column<string>(nullable: false),
                    Description = table.Column<string>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_FileTable", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "UserTable",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false),
                    State = table.Column<bool>(nullable: false),
                    Name = table.Column<string>(nullable: false),
                    Surname = table.Column<string>(nullable: false),
                    UserName = table.Column<string>(nullable: false),
                    City = table.Column<string>(nullable: false),
                    University = table.Column<string>(nullable: false),
                    Department = table.Column<string>(nullable: false),
                    Email = table.Column<string>(nullable: false),
                    Password = table.Column<string>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserTable", x => x.UserName);
                });

            migrationBuilder.CreateTable(
                name: "UserFileTable",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    State = table.Column<bool>(nullable: false),
                    FilePath = table.Column<string>(nullable: false),
                    UploadDate = table.Column<DateTime>(nullable: false),
                    AddedUserUserName = table.Column<string>(nullable: true),
                    CategoryId = table.Column<int>(nullable: true)
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

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "FileTable");

            migrationBuilder.DropTable(
                name: "UserFileTable");

            migrationBuilder.DropTable(
                name: "UserTable");

            migrationBuilder.DropTable(
                name: "CategoryTable");

            migrationBuilder.CreateTable(
                name: "Category",
                columns: table => new
                {
                    ID = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    CategoriesIDID = table.Column<int>(nullable: true),
                    Name = table.Column<string>(nullable: false),
                    State = table.Column<bool>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Category", x => x.ID);
                    table.ForeignKey(
                        name: "FK_Category_Category_CategoriesIDID",
                        column: x => x.CategoriesIDID,
                        principalTable: "Category",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Category_CategoriesIDID",
                table: "Category",
                column: "CategoriesIDID");
        }
    }
}
