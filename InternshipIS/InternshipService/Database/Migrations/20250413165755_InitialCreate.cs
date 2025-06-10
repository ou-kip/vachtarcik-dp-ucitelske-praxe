using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace InternshipService.Database.Migrations
{
    /// <inheritdoc />
    public partial class InitialCreate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.EnsureSchema(
                name: "intern");

            migrationBuilder.CreateTable(
                name: "InternshipCategories",
                schema: "intern",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Code = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    CreationAuthor = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    CreationDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdateAuthor = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    UpdatedDate = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_InternshipCategories", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "InternshipCompanyRelatives",
                schema: "intern",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    CompanyName = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    CreationAuthor = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    CreationDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdateAuthor = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    UpdatedDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UserId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    LastName = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    Email = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_InternshipCompanyRelatives", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "InternshipStudents",
                schema: "intern",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    StudentCode = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    CreationAuthor = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    CreationDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdateAuthor = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    UpdatedDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UserId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    LastName = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    Email = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_InternshipStudents", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "InternshipTeachers",
                schema: "intern",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    CreationAuthor = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    CreationDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdateAuthor = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    UpdatedDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UserId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    LastName = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    Email = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_InternshipTeachers", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Internships",
                schema: "intern",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(128)", maxLength: 128, nullable: false),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    CompanyName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    SelectedOn = table.Column<DateTime>(type: "datetime2", nullable: true),
                    FinishedOn = table.Column<DateTime>(type: "datetime2", nullable: true),
                    State = table.Column<int>(type: "int", nullable: false),
                    CategoryId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    IsTemplate = table.Column<bool>(type: "bit", nullable: false),
                    CreationAuthor = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    CreationDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdateAuthor = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    UpdatedDate = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Internships", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Internships_InternshipCategories_CategoryId",
                        column: x => x.CategoryId,
                        principalSchema: "intern",
                        principalTable: "InternshipCategories",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "Internship_Related_Person",
                schema: "intern",
                columns: table => new
                {
                    CompanyRelativesId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    InternshipsId = table.Column<Guid>(type: "uniqueidentifier", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Internship_Related_Person", x => new { x.CompanyRelativesId, x.InternshipsId });
                    table.ForeignKey(
                        name: "FK_Internship_Related_Person_InternshipCompanyRelatives_CompanyRelativesId",
                        column: x => x.CompanyRelativesId,
                        principalSchema: "intern",
                        principalTable: "InternshipCompanyRelatives",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Internship_Related_Person_Internships_InternshipsId",
                        column: x => x.InternshipsId,
                        principalSchema: "intern",
                        principalTable: "Internships",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Internship_Related_Student",
                schema: "intern",
                columns: table => new
                {
                    InternshipsId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    StudentsId = table.Column<Guid>(type: "uniqueidentifier", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Internship_Related_Student", x => new { x.InternshipsId, x.StudentsId });
                    table.ForeignKey(
                        name: "FK_Internship_Related_Student_InternshipStudents_StudentsId",
                        column: x => x.StudentsId,
                        principalSchema: "intern",
                        principalTable: "InternshipStudents",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Internship_Related_Student_Internships_InternshipsId",
                        column: x => x.InternshipsId,
                        principalSchema: "intern",
                        principalTable: "Internships",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Internship_Related_Teacher",
                schema: "intern",
                columns: table => new
                {
                    InternshipsId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    TeachersId = table.Column<Guid>(type: "uniqueidentifier", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Internship_Related_Teacher", x => new { x.InternshipsId, x.TeachersId });
                    table.ForeignKey(
                        name: "FK_Internship_Related_Teacher_InternshipTeachers_TeachersId",
                        column: x => x.TeachersId,
                        principalSchema: "intern",
                        principalTable: "InternshipTeachers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Internship_Related_Teacher_Internships_InternshipsId",
                        column: x => x.InternshipsId,
                        principalSchema: "intern",
                        principalTable: "Internships",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "InternshipLinks",
                schema: "intern",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Url = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    InternshipId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    CreationAuthor = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    CreationDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdateAuthor = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    UpdatedDate = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_InternshipLinks", x => x.Id);
                    table.ForeignKey(
                        name: "FK_InternshipLinks_Internships_InternshipId",
                        column: x => x.InternshipId,
                        principalSchema: "intern",
                        principalTable: "Internships",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "InternshipTasks",
                schema: "intern",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    IsCompleted = table.Column<bool>(type: "bit", nullable: false),
                    IsReported = table.Column<bool>(type: "bit", nullable: false),
                    Summary = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    TeacherSummary = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    State = table.Column<int>(type: "int", nullable: false),
                    EndsOn = table.Column<DateTime>(type: "datetime2", nullable: false),
                    InternshipId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    CreationAuthor = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    CreationDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdateAuthor = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    UpdatedDate = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_InternshipTasks", x => x.Id);
                    table.ForeignKey(
                        name: "FK_InternshipTasks_Internships_InternshipId",
                        column: x => x.InternshipId,
                        principalSchema: "intern",
                        principalTable: "Internships",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "InternshipTaskFiles",
                schema: "intern",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    FileName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    FileNameWithoutExt = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Extension = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    ContentType = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    TaskId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    CreationAuthor = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    CreationDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdateAuthor = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    UpdatedDate = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_InternshipTaskFiles", x => x.Id);
                    table.ForeignKey(
                        name: "FK_InternshipTaskFiles_InternshipTasks_TaskId",
                        column: x => x.TaskId,
                        principalSchema: "intern",
                        principalTable: "InternshipTasks",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "InternshipTaskLinks",
                schema: "intern",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Url = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    TaskId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    CreationAuthor = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    CreationDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdateAuthor = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    UpdatedDate = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_InternshipTaskLinks", x => x.Id);
                    table.ForeignKey(
                        name: "FK_InternshipTaskLinks_InternshipTasks_TaskId",
                        column: x => x.TaskId,
                        principalSchema: "intern",
                        principalTable: "InternshipTasks",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Internship_Related_Person_InternshipsId",
                schema: "intern",
                table: "Internship_Related_Person",
                column: "InternshipsId");

            migrationBuilder.CreateIndex(
                name: "IX_Internship_Related_Student_StudentsId",
                schema: "intern",
                table: "Internship_Related_Student",
                column: "StudentsId");

            migrationBuilder.CreateIndex(
                name: "IX_Internship_Related_Teacher_TeachersId",
                schema: "intern",
                table: "Internship_Related_Teacher",
                column: "TeachersId");

            migrationBuilder.CreateIndex(
                name: "IX_InternshipCompanyRelatives_CompanyName",
                schema: "intern",
                table: "InternshipCompanyRelatives",
                column: "CompanyName");

            migrationBuilder.CreateIndex(
                name: "IX_InternshipCompanyRelatives_LastName",
                schema: "intern",
                table: "InternshipCompanyRelatives",
                column: "LastName");

            migrationBuilder.CreateIndex(
                name: "IX_InternshipCompanyRelatives_Name",
                schema: "intern",
                table: "InternshipCompanyRelatives",
                column: "Name");

            migrationBuilder.CreateIndex(
                name: "IX_InternshipLinks_InternshipId",
                schema: "intern",
                table: "InternshipLinks",
                column: "InternshipId");

            migrationBuilder.CreateIndex(
                name: "IX_Internships_CategoryId",
                schema: "intern",
                table: "Internships",
                column: "CategoryId");

            migrationBuilder.CreateIndex(
                name: "IX_Internships_Name",
                schema: "intern",
                table: "Internships",
                column: "Name");

            migrationBuilder.CreateIndex(
                name: "IX_InternshipStudents_LastName",
                schema: "intern",
                table: "InternshipStudents",
                column: "LastName");

            migrationBuilder.CreateIndex(
                name: "IX_InternshipStudents_Name",
                schema: "intern",
                table: "InternshipStudents",
                column: "Name");

            migrationBuilder.CreateIndex(
                name: "IX_InternshipTaskFiles_TaskId",
                schema: "intern",
                table: "InternshipTaskFiles",
                column: "TaskId");

            migrationBuilder.CreateIndex(
                name: "IX_InternshipTaskLinks_TaskId",
                schema: "intern",
                table: "InternshipTaskLinks",
                column: "TaskId");

            migrationBuilder.CreateIndex(
                name: "IX_InternshipTasks_InternshipId",
                schema: "intern",
                table: "InternshipTasks",
                column: "InternshipId");

            migrationBuilder.CreateIndex(
                name: "IX_InternshipTeachers_LastName",
                schema: "intern",
                table: "InternshipTeachers",
                column: "LastName");

            migrationBuilder.CreateIndex(
                name: "IX_InternshipTeachers_Name",
                schema: "intern",
                table: "InternshipTeachers",
                column: "Name");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Internship_Related_Person",
                schema: "intern");

            migrationBuilder.DropTable(
                name: "Internship_Related_Student",
                schema: "intern");

            migrationBuilder.DropTable(
                name: "Internship_Related_Teacher",
                schema: "intern");

            migrationBuilder.DropTable(
                name: "InternshipLinks",
                schema: "intern");

            migrationBuilder.DropTable(
                name: "InternshipTaskFiles",
                schema: "intern");

            migrationBuilder.DropTable(
                name: "InternshipTaskLinks",
                schema: "intern");

            migrationBuilder.DropTable(
                name: "InternshipCompanyRelatives",
                schema: "intern");

            migrationBuilder.DropTable(
                name: "InternshipStudents",
                schema: "intern");

            migrationBuilder.DropTable(
                name: "InternshipTeachers",
                schema: "intern");

            migrationBuilder.DropTable(
                name: "InternshipTasks",
                schema: "intern");

            migrationBuilder.DropTable(
                name: "Internships",
                schema: "intern");

            migrationBuilder.DropTable(
                name: "InternshipCategories",
                schema: "intern");
        }
    }
}
