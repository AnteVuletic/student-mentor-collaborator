using Microsoft.EntityFrameworkCore.Migrations;

namespace StudentMentor.Data.Migrations
{
    public partial class AddGithubAccessKeyToStudent : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "GithubAccessKey",
                table: "Users",
                type: "nvarchar(450)",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Users_GithubAccessKey",
                table: "Users",
                column: "GithubAccessKey",
                unique: true,
                filter: "[GithubAccessKey] IS NOT NULL");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Users_GithubAccessKey",
                table: "Users");

            migrationBuilder.DropColumn(
                name: "GithubAccessKey",
                table: "Users");
        }
    }
}
