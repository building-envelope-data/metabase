using System.Text.Json;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Metabase.Migrations
{
    /// <inheritdoc />
    public partial class AddExtrasToComponentAndInstitution : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<JsonDocument>(
                name: "Extras",
                schema: "metabase",
                table: "institution",
                type: "jsonb",
                nullable: true);

            migrationBuilder.AddColumn<JsonDocument>(
                name: "Extras",
                schema: "metabase",
                table: "component",
                type: "jsonb",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Extras",
                schema: "metabase",
                table: "institution");

            migrationBuilder.DropColumn(
                name: "Extras",
                schema: "metabase",
                table: "component");
        }
    }
}