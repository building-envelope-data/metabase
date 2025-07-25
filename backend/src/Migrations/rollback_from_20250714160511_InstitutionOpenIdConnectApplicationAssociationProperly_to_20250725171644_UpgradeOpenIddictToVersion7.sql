START TRANSACTION;
ALTER TABLE metabase."OpenIddictTokens" ALTER COLUMN "Type" TYPE character varying(50);

DELETE FROM "__EFMigrationsHistory"
WHERE "MigrationId" = '20250725171644_UpgradeOpenIddictToVersion7';

COMMIT;

