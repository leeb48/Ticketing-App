using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TicketingApp.Migrations
{
    /// <inheritdoc />
    public partial class removed_seat_section : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Section",
                schema: "ticketing_app_schema",
                table: "Seats");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Section",
                schema: "ticketing_app_schema",
                table: "Seats",
                type: "text",
                nullable: false,
                defaultValue: "");
        }
    }
}
