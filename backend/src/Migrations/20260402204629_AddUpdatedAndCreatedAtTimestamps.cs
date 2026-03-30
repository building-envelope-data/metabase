using System;
using Microsoft.EntityFrameworkCore.Migrations;
using NodaTime;

#nullable disable

namespace Metabase.Migrations
{
    /// <inheritdoc />
    public partial class AddUpdatedAndCreatedAtTimestamps : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<Instant>(
                name: "CreatedAt",
                schema: "metabase",
                table: "user_method_developer",
                type: "timestamp with time zone",
                nullable: false,
                defaultValueSql: "now()");

            migrationBuilder.AddColumn<Instant>(
                name: "UpdatedAt",
                schema: "metabase",
                table: "user_method_developer",
                type: "timestamp with time zone",
                nullable: false,
                defaultValueSql: "now()");

            migrationBuilder.AddColumn<uint>(
                name: "xmin",
                schema: "metabase",
                table: "user_method_developer",
                type: "xid",
                rowVersion: true,
                nullable: false,
                defaultValue: 0u);

            migrationBuilder.AlterColumn<Guid>(
                name: "Id",
                schema: "metabase",
                table: "user",
                type: "uuid",
                nullable: false,
                defaultValueSql: "gen_random_uuid()",
                oldClrType: typeof(Guid),
                oldType: "uuid");

            migrationBuilder.AddColumn<Instant>(
                name: "CreatedAt",
                schema: "metabase",
                table: "user",
                type: "timestamp with time zone",
                nullable: false,
                defaultValueSql: "now()");

            migrationBuilder.AddColumn<Instant>(
                name: "UpdatedAt",
                schema: "metabase",
                table: "user",
                type: "timestamp with time zone",
                nullable: false,
                defaultValueSql: "now()");

            migrationBuilder.AlterColumn<Guid>(
                name: "Id",
                schema: "metabase",
                table: "OpenIddictTokens",
                type: "uuid",
                nullable: false,
                defaultValueSql: "gen_random_uuid()",
                oldClrType: typeof(Guid),
                oldType: "uuid");

            migrationBuilder.AddColumn<Instant>(
                name: "CreatedAt",
                schema: "metabase",
                table: "OpenIddictTokens",
                type: "timestamp with time zone",
                nullable: false,
                defaultValueSql: "now()");

            migrationBuilder.AddColumn<Instant>(
                name: "UpdatedAt",
                schema: "metabase",
                table: "OpenIddictTokens",
                type: "timestamp with time zone",
                nullable: false,
                defaultValueSql: "now()");

            migrationBuilder.AlterColumn<Guid>(
                name: "Id",
                schema: "metabase",
                table: "OpenIddictScopes",
                type: "uuid",
                nullable: false,
                defaultValueSql: "gen_random_uuid()",
                oldClrType: typeof(Guid),
                oldType: "uuid");

            migrationBuilder.AddColumn<Instant>(
                name: "CreatedAt",
                schema: "metabase",
                table: "OpenIddictScopes",
                type: "timestamp with time zone",
                nullable: false,
                defaultValue: NodaTime.Instant.FromUnixTimeTicks(0L));

            migrationBuilder.AddColumn<Instant>(
                name: "UpdatedAt",
                schema: "metabase",
                table: "OpenIddictScopes",
                type: "timestamp with time zone",
                nullable: false,
                defaultValue: NodaTime.Instant.FromUnixTimeTicks(0L));

            migrationBuilder.AlterColumn<Guid>(
                name: "Id",
                schema: "metabase",
                table: "OpenIddictAuthorizations",
                type: "uuid",
                nullable: false,
                defaultValueSql: "gen_random_uuid()",
                oldClrType: typeof(Guid),
                oldType: "uuid");

            migrationBuilder.AddColumn<Instant>(
                name: "CreatedAt",
                schema: "metabase",
                table: "OpenIddictAuthorizations",
                type: "timestamp with time zone",
                nullable: false,
                defaultValueSql: "now()");

            migrationBuilder.AddColumn<Instant>(
                name: "UpdatedAt",
                schema: "metabase",
                table: "OpenIddictAuthorizations",
                type: "timestamp with time zone",
                nullable: false,
                defaultValueSql: "now()");

            migrationBuilder.AlterColumn<Guid>(
                name: "Id",
                schema: "metabase",
                table: "OpenIddictApplications",
                type: "uuid",
                nullable: false,
                defaultValueSql: "gen_random_uuid()",
                oldClrType: typeof(Guid),
                oldType: "uuid");

            migrationBuilder.AddColumn<Instant>(
                name: "CreatedAt",
                schema: "metabase",
                table: "OpenIddictApplications",
                type: "timestamp with time zone",
                nullable: false,
                defaultValueSql: "now()");

