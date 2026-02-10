using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Metabase.Migrations
{
    /// <inheritdoc />
    public partial class AddManagerToComponent : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "Reference_Publication_Exists",
                schema: "metabase",
                table: "method",
                type: "boolean",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "Reference_Standard_Exists",
                schema: "metabase",
                table: "method",
                type: "boolean",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "Reference_Publication_Exists",
                schema: "metabase",
                table: "data_format",
                type: "boolean",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "Reference_Standard_Exists",
                schema: "metabase",
                table: "data_format",
                type: "boolean",
                nullable: true);

            migrationBuilder.AddColumn<Guid>(
                name: "ManagerId",
                schema: "metabase",
                table: "component",
                type: "uuid",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.AddColumn<bool>(
                name: "PrimeDirection_Reference_Publication_Exists",
                schema: "metabase",
                table: "component",
                type: "boolean",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "PrimeDirection_Reference_Standard_Exists",
                schema: "metabase",
                table: "component",
                type: "boolean",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "PrimeSurface_Reference_Publication_Exists",
                schema: "metabase",
                table: "component",
                type: "boolean",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "PrimeSurface_Reference_Standard_Exists",
                schema: "metabase",
                table: "component",
                type: "boolean",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "SwitchableLayers_Reference_Publication_Exists",
                schema: "metabase",
                table: "component",
                type: "boolean",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "SwitchableLayers_Reference_Standard_Exists",
                schema: "metabase",
                table: "component",
                type: "boolean",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_component_ManagerId",
                schema: "metabase",
                table: "component",
                column: "ManagerId");

            migrationBuilder.AddForeignKey(
                name: "FK_component_institution_ManagerId",
                schema: "metabase",
                table: "component",
                column: "ManagerId",
                principalSchema: "metabase",
                principalTable: "institution",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_component_institution_ManagerId",
                schema: "metabase",
                table: "component");

            migrationBuilder.DropIndex(
                name: "IX_component_ManagerId",
                schema: "metabase",
                table: "component");

            migrationBuilder.DropColumn(
                name: "Reference_Publication_Exists",
                schema: "metabase",
                table: "method");

            migrationBuilder.DropColumn(
                name: "Reference_Standard_Exists",
                schema: "metabase",
                table: "method");

            migrationBuilder.DropColumn(
                name: "Reference_Publication_Exists",
                schema: "metabase",
                table: "data_format");

            migrationBuilder.DropColumn(
                name: "Reference_Standard_Exists",
                schema: "metabase",
                table: "data_format");

            migrationBuilder.DropColumn(
                name: "ManagerId",
                schema: "metabase",
                table: "component");

            migrationBuilder.DropColumn(
                name: "PrimeDirection_Reference_Publication_Exists",
                schema: "metabase",
                table: "component");

            migrationBuilder.DropColumn(
                name: "PrimeDirection_Reference_Standard_Exists",
                schema: "metabase",
                table: "component");

            migrationBuilder.DropColumn(
                name: "PrimeSurface_Reference_Publication_Exists",
                schema: "metabase",
                table: "component");

            migrationBuilder.DropColumn(
                name: "PrimeSurface_Reference_Standard_Exists",
                schema: "metabase",
                table: "component");

            migrationBuilder.DropColumn(
                name: "SwitchableLayers_Reference_Publication_Exists",
                schema: "metabase",
                table: "component");

            migrationBuilder.DropColumn(
                name: "SwitchableLayers_Reference_Standard_Exists",
                schema: "metabase",
                table: "component");
        }
    }
}
