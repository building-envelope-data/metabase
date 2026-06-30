START TRANSACTION;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "MigrationId" = '20260508172623_AddIndicesForNameAndCreatedAtAndAtDefaultValuesForAuditableEntitiesAndAssociations') THEN
    ALTER TABLE metabase."OpenIddictScopes" ALTER COLUMN "UpdatedAt" SET DEFAULT (now());
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "MigrationId" = '20260508172623_AddIndicesForNameAndCreatedAtAndAtDefaultValuesForAuditableEntitiesAndAssociations') THEN
    ALTER TABLE metabase."OpenIddictScopes" ALTER COLUMN "CreatedAt" SET DEFAULT (now());
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "MigrationId" = '20260508172623_AddIndicesForNameAndCreatedAtAndAtDefaultValuesForAuditableEntitiesAndAssociations') THEN
    CREATE UNIQUE INDEX "IX_user_CreatedAt_Id" ON metabase."user" ("CreatedAt", "Id");
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "MigrationId" = '20260508172623_AddIndicesForNameAndCreatedAtAndAtDefaultValuesForAuditableEntitiesAndAssociations') THEN
    CREATE UNIQUE INDEX "IX_user_Name_Id" ON metabase."user" ("Name", "Id");
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "MigrationId" = '20260508172623_AddIndicesForNameAndCreatedAtAndAtDefaultValuesForAuditableEntitiesAndAssociations') THEN
    CREATE UNIQUE INDEX "IX_OpenIddictTokens_CreatedAt_Id" ON metabase."OpenIddictTokens" ("CreatedAt", "Id");
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "MigrationId" = '20260508172623_AddIndicesForNameAndCreatedAtAndAtDefaultValuesForAuditableEntitiesAndAssociations') THEN
    CREATE UNIQUE INDEX "IX_OpenIddictScopes_CreatedAt_Id" ON metabase."OpenIddictScopes" ("CreatedAt", "Id");
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "MigrationId" = '20260508172623_AddIndicesForNameAndCreatedAtAndAtDefaultValuesForAuditableEntitiesAndAssociations') THEN
    CREATE UNIQUE INDEX "IX_OpenIddictAuthorizations_CreatedAt_Id" ON metabase."OpenIddictAuthorizations" ("CreatedAt", "Id");
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "MigrationId" = '20260508172623_AddIndicesForNameAndCreatedAtAndAtDefaultValuesForAuditableEntitiesAndAssociations') THEN
    CREATE UNIQUE INDEX "IX_OpenIddictApplications_CreatedAt_Id" ON metabase."OpenIddictApplications" ("CreatedAt", "Id");
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "MigrationId" = '20260508172623_AddIndicesForNameAndCreatedAtAndAtDefaultValuesForAuditableEntitiesAndAssociations') THEN
    CREATE UNIQUE INDEX "IX_method_CreatedAt_Id" ON metabase.method ("CreatedAt", "Id");
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "MigrationId" = '20260508172623_AddIndicesForNameAndCreatedAtAndAtDefaultValuesForAuditableEntitiesAndAssociations') THEN
    CREATE UNIQUE INDEX "IX_method_Name_Id" ON metabase.method ("Name", "Id");
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "MigrationId" = '20260508172623_AddIndicesForNameAndCreatedAtAndAtDefaultValuesForAuditableEntitiesAndAssociations') THEN
    CREATE UNIQUE INDEX "IX_institution_CreatedAt_Id" ON metabase.institution ("CreatedAt", "Id");
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "MigrationId" = '20260508172623_AddIndicesForNameAndCreatedAtAndAtDefaultValuesForAuditableEntitiesAndAssociations') THEN
    CREATE UNIQUE INDEX "IX_institution_Name_Id" ON metabase.institution ("Name", "Id");
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "MigrationId" = '20260508172623_AddIndicesForNameAndCreatedAtAndAtDefaultValuesForAuditableEntitiesAndAssociations') THEN
    CREATE UNIQUE INDEX "IX_gnu_pg_fingerprint_CreatedAt_Id" ON metabase.gnu_pg_fingerprint ("CreatedAt", "Id");
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "MigrationId" = '20260508172623_AddIndicesForNameAndCreatedAtAndAtDefaultValuesForAuditableEntitiesAndAssociations') THEN
    CREATE UNIQUE INDEX "IX_database_CreatedAt_Id" ON metabase.database ("CreatedAt", "Id");
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "MigrationId" = '20260508172623_AddIndicesForNameAndCreatedAtAndAtDefaultValuesForAuditableEntitiesAndAssociations') THEN
    CREATE UNIQUE INDEX "IX_database_Name_Id" ON metabase.database ("Name", "Id");
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "MigrationId" = '20260508172623_AddIndicesForNameAndCreatedAtAndAtDefaultValuesForAuditableEntitiesAndAssociations') THEN
    CREATE UNIQUE INDEX "IX_data_format_CreatedAt_Id" ON metabase.data_format ("CreatedAt", "Id");
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "MigrationId" = '20260508172623_AddIndicesForNameAndCreatedAtAndAtDefaultValuesForAuditableEntitiesAndAssociations') THEN
    CREATE UNIQUE INDEX "IX_data_format_Name_Id" ON metabase.data_format ("Name", "Id");
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "MigrationId" = '20260508172623_AddIndicesForNameAndCreatedAtAndAtDefaultValuesForAuditableEntitiesAndAssociations') THEN
    CREATE UNIQUE INDEX "IX_component_CreatedAt_Id" ON metabase.component ("CreatedAt", "Id");
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "MigrationId" = '20260508172623_AddIndicesForNameAndCreatedAtAndAtDefaultValuesForAuditableEntitiesAndAssociations') THEN
    CREATE UNIQUE INDEX "IX_component_Name_Id" ON metabase.component ("Name", "Id");
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "MigrationId" = '20260508172623_AddIndicesForNameAndCreatedAtAndAtDefaultValuesForAuditableEntitiesAndAssociations') THEN
    INSERT INTO "__EFMigrationsHistory" ("MigrationId", "ProductVersion")
    VALUES ('20260508172623_AddIndicesForNameAndCreatedAtAndAtDefaultValuesForAuditableEntitiesAndAssociations', '10.0.7');
    END IF;
END $EF$;
COMMIT;

