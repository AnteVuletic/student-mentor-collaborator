using Microsoft.EntityFrameworkCore.Migrations;

namespace StudentMentor.Data.Migrations
{
    public partial class AddGithubBearerToken : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "GithubBearerToken",
                table: "Users",
                type: "nvarchar(max)",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "GithubBearerToken",
                table: "Users");
        }
    }
}
