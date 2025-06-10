using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Identity.Database.Migrations
{
    /// <inheritdoc />
    public partial class UserNameAndLastNameAdded : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "LastName",
                schema: "auth",
                table: "AspNetUsers",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "Name",
                schema: "auth",
                table: "AspNetUsers",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "LastName",
                schema: "auth",
                table: "AspNetUsers");

            migrationBuilder.DropColumn(
                name: "Name",
                schema: "auth",
                table: "AspNetUsers");
        }
    }
}
