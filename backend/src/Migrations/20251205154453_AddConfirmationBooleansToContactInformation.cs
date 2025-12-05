using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Metabase.Migrations
{
    /// <inheritdoc />
    public partial class AddConfirmationBooleansToContactInformation : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Contact_Exists",
                schema: "metabase",
                table: "institution");

            migrationBuilder.AddColumn<bool>(
                name: "Contact_IsPhoneNumberConfirmed",
                schema: "metabase",
                table: "institution",
                type: "boolean",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "Contact_IsEmailAddressConfirmed",
                schema: "metabase",
                table: "institution",
                type: "boolean",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Contact_IsEmailAddressConfirmed",
                schema: "metabase",
                table: "institution");

            migrationBuilder.DropColumn(
                name: "Contact_IsPhoneNumberConfirmed",
                schema: "metabase",
                table: "institution");

            migrationBuilder.AddColumn<bool>(
                name: "Contact_Exists",
                schema: "metabase",
                table: "institution",
                type: "boolean",
                nullable: true);
        }
    }
}