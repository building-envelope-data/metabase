START TRANSACTION;

DO $EF$
BEGIN
    IF EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "MigrationId" = '20260625115427_DropContactExistsColumnFromInstitution') THEN
    ALTER TABLE metabase.institution ADD "Contact_Exists" boolean;
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "MigrationId" = '20260625115427_DropContactExistsColumnFromInstitution') THEN
    DELETE FROM "__EFMigrationsHistory"
    WHERE "MigrationId" = '20260625115427_DropContactExistsColumnFromInstitution';
    END IF;
END $EF$;

COMMIT;

