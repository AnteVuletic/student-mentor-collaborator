using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace StudentMentor.Data.Migrations
{
    public partial class FinalsPaperVersioning : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Users_Files_FinalsPaperId",
                table: "Users");

            migrationBuilder.DropIndex(
                name: "IX_Users_FinalsPaperId",
                table: "Users");

            migrationBuilder.DropColumn(
                name: "FinalsPaperId",
                table: "Users");

            migrationBuilder.CreateTable(
                name: "StudentFiles",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    StudentId = table.Column<int>(type: "int", nullable: false),
                    FileId = table.Column<int>(type: "int", nullable: false),
                    TimeCreated = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_StudentFiles", x => x.Id);
                    table.ForeignKey(
                        name: "FK_StudentFiles_Files_FileId",
                        column: x => x.FileId,
                        principalTable: "Files",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_StudentFiles_Users_StudentId",
                        column: x => x.StudentId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_StudentFiles_FileId",
                table: "StudentFiles",
                column: "FileId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_StudentFiles_StudentId",
                table: "StudentFiles",
                column: "StudentId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "StudentFiles");

            migrationBuilder.AddColumn<int>(
                name: "FinalsPaperId",
                table: "Users",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Users_FinalsPaperId",
                table: "Users",
                column: "FinalsPaperId",
                unique: true,
                filter: "[FinalsPaperId] IS NOT NULL");

            migrationBuilder.AddForeignKey(
                name: "FK_Users_Files_FinalsPaperId",
                table: "Users",
                column: "FinalsPaperId",
                principalTable: "Files",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
