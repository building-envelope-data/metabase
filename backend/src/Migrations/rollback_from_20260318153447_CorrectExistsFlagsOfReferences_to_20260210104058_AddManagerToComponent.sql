START TRANSACTION;
DO $EF$
BEGIN
    IF EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "MigrationId" = '20260318153447_CorrectExistsFlagsOfReferences') THEN
    DELETE FROM "__EFMigrationsHistory"
    WHERE "MigrationId" = '20260318153447_CorrectExistsFlagsOfReferences';
    END IF;
END $EF$;
COMMIT;

