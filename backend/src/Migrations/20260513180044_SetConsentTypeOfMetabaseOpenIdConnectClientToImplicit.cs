using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Metabase.Migrations
{
    /// <inheritdoc />
    public partial class SetConsentTypeOfMetabaseOpenIdConnectClientToImplicit : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql("""
                UPDATE metabase."OpenIddictApplications" 
                SET "ConsentType" = 'implicit'
                WHERE "ClientId" = 'metabase';
            """
            );
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql("""
                UPDATE metabase."OpenIddictApplications" 
                SET "ConsentType" = 'explicit'
                WHERE "ClientId" = 'metabase';
            """
            );
        }
    }
}