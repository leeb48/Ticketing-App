using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TicketingApp.Migrations
{
    /// <inheritdoc />
    public partial class event_artist_venue_req : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Events_Artists_ArtistId",
                schema: "ticketing_app_schema",
                table: "Events");

            migrationBuilder.AlterColumn<int>(
                name: "ArtistId",
                schema: "ticketing_app_schema",
                table: "Events",
                type: "integer",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "integer",
                oldNullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_Events_Artists_ArtistId",
                schema: "ticketing_app_schema",
                table: "Events",
                column: "ArtistId",
                principalSchema: "ticketing_app_schema",
                principalTable: "Artists",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Events_Artists_ArtistId",
                schema: "ticketing_app_schema",
                table: "Events");

            migrationBuilder.AlterColumn<int>(
                name: "ArtistId",
                schema: "ticketing_app_schema",
                table: "Events",
                type: "integer",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "integer");

            migrationBuilder.AddForeignKey(
                name: "FK_Events_Artists_ArtistId",
                schema: "ticketing_app_schema",
                table: "Events",
                column: "ArtistId",
                principalSchema: "ticketing_app_schema",
                principalTable: "Artists",
                principalColumn: "Id");
        }
    }
}
