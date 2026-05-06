START TRANSACTION;
DO $EF$
BEGIN
    IF EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "MigrationId" = '20260402204629_AddUpdatedAndCreatedAtTimestamps') THEN
    ALTER TABLE metabase.user_method_developer DROP COLUMN "CreatedAt";
    END IF;
END $EF$;
DO $EF$
BEGIN
    IF EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "MigrationId" = '20260402204629_AddUpdatedAndCreatedAtTimestamps') THEN
    ALTER TABLE metabase.user_method_developer DROP COLUMN "UpdatedAt";
    END IF;
END $EF$;
DO $EF$
BEGIN
    IF EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "MigrationId" = '20260402204629_AddUpdatedAndCreatedAtTimestamps') THEN
    ALTER TABLE metabase."user" DROP COLUMN "CreatedAt";
    END IF;
END $EF$;
DO $EF$
BEGIN
    IF EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "MigrationId" = '20260402204629_AddUpdatedAndCreatedAtTimestamps') THEN
    ALTER TABLE metabase."user" DROP COLUMN "UpdatedAt";
    END IF;
END $EF$;
DO $EF$
BEGIN
    IF EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "MigrationId" = '20260402204629_AddUpdatedAndCreatedAtTimestamps') THEN
    ALTER TABLE metabase."OpenIddictTokens" DROP COLUMN "CreatedAt";
    END IF;
END $EF$;
DO $EF$
BEGIN
    IF EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "MigrationId" = '20260402204629_AddUpdatedAndCreatedAtTimestamps') THEN
    ALTER TABLE metabase."OpenIddictTokens" DROP COLUMN "UpdatedAt";
    END IF;
END $EF$;
DO $EF$
BEGIN
    IF EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "MigrationId" = '20260402204629_AddUpdatedAndCreatedAtTimestamps') THEN
    ALTER TABLE metabase."OpenIddictScopes" DROP COLUMN "CreatedAt";
    END IF;
END $EF$;
DO $EF$
BEGIN
    IF EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "MigrationId" = '20260402204629_AddUpdatedAndCreatedAtTimestamps') THEN
    ALTER TABLE metabase."OpenIddictScopes" DROP COLUMN "UpdatedAt";
    END IF;
END $EF$;
DO $EF$
BEGIN
    IF EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "MigrationId" = '20260402204629_AddUpdatedAndCreatedAtTimestamps') THEN
    ALTER TABLE metabase."OpenIddictAuthorizations" DROP COLUMN "CreatedAt";
    END IF;
END $EF$;
DO $EF$
BEGIN
    IF EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "MigrationId" = '20260402204629_AddUpdatedAndCreatedAtTimestamps') THEN
    ALTER TABLE metabase."OpenIddictAuthorizations" DROP COLUMN "UpdatedAt";
    END IF;
END $EF$;
DO $EF$
BEGIN
    IF EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "MigrationId" = '20260402204629_AddUpdatedAndCreatedAtTimestamps') THEN
    ALTER TABLE metabase."OpenIddictApplications" DROP COLUMN "CreatedAt";
    END IF;
END $EF$;
DO $EF$
BEGIN
    IF EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "MigrationId" = '20260402204629_AddUpdatedAndCreatedAtTimestamps') THEN
    ALTER TABLE metabase."OpenIddictApplications" DROP COLUMN "UpdatedAt";
    END IF;
END $EF$;
DO $EF$
BEGIN
    IF EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "MigrationId" = '20260402204629_AddUpdatedAndCreatedAtTimestamps') THEN
    ALTER TABLE metabase.method DROP COLUMN "CreatedAt";
    END IF;
END $EF$;
DO $EF$
BEGIN
    IF EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "MigrationId" = '20260402204629_AddUpdatedAndCreatedAtTimestamps') THEN
    ALTER TABLE metabase.method DROP COLUMN "UpdatedAt";
    END IF;
