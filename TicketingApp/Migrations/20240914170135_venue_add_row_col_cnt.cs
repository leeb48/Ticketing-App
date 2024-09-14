using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TicketingApp.Migrations
{
    /// <inheritdoc />
    public partial class venue_add_row_col_cnt : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "ColumnCount",
                schema: "ticketing_app_schema",
                table: "Venues",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "RowCount",
                schema: "ticketing_app_schema",
                table: "Venues",
                type: "integer",
                nullable: false,
                defaultValue: 0);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ColumnCount",
                schema: "ticketing_app_schema",
                table: "Venues");

            migrationBuilder.DropColumn(
                name: "RowCount",
                schema: "ticketing_app_schema",
                table: "Venues");
        }
    }
}
