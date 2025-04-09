using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Metabase.Migrations
{
    /// <inheritdoc />
    public partial class AddSigningPermissionAndFingerprint : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "DataSigningPermission",
                schema: "metabase",
                table: "institution_representative",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<string[]>(
                name: "KeyFingerprints",
                schema: "metabase",
                table: "institution_representative",
                type: "text[]",
                nullable: false,
                defaultValue: new string[0]);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "DataSigningPermission",
                schema: "metabase",
                table: "institution_representative");

            migrationBuilder.DropColumn(
                name: "KeyFingerprints",
                schema: "metabase",
                table: "institution_representative");
        }
    }
}
