START TRANSACTION;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "MigrationId" = '20260210104058_AddManagerToComponent') THEN
    ALTER TABLE metabase.method ADD "Reference_Publication_Exists" boolean;
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "MigrationId" = '20260210104058_AddManagerToComponent') THEN
    ALTER TABLE metabase.method ADD "Reference_Standard_Exists" boolean;
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "MigrationId" = '20260210104058_AddManagerToComponent') THEN
    ALTER TABLE metabase.data_format ADD "Reference_Publication_Exists" boolean;
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "MigrationId" = '20260210104058_AddManagerToComponent') THEN
    ALTER TABLE metabase.data_format ADD "Reference_Standard_Exists" boolean;
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "MigrationId" = '20260210104058_AddManagerToComponent') THEN
    ALTER TABLE metabase.component ADD "ManagerId" uuid NOT NULL DEFAULT '00000000-0000-0000-0000-000000000000';
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "MigrationId" = '20260210104058_AddManagerToComponent') THEN
    ALTER TABLE metabase.component ADD "PrimeDirection_Reference_Publication_Exists" boolean;
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "MigrationId" = '20260210104058_AddManagerToComponent') THEN
    ALTER TABLE metabase.component ADD "PrimeDirection_Reference_Standard_Exists" boolean;
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "MigrationId" = '20260210104058_AddManagerToComponent') THEN
    ALTER TABLE metabase.component ADD "PrimeSurface_Reference_Publication_Exists" boolean;
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "MigrationId" = '20260210104058_AddManagerToComponent') THEN
    ALTER TABLE metabase.component ADD "PrimeSurface_Reference_Standard_Exists" boolean;
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "MigrationId" = '20260210104058_AddManagerToComponent') THEN
    ALTER TABLE metabase.component ADD "SwitchableLayers_Reference_Publication_Exists" boolean;
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "MigrationId" = '20260210104058_AddManagerToComponent') THEN
    ALTER TABLE metabase.component ADD "SwitchableLayers_Reference_Standard_Exists" boolean;
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "MigrationId" = '20260210104058_AddManagerToComponent') THEN
    CREATE INDEX "IX_component_ManagerId" ON metabase.component ("ManagerId");
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "MigrationId" = '20260210104058_AddManagerToComponent') THEN
    ALTER TABLE metabase.component ADD CONSTRAINT "FK_component_institution_ManagerId" FOREIGN KEY ("ManagerId") REFERENCES metabase.institution ("Id") ON DELETE RESTRICT;
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "MigrationId" = '20260210104058_AddManagerToComponent') THEN
    INSERT INTO "__EFMigrationsHistory" ("MigrationId", "ProductVersion")
    VALUES ('20260210104058_AddManagerToComponent', '10.0.2');
    END IF;
END $EF$;
COMMIT;

