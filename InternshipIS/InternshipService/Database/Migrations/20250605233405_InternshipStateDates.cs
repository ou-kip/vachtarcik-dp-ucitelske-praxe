using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace InternshipService.Database.Migrations
{
    /// <inheritdoc />
    public partial class InternshipStateDates : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<DateTime>(
                name: "CanceledOn",
                schema: "intern",
                table: "Internships",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "PublishedOn",
                schema: "intern",
                table: "Internships",
                type: "datetime2",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "CanceledOn",
                schema: "intern",
                table: "Internships");

            migrationBuilder.DropColumn(
                name: "PublishedOn",
                schema: "intern",
                table: "Internships");
        }
    }
}
