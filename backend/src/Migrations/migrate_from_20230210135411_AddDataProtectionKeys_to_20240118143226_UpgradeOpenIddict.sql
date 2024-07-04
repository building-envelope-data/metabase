START TRANSACTION;

CREATE TYPE public.prime_surface AS ENUM ('inside', 'outside');

ALTER TABLE metabase.component_assembly ADD "Index" smallint;

ALTER TABLE metabase.component_assembly ADD "PrimeSurface" prime_surface;

INSERT INTO "__EFMigrationsHistory" ("MigrationId", "ProductVersion")
VALUES ('20230306150049_AddIndexAndPrimeSurfaceToAssembly', '8.0.6');

COMMIT;

START TRANSACTION;

ALTER TABLE metabase.data_format DROP CONSTRAINT "FK_data_format_institution_ManagerId";

ALTER TABLE metabase.database DROP CONSTRAINT "FK_database_institution_OperatorId";

ALTER TABLE metabase.institution DROP CONSTRAINT "FK_institution_institution_ManagerId";

ALTER TABLE metabase.method DROP CONSTRAINT "FK_method_institution_ManagerId";

ALTER TABLE metabase.data_format ADD CONSTRAINT "FK_data_format_institution_ManagerId" FOREIGN KEY ("ManagerId") REFERENCES metabase.institution ("Id") ON DELETE RESTRICT;

ALTER TABLE metabase.database ADD CONSTRAINT "FK_database_institution_OperatorId" FOREIGN KEY ("OperatorId") REFERENCES metabase.institution ("Id") ON DELETE RESTRICT;

ALTER TABLE metabase.institution ADD CONSTRAINT "FK_institution_institution_ManagerId" FOREIGN KEY ("ManagerId") REFERENCES metabase.institution ("Id") ON DELETE RESTRICT;

ALTER TABLE metabase.method ADD CONSTRAINT "FK_method_institution_ManagerId" FOREIGN KEY ("ManagerId") REFERENCES metabase.institution ("Id") ON DELETE RESTRICT;

INSERT INTO "__EFMigrationsHistory" ("MigrationId", "ProductVersion")
VALUES ('20230412131346_ConfigureOnDeleteAction', '8.0.6');

COMMIT;

START TRANSACTION;

CREATE TYPE public.database_verification_state AS ENUM ('pending', 'verified');

ALTER TABLE metabase.database ADD "VerificationCode" text NOT NULL DEFAULT '';

ALTER TABLE metabase.database ADD "VerificationState" database_verification_state NOT NULL DEFAULT 'pending'::database_verification_state;

INSERT INTO "__EFMigrationsHistory" ("MigrationId", "ProductVersion")
VALUES ('20230419100526_AddVerificationStateAndCodeToDatabaseTable', '8.0.6');

COMMIT;

START TRANSACTION;

UPDATE metabase.database SET "VerificationState" = 'verified';

INSERT INTO "__EFMigrationsHistory" ("MigrationId", "ProductVersion")
VALUES ('20230419112023_InitializeDatabaseVerificationCodesAndSetVerificationStateToVerified', '8.0.6');

COMMIT;

START TRANSACTION;

ALTER TABLE metabase."OpenIddictApplications" RENAME COLUMN "Type" TO "ClientType";

ALTER TABLE metabase."OpenIddictApplications" ADD "ApplicationType" character varying(50);

ALTER TABLE metabase."OpenIddictApplications" ADD "JsonWebKeySet" text;

ALTER TABLE metabase."OpenIddictApplications" ADD "Settings" text;

INSERT INTO "__EFMigrationsHistory" ("MigrationId", "ProductVersion")
VALUES ('20240118143226_UpgradeOpenIddict', '8.0.6');

COMMIT;

