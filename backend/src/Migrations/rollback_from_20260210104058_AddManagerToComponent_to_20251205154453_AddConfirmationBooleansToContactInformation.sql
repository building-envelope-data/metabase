START TRANSACTION;
DO $EF$
BEGIN
    IF EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "MigrationId" = '20260210104058_AddManagerToComponent') THEN
    ALTER TABLE metabase.component DROP CONSTRAINT "FK_component_institution_ManagerId";
    END IF;
END $EF$;
DO $EF$
BEGIN
    IF EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "MigrationId" = '20260210104058_AddManagerToComponent') THEN
    DROP INDEX metabase."IX_component_ManagerId";
    END IF;
END $EF$;
DO $EF$
BEGIN
    IF EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "MigrationId" = '20260210104058_AddManagerToComponent') THEN
    ALTER TABLE metabase.method DROP COLUMN "Reference_Publication_Exists";
    END IF;
END $EF$;
DO $EF$
BEGIN
    IF EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "MigrationId" = '20260210104058_AddManagerToComponent') THEN
    ALTER TABLE metabase.method DROP COLUMN "Reference_Standard_Exists";
    END IF;
END $EF$;
DO $EF$
BEGIN
    IF EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "MigrationId" = '20260210104058_AddManagerToComponent') THEN
    ALTER TABLE metabase.data_format DROP COLUMN "Reference_Publication_Exists";
    END IF;
END $EF$;
DO $EF$
BEGIN
    IF EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "MigrationId" = '20260210104058_AddManagerToComponent') THEN
    ALTER TABLE metabase.data_format DROP COLUMN "Reference_Standard_Exists";
    END IF;
END $EF$;
DO $EF$
BEGIN
    IF EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "MigrationId" = '20260210104058_AddManagerToComponent') THEN
    ALTER TABLE metabase.component DROP COLUMN "ManagerId";
    END IF;
END $EF$;
DO $EF$
BEGIN
    IF EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "MigrationId" = '20260210104058_AddManagerToComponent') THEN
    ALTER TABLE metabase.component DROP COLUMN "PrimeDirection_Reference_Publication_Exists";
    END IF;
END $EF$;
DO $EF$
BEGIN
    IF EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "MigrationId" = '20260210104058_AddManagerToComponent') THEN
    ALTER TABLE metabase.component DROP COLUMN "PrimeDirection_Reference_Standard_Exists";
    END IF;
END $EF$;
DO $EF$
BEGIN
    IF EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "MigrationId" = '20260210104058_AddManagerToComponent') THEN
    ALTER TABLE metabase.component DROP COLUMN "PrimeSurface_Reference_Publication_Exists";
    END IF;
END $EF$;
DO $EF$
BEGIN
    IF EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "MigrationId" = '20260210104058_AddManagerToComponent') THEN
    ALTER TABLE metabase.component DROP COLUMN "PrimeSurface_Reference_Standard_Exists";
    END IF;
END $EF$;
DO $EF$
BEGIN
    IF EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "MigrationId" = '20260210104058_AddManagerToComponent') THEN
    ALTER TABLE metabase.component DROP COLUMN "SwitchableLayers_Reference_Publication_Exists";
    END IF;
END $EF$;
DO $EF$
BEGIN
    IF EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "MigrationId" = '20260210104058_AddManagerToComponent') THEN
    ALTER TABLE metabase.component DROP COLUMN "SwitchableLayers_Reference_Standard_Exists";
    END IF;
END $EF$;
DO $EF$
BEGIN
    IF EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "MigrationId" = '20260210104058_AddManagerToComponent') THEN
    DELETE FROM "__EFMigrationsHistory"
    WHERE "MigrationId" = '20260210104058_AddManagerToComponent';
    END IF;
END $EF$;
COMMIT;

