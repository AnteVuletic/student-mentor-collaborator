using Microsoft.EntityFrameworkCore.Migrations;

namespace StudentMentor.Data.Migrations
{
    public partial class AddRepositoryIdToStudent : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "GithubRepositoryId",
                table: "Users",
                type: "int",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "GithubRepositoryId",
                table: "Users");
        }
    }
}
