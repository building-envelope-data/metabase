START TRANSACTION;
DO $EF$
BEGIN
    IF EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "MigrationId" = '20251128175152_AddParametersAndSourcesToMethods') THEN
    DROP TABLE metabase."MethodParameter";
    END IF;
END $EF$;
DO $EF$
BEGIN
    IF EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "MigrationId" = '20251128175152_AddParametersAndSourcesToMethods') THEN
    DROP TABLE metabase."MethodSource";
    END IF;
END $EF$;
DO $EF$
BEGIN
    IF EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "MigrationId" = '20251128175152_AddParametersAndSourcesToMethods') THEN
    ALTER TABLE metabase.method DROP COLUMN "Reference_Exists";
    END IF;
END $EF$;
DO $EF$
BEGIN
    IF EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "MigrationId" = '20251128175152_AddParametersAndSourcesToMethods') THEN
    ALTER TABLE metabase.data_format DROP COLUMN "Reference_Exists";
    END IF;
END $EF$;
DO $EF$
BEGIN
    IF EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "MigrationId" = '20251128175152_AddParametersAndSourcesToMethods') THEN
    ALTER TABLE metabase.method RENAME COLUMN "Reference_Standard_Year" TO "Standard_Year";
    END IF;
END $EF$;
DO $EF$
BEGIN
    IF EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "MigrationId" = '20251128175152_AddParametersAndSourcesToMethods') THEN
    ALTER TABLE metabase.method RENAME COLUMN "Reference_Standard_Title" TO "Standard_Title";
    END IF;
END $EF$;
DO $EF$
BEGIN
    IF EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "MigrationId" = '20251128175152_AddParametersAndSourcesToMethods') THEN
    ALTER TABLE metabase.method RENAME COLUMN "Reference_Standard_Standardizers" TO "Standard_Standardizers";
    END IF;
END $EF$;
DO $EF$
BEGIN
    IF EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "MigrationId" = '20251128175152_AddParametersAndSourcesToMethods') THEN
    ALTER TABLE metabase.method RENAME COLUMN "Reference_Standard_Section" TO "Standard_Section";
    END IF;
END $EF$;
DO $EF$
BEGIN
    IF EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "MigrationId" = '20251128175152_AddParametersAndSourcesToMethods') THEN
    ALTER TABLE metabase.method RENAME COLUMN "Reference_Standard_Numeration_Suffix" TO "Standard_Numeration_Suffix";
    END IF;
END $EF$;
DO $EF$
BEGIN
    IF EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "MigrationId" = '20251128175152_AddParametersAndSourcesToMethods') THEN
    ALTER TABLE metabase.method RENAME COLUMN "Reference_Standard_Numeration_Prefix" TO "Standard_Numeration_Prefix";
    END IF;
END $EF$;
DO $EF$
BEGIN
    IF EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "MigrationId" = '20251128175152_AddParametersAndSourcesToMethods') THEN
    ALTER TABLE metabase.method RENAME COLUMN "Reference_Standard_Numeration_MainNumber" TO "Standard_Numeration_MainNumber";
    END IF;
END $EF$;
DO $EF$
BEGIN
    IF EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "MigrationId" = '20251128175152_AddParametersAndSourcesToMethods') THEN
    ALTER TABLE metabase.method RENAME COLUMN "Reference_Standard_Locator" TO "Standard_Locator";
    END IF;
END $EF$;
DO $EF$
BEGIN
    IF EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "MigrationId" = '20251128175152_AddParametersAndSourcesToMethods') THEN
    ALTER TABLE metabase.method RENAME COLUMN "Reference_Standard_Abstract" TO "Standard_Abstract";
    END IF;
END $EF$;
DO $EF$
BEGIN
    IF EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "MigrationId" = '20251128175152_AddParametersAndSourcesToMethods') THEN
    ALTER TABLE metabase.method RENAME COLUMN "Reference_Publication_WebAddress" TO "Publication_WebAddress";
    END IF;
END $EF$;
DO $EF$
BEGIN
    IF EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "MigrationId" = '20251128175152_AddParametersAndSourcesToMethods') THEN
    ALTER TABLE metabase.method RENAME COLUMN "Reference_Publication_Urn" TO "Publication_Urn";
    END IF;
END $EF$;
DO $EF$
BEGIN
    IF EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "MigrationId" = '20251128175152_AddParametersAndSourcesToMethods') THEN
    ALTER TABLE metabase.method RENAME COLUMN "Reference_Publication_Title" TO "Publication_Title";
    END IF;
END $EF$;
DO $EF$
BEGIN
    IF EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "MigrationId" = '20251128175152_AddParametersAndSourcesToMethods') THEN
    ALTER TABLE metabase.method RENAME COLUMN "Reference_Publication_Section" TO "Publication_Section";
    END IF;
END $EF$;
DO $EF$
BEGIN
    IF EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "MigrationId" = '20251128175152_AddParametersAndSourcesToMethods') THEN
    ALTER TABLE metabase.method RENAME COLUMN "Reference_Publication_Doi" TO "Publication_Doi";
    END IF;
END $EF$;
DO $EF$
BEGIN
    IF EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "MigrationId" = '20251128175152_AddParametersAndSourcesToMethods') THEN
    ALTER TABLE metabase.method RENAME COLUMN "Reference_Publication_Authors" TO "Publication_Authors";
    END IF;
END $EF$;
DO $EF$
BEGIN
    IF EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "MigrationId" = '20251128175152_AddParametersAndSourcesToMethods') THEN
    ALTER TABLE metabase.method RENAME COLUMN "Reference_Publication_ArXiv" TO "Publication_ArXiv";
    END IF;
