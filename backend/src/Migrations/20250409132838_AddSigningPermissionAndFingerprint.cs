using System;
using Metabase.Enumerations;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Metabase.Migrations;

/// <inheritdoc />
public partial class AddSigningPermissionAndFingerprint : Migration
{
    public enum DataSigningPermission
    {
        NEVER,
        ALLOWED,
        FORBIDDEN
    }

    /// <inheritdoc />
    protected override void Up(MigrationBuilder migrationBuilder)
    {
        migrationBuilder.AlterDatabase()
            .Annotation("Npgsql:Enum:metabase.data_signing_permission", "allowed,forbidden,never");

        migrationBuilder.AddColumn<DataSigningPermission>(
            name: "DataSigningPermission",
            schema: "metabase",
            table: "institution_representative",
            type: "metabase.data_signing_permission",
            nullable: false,
            defaultValue: DataSigningPermission.NEVER);

        migrationBuilder.AddColumn<string[]>(
            name: "KeyFingerprints",
            schema: "metabase",
            table: "institution_representative",
            type: "text[]",
            nullable: false,
            defaultValue: Array.Empty<string>());
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

        migrationBuilder.AlterDatabase()
            .OldAnnotation("Npgsql:Enum:metabase.data_signing_permission", "allowed,forbidden,never");
    }
}