START TRANSACTION;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "MigrationId" = '20251128175152_AddParametersAndSourcesToMethods') THEN
    ALTER TABLE metabase.method RENAME COLUMN "Standard_Year" TO "Reference_Standard_Year";
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "MigrationId" = '20251128175152_AddParametersAndSourcesToMethods') THEN
    ALTER TABLE metabase.method RENAME COLUMN "Standard_Title" TO "Reference_Standard_Title";
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "MigrationId" = '20251128175152_AddParametersAndSourcesToMethods') THEN
    ALTER TABLE metabase.method RENAME COLUMN "Standard_Standardizers" TO "Reference_Standard_Standardizers";
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "MigrationId" = '20251128175152_AddParametersAndSourcesToMethods') THEN
    ALTER TABLE metabase.method RENAME COLUMN "Standard_Section" TO "Reference_Standard_Section";
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "MigrationId" = '20251128175152_AddParametersAndSourcesToMethods') THEN
    ALTER TABLE metabase.method RENAME COLUMN "Standard_Numeration_Suffix" TO "Reference_Standard_Numeration_Suffix";
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "MigrationId" = '20251128175152_AddParametersAndSourcesToMethods') THEN
    ALTER TABLE metabase.method RENAME COLUMN "Standard_Numeration_Prefix" TO "Reference_Standard_Numeration_Prefix";
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "MigrationId" = '20251128175152_AddParametersAndSourcesToMethods') THEN
    ALTER TABLE metabase.method RENAME COLUMN "Standard_Numeration_MainNumber" TO "Reference_Standard_Numeration_MainNumber";
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "MigrationId" = '20251128175152_AddParametersAndSourcesToMethods') THEN
    ALTER TABLE metabase.method RENAME COLUMN "Standard_Locator" TO "Reference_Standard_Locator";
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "MigrationId" = '20251128175152_AddParametersAndSourcesToMethods') THEN
    ALTER TABLE metabase.method RENAME COLUMN "Standard_Abstract" TO "Reference_Standard_Abstract";
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "MigrationId" = '20251128175152_AddParametersAndSourcesToMethods') THEN
    ALTER TABLE metabase.method RENAME COLUMN "Publication_WebAddress" TO "Reference_Publication_WebAddress";
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "MigrationId" = '20251128175152_AddParametersAndSourcesToMethods') THEN
    ALTER TABLE metabase.method RENAME COLUMN "Publication_Urn" TO "Reference_Publication_Urn";
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "MigrationId" = '20251128175152_AddParametersAndSourcesToMethods') THEN
    ALTER TABLE metabase.method RENAME COLUMN "Publication_Title" TO "Reference_Publication_Title";
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "MigrationId" = '20251128175152_AddParametersAndSourcesToMethods') THEN
    ALTER TABLE metabase.method RENAME COLUMN "Publication_Section" TO "Reference_Publication_Section";
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "MigrationId" = '20251128175152_AddParametersAndSourcesToMethods') THEN
    ALTER TABLE metabase.method RENAME COLUMN "Publication_Doi" TO "Reference_Publication_Doi";
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "MigrationId" = '20251128175152_AddParametersAndSourcesToMethods') THEN
    ALTER TABLE metabase.method RENAME COLUMN "Publication_Authors" TO "Reference_Publication_Authors";
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "MigrationId" = '20251128175152_AddParametersAndSourcesToMethods') THEN
    ALTER TABLE metabase.method RENAME COLUMN "Publication_ArXiv" TO "Reference_Publication_ArXiv";
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "MigrationId" = '20251128175152_AddParametersAndSourcesToMethods') THEN
    ALTER TABLE metabase.method RENAME COLUMN "Publication_Abstract" TO "Reference_Publication_Abstract";
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "MigrationId" = '20251128175152_AddParametersAndSourcesToMethods') THEN
    ALTER TABLE metabase.data_format RENAME COLUMN "Standard_Year" TO "Reference_Standard_Year";
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "MigrationId" = '20251128175152_AddParametersAndSourcesToMethods') THEN
    ALTER TABLE metabase.data_format RENAME COLUMN "Standard_Title" TO "Reference_Standard_Title";
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "MigrationId" = '20251128175152_AddParametersAndSourcesToMethods') THEN
    ALTER TABLE metabase.data_format RENAME COLUMN "Standard_Standardizers" TO "Reference_Standard_Standardizers";
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "MigrationId" = '20251128175152_AddParametersAndSourcesToMethods') THEN
    ALTER TABLE metabase.data_format RENAME COLUMN "Standard_Section" TO "Reference_Standard_Section";
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "MigrationId" = '20251128175152_AddParametersAndSourcesToMethods') THEN
    ALTER TABLE metabase.data_format RENAME COLUMN "Standard_Numeration_Suffix" TO "Reference_Standard_Numeration_Suffix";
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "MigrationId" = '20251128175152_AddParametersAndSourcesToMethods') THEN
    ALTER TABLE metabase.data_format RENAME COLUMN "Standard_Numeration_Prefix" TO "Reference_Standard_Numeration_Prefix";
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "MigrationId" = '20251128175152_AddParametersAndSourcesToMethods') THEN
    ALTER TABLE metabase.data_format RENAME COLUMN "Standard_Numeration_MainNumber" TO "Reference_Standard_Numeration_MainNumber";
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "MigrationId" = '20251128175152_AddParametersAndSourcesToMethods') THEN
    ALTER TABLE metabase.data_format RENAME COLUMN "Standard_Locator" TO "Reference_Standard_Locator";
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "MigrationId" = '20251128175152_AddParametersAndSourcesToMethods') THEN
    ALTER TABLE metabase.data_format RENAME COLUMN "Standard_Abstract" TO "Reference_Standard_Abstract";
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "MigrationId" = '20251128175152_AddParametersAndSourcesToMethods') THEN
    ALTER TABLE metabase.data_format RENAME COLUMN "Publication_WebAddress" TO "Reference_Publication_WebAddress";
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "MigrationId" = '20251128175152_AddParametersAndSourcesToMethods') THEN
    ALTER TABLE metabase.data_format RENAME COLUMN "Publication_Urn" TO "Reference_Publication_Urn";
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "MigrationId" = '20251128175152_AddParametersAndSourcesToMethods') THEN
    ALTER TABLE metabase.data_format RENAME COLUMN "Publication_Title" TO "Reference_Publication_Title";
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "MigrationId" = '20251128175152_AddParametersAndSourcesToMethods') THEN
    ALTER TABLE metabase.data_format RENAME COLUMN "Publication_Section" TO "Reference_Publication_Section";
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "MigrationId" = '20251128175152_AddParametersAndSourcesToMethods') THEN
    ALTER TABLE metabase.data_format RENAME COLUMN "Publication_Doi" TO "Reference_Publication_Doi";
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "MigrationId" = '20251128175152_AddParametersAndSourcesToMethods') THEN
    ALTER TABLE metabase.data_format RENAME COLUMN "Publication_Authors" TO "Reference_Publication_Authors";
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "MigrationId" = '20251128175152_AddParametersAndSourcesToMethods') THEN
    ALTER TABLE metabase.data_format RENAME COLUMN "Publication_ArXiv" TO "Reference_Publication_ArXiv";
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "MigrationId" = '20251128175152_AddParametersAndSourcesToMethods') THEN
    ALTER TABLE metabase.data_format RENAME COLUMN "Publication_Abstract" TO "Reference_Publication_Abstract";
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "MigrationId" = '20251128175152_AddParametersAndSourcesToMethods') THEN
    ALTER TABLE metabase.method ADD "Reference_Exists" boolean;
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "MigrationId" = '20251128175152_AddParametersAndSourcesToMethods') THEN
    ALTER TABLE metabase.data_format ADD "Reference_Exists" boolean;
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "MigrationId" = '20251128175152_AddParametersAndSourcesToMethods') THEN
    CREATE TABLE metabase."MethodParameter" (
        "MethodId" uuid NOT NULL,
        "Id" integer GENERATED BY DEFAULT AS IDENTITY,
        "Name" text NOT NULL,
        "Type" jsonb NOT NULL,
        CONSTRAINT "PK_MethodParameter" PRIMARY KEY ("MethodId", "Id"),
        CONSTRAINT "FK_MethodParameter_method_MethodId" FOREIGN KEY ("MethodId") REFERENCES metabase.method ("Id") ON DELETE CASCADE
    );
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "MigrationId" = '20251128175152_AddParametersAndSourcesToMethods') THEN
    CREATE TABLE metabase."MethodSource" (
        "MethodId" uuid NOT NULL,
        "Id" integer GENERATED BY DEFAULT AS IDENTITY,
        "Name" text NOT NULL,
        "Description" text NOT NULL,
        CONSTRAINT "PK_MethodSource" PRIMARY KEY ("MethodId", "Id"),
        CONSTRAINT "FK_MethodSource_method_MethodId" FOREIGN KEY ("MethodId") REFERENCES metabase.method ("Id") ON DELETE CASCADE
    );
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "MigrationId" = '20251128175152_AddParametersAndSourcesToMethods') THEN
    INSERT INTO "__EFMigrationsHistory" ("MigrationId", "ProductVersion")
    VALUES ('20251128175152_AddParametersAndSourcesToMethods', '9.0.10');
    END IF;
END $EF$;
COMMIT;

