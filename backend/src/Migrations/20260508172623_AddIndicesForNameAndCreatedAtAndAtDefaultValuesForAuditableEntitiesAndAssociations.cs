using System;
using System.Diagnostics.CodeAnalysis;
using Microsoft.EntityFrameworkCore.Migrations;
using NodaTime;

#nullable disable

namespace Metabase.Migrations
{
    /// <inheritdoc />
    public partial class AddIndicesForNameAndCreatedAtAndAtDefaultValuesForAuditableEntitiesAndAssociations : Migration
    {
        /// <inheritdoc />
        [SuppressMessage("Performance", "CA1861")]
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<DateTimeOffset>(
                name: "UpdatedAt",
                schema: "metabase",
                table: "OpenIddictScopes",
                type: "timestamp with time zone",
                nullable: false,
                defaultValueSql: "now()",
                oldClrType: typeof(Instant),
                oldType: "timestamp with time zone");

            migrationBuilder.AlterColumn<DateTimeOffset>(
                name: "CreatedAt",
                schema: "metabase",
                table: "OpenIddictScopes",
                type: "timestamp with time zone",
                nullable: false,
                defaultValueSql: "now()",
                oldClrType: typeof(Instant),
                oldType: "timestamp with time zone");

            migrationBuilder.CreateIndex(
                name: "IX_user_CreatedAt_Id",
                schema: "metabase",
                table: "user",
                columns: new[] { "CreatedAt", "Id" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_user_Name_Id",
                schema: "metabase",
                table: "user",
                columns: new[] { "Name", "Id" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_OpenIddictTokens_CreatedAt_Id",
                schema: "metabase",
                table: "OpenIddictTokens",
                columns: new[] { "CreatedAt", "Id" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_OpenIddictScopes_CreatedAt_Id",
                schema: "metabase",
                table: "OpenIddictScopes",
                columns: new[] { "CreatedAt", "Id" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_OpenIddictAuthorizations_CreatedAt_Id",
                schema: "metabase",
                table: "OpenIddictAuthorizations",
                columns: new[] { "CreatedAt", "Id" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_OpenIddictApplications_CreatedAt_Id",
                schema: "metabase",
                table: "OpenIddictApplications",
                columns: new[] { "CreatedAt", "Id" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_method_CreatedAt_Id",
                schema: "metabase",
                table: "method",
                columns: new[] { "CreatedAt", "Id" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_method_Name_Id",
                schema: "metabase",
                table: "method",
                columns: new[] { "Name", "Id" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_institution_CreatedAt_Id",
                schema: "metabase",
                table: "institution",
                columns: new[] { "CreatedAt", "Id" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_institution_Name_Id",
                schema: "metabase",
                table: "institution",
                columns: new[] { "Name", "Id" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_gnu_pg_fingerprint_CreatedAt_Id",
                schema: "metabase",
                table: "gnu_pg_fingerprint",
                columns: new[] { "CreatedAt", "Id" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_database_CreatedAt_Id",
                schema: "metabase",
                table: "database",
                columns: new[] { "CreatedAt", "Id" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_database_Name_Id",
                schema: "metabase",
                table: "database",
                columns: new[] { "Name", "Id" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_data_format_CreatedAt_Id",
                schema: "metabase",
                table: "data_format",
                columns: new[] { "CreatedAt", "Id" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_data_format_Name_Id",
                schema: "metabase",
                table: "data_format",
                columns: new[] { "Name", "Id" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_component_CreatedAt_Id",
                schema: "metabase",
                table: "component",
                columns: new[] { "CreatedAt", "Id" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_component_Name_Id",
                schema: "metabase",
                table: "component",
                columns: new[] { "Name", "Id" },
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_user_CreatedAt_Id",
                schema: "metabase",
                table: "user");

            migrationBuilder.DropIndex(
                name: "IX_user_Name_Id",
                schema: "metabase",
                table: "user");

            migrationBuilder.DropIndex(
                name: "IX_OpenIddictTokens_CreatedAt_Id",
                schema: "metabase",
                table: "OpenIddictTokens");

            migrationBuilder.DropIndex(
                name: "IX_OpenIddictScopes_CreatedAt_Id",
                schema: "metabase",
                table: "OpenIddictScopes");

            migrationBuilder.DropIndex(
                name: "IX_OpenIddictAuthorizations_CreatedAt_Id",
                schema: "metabase",
                table: "OpenIddictAuthorizations");

            migrationBuilder.DropIndex(
                name: "IX_OpenIddictApplications_CreatedAt_Id",
                schema: "metabase",
                table: "OpenIddictApplications");

            migrationBuilder.DropIndex(
                name: "IX_method_CreatedAt_Id",
                schema: "metabase",
                table: "method");

            migrationBuilder.DropIndex(
                name: "IX_method_Name_Id",
                schema: "metabase",
                table: "method");

            migrationBuilder.DropIndex(
                name: "IX_institution_CreatedAt_Id",
                schema: "metabase",
                table: "institution");

            migrationBuilder.DropIndex(
                name: "IX_institution_Name_Id",
                schema: "metabase",
                table: "institution");

            migrationBuilder.DropIndex(
                name: "IX_gnu_pg_fingerprint_CreatedAt_Id",
                schema: "metabase",
                table: "gnu_pg_fingerprint");

            migrationBuilder.DropIndex(
                name: "IX_database_CreatedAt_Id",
                schema: "metabase",
                table: "database");

            migrationBuilder.DropIndex(
                name: "IX_database_Name_Id",
                schema: "metabase",
                table: "database");

            migrationBuilder.DropIndex(
                name: "IX_data_format_CreatedAt_Id",
                schema: "metabase",
                table: "data_format");

            migrationBuilder.DropIndex(
                name: "IX_data_format_Name_Id",
                schema: "metabase",
                table: "data_format");

            migrationBuilder.DropIndex(
                name: "IX_component_CreatedAt_Id",
                schema: "metabase",
                table: "component");

            migrationBuilder.DropIndex(
                name: "IX_component_Name_Id",
                schema: "metabase",
                table: "component");

            migrationBuilder.AlterColumn<Instant>(
                name: "UpdatedAt",
                schema: "metabase",
                table: "OpenIddictScopes",
                type: "timestamp with time zone",
                nullable: false,
                oldClrType: typeof(DateTimeOffset),
                oldType: "timestamp with time zone",
                oldDefaultValueSql: "now()");

            migrationBuilder.AlterColumn<Instant>(
                name: "CreatedAt",
                schema: "metabase",
                table: "OpenIddictScopes",
                type: "timestamp with time zone",
                nullable: false,
                oldClrType: typeof(DateTimeOffset),
                oldType: "timestamp with time zone",
                oldDefaultValueSql: "now()");
        }
    }
}
