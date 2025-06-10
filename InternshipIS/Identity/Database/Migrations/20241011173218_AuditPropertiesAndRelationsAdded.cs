using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Identity.Database.Migrations
{
    /// <inheritdoc />
    public partial class AuditPropertiesAndRelationsAdded : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "CreationAuthor",
                schema: "auth",
                table: "AspNetUsers",
                type: "nvarchar(50)",
                maxLength: 50,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<DateTime>(
                name: "CreationDate",
                schema: "auth",
                table: "AspNetUsers",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<Guid>(
                name: "RoleId",
                schema: "auth",
                table: "AspNetUsers",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.AddColumn<string>(
                name: "UpdateAuthor",
                schema: "auth",
                table: "AspNetUsers",
                type: "nvarchar(50)",
                maxLength: 50,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<DateTime>(
                name: "UpdatedDate",
                schema: "auth",
                table: "AspNetUsers",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<string>(
                name: "CreationAuthor",
                schema: "auth",
                table: "AspNetRoles",
                type: "nvarchar(50)",
                maxLength: 50,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<DateTime>(
                name: "CreationDate",
                schema: "auth",
                table: "AspNetRoles",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<string>(
                name: "UpdateAuthor",
                schema: "auth",
                table: "AspNetRoles",
                type: "nvarchar(50)",
                maxLength: 50,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<DateTime>(
                name: "UpdatedDate",
                schema: "auth",
                table: "AspNetRoles",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "CreationAuthor",
                schema: "auth",
                table: "AspNetUsers");

            migrationBuilder.DropColumn(
                name: "CreationDate",
                schema: "auth",
                table: "AspNetUsers");

            migrationBuilder.DropColumn(
                name: "RoleId",
                schema: "auth",
                table: "AspNetUsers");

            migrationBuilder.DropColumn(
                name: "UpdateAuthor",
                schema: "auth",
                table: "AspNetUsers");

            migrationBuilder.DropColumn(
                name: "UpdatedDate",
                schema: "auth",
                table: "AspNetUsers");

            migrationBuilder.DropColumn(
                name: "CreationAuthor",
                schema: "auth",
                table: "AspNetRoles");

            migrationBuilder.DropColumn(
                name: "CreationDate",
                schema: "auth",
                table: "AspNetRoles");

            migrationBuilder.DropColumn(
                name: "UpdateAuthor",
                schema: "auth",
                table: "AspNetRoles");

            migrationBuilder.DropColumn(
                name: "UpdatedDate",
                schema: "auth",
                table: "AspNetRoles");
        }
    }
}
