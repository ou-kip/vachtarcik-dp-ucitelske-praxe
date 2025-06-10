using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Identity.Database.Migrations
{
    /// <inheritdoc />
    public partial class StudentCodeMigration : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Code",
                schema: "auth",
                table: "AspNetUsers",
                type: "nvarchar(max)",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Code",
                schema: "auth",
                table: "AspNetUsers");
        }
    }
}
