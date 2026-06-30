START TRANSACTION;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "MigrationId" = '20260511201930_AddCustomerSupportScopeToMetabaseOpenIdConnectClientApplication') THEN
                UPDATE metabase."OpenIddictApplications" 
                SET "Permissions" = ("Permissions"::jsonb || '["scp:api:support"]'::jsonb)::text
                WHERE "ClientId" = 'metabase';
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "MigrationId" = '20260511201930_AddCustomerSupportScopeToMetabaseOpenIdConnectClientApplication') THEN
    INSERT INTO "__EFMigrationsHistory" ("MigrationId", "ProductVersion")
    VALUES ('20260511201930_AddCustomerSupportScopeToMetabaseOpenIdConnectClientApplication', '10.0.7');
    END IF;
END $EF$;

COMMIT;

