START TRANSACTION;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "MigrationId" = '20260402204629_AddUpdatedAndCreatedAtTimestamps') THEN
    ALTER TABLE metabase.user_method_developer ADD "CreatedAt" timestamp with time zone NOT NULL DEFAULT (now());
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "MigrationId" = '20260402204629_AddUpdatedAndCreatedAtTimestamps') THEN
    ALTER TABLE metabase.user_method_developer ADD "UpdatedAt" timestamp with time zone NOT NULL DEFAULT (now());
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "MigrationId" = '20260402204629_AddUpdatedAndCreatedAtTimestamps') THEN
    ALTER TABLE metabase."user" ALTER COLUMN "Id" SET DEFAULT (gen_random_uuid());
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "MigrationId" = '20260402204629_AddUpdatedAndCreatedAtTimestamps') THEN
    ALTER TABLE metabase."user" ADD "CreatedAt" timestamp with time zone NOT NULL DEFAULT (now());
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "MigrationId" = '20260402204629_AddUpdatedAndCreatedAtTimestamps') THEN
    ALTER TABLE metabase."user" ADD "UpdatedAt" timestamp with time zone NOT NULL DEFAULT (now());
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "MigrationId" = '20260402204629_AddUpdatedAndCreatedAtTimestamps') THEN
    ALTER TABLE metabase."OpenIddictTokens" ALTER COLUMN "Id" SET DEFAULT (gen_random_uuid());
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "MigrationId" = '20260402204629_AddUpdatedAndCreatedAtTimestamps') THEN
    ALTER TABLE metabase."OpenIddictTokens" ADD "CreatedAt" timestamp with time zone NOT NULL DEFAULT (now());
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "MigrationId" = '20260402204629_AddUpdatedAndCreatedAtTimestamps') THEN
    ALTER TABLE metabase."OpenIddictTokens" ADD "UpdatedAt" timestamp with time zone NOT NULL DEFAULT (now());
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "MigrationId" = '20260402204629_AddUpdatedAndCreatedAtTimestamps') THEN
    ALTER TABLE metabase."OpenIddictScopes" ALTER COLUMN "Id" SET DEFAULT (gen_random_uuid());
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "MigrationId" = '20260402204629_AddUpdatedAndCreatedAtTimestamps') THEN
    ALTER TABLE metabase."OpenIddictScopes" ADD "CreatedAt" timestamp with time zone NOT NULL DEFAULT TIMESTAMPTZ '1970-01-01T00:00:00Z';
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "MigrationId" = '20260402204629_AddUpdatedAndCreatedAtTimestamps') THEN
    ALTER TABLE metabase."OpenIddictScopes" ADD "UpdatedAt" timestamp with time zone NOT NULL DEFAULT TIMESTAMPTZ '1970-01-01T00:00:00Z';
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "MigrationId" = '20260402204629_AddUpdatedAndCreatedAtTimestamps') THEN
    ALTER TABLE metabase."OpenIddictAuthorizations" ALTER COLUMN "Id" SET DEFAULT (gen_random_uuid());
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "MigrationId" = '20260402204629_AddUpdatedAndCreatedAtTimestamps') THEN
    ALTER TABLE metabase."OpenIddictAuthorizations" ADD "CreatedAt" timestamp with time zone NOT NULL DEFAULT (now());
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "MigrationId" = '20260402204629_AddUpdatedAndCreatedAtTimestamps') THEN
    ALTER TABLE metabase."OpenIddictAuthorizations" ADD "UpdatedAt" timestamp with time zone NOT NULL DEFAULT (now());
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "MigrationId" = '20260402204629_AddUpdatedAndCreatedAtTimestamps') THEN
    ALTER TABLE metabase."OpenIddictApplications" ALTER COLUMN "Id" SET DEFAULT (gen_random_uuid());
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "MigrationId" = '20260402204629_AddUpdatedAndCreatedAtTimestamps') THEN
    ALTER TABLE metabase."OpenIddictApplications" ADD "CreatedAt" timestamp with time zone NOT NULL DEFAULT (now());
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "MigrationId" = '20260402204629_AddUpdatedAndCreatedAtTimestamps') THEN
    ALTER TABLE metabase."OpenIddictApplications" ADD "UpdatedAt" timestamp with time zone NOT NULL DEFAULT (now());
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "MigrationId" = '20260402204629_AddUpdatedAndCreatedAtTimestamps') THEN
    ALTER TABLE metabase.method ADD "CreatedAt" timestamp with time zone NOT NULL DEFAULT (now());
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "MigrationId" = '20260402204629_AddUpdatedAndCreatedAtTimestamps') THEN
    ALTER TABLE metabase.method ADD "UpdatedAt" timestamp with time zone NOT NULL DEFAULT (now());
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "MigrationId" = '20260402204629_AddUpdatedAndCreatedAtTimestamps') THEN
    ALTER TABLE metabase.institution_representative ADD "CreatedAt" timestamp with time zone NOT NULL DEFAULT (now());
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "MigrationId" = '20260402204629_AddUpdatedAndCreatedAtTimestamps') THEN
    ALTER TABLE metabase.institution_representative ADD "UpdatedAt" timestamp with time zone NOT NULL DEFAULT (now());
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "MigrationId" = '20260402204629_AddUpdatedAndCreatedAtTimestamps') THEN
    ALTER TABLE metabase.institution_method_developer ADD "CreatedAt" timestamp with time zone NOT NULL DEFAULT (now());
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "MigrationId" = '20260402204629_AddUpdatedAndCreatedAtTimestamps') THEN
    ALTER TABLE metabase.institution_method_developer ADD "UpdatedAt" timestamp with time zone NOT NULL DEFAULT (now());
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "MigrationId" = '20260402204629_AddUpdatedAndCreatedAtTimestamps') THEN
    ALTER TABLE metabase.institution ADD "CreatedAt" timestamp with time zone NOT NULL DEFAULT (now());
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "MigrationId" = '20260402204629_AddUpdatedAndCreatedAtTimestamps') THEN
    ALTER TABLE metabase.institution ADD "UpdatedAt" timestamp with time zone NOT NULL DEFAULT (now());
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "MigrationId" = '20260402204629_AddUpdatedAndCreatedAtTimestamps') THEN
    ALTER TABLE metabase.gnu_pg_fingerprint ALTER COLUMN "CreatedAt" SET DEFAULT (now());
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "MigrationId" = '20260402204629_AddUpdatedAndCreatedAtTimestamps') THEN
    ALTER TABLE metabase.gnu_pg_fingerprint ADD "UpdatedAt" timestamp with time zone NOT NULL DEFAULT (now());
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "MigrationId" = '20260402204629_AddUpdatedAndCreatedAtTimestamps') THEN
    ALTER TABLE metabase.database ADD "CreatedAt" timestamp with time zone NOT NULL DEFAULT (now());
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "MigrationId" = '20260402204629_AddUpdatedAndCreatedAtTimestamps') THEN
    ALTER TABLE metabase.database ADD "UpdatedAt" timestamp with time zone NOT NULL DEFAULT (now());
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "MigrationId" = '20260402204629_AddUpdatedAndCreatedAtTimestamps') THEN
    ALTER TABLE metabase.data_format ADD "CreatedAt" timestamp with time zone NOT NULL DEFAULT (now());
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "MigrationId" = '20260402204629_AddUpdatedAndCreatedAtTimestamps') THEN
    ALTER TABLE metabase.data_format ADD "UpdatedAt" timestamp with time zone NOT NULL DEFAULT (now());
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "MigrationId" = '20260402204629_AddUpdatedAndCreatedAtTimestamps') THEN
    ALTER TABLE metabase.component_variant ADD "CreatedAt" timestamp with time zone NOT NULL DEFAULT (now());
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "MigrationId" = '20260402204629_AddUpdatedAndCreatedAtTimestamps') THEN
    ALTER TABLE metabase.component_variant ADD "UpdatedAt" timestamp with time zone NOT NULL DEFAULT (now());
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "MigrationId" = '20260402204629_AddUpdatedAndCreatedAtTimestamps') THEN
    ALTER TABLE metabase.component_manufacturer ADD "CreatedAt" timestamp with time zone NOT NULL DEFAULT (now());
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "MigrationId" = '20260402204629_AddUpdatedAndCreatedAtTimestamps') THEN
    ALTER TABLE metabase.component_manufacturer ADD "UpdatedAt" timestamp with time zone NOT NULL DEFAULT (now());
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "MigrationId" = '20260402204629_AddUpdatedAndCreatedAtTimestamps') THEN
    ALTER TABLE metabase.component_concretization_and_generalization ADD "CreatedAt" timestamp with time zone NOT NULL DEFAULT (now());
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "MigrationId" = '20260402204629_AddUpdatedAndCreatedAtTimestamps') THEN
    ALTER TABLE metabase.component_concretization_and_generalization ADD "UpdatedAt" timestamp with time zone NOT NULL DEFAULT (now());
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "MigrationId" = '20260402204629_AddUpdatedAndCreatedAtTimestamps') THEN
    ALTER TABLE metabase.component_assembly ADD "CreatedAt" timestamp with time zone NOT NULL DEFAULT (now());
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "MigrationId" = '20260402204629_AddUpdatedAndCreatedAtTimestamps') THEN
    ALTER TABLE metabase.component_assembly ADD "UpdatedAt" timestamp with time zone NOT NULL DEFAULT (now());
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "MigrationId" = '20260402204629_AddUpdatedAndCreatedAtTimestamps') THEN
    ALTER TABLE metabase.component ADD "CreatedAt" timestamp with time zone NOT NULL DEFAULT (now());
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "MigrationId" = '20260402204629_AddUpdatedAndCreatedAtTimestamps') THEN
    ALTER TABLE metabase.component ADD "UpdatedAt" timestamp with time zone NOT NULL DEFAULT (now());
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "MigrationId" = '20260402204629_AddUpdatedAndCreatedAtTimestamps') THEN
    INSERT INTO "__EFMigrationsHistory" ("MigrationId", "ProductVersion")
    VALUES ('20260402204629_AddUpdatedAndCreatedAtTimestamps', '10.0.5');
    END IF;
END $EF$;
COMMIT;

