using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Metabase.Migrations
{
    /// <inheritdoc />
    public partial class AddPrefixGnuPgToKeyFingerprints : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "KeyFingerprints",
                schema: "metabase",
                table: "institution_representative",
                newName: "GnuPgKeyFingerprints");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "GnuPgKeyFingerprints",
                schema: "metabase",
                table: "institution_representative",
                newName: "KeyFingerprints");
        }
    }
}