            migrationBuilder.AddColumn<Instant>(
                name: "UpdatedAt",
                schema: "metabase",
                table: "OpenIddictApplications",
                type: "timestamp with time zone",
                nullable: false,
                defaultValueSql: "now()");

            migrationBuilder.AddColumn<Instant>(
                name: "CreatedAt",
                schema: "metabase",
                table: "method",
                type: "timestamp with time zone",
                nullable: false,
                defaultValueSql: "now()");

            migrationBuilder.AddColumn<Instant>(
                name: "UpdatedAt",
                schema: "metabase",
                table: "method",
                type: "timestamp with time zone",
                nullable: false,
                defaultValueSql: "now()");

            migrationBuilder.AddColumn<Instant>(
                name: "CreatedAt",
                schema: "metabase",
                table: "institution_representative",
                type: "timestamp with time zone",
                nullable: false,
                defaultValueSql: "now()");

            migrationBuilder.AddColumn<Instant>(
                name: "UpdatedAt",
                schema: "metabase",
                table: "institution_representative",
                type: "timestamp with time zone",
                nullable: false,
                defaultValueSql: "now()");

            migrationBuilder.AddColumn<uint>(
                name: "xmin",
                schema: "metabase",
                table: "institution_representative",
                type: "xid",
                rowVersion: true,
                nullable: false,
                defaultValue: 0u);

            migrationBuilder.AddColumn<Instant>(
                name: "CreatedAt",
                schema: "metabase",
                table: "institution_method_developer",
                type: "timestamp with time zone",
                nullable: false,
                defaultValueSql: "now()");

            migrationBuilder.AddColumn<Instant>(
                name: "UpdatedAt",
                schema: "metabase",
                table: "institution_method_developer",
                type: "timestamp with time zone",
                nullable: false,
                defaultValueSql: "now()");

            migrationBuilder.AddColumn<uint>(
                name: "xmin",
                schema: "metabase",
                table: "institution_method_developer",
                type: "xid",
                rowVersion: true,
                nullable: false,
                defaultValue: 0u);

            migrationBuilder.AddColumn<Instant>(
                name: "CreatedAt",
                schema: "metabase",
                table: "institution",
                type: "timestamp with time zone",
                nullable: false,
                defaultValueSql: "now()");

            migrationBuilder.AddColumn<Instant>(
                name: "UpdatedAt",
                schema: "metabase",
                table: "institution",
                type: "timestamp with time zone",
                nullable: false,
                defaultValueSql: "now()");

            migrationBuilder.AlterColumn<Instant>(
                name: "CreatedAt",
                schema: "metabase",
                table: "gnu_pg_fingerprint",
                type: "timestamp with time zone",
                nullable: false,
                defaultValueSql: "now()",
                oldClrType: typeof(OffsetDateTime),
                oldType: "timestamp with time zone");

            migrationBuilder.AddColumn<Instant>(
                name: "UpdatedAt",
                schema: "metabase",
                table: "gnu_pg_fingerprint",
                type: "timestamp with time zone",
                nullable: false,
                defaultValueSql: "now()");

            migrationBuilder.AddColumn<Instant>(
                name: "CreatedAt",
                schema: "metabase",
                table: "database",
                type: "timestamp with time zone",
                nullable: false,
                defaultValueSql: "now()");

            migrationBuilder.AddColumn<Instant>(
                name: "UpdatedAt",
                schema: "metabase",
                table: "database",
                type: "timestamp with time zone",
                nullable: false,
                defaultValueSql: "now()");

            migrationBuilder.AddColumn<Instant>(
                name: "CreatedAt",
                schema: "metabase",
                table: "data_format",
                type: "timestamp with time zone",
                nullable: false,
                defaultValueSql: "now()");

            migrationBuilder.AddColumn<Instant>(
                name: "UpdatedAt",
                schema: "metabase",
                table: "data_format",
                type: "timestamp with time zone",
                nullable: false,
                defaultValueSql: "now()");

            migrationBuilder.AddColumn<Instant>(
                name: "CreatedAt",
                schema: "metabase",
                table: "component_variant",
                type: "timestamp with time zone",
                nullable: false,
                defaultValueSql: "now()");

            migrationBuilder.AddColumn<Instant>(
                name: "UpdatedAt",
                schema: "metabase",
                table: "component_variant",
                type: "timestamp with time zone",
                nullable: false,
                defaultValueSql: "now()");

            migrationBuilder.AddColumn<uint>(
                name: "xmin",
                schema: "metabase",
                table: "component_variant",
                type: "xid",
                rowVersion: true,
                nullable: false,
                defaultValue: 0u);

            migrationBuilder.AddColumn<Instant>(
                name: "CreatedAt",
                schema: "metabase",
                table: "component_manufacturer",
                type: "timestamp with time zone",
                nullable: false,
                defaultValueSql: "now()");

