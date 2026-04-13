START TRANSACTION;
ALTER TABLE metabase."OpenIddictTokens" ALTER COLUMN "Type" TYPE character varying(150);

INSERT INTO "__EFMigrationsHistory" ("MigrationId", "ProductVersion")
VALUES ('20250725171644_UpgradeOpenIddictToVersion7', '9.0.7');

COMMIT;

