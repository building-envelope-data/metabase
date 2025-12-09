using System;
using System.Text.Json;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace Metabase.Migrations
{
    /// <inheritdoc />
    public partial class AddParametersAndSourcesToMethods : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Standard_Year",
                schema: "metabase",
                table: "method",
                newName: "Reference_Standard_Year");

            migrationBuilder.RenameColumn(
                name: "Standard_Title",
                schema: "metabase",
                table: "method",
                newName: "Reference_Standard_Title");

            migrationBuilder.RenameColumn(
                name: "Standard_Standardizers",
                schema: "metabase",
                table: "method",
                newName: "Reference_Standard_Standardizers");

            migrationBuilder.RenameColumn(
                name: "Standard_Section",
                schema: "metabase",
                table: "method",
                newName: "Reference_Standard_Section");

            migrationBuilder.RenameColumn(
                name: "Standard_Numeration_Suffix",
                schema: "metabase",
                table: "method",
                newName: "Reference_Standard_Numeration_Suffix");

            migrationBuilder.RenameColumn(
                name: "Standard_Numeration_Prefix",
                schema: "metabase",
                table: "method",
                newName: "Reference_Standard_Numeration_Prefix");

            migrationBuilder.RenameColumn(
                name: "Standard_Numeration_MainNumber",
                schema: "metabase",
                table: "method",
                newName: "Reference_Standard_Numeration_MainNumber");

            migrationBuilder.RenameColumn(
                name: "Standard_Locator",
                schema: "metabase",
                table: "method",
                newName: "Reference_Standard_Locator");

            migrationBuilder.RenameColumn(
                name: "Standard_Abstract",
                schema: "metabase",
                table: "method",
                newName: "Reference_Standard_Abstract");

            migrationBuilder.RenameColumn(
                name: "Publication_WebAddress",
                schema: "metabase",
                table: "method",
                newName: "Reference_Publication_WebAddress");

            migrationBuilder.RenameColumn(
                name: "Publication_Urn",
                schema: "metabase",
                table: "method",
                newName: "Reference_Publication_Urn");

            migrationBuilder.RenameColumn(
                name: "Publication_Title",
                schema: "metabase",
                table: "method",
                newName: "Reference_Publication_Title");

            migrationBuilder.RenameColumn(
                name: "Publication_Section",
                schema: "metabase",
                table: "method",
                newName: "Reference_Publication_Section");

            migrationBuilder.RenameColumn(
                name: "Publication_Doi",
                schema: "metabase",
                table: "method",
                newName: "Reference_Publication_Doi");

            migrationBuilder.RenameColumn(
                name: "Publication_Authors",
                schema: "metabase",
                table: "method",
                newName: "Reference_Publication_Authors");

            migrationBuilder.RenameColumn(
                name: "Publication_ArXiv",
                schema: "metabase",
                table: "method",
                newName: "Reference_Publication_ArXiv");

            migrationBuilder.RenameColumn(
                name: "Publication_Abstract",
                schema: "metabase",
                table: "method",
                newName: "Reference_Publication_Abstract");

            migrationBuilder.RenameColumn(
                name: "Standard_Year",
                schema: "metabase",
                table: "data_format",
                newName: "Reference_Standard_Year");

            migrationBuilder.RenameColumn(
                name: "Standard_Title",
                schema: "metabase",
                table: "data_format",
                newName: "Reference_Standard_Title");

            migrationBuilder.RenameColumn(
                name: "Standard_Standardizers",
                schema: "metabase",
                table: "data_format",
                newName: "Reference_Standard_Standardizers");

            migrationBuilder.RenameColumn(
                name: "Standard_Section",
                schema: "metabase",
                table: "data_format",
                newName: "Reference_Standard_Section");

            migrationBuilder.RenameColumn(
                name: "Standard_Numeration_Suffix",
                schema: "metabase",
                table: "data_format",
                newName: "Reference_Standard_Numeration_Suffix");

            migrationBuilder.RenameColumn(
                name: "Standard_Numeration_Prefix",
                schema: "metabase",
                table: "data_format",
                newName: "Reference_Standard_Numeration_Prefix");

            migrationBuilder.RenameColumn(
                name: "Standard_Numeration_MainNumber",
                schema: "metabase",
                table: "data_format",
                newName: "Reference_Standard_Numeration_MainNumber");

            migrationBuilder.RenameColumn(
                name: "Standard_Locator",
                schema: "metabase",
                table: "data_format",
                newName: "Reference_Standard_Locator");

            migrationBuilder.RenameColumn(
                name: "Standard_Abstract",
                schema: "metabase",
                table: "data_format",
                newName: "Reference_Standard_Abstract");

            migrationBuilder.RenameColumn(
                name: "Publication_WebAddress",
                schema: "metabase",
                table: "data_format",
                newName: "Reference_Publication_WebAddress");

            migrationBuilder.RenameColumn(
                name: "Publication_Urn",
                schema: "metabase",
                table: "data_format",
                newName: "Reference_Publication_Urn");

            migrationBuilder.RenameColumn(
                name: "Publication_Title",
                schema: "metabase",
                table: "data_format",
                newName: "Reference_Publication_Title");

            migrationBuilder.RenameColumn(
                name: "Publication_Section",
                schema: "metabase",
                table: "data_format",
                newName: "Reference_Publication_Section");

            migrationBuilder.RenameColumn(
                name: "Publication_Doi",
                schema: "metabase",
                table: "data_format",
                newName: "Reference_Publication_Doi");

            migrationBuilder.RenameColumn(
                name: "Publication_Authors",
                schema: "metabase",
                table: "data_format",
                newName: "Reference_Publication_Authors");

            migrationBuilder.RenameColumn(
                name: "Publication_ArXiv",
                schema: "metabase",
                table: "data_format",
                newName: "Reference_Publication_ArXiv");

            migrationBuilder.RenameColumn(
                name: "Publication_Abstract",
                schema: "metabase",
                table: "data_format",
                newName: "Reference_Publication_Abstract");

            migrationBuilder.AddColumn<bool>(
                name: "Reference_Exists",
                schema: "metabase",
                table: "method",
                type: "boolean",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "Reference_Exists",
                schema: "metabase",
                table: "data_format",
                type: "boolean",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "MethodParameter",
                schema: "metabase",
                columns: table => new
                {
                    MethodId = table.Column<Guid>(type: "uuid", nullable: false),
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Name = table.Column<string>(type: "text", nullable: false),
                    Type = table.Column<JsonElement>(type: "jsonb", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MethodParameter", x => new { x.MethodId, x.Id });
                    table.ForeignKey(
                        name: "FK_MethodParameter_method_MethodId",
                        column: x => x.MethodId,
                        principalSchema: "metabase",
                        principalTable: "method",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "MethodSource",
                schema: "metabase",
                columns: table => new
                {
                    MethodId = table.Column<Guid>(type: "uuid", nullable: false),
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Name = table.Column<string>(type: "text", nullable: false),
                    Description = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MethodSource", x => new { x.MethodId, x.Id });
                    table.ForeignKey(
                        name: "FK_MethodSource_method_MethodId",
                        column: x => x.MethodId,
                        principalSchema: "metabase",
                        principalTable: "method",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "MethodParameter",
                schema: "metabase");

            migrationBuilder.DropTable(
                name: "MethodSource",
                schema: "metabase");

            migrationBuilder.DropColumn(
                name: "Reference_Exists",
                schema: "metabase",
                table: "method");

            migrationBuilder.DropColumn(
                name: "Reference_Exists",
                schema: "metabase",
                table: "data_format");

            migrationBuilder.RenameColumn(
                name: "Reference_Standard_Year",
                schema: "metabase",
                table: "method",
                newName: "Standard_Year");

            migrationBuilder.RenameColumn(
                name: "Reference_Standard_Title",
                schema: "metabase",
                table: "method",
                newName: "Standard_Title");

            migrationBuilder.RenameColumn(
                name: "Reference_Standard_Standardizers",
                schema: "metabase",
                table: "method",
                newName: "Standard_Standardizers");

            migrationBuilder.RenameColumn(
                name: "Reference_Standard_Section",
                schema: "metabase",
                table: "method",
                newName: "Standard_Section");

            migrationBuilder.RenameColumn(
                name: "Reference_Standard_Numeration_Suffix",
                schema: "metabase",
                table: "method",
                newName: "Standard_Numeration_Suffix");

            migrationBuilder.RenameColumn(
                name: "Reference_Standard_Numeration_Prefix",
                schema: "metabase",
                table: "method",
                newName: "Standard_Numeration_Prefix");

            migrationBuilder.RenameColumn(
                name: "Reference_Standard_Numeration_MainNumber",
                schema: "metabase",
                table: "method",
                newName: "Standard_Numeration_MainNumber");

            migrationBuilder.RenameColumn(
                name: "Reference_Standard_Locator",
                schema: "metabase",
                table: "method",
                newName: "Standard_Locator");

            migrationBuilder.RenameColumn(
                name: "Reference_Standard_Abstract",
                schema: "metabase",
                table: "method",
                newName: "Standard_Abstract");

            migrationBuilder.RenameColumn(
                name: "Reference_Publication_WebAddress",
                schema: "metabase",
                table: "method",
                newName: "Publication_WebAddress");

            migrationBuilder.RenameColumn(
                name: "Reference_Publication_Urn",
                schema: "metabase",
                table: "method",
                newName: "Publication_Urn");

            migrationBuilder.RenameColumn(
                name: "Reference_Publication_Title",
                schema: "metabase",
                table: "method",
                newName: "Publication_Title");

            migrationBuilder.RenameColumn(
                name: "Reference_Publication_Section",
                schema: "metabase",
                table: "method",
                newName: "Publication_Section");

            migrationBuilder.RenameColumn(
                name: "Reference_Publication_Doi",
                schema: "metabase",
                table: "method",
                newName: "Publication_Doi");

            migrationBuilder.RenameColumn(
                name: "Reference_Publication_Authors",
                schema: "metabase",
                table: "method",
                newName: "Publication_Authors");

            migrationBuilder.RenameColumn(
                name: "Reference_Publication_ArXiv",
                schema: "metabase",
                table: "method",
                newName: "Publication_ArXiv");

            migrationBuilder.RenameColumn(
                name: "Reference_Publication_Abstract",
                schema: "metabase",
                table: "method",
                newName: "Publication_Abstract");

            migrationBuilder.RenameColumn(
                name: "Reference_Standard_Year",
                schema: "metabase",
                table: "data_format",
                newName: "Standard_Year");

            migrationBuilder.RenameColumn(
                name: "Reference_Standard_Title",
                schema: "metabase",
                table: "data_format",
                newName: "Standard_Title");

            migrationBuilder.RenameColumn(
                name: "Reference_Standard_Standardizers",
                schema: "metabase",
                table: "data_format",
                newName: "Standard_Standardizers");

            migrationBuilder.RenameColumn(
                name: "Reference_Standard_Section",
                schema: "metabase",
                table: "data_format",
                newName: "Standard_Section");

            migrationBuilder.RenameColumn(
                name: "Reference_Standard_Numeration_Suffix",
                schema: "metabase",
                table: "data_format",
                newName: "Standard_Numeration_Suffix");

            migrationBuilder.RenameColumn(
                name: "Reference_Standard_Numeration_Prefix",
                schema: "metabase",
                table: "data_format",
                newName: "Standard_Numeration_Prefix");

            migrationBuilder.RenameColumn(
                name: "Reference_Standard_Numeration_MainNumber",
                schema: "metabase",
                table: "data_format",
                newName: "Standard_Numeration_MainNumber");

            migrationBuilder.RenameColumn(
                name: "Reference_Standard_Locator",
                schema: "metabase",
                table: "data_format",
                newName: "Standard_Locator");

            migrationBuilder.RenameColumn(
                name: "Reference_Standard_Abstract",
                schema: "metabase",
                table: "data_format",
                newName: "Standard_Abstract");

            migrationBuilder.RenameColumn(
                name: "Reference_Publication_WebAddress",
                schema: "metabase",
                table: "data_format",
                newName: "Publication_WebAddress");

            migrationBuilder.RenameColumn(
                name: "Reference_Publication_Urn",
                schema: "metabase",
                table: "data_format",
                newName: "Publication_Urn");

            migrationBuilder.RenameColumn(
                name: "Reference_Publication_Title",
                schema: "metabase",
                table: "data_format",
                newName: "Publication_Title");

            migrationBuilder.RenameColumn(
                name: "Reference_Publication_Section",
                schema: "metabase",
                table: "data_format",
                newName: "Publication_Section");

            migrationBuilder.RenameColumn(
                name: "Reference_Publication_Doi",
                schema: "metabase",
                table: "data_format",
                newName: "Publication_Doi");

            migrationBuilder.RenameColumn(
                name: "Reference_Publication_Authors",
                schema: "metabase",
                table: "data_format",
                newName: "Publication_Authors");

            migrationBuilder.RenameColumn(
                name: "Reference_Publication_ArXiv",
                schema: "metabase",
                table: "data_format",
                newName: "Publication_ArXiv");

            migrationBuilder.RenameColumn(
                name: "Reference_Publication_Abstract",
                schema: "metabase",
                table: "data_format",
                newName: "Publication_Abstract");
        }
    }
}