            migrationBuilder.AddColumn<Instant>(
                name: "UpdatedAt",
                schema: "metabase",
                table: "component_manufacturer",
                type: "timestamp with time zone",
                nullable: false,
                defaultValueSql: "now()");

            migrationBuilder.AddColumn<uint>(
                name: "xmin",
                schema: "metabase",
                table: "component_manufacturer",
                type: "xid",
                rowVersion: true,
                nullable: false,
                defaultValue: 0u);

            migrationBuilder.AddColumn<Instant>(
                name: "CreatedAt",
                schema: "metabase",
                table: "component_concretization_and_generalization",
                type: "timestamp with time zone",
                nullable: false,
                defaultValueSql: "now()");

            migrationBuilder.AddColumn<Instant>(
                name: "UpdatedAt",
                schema: "metabase",
                table: "component_concretization_and_generalization",
                type: "timestamp with time zone",
                nullable: false,
                defaultValueSql: "now()");

            migrationBuilder.AddColumn<uint>(
                name: "xmin",
                schema: "metabase",
                table: "component_concretization_and_generalization",
                type: "xid",
                rowVersion: true,
                nullable: false,
                defaultValue: 0u);

            migrationBuilder.AddColumn<Instant>(
                name: "CreatedAt",
                schema: "metabase",
                table: "component_assembly",
                type: "timestamp with time zone",
                nullable: false,
                defaultValueSql: "now()");

            migrationBuilder.AddColumn<Instant>(
                name: "UpdatedAt",
                schema: "metabase",
                table: "component_assembly",
                type: "timestamp with time zone",
                nullable: false,
                defaultValueSql: "now()");

            migrationBuilder.AddColumn<uint>(
                name: "xmin",
                schema: "metabase",
                table: "component_assembly",
                type: "xid",
                rowVersion: true,
                nullable: false,
                defaultValue: 0u);

            migrationBuilder.AddColumn<Instant>(
                name: "CreatedAt",
                schema: "metabase",
                table: "component",
                type: "timestamp with time zone",
                nullable: false,
                defaultValueSql: "now()");

