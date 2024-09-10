using Microsoft.EntityFrameworkCore.Migrations;
using NpgsqlTypes;

#nullable disable

namespace TicketingApp.Migrations
{
    /// <inheritdoc />
    public partial class venue_search_vector : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<NpgsqlTsVector>(
                name: "SearchVector",
                schema: "ticketing_app_schema",
                table: "Venues",
                type: "tsvector",
                nullable: false)
                .Annotation("Npgsql:TsVectorConfig", "english")
                .Annotation("Npgsql:TsVectorProperties", new[] { "Name" });

            migrationBuilder.AlterColumn<NpgsqlTsVector>(
                name: "SearchVector",
                schema: "ticketing_app_schema",
                table: "Artists",
                type: "tsvector",
                nullable: false,
                oldClrType: typeof(NpgsqlTsVector),
                oldType: "tsvector",
                oldNullable: true)
                .Annotation("Npgsql:TsVectorConfig", "english")
                .Annotation("Npgsql:TsVectorProperties", new[] { "Name" })
                .OldAnnotation("Npgsql:TsVectorConfig", "english")
                .OldAnnotation("Npgsql:TsVectorProperties", new[] { "Name" });

            migrationBuilder.CreateIndex(
                name: "IX_Venues_SearchVector",
                schema: "ticketing_app_schema",
                table: "Venues",
                column: "SearchVector")
                .Annotation("Npgsql:IndexMethod", "GIN");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Venues_SearchVector",
                schema: "ticketing_app_schema",
                table: "Venues");

            migrationBuilder.DropColumn(
                name: "SearchVector",
                schema: "ticketing_app_schema",
                table: "Venues");

            migrationBuilder.AlterColumn<NpgsqlTsVector>(
                name: "SearchVector",
                schema: "ticketing_app_schema",
                table: "Artists",
                type: "tsvector",
                nullable: true,
                oldClrType: typeof(NpgsqlTsVector),
                oldType: "tsvector")
                .Annotation("Npgsql:TsVectorConfig", "english")
                .Annotation("Npgsql:TsVectorProperties", new[] { "Name" })
                .OldAnnotation("Npgsql:TsVectorConfig", "english")
                .OldAnnotation("Npgsql:TsVectorProperties", new[] { "Name" });
        }
    }
}
