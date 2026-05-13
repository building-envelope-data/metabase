START TRANSACTION;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "MigrationId" = '20260513180044_SetConsentTypeOfMetabaseOpenIdConnectClientToImplicit') THEN
                UPDATE metabase."OpenIddictApplications" 
                SET "ConsentType" = 'explicit'
                WHERE "ClientId" = 'metabase';
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "MigrationId" = '20260513180044_SetConsentTypeOfMetabaseOpenIdConnectClientToImplicit') THEN
    DELETE FROM "__EFMigrationsHistory"
    WHERE "MigrationId" = '20260513180044_SetConsentTypeOfMetabaseOpenIdConnectClientToImplicit';
    END IF;
END $EF$;

COMMIT;

