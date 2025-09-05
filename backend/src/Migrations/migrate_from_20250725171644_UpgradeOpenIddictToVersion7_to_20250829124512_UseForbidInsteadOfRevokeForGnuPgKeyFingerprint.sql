START TRANSACTION;
ALTER TABLE metabase.institution_representative RENAME COLUMN "KeyFingerprints" TO "GnuPgKeyFingerprints";

INSERT INTO "__EFMigrationsHistory" ("MigrationId", "ProductVersion")
VALUES ('20250812162009_AddPrefixGnuPgToKeyFingerprints', '9.0.7');

ALTER TABLE metabase."OpenIddictApplications" ADD "OwnerId" uuid NOT NULL DEFAULT '00000000-0000-0000-0000-000000000000';

CREATE INDEX "IX_OpenIddictApplications_OwnerId" ON metabase."OpenIddictApplications" ("OwnerId");

UPDATE metabase."OpenIddictApplications" SET "OwnerId" = "InstitutionId" FROM metabase.institution_open_id_connect_application WHERE "ApplicationId" = "Id";

ALTER TABLE metabase."OpenIddictApplications" ADD CONSTRAINT "FK_OpenIddictApplications_institution_OwnerId" FOREIGN KEY ("OwnerId") REFERENCES metabase.institution ("Id") ON DELETE RESTRICT;

DROP TABLE metabase.institution_open_id_connect_application;

INSERT INTO "__EFMigrationsHistory" ("MigrationId", "ProductVersion")
VALUES ('20250815155558_MakeOpenIddictApplicationsBelongToExactlyOneInstitution', '9.0.7');

ALTER TABLE metabase.institution_representative DROP COLUMN "DataSigningPermission";

ALTER TABLE metabase.institution_representative DROP COLUMN "GnuPgKeyFingerprints";

DROP TYPE metabase.data_signing_permission;

CREATE TABLE metabase.gnu_pg_fingerprint (
    "Id" uuid NOT NULL DEFAULT (gen_random_uuid()),
    "Fingerprint" text NOT NULL,
    "CreationDate" timestamp with time zone NOT NULL,
    "RevocationDate" timestamp with time zone,
    "UserId" uuid NOT NULL,
    "InstitutionId" uuid NOT NULL,
    CONSTRAINT "PK_gnu_pg_fingerprint" PRIMARY KEY ("Id"),
    CONSTRAINT "FK_gnu_pg_fingerprint_institution_InstitutionId" FOREIGN KEY ("InstitutionId") REFERENCES metabase.institution ("Id") ON DELETE CASCADE,
    CONSTRAINT "FK_gnu_pg_fingerprint_user_UserId" FOREIGN KEY ("UserId") REFERENCES metabase."user" ("Id") ON DELETE CASCADE
);

CREATE UNIQUE INDEX "IX_gnu_pg_fingerprint_Fingerprint" ON metabase.gnu_pg_fingerprint ("Fingerprint");

CREATE INDEX "IX_gnu_pg_fingerprint_InstitutionId" ON metabase.gnu_pg_fingerprint ("InstitutionId");

CREATE INDEX "IX_gnu_pg_fingerprint_UserId" ON metabase.gnu_pg_fingerprint ("UserId");

INSERT INTO "__EFMigrationsHistory" ("MigrationId", "ProductVersion")
VALUES ('20250826140321_MakeGnuPgFingerprintItsOwnEntity', '9.0.7');

ALTER TABLE metabase.gnu_pg_fingerprint RENAME COLUMN "RevocationDate" TO "RevokedAt";

ALTER TABLE metabase.gnu_pg_fingerprint RENAME COLUMN "CreationDate" TO "CreatedAt";

ALTER TABLE metabase.gnu_pg_fingerprint ADD "AllowedAt" timestamp with time zone;

INSERT INTO "__EFMigrationsHistory" ("MigrationId", "ProductVersion")
VALUES ('20250828103451_AlignFieldNamesOfFingerprint', '9.0.7');

ALTER TABLE metabase.gnu_pg_fingerprint RENAME COLUMN "RevokedAt" TO "ForbiddenAt";

INSERT INTO "__EFMigrationsHistory" ("MigrationId", "ProductVersion")
VALUES ('20250829124512_UseForbidInsteadOfRevokeForGnuPgKeyFingerprint', '9.0.7');

COMMIT;

