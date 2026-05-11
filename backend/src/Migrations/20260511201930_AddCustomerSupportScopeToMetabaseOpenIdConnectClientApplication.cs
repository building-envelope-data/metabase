using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Metabase.Migrations
{
    /// <inheritdoc />
    public partial class AddCustomerSupportScopeToMetabaseOpenIdConnectClientApplication : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql("""
                UPDATE metabase."OpenIddictApplications" 
                SET "Permissions" = ("Permissions"::jsonb || '["scp:api:support"]'::jsonb)::text
                WHERE "ClientId" = 'metabase';
            """
            );
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql("""
                UPDATE metabase."OpenIddictApplications" 
                SET "Permissions" = ("Permissions"::jsonb - 'scp:api:support')::text
                WHERE "ClientId" = 'metabase';
            """
            );
        }
    }
}