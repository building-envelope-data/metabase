START TRANSACTION;

ALTER TABLE metabase."OpenIddictApplications" DROP COLUMN "ApplicationType";

ALTER TABLE metabase."OpenIddictApplications" DROP COLUMN "JsonWebKeySet";

ALTER TABLE metabase."OpenIddictApplications" DROP COLUMN "Settings";

ALTER TABLE metabase."OpenIddictApplications" RENAME COLUMN "ClientType" TO "Type";

DELETE FROM "__EFMigrationsHistory"
WHERE "MigrationId" = '20240118143226_UpgradeOpenIddict';

COMMIT;

START TRANSACTION;

UPDATE metabase.database SET "VerificationState" = 'pending';

DELETE FROM "__EFMigrationsHistory"
WHERE "MigrationId" = '20230419112023_InitializeDatabaseVerificationCodesAndSetVerificationStateToVerified';

COMMIT;

START TRANSACTION;

ALTER TABLE metabase.database DROP COLUMN "VerificationCode";

ALTER TABLE metabase.database DROP COLUMN "VerificationState";

DROP TYPE public.database_verification_state;

DELETE FROM "__EFMigrationsHistory"
WHERE "MigrationId" = '20230419100526_AddVerificationStateAndCodeToDatabaseTable';

COMMIT;

START TRANSACTION;

ALTER TABLE metabase.data_format DROP CONSTRAINT "FK_data_format_institution_ManagerId";

ALTER TABLE metabase.database DROP CONSTRAINT "FK_database_institution_OperatorId";

ALTER TABLE metabase.institution DROP CONSTRAINT "FK_institution_institution_ManagerId";

ALTER TABLE metabase.method DROP CONSTRAINT "FK_method_institution_ManagerId";

ALTER TABLE metabase.data_format ADD CONSTRAINT "FK_data_format_institution_ManagerId" FOREIGN KEY ("ManagerId") REFERENCES metabase.institution ("Id") ON DELETE CASCADE;

ALTER TABLE metabase.database ADD CONSTRAINT "FK_database_institution_OperatorId" FOREIGN KEY ("OperatorId") REFERENCES metabase.institution ("Id") ON DELETE CASCADE;

ALTER TABLE metabase.institution ADD CONSTRAINT "FK_institution_institution_ManagerId" FOREIGN KEY ("ManagerId") REFERENCES metabase.institution ("Id");

ALTER TABLE metabase.method ADD CONSTRAINT "FK_method_institution_ManagerId" FOREIGN KEY ("ManagerId") REFERENCES metabase.institution ("Id") ON DELETE CASCADE;

DELETE FROM "__EFMigrationsHistory"
WHERE "MigrationId" = '20230412131346_ConfigureOnDeleteAction';

COMMIT;

START TRANSACTION;

ALTER TABLE metabase.component_assembly DROP COLUMN "Index";

ALTER TABLE metabase.component_assembly DROP COLUMN "PrimeSurface";

DROP TYPE public.prime_surface;

DELETE FROM "__EFMigrationsHistory"
WHERE "MigrationId" = '20230306150049_AddIndexAndPrimeSurfaceToAssembly';

COMMIT;

