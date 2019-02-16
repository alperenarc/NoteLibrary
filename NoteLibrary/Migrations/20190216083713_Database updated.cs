using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace NoteLibrary.Migrations
{
    public partial class Databaseupdated : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "ConfirmGuid",
                table: "UserTable",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "Date",
                table: "UserTable",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<bool>(
                name: "IsConfirmed",
                table: "UserTable",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "IsTeacher",
                table: "UserTable",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<string>(
                name: "Password",
                table: "FileTable",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ConfirmGuid",
                table: "UserTable");

            migrationBuilder.DropColumn(
                name: "Date",
                table: "UserTable");

            migrationBuilder.DropColumn(
                name: "IsConfirmed",
                table: "UserTable");

            migrationBuilder.DropColumn(
                name: "IsTeacher",
                table: "UserTable");

            migrationBuilder.DropColumn(
                name: "Password",
                table: "FileTable");
        }
    }
}
