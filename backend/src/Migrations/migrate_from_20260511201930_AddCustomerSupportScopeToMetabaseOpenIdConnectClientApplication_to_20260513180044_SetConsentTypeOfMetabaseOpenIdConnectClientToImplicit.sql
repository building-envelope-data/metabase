START TRANSACTION;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "MigrationId" = '20260513180044_SetConsentTypeOfMetabaseOpenIdConnectClientToImplicit') THEN
                UPDATE metabase."OpenIddictApplications" 
                SET "ConsentType" = 'implicit'
                WHERE "ClientId" = 'metabase';
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "MigrationId" = '20260513180044_SetConsentTypeOfMetabaseOpenIdConnectClientToImplicit') THEN
    INSERT INTO "__EFMigrationsHistory" ("MigrationId", "ProductVersion")
    VALUES ('20260513180044_SetConsentTypeOfMetabaseOpenIdConnectClientToImplicit', '10.0.8');
    END IF;
END $EF$;
COMMIT;