END $EF$;
DO $EF$
BEGIN
    IF EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "MigrationId" = '20260402204629_AddUpdatedAndCreatedAtTimestamps') THEN
    ALTER TABLE metabase.institution_representative DROP COLUMN "CreatedAt";
    END IF;
END $EF$;
DO $EF$
BEGIN
    IF EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "MigrationId" = '20260402204629_AddUpdatedAndCreatedAtTimestamps') THEN
    ALTER TABLE metabase.institution_representative DROP COLUMN "UpdatedAt";
    END IF;
END $EF$;
DO $EF$
BEGIN
    IF EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "MigrationId" = '20260402204629_AddUpdatedAndCreatedAtTimestamps') THEN
    ALTER TABLE metabase.institution_method_developer DROP COLUMN "CreatedAt";
    END IF;
END $EF$;
DO $EF$
BEGIN
    IF EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "MigrationId" = '20260402204629_AddUpdatedAndCreatedAtTimestamps') THEN
    ALTER TABLE metabase.institution_method_developer DROP COLUMN "UpdatedAt";
    END IF;
END $EF$;
DO $EF$
BEGIN
    IF EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "MigrationId" = '20260402204629_AddUpdatedAndCreatedAtTimestamps') THEN
    ALTER TABLE metabase.institution DROP COLUMN "CreatedAt";
    END IF;
END $EF$;
DO $EF$
BEGIN
    IF EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "MigrationId" = '20260402204629_AddUpdatedAndCreatedAtTimestamps') THEN
    ALTER TABLE metabase.institution DROP COLUMN "UpdatedAt";
    END IF;
END $EF$;
DO $EF$
BEGIN
    IF EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "MigrationId" = '20260402204629_AddUpdatedAndCreatedAtTimestamps') THEN
    ALTER TABLE metabase.gnu_pg_fingerprint DROP COLUMN "UpdatedAt";
    END IF;
END $EF$;
DO $EF$
BEGIN
    IF EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "MigrationId" = '20260402204629_AddUpdatedAndCreatedAtTimestamps') THEN
    ALTER TABLE metabase.database DROP COLUMN "CreatedAt";
    END IF;
END $EF$;
DO $EF$
BEGIN
    IF EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "MigrationId" = '20260402204629_AddUpdatedAndCreatedAtTimestamps') THEN
    ALTER TABLE metabase.database DROP COLUMN "UpdatedAt";
    END IF;
END $EF$;
DO $EF$
BEGIN
    IF EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "MigrationId" = '20260402204629_AddUpdatedAndCreatedAtTimestamps') THEN
    ALTER TABLE metabase.data_format DROP COLUMN "CreatedAt";
    END IF;
END $EF$;
DO $EF$
BEGIN
    IF EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "MigrationId" = '20260402204629_AddUpdatedAndCreatedAtTimestamps') THEN
    ALTER TABLE metabase.data_format DROP COLUMN "UpdatedAt";
    END IF;
END $EF$;
DO $EF$
BEGIN
    IF EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "MigrationId" = '20260402204629_AddUpdatedAndCreatedAtTimestamps') THEN
    ALTER TABLE metabase.component_variant DROP COLUMN "CreatedAt";
    END IF;
END $EF$;
DO $EF$
BEGIN
    IF EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "MigrationId" = '20260402204629_AddUpdatedAndCreatedAtTimestamps') THEN
    ALTER TABLE metabase.component_variant DROP COLUMN "UpdatedAt";
    END IF;
END $EF$;
DO $EF$
BEGIN
    IF EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "MigrationId" = '20260402204629_AddUpdatedAndCreatedAtTimestamps') THEN
    ALTER TABLE metabase.component_manufacturer DROP COLUMN "CreatedAt";
    END IF;
END $EF$;
DO $EF$
BEGIN
    IF EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "MigrationId" = '20260402204629_AddUpdatedAndCreatedAtTimestamps') THEN
    ALTER TABLE metabase.component_manufacturer DROP COLUMN "UpdatedAt";
    END IF;
