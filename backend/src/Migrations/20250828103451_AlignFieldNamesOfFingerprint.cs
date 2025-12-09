using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Metabase.Migrations
{
    /// <inheritdoc />
    public partial class AlignFieldNamesOfFingerprint : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "RevocationDate",
                schema: "metabase",
                table: "gnu_pg_fingerprint",
                newName: "RevokedAt");

            migrationBuilder.RenameColumn(
                name: "CreationDate",
                schema: "metabase",
                table: "gnu_pg_fingerprint",
                newName: "CreatedAt");

            migrationBuilder.AddColumn<DateTime>(
                name: "AllowedAt",
                schema: "metabase",
                table: "gnu_pg_fingerprint",
                type: "timestamp with time zone",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "AllowedAt",
                schema: "metabase",
                table: "gnu_pg_fingerprint");

            migrationBuilder.RenameColumn(
                name: "RevokedAt",
                schema: "metabase",
                table: "gnu_pg_fingerprint",
                newName: "RevocationDate");

            migrationBuilder.RenameColumn(
                name: "CreatedAt",
                schema: "metabase",
                table: "gnu_pg_fingerprint",
                newName: "CreationDate");
        }
    }
}