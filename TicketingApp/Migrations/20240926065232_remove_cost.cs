using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TicketingApp.Migrations
{
    /// <inheritdoc />
    public partial class remove_cost : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "TotalCost",
                schema: "ticketing_app_schema",
                table: "Bookings");

            migrationBuilder.RenameColumn(
                name: "status",
                schema: "ticketing_app_schema",
                table: "Bookings",
                newName: "Status");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Status",
                schema: "ticketing_app_schema",
                table: "Bookings",
                newName: "status");

            migrationBuilder.AddColumn<decimal>(
                name: "TotalCost",
                schema: "ticketing_app_schema",
                table: "Bookings",
                type: "numeric",
                nullable: false,
                defaultValue: 0m);
        }
    }
}