END $EF$;
DO $EF$
BEGIN
    IF EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "MigrationId" = '20260402204629_AddUpdatedAndCreatedAtTimestamps') THEN
    ALTER TABLE metabase.component_concretization_and_generalization DROP COLUMN "CreatedAt";
    END IF;
END $EF$;
DO $EF$
BEGIN
    IF EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "MigrationId" = '20260402204629_AddUpdatedAndCreatedAtTimestamps') THEN
    ALTER TABLE metabase.component_concretization_and_generalization DROP COLUMN "UpdatedAt";
    END IF;
END $EF$;
DO $EF$
BEGIN
    IF EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "MigrationId" = '20260402204629_AddUpdatedAndCreatedAtTimestamps') THEN
    ALTER TABLE metabase.component_assembly DROP COLUMN "CreatedAt";
    END IF;
END $EF$;
DO $EF$
BEGIN
    IF EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "MigrationId" = '20260402204629_AddUpdatedAndCreatedAtTimestamps') THEN
    ALTER TABLE metabase.component_assembly DROP COLUMN "UpdatedAt";
    END IF;
END $EF$;
DO $EF$
BEGIN
    IF EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "MigrationId" = '20260402204629_AddUpdatedAndCreatedAtTimestamps') THEN
    ALTER TABLE metabase.component DROP COLUMN "CreatedAt";
    END IF;
END $EF$;
DO $EF$
BEGIN
    IF EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "MigrationId" = '20260402204629_AddUpdatedAndCreatedAtTimestamps') THEN
    ALTER TABLE metabase.component DROP COLUMN "UpdatedAt";
    END IF;
END $EF$;
DO $EF$
BEGIN
    IF EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "MigrationId" = '20260402204629_AddUpdatedAndCreatedAtTimestamps') THEN
    ALTER TABLE metabase."user" ALTER COLUMN "Id" DROP DEFAULT;
    END IF;
END $EF$;
DO $EF$
BEGIN
    IF EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "MigrationId" = '20260402204629_AddUpdatedAndCreatedAtTimestamps') THEN
    ALTER TABLE metabase."OpenIddictTokens" ALTER COLUMN "Id" DROP DEFAULT;
    END IF;
END $EF$;
DO $EF$
BEGIN
    IF EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "MigrationId" = '20260402204629_AddUpdatedAndCreatedAtTimestamps') THEN
    ALTER TABLE metabase."OpenIddictScopes" ALTER COLUMN "Id" DROP DEFAULT;
    END IF;
END $EF$;
DO $EF$
BEGIN
    IF EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "MigrationId" = '20260402204629_AddUpdatedAndCreatedAtTimestamps') THEN
    ALTER TABLE metabase."OpenIddictAuthorizations" ALTER COLUMN "Id" DROP DEFAULT;
    END IF;
END $EF$;
DO $EF$
BEGIN
    IF EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "MigrationId" = '20260402204629_AddUpdatedAndCreatedAtTimestamps') THEN
    ALTER TABLE metabase."OpenIddictApplications" ALTER COLUMN "Id" DROP DEFAULT;
    END IF;
END $EF$;
DO $EF$
BEGIN
    IF EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "MigrationId" = '20260402204629_AddUpdatedAndCreatedAtTimestamps') THEN
    ALTER TABLE metabase.gnu_pg_fingerprint ALTER COLUMN "CreatedAt" DROP DEFAULT;
    END IF;
END $EF$;
DO $EF$
BEGIN
    IF EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "MigrationId" = '20260402204629_AddUpdatedAndCreatedAtTimestamps') THEN
    DELETE FROM "__EFMigrationsHistory"
    WHERE "MigrationId" = '20260402204629_AddUpdatedAndCreatedAtTimestamps';
    END IF;
END $EF$;
COMMIT;