            migrationBuilder.AddColumn<Instant>(
                name: "UpdatedAt",
                schema: "metabase",
                table: "component",
                type: "timestamp with time zone",
                nullable: false,
                defaultValueSql: "now()");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "CreatedAt",
                schema: "metabase",
                table: "user_method_developer");

            migrationBuilder.DropColumn(
                name: "UpdatedAt",
                schema: "metabase",
                table: "user_method_developer");

            migrationBuilder.DropColumn(
                name: "xmin",
                schema: "metabase",
                table: "user_method_developer");

            migrationBuilder.DropColumn(
                name: "CreatedAt",
                schema: "metabase",
                table: "user");

            migrationBuilder.DropColumn(
                name: "UpdatedAt",
                schema: "metabase",
                table: "user");

            migrationBuilder.DropColumn(
                name: "CreatedAt",
                schema: "metabase",
                table: "OpenIddictTokens");

            migrationBuilder.DropColumn(
                name: "UpdatedAt",
                schema: "metabase",
                table: "OpenIddictTokens");

            migrationBuilder.DropColumn(
                name: "CreatedAt",
                schema: "metabase",
                table: "OpenIddictScopes");

            migrationBuilder.DropColumn(
                name: "UpdatedAt",
                schema: "metabase",
                table: "OpenIddictScopes");

            migrationBuilder.DropColumn(
                name: "CreatedAt",
                schema: "metabase",
                table: "OpenIddictAuthorizations");

            migrationBuilder.DropColumn(
                name: "UpdatedAt",
                schema: "metabase",
                table: "OpenIddictAuthorizations");

            migrationBuilder.DropColumn(
                name: "CreatedAt",
                schema: "metabase",
                table: "OpenIddictApplications");

            migrationBuilder.DropColumn(
                name: "UpdatedAt",
                schema: "metabase",
                table: "OpenIddictApplications");

            migrationBuilder.DropColumn(
                name: "CreatedAt",
                schema: "metabase",
                table: "method");

            migrationBuilder.DropColumn(
                name: "UpdatedAt",
                schema: "metabase",
                table: "method");

            migrationBuilder.DropColumn(
                name: "CreatedAt",
                schema: "metabase",
                table: "institution_representative");

            migrationBuilder.DropColumn(
                name: "UpdatedAt",
                schema: "metabase",
                table: "institution_representative");

            migrationBuilder.DropColumn(
                name: "xmin",
                schema: "metabase",
                table: "institution_representative");

            migrationBuilder.DropColumn(
                name: "CreatedAt",
                schema: "metabase",
                table: "institution_method_developer");

            migrationBuilder.DropColumn(
                name: "UpdatedAt",
                schema: "metabase",
                table: "institution_method_developer");

            migrationBuilder.DropColumn(
                name: "xmin",
                schema: "metabase",
                table: "institution_method_developer");

            migrationBuilder.DropColumn(
                name: "CreatedAt",
                schema: "metabase",
                table: "institution");

            migrationBuilder.DropColumn(
                name: "UpdatedAt",
                schema: "metabase",
                table: "institution");

            migrationBuilder.DropColumn(
                name: "UpdatedAt",
                schema: "metabase",
                table: "gnu_pg_fingerprint");

            migrationBuilder.DropColumn(
                name: "CreatedAt",
                schema: "metabase",
                table: "database");

            migrationBuilder.DropColumn(
                name: "UpdatedAt",
                schema: "metabase",
                table: "database");

            migrationBuilder.DropColumn(
                name: "CreatedAt",
                schema: "metabase",
                table: "data_format");

            migrationBuilder.DropColumn(
                name: "UpdatedAt",
                schema: "metabase",
                table: "data_format");

            migrationBuilder.DropColumn(
                name: "CreatedAt",
                schema: "metabase",
                table: "component_variant");

            migrationBuilder.DropColumn(
                name: "UpdatedAt",
                schema: "metabase",
                table: "component_variant");

            migrationBuilder.DropColumn(
                name: "xmin",
                schema: "metabase",
                table: "component_variant");

            migrationBuilder.DropColumn(
                name: "CreatedAt",
                schema: "metabase",
                table: "component_manufacturer");

            migrationBuilder.DropColumn(
                name: "UpdatedAt",
                schema: "metabase",
                table: "component_manufacturer");

            migrationBuilder.DropColumn(
                name: "xmin",
                schema: "metabase",
                table: "component_manufacturer");

            migrationBuilder.DropColumn(
                name: "CreatedAt",
                schema: "metabase",
                table: "component_concretization_and_generalization");

            migrationBuilder.DropColumn(
                name: "UpdatedAt",
                schema: "metabase",
                table: "component_concretization_and_generalization");

            migrationBuilder.DropColumn(
                name: "xmin",
                schema: "metabase",
                table: "component_concretization_and_generalization");

            migrationBuilder.DropColumn(
                name: "CreatedAt",
                schema: "metabase",
                table: "component_assembly");

            migrationBuilder.DropColumn(
                name: "UpdatedAt",
                schema: "metabase",
                table: "component_assembly");

            migrationBuilder.DropColumn(
                name: "xmin",
                schema: "metabase",
                table: "component_assembly");

            migrationBuilder.DropColumn(
                name: "CreatedAt",
                schema: "metabase",
                table: "component");

            migrationBuilder.DropColumn(
                name: "UpdatedAt",
                schema: "metabase",
                table: "component");

            migrationBuilder.AlterColumn<Guid>(
                name: "Id",
                schema: "metabase",
                table: "user",
                type: "uuid",
                nullable: false,
                oldClrType: typeof(Guid),
                oldType: "uuid",
                oldDefaultValueSql: "gen_random_uuid()");

            migrationBuilder.AlterColumn<Guid>(
                name: "Id",
                schema: "metabase",
                table: "OpenIddictTokens",
                type: "uuid",
                nullable: false,
                oldClrType: typeof(Guid),
                oldType: "uuid",
                oldDefaultValueSql: "gen_random_uuid()");

            migrationBuilder.AlterColumn<Guid>(
                name: "Id",
                schema: "metabase",
                table: "OpenIddictScopes",
                type: "uuid",
                nullable: false,
                oldClrType: typeof(Guid),
                oldType: "uuid",
                oldDefaultValueSql: "gen_random_uuid()");

            migrationBuilder.AlterColumn<Guid>(
                name: "Id",
                schema: "metabase",
                table: "OpenIddictAuthorizations",
                type: "uuid",
                nullable: false,
                oldClrType: typeof(Guid),
                oldType: "uuid",
                oldDefaultValueSql: "gen_random_uuid()");

            migrationBuilder.AlterColumn<Guid>(
                name: "Id",
                schema: "metabase",
                table: "OpenIddictApplications",
                type: "uuid",
                nullable: false,
                oldClrType: typeof(Guid),
                oldType: "uuid",
                oldDefaultValueSql: "gen_random_uuid()");

            migrationBuilder.AlterColumn<OffsetDateTime>(
                name: "CreatedAt",
                schema: "metabase",
                table: "gnu_pg_fingerprint",
                type: "timestamp with time zone",
                nullable: false,
                oldClrType: typeof(Instant),
                oldType: "timestamp with time zone",
                oldDefaultValueSql: "now()");
        }
    }
}
