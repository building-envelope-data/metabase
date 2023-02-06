START TRANSACTION;

ALTER TABLE metabase."OpenIddictTokens" ALTER COLUMN "RedemptionDate" TYPE timestamp without time zone;

ALTER TABLE metabase."OpenIddictTokens" ALTER COLUMN "ExpirationDate" TYPE timestamp without time zone;

ALTER TABLE metabase."OpenIddictTokens" ALTER COLUMN "CreationDate" TYPE timestamp without time zone;

ALTER TABLE metabase."OpenIddictAuthorizations" ALTER COLUMN "CreationDate" TYPE timestamp without time zone;

ALTER TABLE metabase.method ALTER COLUMN "Validity" TYPE tsrange;

ALTER TABLE metabase.method ALTER COLUMN "Availability" TYPE tsrange;

ALTER TABLE metabase.component ALTER COLUMN "Availability" TYPE tsrange;

DELETE FROM "__EFMigrationsHistory"
WHERE "MigrationId" = '20230126154403_UpgradeNpqsqlToVersion7';

COMMIT;

