using Metabase.Enumerations;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Metabase.Migrations;

/// <inheritdoc />
public partial class AddOperatingState : Migration
{
    /// <inheritdoc />
    protected override void Up(MigrationBuilder migrationBuilder)
    {
        migrationBuilder.AlterDatabase()
            .Annotation("Npgsql:Enum:public.institution_operating_state", "operating,not_operating");

        migrationBuilder.AddColumn<InstitutionOperatingState>(
            name: "OperatingState",
            schema: "metabase",
            table: "institution",
            type: "institution_operating_state",
            nullable: false,
            defaultValue: InstitutionOperatingState.OPERATING);
    }

    /// <inheritdoc />
    protected override void Down(MigrationBuilder migrationBuilder)
    {
        migrationBuilder.DropColumn(
            name: "OperatingState",
            schema: "metabase",
            table: "institution");

        migrationBuilder.AlterDatabase()
            .OldAnnotation("Npgsql:Enum:public.institution_operating_state", "operating,not_operating");
    }
}