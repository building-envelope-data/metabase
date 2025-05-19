using Metabase.Enumerations;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Metabase.Migrations
{
    /// <inheritdoc />
    public partial class AddSigningPermissionAndFingerprint : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterDatabase()
                .Annotation("Npgsql:Enum:metabase.component_category", "layer,material,unit")
                .Annotation("Npgsql:Enum:metabase.data_signing_permission", "allowed,forbidden,never")
                .Annotation("Npgsql:Enum:metabase.database_verification_state", "pending,verified")
                .Annotation("Npgsql:Enum:metabase.institution_operating_state", "not_operating,operating")
                .Annotation("Npgsql:Enum:metabase.institution_representative_role", "assistant,owner")
                .Annotation("Npgsql:Enum:metabase.institution_state", "pending,verified")
                .Annotation("Npgsql:Enum:metabase.method_category", "calculation,measurement")
                .Annotation("Npgsql:Enum:metabase.prime_surface", "inside,outside")
                .Annotation("Npgsql:Enum:metabase.standardizer", "aerc,agi,ashrae,breeam,bs,bsi,cen,cie,dgnb,din,dvwg,iec,ies,ift,iso,jis,leed,nfrc,riba,ul,unece,vdi,vff,well")
                .Annotation("Npgsql:PostgresExtension:pgcrypto", ",,")
                .OldAnnotation("Npgsql:Enum:metabase.component_category", "layer,material,unit")
                .OldAnnotation("Npgsql:Enum:metabase.database_verification_state", "pending,verified")
                .OldAnnotation("Npgsql:Enum:metabase.institution_operating_state", "not_operating,operating")
                .OldAnnotation("Npgsql:Enum:metabase.institution_representative_role", "assistant,owner")
                .OldAnnotation("Npgsql:Enum:metabase.institution_state", "pending,verified")
                .OldAnnotation("Npgsql:Enum:metabase.method_category", "calculation,measurement")
                .OldAnnotation("Npgsql:Enum:metabase.prime_surface", "inside,outside")
                .OldAnnotation("Npgsql:Enum:metabase.standardizer", "aerc,agi,ashrae,breeam,bs,bsi,cen,cie,dgnb,din,dvwg,iec,ies,ift,iso,jis,leed,nfrc,riba,ul,unece,vdi,vff,well")
                .OldAnnotation("Npgsql:PostgresExtension:pgcrypto", ",,");

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
                defaultValue: new string[0]);
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
                .Annotation("Npgsql:Enum:metabase.component_category", "layer,material,unit")
                .Annotation("Npgsql:Enum:metabase.database_verification_state", "pending,verified")
                .Annotation("Npgsql:Enum:metabase.institution_operating_state", "not_operating,operating")
                .Annotation("Npgsql:Enum:metabase.institution_representative_role", "assistant,owner")
                .Annotation("Npgsql:Enum:metabase.institution_state", "pending,verified")
                .Annotation("Npgsql:Enum:metabase.method_category", "calculation,measurement")
                .Annotation("Npgsql:Enum:metabase.prime_surface", "inside,outside")
                .Annotation("Npgsql:Enum:metabase.standardizer", "aerc,agi,ashrae,breeam,bs,bsi,cen,cie,dgnb,din,dvwg,iec,ies,ift,iso,jis,leed,nfrc,riba,ul,unece,vdi,vff,well")
                .Annotation("Npgsql:PostgresExtension:pgcrypto", ",,")
                .OldAnnotation("Npgsql:Enum:metabase.component_category", "layer,material,unit")
                .OldAnnotation("Npgsql:Enum:metabase.data_signing_permission", "allowed,forbidden,never")
                .OldAnnotation("Npgsql:Enum:metabase.database_verification_state", "pending,verified")
                .OldAnnotation("Npgsql:Enum:metabase.institution_operating_state", "not_operating,operating")
                .OldAnnotation("Npgsql:Enum:metabase.institution_representative_role", "assistant,owner")
                .OldAnnotation("Npgsql:Enum:metabase.institution_state", "pending,verified")
                .OldAnnotation("Npgsql:Enum:metabase.method_category", "calculation,measurement")
                .OldAnnotation("Npgsql:Enum:metabase.prime_surface", "inside,outside")
                .OldAnnotation("Npgsql:Enum:metabase.standardizer", "aerc,agi,ashrae,breeam,bs,bsi,cen,cie,dgnb,din,dvwg,iec,ies,ift,iso,jis,leed,nfrc,riba,ul,unece,vdi,vff,well")
                .OldAnnotation("Npgsql:PostgresExtension:pgcrypto", ",,");
        }
    }
}
