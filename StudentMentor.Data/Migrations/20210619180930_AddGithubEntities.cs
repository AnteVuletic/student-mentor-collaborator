using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace StudentMentor.Data.Migrations
{
    public partial class AddGithubEntities : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.EnsureSchema(
                name: "github");

            migrationBuilder.AddColumn<int>(
                name: "PushActivityId",
                table: "Messages",
                type: "int",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "PushActivities",
                schema: "github",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Ref = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    RepositoryId = table.Column<int>(type: "int", nullable: false),
                    RepositoryName = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PushActivities", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Commits",
                schema: "github",
                columns: table => new
                {
                    Id = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    TimeStamp = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Message = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Url = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    PushActivityId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Commits", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Commits_PushActivities_PushActivityId",
                        column: x => x.PushActivityId,
                        principalSchema: "github",
                        principalTable: "PushActivities",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "FileLogs",
                schema: "github",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    File = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ChangeType = table.Column<int>(type: "int", nullable: false),
                    CommitId = table.Column<string>(type: "nvarchar(450)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_FileLogs", x => x.Id);
                    table.ForeignKey(
                        name: "FK_FileLogs_Commits_CommitId",
                        column: x => x.CommitId,
                        principalSchema: "github",
                        principalTable: "Commits",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Messages_PushActivityId",
                table: "Messages",
                column: "PushActivityId",
                unique: true,
                filter: "[PushActivityId] IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "IX_Commits_PushActivityId",
                schema: "github",
                table: "Commits",
                column: "PushActivityId");

            migrationBuilder.CreateIndex(
                name: "IX_FileLogs_CommitId",
                schema: "github",
                table: "FileLogs",
                column: "CommitId");

            migrationBuilder.CreateIndex(
                name: "IX_PushActivities_RepositoryId",
                schema: "github",
                table: "PushActivities",
                column: "RepositoryId");

            migrationBuilder.AddForeignKey(
                name: "FK_Messages_PushActivities_PushActivityId",
                table: "Messages",
                column: "PushActivityId",
                principalSchema: "github",
                principalTable: "PushActivities",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Messages_PushActivities_PushActivityId",
                table: "Messages");

            migrationBuilder.DropTable(
                name: "FileLogs",
                schema: "github");

            migrationBuilder.DropTable(
                name: "Commits",
                schema: "github");

            migrationBuilder.DropTable(
                name: "PushActivities",
                schema: "github");

            migrationBuilder.DropIndex(
                name: "IX_Messages_PushActivityId",
                table: "Messages");

            migrationBuilder.DropColumn(
                name: "PushActivityId",
                table: "Messages");
        }
    }
}
