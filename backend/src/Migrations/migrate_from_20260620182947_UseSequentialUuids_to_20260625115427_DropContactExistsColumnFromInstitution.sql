START TRANSACTION;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "MigrationId" = '20260625115427_DropContactExistsColumnFromInstitution') THEN
    ALTER TABLE metabase.institution DROP COLUMN IF EXISTS "Contact_Exists";
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "MigrationId" = '20260625115427_DropContactExistsColumnFromInstitution') THEN
    INSERT INTO "__EFMigrationsHistory" ("MigrationId", "ProductVersion")
    VALUES ('20260625115427_DropContactExistsColumnFromInstitution', '10.0.9');
    END IF;
END $EF$;

COMMIT;

