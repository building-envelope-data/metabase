using System;
using Microsoft.EntityFrameworkCore.Migrations;
using NpgsqlTypes;

#nullable disable

namespace Metabase.Migrations
{
    /// <inheritdoc />
    public partial class UpgradeNpqsqlToVersion7 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterDatabase()
                .Annotation("Npgsql:Enum:public.component_category", "material,layer,unit")
                .Annotation("Npgsql:Enum:public.institution_representative_role", "owner,assistant")
                .Annotation("Npgsql:Enum:public.institution_state", "pending,verified")
                .Annotation("Npgsql:Enum:public.method_category", "measurement,calculation")
                .Annotation("Npgsql:Enum:public.standardizer", "aerc,agi,ashrae,breeam,bs,bsi,cen,cie,dgnb,din,dvwg,iec,ies,ift,iso,jis,leed,nfrc,riba,ul,unece,vdi,vff,well")
                .Annotation("Npgsql:PostgresExtension:pgcrypto", ",,")
                .OldAnnotation("Npgsql:Enum:component_category", "material,layer,unit")
                .OldAnnotation("Npgsql:Enum:institution_representative_role", "owner,assistant")
                .OldAnnotation("Npgsql:Enum:institution_state", "pending,verified")
                .OldAnnotation("Npgsql:Enum:method_category", "measurement,calculation")
                .OldAnnotation("Npgsql:Enum:standardizer", "aerc,agi,ashrae,breeam,bs,bsi,cen,cie,dgnb,din,dvwg,iec,ies,ift,iso,jis,leed,nfrc,riba,ul,unece,vdi,vff,well")
                .OldAnnotation("Npgsql:PostgresExtension:pgcrypto", ",,");

            migrationBuilder.AlterColumn<DateTime>(
                name: "RedemptionDate",
                schema: "metabase",
                table: "OpenIddictTokens",
                type: "timestamp with time zone",
                nullable: true,
                oldClrType: typeof(DateTime),
                oldType: "timestamp without time zone",
                oldNullable: true);

            migrationBuilder.AlterColumn<DateTime>(
                name: "ExpirationDate",
                schema: "metabase",
                table: "OpenIddictTokens",
                type: "timestamp with time zone",
                nullable: true,
                oldClrType: typeof(DateTime),
                oldType: "timestamp without time zone",
                oldNullable: true);

            migrationBuilder.AlterColumn<DateTime>(
                name: "CreationDate",
                schema: "metabase",
                table: "OpenIddictTokens",
                type: "timestamp with time zone",
                nullable: true,
                oldClrType: typeof(DateTime),
                oldType: "timestamp without time zone",
                oldNullable: true);

            migrationBuilder.AlterColumn<DateTime>(
                name: "CreationDate",
                schema: "metabase",
                table: "OpenIddictAuthorizations",
                type: "timestamp with time zone",
                nullable: true,
                oldClrType: typeof(DateTime),
                oldType: "timestamp without time zone",
                oldNullable: true);

            migrationBuilder.AlterColumn<NpgsqlRange<DateTime>>(
                name: "Validity",
                schema: "metabase",
                table: "method",
                type: "tstzrange",
                nullable: true,
                oldClrType: typeof(NpgsqlRange<DateTime>),
                oldType: "tsrange",
                oldNullable: true);

            migrationBuilder.AlterColumn<NpgsqlRange<DateTime>>(
                name: "Availability",
                schema: "metabase",
                table: "method",
                type: "tstzrange",
                nullable: true,
                oldClrType: typeof(NpgsqlRange<DateTime>),
                oldType: "tsrange",
                oldNullable: true);

            migrationBuilder.AlterColumn<NpgsqlRange<DateTime>>(
                name: "Availability",
                schema: "metabase",
                table: "component",
                type: "tstzrange",
                nullable: true,
                oldClrType: typeof(NpgsqlRange<DateTime>),
                oldType: "tsrange",
                oldNullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterDatabase()
                .Annotation("Npgsql:Enum:component_category", "material,layer,unit")
                .Annotation("Npgsql:Enum:institution_representative_role", "owner,assistant")
                .Annotation("Npgsql:Enum:institution_state", "pending,verified")
                .Annotation("Npgsql:Enum:method_category", "measurement,calculation")
                .Annotation("Npgsql:Enum:standardizer", "aerc,agi,ashrae,breeam,bs,bsi,cen,cie,dgnb,din,dvwg,iec,ies,ift,iso,jis,leed,nfrc,riba,ul,unece,vdi,vff,well")
                .Annotation("Npgsql:PostgresExtension:pgcrypto", ",,")
                .OldAnnotation("Npgsql:Enum:public.component_category", "material,layer,unit")
                .OldAnnotation("Npgsql:Enum:public.institution_representative_role", "owner,assistant")
                .OldAnnotation("Npgsql:Enum:public.institution_state", "pending,verified")
                .OldAnnotation("Npgsql:Enum:public.method_category", "measurement,calculation")
                .OldAnnotation("Npgsql:Enum:public.standardizer", "aerc,agi,ashrae,breeam,bs,bsi,cen,cie,dgnb,din,dvwg,iec,ies,ift,iso,jis,leed,nfrc,riba,ul,unece,vdi,vff,well")
                .OldAnnotation("Npgsql:PostgresExtension:pgcrypto", ",,");

            migrationBuilder.AlterColumn<DateTime>(
                name: "RedemptionDate",
                schema: "metabase",
                table: "OpenIddictTokens",
                type: "timestamp without time zone",
                nullable: true,
                oldClrType: typeof(DateTime),
                oldType: "timestamp with time zone",
                oldNullable: true);

            migrationBuilder.AlterColumn<DateTime>(
                name: "ExpirationDate",
                schema: "metabase",
                table: "OpenIddictTokens",
                type: "timestamp without time zone",
                nullable: true,
                oldClrType: typeof(DateTime),
                oldType: "timestamp with time zone",
                oldNullable: true);

            migrationBuilder.AlterColumn<DateTime>(
                name: "CreationDate",
                schema: "metabase",
                table: "OpenIddictTokens",
                type: "timestamp without time zone",
                nullable: true,
                oldClrType: typeof(DateTime),
                oldType: "timestamp with time zone",
                oldNullable: true);

            migrationBuilder.AlterColumn<DateTime>(
                name: "CreationDate",
                schema: "metabase",
                table: "OpenIddictAuthorizations",
                type: "timestamp without time zone",
                nullable: true,
                oldClrType: typeof(DateTime),
                oldType: "timestamp with time zone",
                oldNullable: true);

            migrationBuilder.AlterColumn<NpgsqlRange<DateTime>>(
                name: "Validity",
                schema: "metabase",
                table: "method",
                type: "tsrange",
                nullable: true,
                oldClrType: typeof(NpgsqlRange<DateTime>),
                oldType: "tstzrange",
                oldNullable: true);

            migrationBuilder.AlterColumn<NpgsqlRange<DateTime>>(
                name: "Availability",
                schema: "metabase",
                table: "method",
                type: "tsrange",
                nullable: true,
                oldClrType: typeof(NpgsqlRange<DateTime>),
                oldType: "tstzrange",
                oldNullable: true);

            migrationBuilder.AlterColumn<NpgsqlRange<DateTime>>(
                name: "Availability",
                schema: "metabase",
                table: "component",
                type: "tsrange",
                nullable: true,
                oldClrType: typeof(NpgsqlRange<DateTime>),
                oldType: "tstzrange",
                oldNullable: true);
        }
    }
}
