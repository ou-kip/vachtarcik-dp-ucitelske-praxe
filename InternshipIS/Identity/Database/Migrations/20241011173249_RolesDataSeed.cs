using Core.Database.Utils;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Identity.Database.Migrations
{
    /// <inheritdoc />
    public partial class RolesDataSeed : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql(SqlExecuter.GetSql(nameof(RolesDataSeed)));
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {

        }
    }
}
