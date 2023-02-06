START TRANSACTION;

ALTER TABLE metabase."OpenIddictTokens" ALTER COLUMN "RedemptionDate" TYPE timestamp with time zone;

ALTER TABLE metabase."OpenIddictTokens" ALTER COLUMN "ExpirationDate" TYPE timestamp with time zone;

ALTER TABLE metabase."OpenIddictTokens" ALTER COLUMN "CreationDate" TYPE timestamp with time zone;

ALTER TABLE metabase."OpenIddictAuthorizations" ALTER COLUMN "CreationDate" TYPE timestamp with time zone;

ALTER TABLE metabase.method ALTER COLUMN "Validity" TYPE tstzrange;

ALTER TABLE metabase.method ALTER COLUMN "Availability" TYPE tstzrange;

ALTER TABLE metabase.component ALTER COLUMN "Availability" TYPE tstzrange;

INSERT INTO "__EFMigrationsHistory" ("MigrationId", "ProductVersion")
VALUES ('20230126154403_UpgradeNpqsqlToVersion7', '7.0.2');

COMMIT;

