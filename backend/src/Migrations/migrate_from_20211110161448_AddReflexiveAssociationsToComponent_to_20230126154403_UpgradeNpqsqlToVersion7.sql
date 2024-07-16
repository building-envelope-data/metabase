START TRANSACTION;

ALTER TABLE metabase."OpenIddictTokens" ALTER COLUMN "RedemptionDate" TYPE timestamp with time zone;

ALTER TABLE metabase."OpenIddictTokens" ALTER COLUMN "ExpirationDate" TYPE timestamp with time zone;

ALTER TABLE metabase."OpenIddictTokens" ALTER COLUMN "CreationDate" TYPE timestamp with time zone;

ALTER TABLE metabase."OpenIddictAuthorizations" ALTER COLUMN "CreationDate" TYPE timestamp with time zone;

ALTER TABLE metabase.method ALTER COLUMN "Validity" TYPE tstzrange USING tstzrange(
  lower("Validity"), upper("Validity"),
  concat(
    CASE WHEN lower_inc("Validity") THEN '[' else '(' END,
    CASE WHEN upper_inc("Validity") THEN ']' ELSE ')' END
  )
);

ALTER TABLE metabase.method ALTER COLUMN "Availability" TYPE tstzrange USING tstzrange(
  lower("Validity"), upper("Validity"),
  concat(
    CASE WHEN lower_inc("Validity") THEN '[' else '(' END,
    CASE WHEN upper_inc("Validity") THEN ']' ELSE ')' END
  )
);

ALTER TABLE metabase.component ALTER COLUMN "Availability" TYPE tstzrange USING tstzrange(
  lower("Validity"), upper("Validity"),
  concat(
    CASE WHEN lower_inc("Validity") THEN '[' else '(' END,
    CASE WHEN upper_inc("Validity") THEN ']' ELSE ')' END
  )
);

INSERT INTO "__EFMigrationsHistory" ("MigrationId", "ProductVersion")
VALUES ('20230126154403_UpgradeNpqsqlToVersion7', '7.0.2');

COMMIT;

