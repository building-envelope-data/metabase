using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Metabase.Migrations
{
    /// <inheritdoc />
    public partial class MakeOpenIddictApplicationsBelongToExactlyOneInstitution : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "institution_open_id_connect_application",
                schema: "metabase");

            migrationBuilder.AddColumn<uint>(
                name: "xmin",
                schema: "metabase",
                table: "OpenIddictTokens",
                type: "xid",
                rowVersion: true,
                nullable: false,
                defaultValue: 0u);

            migrationBuilder.AddColumn<uint>(
                name: "xmin",
                schema: "metabase",
                table: "OpenIddictScopes",
                type: "xid",
                rowVersion: true,
                nullable: false,
                defaultValue: 0u);

            migrationBuilder.AddColumn<uint>(
                name: "xmin",
                schema: "metabase",
                table: "OpenIddictAuthorizations",
                type: "xid",
                rowVersion: true,
                nullable: false,
                defaultValue: 0u);

            migrationBuilder.AddColumn<Guid>(
                name: "OwnerId",
                schema: "metabase",
                table: "OpenIddictApplications",
                type: "uuid",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.AddColumn<uint>(
                name: "xmin",
                schema: "metabase",
                table: "OpenIddictApplications",
                type: "xid",
                rowVersion: true,
                nullable: false,
                defaultValue: 0u);

            migrationBuilder.CreateIndex(
                name: "IX_OpenIddictApplications_OwnerId",
                schema: "metabase",
                table: "OpenIddictApplications",
                column: "OwnerId");

            migrationBuilder.AddForeignKey(
                name: "FK_OpenIddictApplications_institution_OwnerId",
                schema: "metabase",
                table: "OpenIddictApplications",
                column: "OwnerId",
                principalSchema: "metabase",
                principalTable: "institution",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_OpenIddictApplications_institution_OwnerId",
                schema: "metabase",
                table: "OpenIddictApplications");

            migrationBuilder.DropIndex(
                name: "IX_OpenIddictApplications_OwnerId",
                schema: "metabase",
                table: "OpenIddictApplications");

            migrationBuilder.DropColumn(
                name: "xmin",
                schema: "metabase",
                table: "OpenIddictTokens");

            migrationBuilder.DropColumn(
                name: "xmin",
                schema: "metabase",
                table: "OpenIddictScopes");

            migrationBuilder.DropColumn(
                name: "xmin",
                schema: "metabase",
                table: "OpenIddictAuthorizations");

            migrationBuilder.DropColumn(
                name: "OwnerId",
                schema: "metabase",
                table: "OpenIddictApplications");

            migrationBuilder.DropColumn(
                name: "xmin",
                schema: "metabase",
                table: "OpenIddictApplications");

            migrationBuilder.CreateTable(
                name: "institution_open_id_connect_application",
                schema: "metabase",
                columns: table => new
                {
                    InstitutionId = table.Column<Guid>(type: "uuid", nullable: false),
                    ApplicationId = table.Column<Guid>(type: "uuid", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_institution_open_id_connect_application", x => new { x.InstitutionId, x.ApplicationId });
                    table.ForeignKey(
                        name: "FK_institution_open_id_connect_application_OpenIddictApplicati~",
                        column: _ => _.ApplicationId,
                        principalSchema: "metabase",
                        principalTable: "OpenIddictApplications",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_institution_open_id_connect_application_institution_Institu~",
                        column: _ => _.InstitutionId,
                        principalSchema: "metabase",
                        principalTable: "institution",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_institution_open_id_connect_application_ApplicationId",
                schema: "metabase",
                table: "institution_open_id_connect_application",
                column: "ApplicationId");
        }
    }
}
