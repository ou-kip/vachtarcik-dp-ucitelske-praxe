using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace InternshipService.Database.Migrations
{
    /// <inheritdoc />
    public partial class TaskSolutionsAdded : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "InternshipTaskSolution",
                schema: "intern",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Solution = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    TaskId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    CreationAuthor = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    CreationDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdateAuthor = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    UpdatedDate = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_InternshipTaskSolution", x => x.Id);
                    table.ForeignKey(
                        name: "FK_InternshipTaskSolution_InternshipTasks_TaskId",
                        column: x => x.TaskId,
                        principalSchema: "intern",
                        principalTable: "InternshipTasks",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "InternshipTaskSolutionFile",
                schema: "intern",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    FileName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    FileNameWithoutExt = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Extension = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    ContentType = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    SolutionId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    CreationAuthor = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    CreationDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdateAuthor = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    UpdatedDate = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_InternshipTaskSolutionFile", x => x.Id);
                    table.ForeignKey(
                        name: "FK_InternshipTaskSolutionFile_InternshipTaskSolution_SolutionId",
                        column: x => x.SolutionId,
                        principalSchema: "intern",
                        principalTable: "InternshipTaskSolution",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateIndex(
                name: "IX_InternshipTaskSolution_TaskId",
                schema: "intern",
                table: "InternshipTaskSolution",
                column: "TaskId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_InternshipTaskSolutionFile_SolutionId",
                schema: "intern",
                table: "InternshipTaskSolutionFile",
                column: "SolutionId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "InternshipTaskSolutionFile",
                schema: "intern");

            migrationBuilder.DropTable(
                name: "InternshipTaskSolution",
                schema: "intern");
        }
    }
}
