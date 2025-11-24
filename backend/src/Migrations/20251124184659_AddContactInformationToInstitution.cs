using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Metabase.Migrations
{
    /// <inheritdoc />
    public partial class AddContactInformationToInstitution : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "WebsiteLocator",
                schema: "metabase",
                table: "institution",
                newName: "Contact_WebsiteLocator");

            migrationBuilder.RenameColumn(
                name: "PublicKey",
                schema: "metabase",
                table: "institution",
                newName: "Contact_PostalAddress");

            migrationBuilder.AddColumn<string>(
                name: "Contact_EmailAddress",
                schema: "metabase",
                table: "institution",
                type: "text",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "Contact_Exists",
                schema: "metabase",
                table: "institution",
                type: "boolean",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Contact_PhoneNumber",
                schema: "metabase",
                table: "institution",
                type: "text",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Contact_EmailAddress",
                schema: "metabase",
                table: "institution");

            migrationBuilder.DropColumn(
                name: "Contact_Exists",
                schema: "metabase",
                table: "institution");

            migrationBuilder.DropColumn(
                name: "Contact_PhoneNumber",
                schema: "metabase",
                table: "institution");

            migrationBuilder.RenameColumn(
                name: "Contact_WebsiteLocator",
                schema: "metabase",
                table: "institution",
                newName: "WebsiteLocator");

            migrationBuilder.RenameColumn(
                name: "Contact_PostalAddress",
                schema: "metabase",
                table: "institution",
                newName: "PublicKey");
        }
    }
}