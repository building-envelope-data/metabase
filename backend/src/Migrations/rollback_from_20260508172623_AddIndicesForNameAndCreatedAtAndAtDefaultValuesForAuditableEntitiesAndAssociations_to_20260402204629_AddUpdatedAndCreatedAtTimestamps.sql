START TRANSACTION;
DO $EF$
BEGIN
    IF EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "MigrationId" = '20260508172623_AddIndicesForNameAndCreatedAtAndAtDefaultValuesForAuditableEntitiesAndAssociations') THEN
    DROP INDEX metabase."IX_user_CreatedAt_Id";
    END IF;
END $EF$;
DO $EF$
BEGIN
    IF EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "MigrationId" = '20260508172623_AddIndicesForNameAndCreatedAtAndAtDefaultValuesForAuditableEntitiesAndAssociations') THEN
    DROP INDEX metabase."IX_user_Name_Id";
    END IF;
END $EF$;
DO $EF$
BEGIN
    IF EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "MigrationId" = '20260508172623_AddIndicesForNameAndCreatedAtAndAtDefaultValuesForAuditableEntitiesAndAssociations') THEN
    DROP INDEX metabase."IX_OpenIddictTokens_CreatedAt_Id";
    END IF;
END $EF$;
DO $EF$
BEGIN
    IF EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "MigrationId" = '20260508172623_AddIndicesForNameAndCreatedAtAndAtDefaultValuesForAuditableEntitiesAndAssociations') THEN
    DROP INDEX metabase."IX_OpenIddictScopes_CreatedAt_Id";
    END IF;
END $EF$;
DO $EF$
BEGIN
    IF EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "MigrationId" = '20260508172623_AddIndicesForNameAndCreatedAtAndAtDefaultValuesForAuditableEntitiesAndAssociations') THEN
    DROP INDEX metabase."IX_OpenIddictAuthorizations_CreatedAt_Id";
    END IF;
END $EF$;
DO $EF$
BEGIN
    IF EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "MigrationId" = '20260508172623_AddIndicesForNameAndCreatedAtAndAtDefaultValuesForAuditableEntitiesAndAssociations') THEN
    DROP INDEX metabase."IX_OpenIddictApplications_CreatedAt_Id";
    END IF;
END $EF$;
DO $EF$
BEGIN
    IF EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "MigrationId" = '20260508172623_AddIndicesForNameAndCreatedAtAndAtDefaultValuesForAuditableEntitiesAndAssociations') THEN
    DROP INDEX metabase."IX_method_CreatedAt_Id";
    END IF;
END $EF$;
DO $EF$
BEGIN
    IF EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "MigrationId" = '20260508172623_AddIndicesForNameAndCreatedAtAndAtDefaultValuesForAuditableEntitiesAndAssociations') THEN
    DROP INDEX metabase."IX_method_Name_Id";
    END IF;
END $EF$;
DO $EF$
BEGIN
    IF EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "MigrationId" = '20260508172623_AddIndicesForNameAndCreatedAtAndAtDefaultValuesForAuditableEntitiesAndAssociations') THEN
    DROP INDEX metabase."IX_institution_CreatedAt_Id";
    END IF;
END $EF$;
DO $EF$
BEGIN
    IF EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "MigrationId" = '20260508172623_AddIndicesForNameAndCreatedAtAndAtDefaultValuesForAuditableEntitiesAndAssociations') THEN
    DROP INDEX metabase."IX_institution_Name_Id";
    END IF;
END $EF$;
DO $EF$
BEGIN
    IF EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "MigrationId" = '20260508172623_AddIndicesForNameAndCreatedAtAndAtDefaultValuesForAuditableEntitiesAndAssociations') THEN
    DROP INDEX metabase."IX_gnu_pg_fingerprint_CreatedAt_Id";
    END IF;
END $EF$;
DO $EF$
BEGIN
    IF EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "MigrationId" = '20260508172623_AddIndicesForNameAndCreatedAtAndAtDefaultValuesForAuditableEntitiesAndAssociations') THEN
    DROP INDEX metabase."IX_database_CreatedAt_Id";
    END IF;
END $EF$;
DO $EF$
BEGIN
    IF EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "MigrationId" = '20260508172623_AddIndicesForNameAndCreatedAtAndAtDefaultValuesForAuditableEntitiesAndAssociations') THEN
    DROP INDEX metabase."IX_database_Name_Id";
    END IF;
END $EF$;
DO $EF$
BEGIN
    IF EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "MigrationId" = '20260508172623_AddIndicesForNameAndCreatedAtAndAtDefaultValuesForAuditableEntitiesAndAssociations') THEN
    DROP INDEX metabase."IX_data_format_CreatedAt_Id";
    END IF;
END $EF$;
DO $EF$
BEGIN
    IF EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "MigrationId" = '20260508172623_AddIndicesForNameAndCreatedAtAndAtDefaultValuesForAuditableEntitiesAndAssociations') THEN
    DROP INDEX metabase."IX_data_format_Name_Id";
    END IF;
END $EF$;
DO $EF$
BEGIN
    IF EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "MigrationId" = '20260508172623_AddIndicesForNameAndCreatedAtAndAtDefaultValuesForAuditableEntitiesAndAssociations') THEN
    DROP INDEX metabase."IX_component_CreatedAt_Id";
    END IF;
END $EF$;
DO $EF$
BEGIN
    IF EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "MigrationId" = '20260508172623_AddIndicesForNameAndCreatedAtAndAtDefaultValuesForAuditableEntitiesAndAssociations') THEN
    DROP INDEX metabase."IX_component_Name_Id";
    END IF;
END $EF$;
DO $EF$
BEGIN
    IF EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "MigrationId" = '20260508172623_AddIndicesForNameAndCreatedAtAndAtDefaultValuesForAuditableEntitiesAndAssociations') THEN
    ALTER TABLE metabase."OpenIddictScopes" ALTER COLUMN "UpdatedAt" DROP DEFAULT;
    END IF;
END $EF$;
DO $EF$
BEGIN
    IF EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "MigrationId" = '20260508172623_AddIndicesForNameAndCreatedAtAndAtDefaultValuesForAuditableEntitiesAndAssociations') THEN
    ALTER TABLE metabase."OpenIddictScopes" ALTER COLUMN "CreatedAt" DROP DEFAULT;
    END IF;
END $EF$;
DO $EF$
BEGIN
    IF EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "MigrationId" = '20260508172623_AddIndicesForNameAndCreatedAtAndAtDefaultValuesForAuditableEntitiesAndAssociations') THEN
    DELETE FROM "__EFMigrationsHistory"
    WHERE "MigrationId" = '20260508172623_AddIndicesForNameAndCreatedAtAndAtDefaultValuesForAuditableEntitiesAndAssociations';
    END IF;
END $EF$;
COMMIT;

