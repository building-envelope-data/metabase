START TRANSACTION;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "MigrationId" = '20260511201930_AddCustomerSupportScopeToMetabaseOpenIdConnectClientApplication') THEN
                UPDATE metabase."OpenIddictApplications" 
                SET "Permissions" = ("Permissions"::jsonb - 'scp:api:support')::text
                WHERE "ClientId" = 'metabase';
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "MigrationId" = '20260511201930_AddCustomerSupportScopeToMetabaseOpenIdConnectClientApplication') THEN
    DELETE FROM "__EFMigrationsHistory"
    WHERE "MigrationId" = '20260511201930_AddCustomerSupportScopeToMetabaseOpenIdConnectClientApplication';
    END IF;
END $EF$;

COMMIT;

