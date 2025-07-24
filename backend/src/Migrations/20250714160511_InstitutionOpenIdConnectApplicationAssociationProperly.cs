using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Metabase.Migrations
{
    /// <inheritdoc />
    public partial class InstitutionOpenIdConnectApplicationAssociationProperly : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_institution_application_OpenIddictApplications_ApplicationId",
                schema: "metabase",
                table: "institution_application");

            migrationBuilder.DropForeignKey(
                name: "FK_institution_application_institution_InstitutionId",
                schema: "metabase",
                table: "institution_application");

            migrationBuilder.DropPrimaryKey(
                name: "PK_institution_application",
                schema: "metabase",
                table: "institution_application");

            migrationBuilder.RenameTable(
                name: "institution_application",
                schema: "metabase",
                newName: "institution_open_id_connect_application",
                newSchema: "metabase");

            migrationBuilder.RenameIndex(
                name: "IX_institution_application_ApplicationId",
                schema: "metabase",
                table: "institution_open_id_connect_application",
                newName: "IX_institution_open_id_connect_application_ApplicationId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_institution_open_id_connect_application",
                schema: "metabase",
                table: "institution_open_id_connect_application",
                columns: new[] { "InstitutionId", "ApplicationId" });

            migrationBuilder.AddForeignKey(
                name: "FK_institution_open_id_connect_application_OpenIddictApplicati~",
                schema: "metabase",
                table: "institution_open_id_connect_application",
                column: "ApplicationId",
                principalSchema: "metabase",
                principalTable: "OpenIddictApplications",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_institution_open_id_connect_application_institution_Institu~",
                schema: "metabase",
                table: "institution_open_id_connect_application",
                column: "InstitutionId",
                principalSchema: "metabase",
                principalTable: "institution",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_institution_open_id_connect_application_OpenIddictApplicati~",
                schema: "metabase",
                table: "institution_open_id_connect_application");

            migrationBuilder.DropForeignKey(
                name: "FK_institution_open_id_connect_application_institution_Institu~",
                schema: "metabase",
                table: "institution_open_id_connect_application");

            migrationBuilder.DropPrimaryKey(
                name: "PK_institution_open_id_connect_application",
                schema: "metabase",
                table: "institution_open_id_connect_application");

            migrationBuilder.RenameTable(
                name: "institution_open_id_connect_application",
                schema: "metabase",
                newName: "institution_application",
                newSchema: "metabase");

            migrationBuilder.RenameIndex(
                name: "IX_institution_open_id_connect_application_ApplicationId",
                schema: "metabase",
                table: "institution_application",
                newName: "IX_institution_application_ApplicationId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_institution_application",
                schema: "metabase",
                table: "institution_application",
                columns: new[] { "InstitutionId", "ApplicationId" });

            migrationBuilder.AddForeignKey(
                name: "FK_institution_application_OpenIddictApplications_ApplicationId",
                schema: "metabase",
                table: "institution_application",
                column: "ApplicationId",
                principalSchema: "metabase",
                principalTable: "OpenIddictApplications",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_institution_application_institution_InstitutionId",
                schema: "metabase",
                table: "institution_application",
                column: "InstitutionId",
                principalSchema: "metabase",
                principalTable: "institution",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}