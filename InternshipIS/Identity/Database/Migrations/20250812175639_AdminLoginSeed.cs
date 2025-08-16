using Core.Database.Utils;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Identity.Database.Migrations
{
    /// <inheritdoc />
    public partial class AdminLoginSeed : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql(SqlExecuter.GetSql(nameof(AdminLoginSeed)));
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {

        }
    }
}
