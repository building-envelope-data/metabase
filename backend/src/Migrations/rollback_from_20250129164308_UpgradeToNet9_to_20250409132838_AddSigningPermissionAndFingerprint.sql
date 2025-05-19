START TRANSACTION;
ALTER TABLE metabase.institution_representative DROP COLUMN "DataSigningPermission";

ALTER TABLE metabase.institution_representative DROP COLUMN "KeyFingerprints";

DROP TYPE metabase.data_signing_permission;

DELETE FROM "__EFMigrationsHistory"
WHERE "MigrationId" = '20250409132838_AddSigningPermissionAndFingerprint';

COMMIT;

