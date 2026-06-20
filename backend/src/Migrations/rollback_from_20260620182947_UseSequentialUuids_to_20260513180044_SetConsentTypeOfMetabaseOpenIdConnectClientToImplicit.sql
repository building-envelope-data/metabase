START TRANSACTION;
DO $EF$
BEGIN
    IF EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "MigrationId" = '20260620182947_UseSequentialUuids') THEN
    ALTER TABLE metabase."user" ALTER COLUMN "Id" SET DEFAULT (gen_random_uuid());
    END IF;
END $EF$;
DO $EF$
BEGIN
    IF EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "MigrationId" = '20260620182947_UseSequentialUuids') THEN
    ALTER TABLE metabase."OpenIddictTokens" ALTER COLUMN "Id" SET DEFAULT (gen_random_uuid());
    END IF;
END $EF$;
DO $EF$
BEGIN
    IF EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "MigrationId" = '20260620182947_UseSequentialUuids') THEN
    ALTER TABLE metabase."OpenIddictScopes" ALTER COLUMN "Id" SET DEFAULT (gen_random_uuid());
    END IF;
END $EF$;
DO $EF$
BEGIN
    IF EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "MigrationId" = '20260620182947_UseSequentialUuids') THEN
    ALTER TABLE metabase."OpenIddictAuthorizations" ALTER COLUMN "Id" SET DEFAULT (gen_random_uuid());
    END IF;
END $EF$;
DO $EF$
BEGIN
    IF EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "MigrationId" = '20260620182947_UseSequentialUuids') THEN
    ALTER TABLE metabase."OpenIddictApplications" ALTER COLUMN "Id" SET DEFAULT (gen_random_uuid());
    END IF;
END $EF$;
DO $EF$
BEGIN
    IF EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "MigrationId" = '20260620182947_UseSequentialUuids') THEN
    ALTER TABLE metabase.method ALTER COLUMN "Id" SET DEFAULT (gen_random_uuid());
    END IF;
END $EF$;
DO $EF$
BEGIN
    IF EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "MigrationId" = '20260620182947_UseSequentialUuids') THEN
    ALTER TABLE metabase.institution ALTER COLUMN "Id" SET DEFAULT (gen_random_uuid());
    END IF;
END $EF$;
DO $EF$
BEGIN
    IF EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "MigrationId" = '20260620182947_UseSequentialUuids') THEN
    ALTER TABLE metabase.gnu_pg_fingerprint ALTER COLUMN "Id" SET DEFAULT (gen_random_uuid());
    END IF;
END $EF$;
DO $EF$
BEGIN
    IF EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "MigrationId" = '20260620182947_UseSequentialUuids') THEN
    ALTER TABLE metabase.database ALTER COLUMN "Id" SET DEFAULT (gen_random_uuid());
    END IF;
END $EF$;
DO $EF$
BEGIN
    IF EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "MigrationId" = '20260620182947_UseSequentialUuids') THEN
    ALTER TABLE metabase.data_format ALTER COLUMN "Id" SET DEFAULT (gen_random_uuid());
    END IF;
END $EF$;
DO $EF$
BEGIN
    IF EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "MigrationId" = '20260620182947_UseSequentialUuids') THEN
    ALTER TABLE metabase.component ALTER COLUMN "Id" SET DEFAULT (gen_random_uuid());
    END IF;
END $EF$;
DO $EF$
BEGIN
    IF EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "MigrationId" = '20260620182947_UseSequentialUuids') THEN
    DELETE FROM "__EFMigrationsHistory"
    WHERE "MigrationId" = '20260620182947_UseSequentialUuids';
    END IF;
END $EF$;
COMMIT;

