START TRANSACTION;
ALTER TABLE metabase.gnu_pg_fingerprint RENAME COLUMN "ForbiddenAt" TO "RevokedAt";

DELETE FROM "__EFMigrationsHistory"
WHERE "MigrationId" = '20250829124512_UseForbidInsteadOfRevokeForGnuPgKeyFingerprint';

ALTER TABLE metabase.gnu_pg_fingerprint DROP COLUMN "AllowedAt";

ALTER TABLE metabase.gnu_pg_fingerprint RENAME COLUMN "RevokedAt" TO "RevocationDate";

ALTER TABLE metabase.gnu_pg_fingerprint RENAME COLUMN "CreatedAt" TO "CreationDate";

DELETE FROM "__EFMigrationsHistory"
WHERE "MigrationId" = '20250828103451_AlignFieldNamesOfFingerprint';

DROP TABLE metabase.gnu_pg_fingerprint;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM pg_namespace WHERE nspname = 'metabase') THEN
        CREATE SCHEMA metabase;
    END IF;
END $EF$;

CREATE TYPE metabase.data_signing_permission AS ENUM ('allowed', 'forbidden', 'never');

ALTER TABLE metabase.institution_representative ADD "DataSigningPermission" metabase.data_signing_permission NOT NULL DEFAULT 0;

ALTER TABLE metabase.institution_representative ADD "GnuPgKeyFingerprints" text[] NOT NULL DEFAULT ARRAY[]::text[];

DELETE FROM "__EFMigrationsHistory"
WHERE "MigrationId" = '20250826140321_MakeGnuPgFingerprintItsOwnEntity';

ALTER TABLE metabase."OpenIddictApplications" DROP CONSTRAINT "FK_OpenIddictApplications_institution_OwnerId";

DROP INDEX metabase."IX_OpenIddictApplications_OwnerId";

ALTER TABLE metabase."OpenIddictApplications" DROP COLUMN "OwnerId";

CREATE TABLE metabase.institution_open_id_connect_application (
    "InstitutionId" uuid NOT NULL,
    "ApplicationId" uuid NOT NULL,
    CONSTRAINT "PK_institution_open_id_connect_application" PRIMARY KEY ("InstitutionId", "ApplicationId"),
    CONSTRAINT "FK_institution_open_id_connect_application_OpenIddictApplicati~" FOREIGN KEY ("ApplicationId") REFERENCES metabase."OpenIddictApplications" ("Id") ON DELETE CASCADE,
    CONSTRAINT "FK_institution_open_id_connect_application_institution_Institu~" FOREIGN KEY ("InstitutionId") REFERENCES metabase.institution ("Id") ON DELETE CASCADE
);

CREATE INDEX "IX_institution_open_id_connect_application_ApplicationId" ON metabase.institution_open_id_connect_application ("ApplicationId");

DELETE FROM "__EFMigrationsHistory"
WHERE "MigrationId" = '20250815155558_MakeOpenIddictApplicationsBelongToExactlyOneInstitution';

ALTER TABLE metabase.institution_representative RENAME COLUMN "GnuPgKeyFingerprints" TO "KeyFingerprints";

DELETE FROM "__EFMigrationsHistory"
WHERE "MigrationId" = '20250812162009_AddPrefixGnuPgToKeyFingerprints';

COMMIT;

