START TRANSACTION;

ALTER TABLE metabase.component_manufacturer DROP COLUMN "Pending";

DELETE FROM "__EFMigrationsHistory"
WHERE "MigrationId" = '20210722150010_AddPendingColumnToComponentManufacturer';

COMMIT;

