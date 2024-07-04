START TRANSACTION;

DROP TABLE metabase."DataProtectionKeys";

DELETE FROM "__EFMigrationsHistory"
WHERE "MigrationId" = '20230210135411_AddDataProtectionKeys';

COMMIT;

