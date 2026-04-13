START TRANSACTION;
DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM pg_namespace WHERE nspname = 'metabase') THEN
        CREATE SCHEMA metabase;
    END IF;
END $EF$;

CREATE TYPE metabase.data_signing_permission AS ENUM ('allowed', 'forbidden', 'never');

ALTER TABLE metabase.institution_representative ADD "DataSigningPermission" metabase.data_signing_permission NOT NULL DEFAULT 'never'::metabase.data_signing_permission;

ALTER TABLE metabase.institution_representative ADD "KeyFingerprints" text[] NOT NULL DEFAULT ARRAY[]::text[];

INSERT INTO "__EFMigrationsHistory" ("MigrationId", "ProductVersion")
VALUES ('20250409132838_AddSigningPermissionAndFingerprint', '9.0.5');

COMMIT;