END $EF$;
DO $EF$
BEGIN
    IF EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "MigrationId" = '20251128175152_AddParametersAndSourcesToMethods') THEN
    ALTER TABLE metabase.method RENAME COLUMN "Reference_Publication_Abstract" TO "Publication_Abstract";
    END IF;
END $EF$;
DO $EF$
BEGIN
    IF EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "MigrationId" = '20251128175152_AddParametersAndSourcesToMethods') THEN
    ALTER TABLE metabase.data_format RENAME COLUMN "Reference_Standard_Year" TO "Standard_Year";
    END IF;
END $EF$;
DO $EF$
BEGIN
    IF EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "MigrationId" = '20251128175152_AddParametersAndSourcesToMethods') THEN
    ALTER TABLE metabase.data_format RENAME COLUMN "Reference_Standard_Title" TO "Standard_Title";
    END IF;
END $EF$;
DO $EF$
BEGIN
    IF EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "MigrationId" = '20251128175152_AddParametersAndSourcesToMethods') THEN
    ALTER TABLE metabase.data_format RENAME COLUMN "Reference_Standard_Standardizers" TO "Standard_Standardizers";
    END IF;
END $EF$;
DO $EF$
BEGIN
    IF EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "MigrationId" = '20251128175152_AddParametersAndSourcesToMethods') THEN
    ALTER TABLE metabase.data_format RENAME COLUMN "Reference_Standard_Section" TO "Standard_Section";
    END IF;
END $EF$;
DO $EF$
BEGIN
    IF EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "MigrationId" = '20251128175152_AddParametersAndSourcesToMethods') THEN
    ALTER TABLE metabase.data_format RENAME COLUMN "Reference_Standard_Numeration_Suffix" TO "Standard_Numeration_Suffix";
    END IF;
END $EF$;
DO $EF$
BEGIN
    IF EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "MigrationId" = '20251128175152_AddParametersAndSourcesToMethods') THEN
    ALTER TABLE metabase.data_format RENAME COLUMN "Reference_Standard_Numeration_Prefix" TO "Standard_Numeration_Prefix";
    END IF;
END $EF$;
DO $EF$
BEGIN
    IF EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "MigrationId" = '20251128175152_AddParametersAndSourcesToMethods') THEN
    ALTER TABLE metabase.data_format RENAME COLUMN "Reference_Standard_Numeration_MainNumber" TO "Standard_Numeration_MainNumber";
    END IF;
END $EF$;
DO $EF$
BEGIN
    IF EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "MigrationId" = '20251128175152_AddParametersAndSourcesToMethods') THEN
    ALTER TABLE metabase.data_format RENAME COLUMN "Reference_Standard_Locator" TO "Standard_Locator";
    END IF;
END $EF$;
DO $EF$
BEGIN
    IF EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "MigrationId" = '20251128175152_AddParametersAndSourcesToMethods') THEN
    ALTER TABLE metabase.data_format RENAME COLUMN "Reference_Standard_Abstract" TO "Standard_Abstract";
    END IF;
END $EF$;
DO $EF$
BEGIN
    IF EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "MigrationId" = '20251128175152_AddParametersAndSourcesToMethods') THEN
    ALTER TABLE metabase.data_format RENAME COLUMN "Reference_Publication_WebAddress" TO "Publication_WebAddress";
    END IF;
END $EF$;
DO $EF$
BEGIN
    IF EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "MigrationId" = '20251128175152_AddParametersAndSourcesToMethods') THEN
    ALTER TABLE metabase.data_format RENAME COLUMN "Reference_Publication_Urn" TO "Publication_Urn";
    END IF;
END $EF$;
DO $EF$
BEGIN
    IF EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "MigrationId" = '20251128175152_AddParametersAndSourcesToMethods') THEN
    ALTER TABLE metabase.data_format RENAME COLUMN "Reference_Publication_Title" TO "Publication_Title";
    END IF;
END $EF$;
DO $EF$
BEGIN
    IF EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "MigrationId" = '20251128175152_AddParametersAndSourcesToMethods') THEN
    ALTER TABLE metabase.data_format RENAME COLUMN "Reference_Publication_Section" TO "Publication_Section";
    END IF;
END $EF$;
DO $EF$
BEGIN
    IF EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "MigrationId" = '20251128175152_AddParametersAndSourcesToMethods') THEN
    ALTER TABLE metabase.data_format RENAME COLUMN "Reference_Publication_Doi" TO "Publication_Doi";
    END IF;
END $EF$;
DO $EF$
BEGIN
    IF EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "MigrationId" = '20251128175152_AddParametersAndSourcesToMethods') THEN
    ALTER TABLE metabase.data_format RENAME COLUMN "Reference_Publication_Authors" TO "Publication_Authors";
    END IF;
END $EF$;
DO $EF$
BEGIN
    IF EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "MigrationId" = '20251128175152_AddParametersAndSourcesToMethods') THEN
    ALTER TABLE metabase.data_format RENAME COLUMN "Reference_Publication_ArXiv" TO "Publication_ArXiv";
    END IF;
END $EF$;
DO $EF$
BEGIN
    IF EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "MigrationId" = '20251128175152_AddParametersAndSourcesToMethods') THEN
    ALTER TABLE metabase.data_format RENAME COLUMN "Reference_Publication_Abstract" TO "Publication_Abstract";
    END IF;
END $EF$;
DO $EF$
BEGIN
    IF EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "MigrationId" = '20251128175152_AddParametersAndSourcesToMethods') THEN
    DELETE FROM "__EFMigrationsHistory"
    WHERE "MigrationId" = '20251128175152_AddParametersAndSourcesToMethods';
    END IF;
END $EF$;
COMMIT;

