using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Metabase.Migrations
{
    /// <inheritdoc />
    public partial class MakeGnuPgFingerprintItsOwnEntity : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "DataSigningPermission",
                schema: "metabase",
                table: "institution_representative");

            migrationBuilder.DropColumn(
                name: "GnuPgKeyFingerprints",
                schema: "metabase",
                table: "institution_representative");

            migrationBuilder.AlterDatabase()
                .OldAnnotation("Npgsql:Enum:metabase.data_signing_permission", "allowed,forbidden,never");

            migrationBuilder.CreateTable(
                name: "gnu_pg_fingerprint",
                schema: "metabase",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false, defaultValueSql: "gen_random_uuid()"),
                    Fingerprint = table.Column<string>(type: "text", nullable: false),
                    CreationDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    RevocationDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    UserId = table.Column<Guid>(type: "uuid", nullable: false),
                    InstitutionId = table.Column<Guid>(type: "uuid", nullable: false),
                    xmin = table.Column<uint>(type: "xid", rowVersion: true, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_gnu_pg_fingerprint", x => x.Id);
                    table.ForeignKey(
                        name: "FK_gnu_pg_fingerprint_institution_InstitutionId",
                        column: x => x.InstitutionId,
                        principalSchema: "metabase",
                        principalTable: "institution",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_gnu_pg_fingerprint_user_UserId",
                        column: x => x.UserId,
                        principalSchema: "metabase",
                        principalTable: "user",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_gnu_pg_fingerprint_Fingerprint",
                schema: "metabase",
                table: "gnu_pg_fingerprint",
                column: "Fingerprint",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_gnu_pg_fingerprint_InstitutionId",
                schema: "metabase",
                table: "gnu_pg_fingerprint",
                column: "InstitutionId");

            migrationBuilder.CreateIndex(
                name: "IX_gnu_pg_fingerprint_UserId",
                schema: "metabase",
                table: "gnu_pg_fingerprint",
                column: "UserId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "gnu_pg_fingerprint",
                schema: "metabase");

            migrationBuilder.AlterDatabase()
                .Annotation("Npgsql:Enum:metabase.data_signing_permission", "allowed,forbidden,never");

            migrationBuilder.AddColumn<int>(
                name: "DataSigningPermission",
                schema: "metabase",
                table: "institution_representative",
                type: "metabase.data_signing_permission",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<string[]>(
                name: "GnuPgKeyFingerprints",
                schema: "metabase",
                table: "institution_representative",
                type: "text[]",
                nullable: false,
                defaultValue: Array.Empty<string>());
        }
    }
}