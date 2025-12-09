using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Metabase.Migrations
{
    /// <inheritdoc />
    public partial class UseForbidInsteadOfRevokeForGnuPgKeyFingerprint : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "RevokedAt",
                schema: "metabase",
                table: "gnu_pg_fingerprint",
                newName: "ForbiddenAt");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "ForbiddenAt",
                schema: "metabase",
                table: "gnu_pg_fingerprint",
                newName: "RevokedAt");
        }
    }
}