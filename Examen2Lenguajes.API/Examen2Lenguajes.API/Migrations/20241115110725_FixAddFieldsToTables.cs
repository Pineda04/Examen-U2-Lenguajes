using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Examen2Lenguajes.API.Migrations
{
    /// <inheritdoc />
    public partial class FixAddFieldsToTables : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "total",
                schema: "dbo",
                table: "accounts");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<decimal>(
                name: "total",
                schema: "dbo",
                table: "accounts",
                type: "decimal(18,2)",
                nullable: false,
                defaultValue: 0m);
        }
    }
}
