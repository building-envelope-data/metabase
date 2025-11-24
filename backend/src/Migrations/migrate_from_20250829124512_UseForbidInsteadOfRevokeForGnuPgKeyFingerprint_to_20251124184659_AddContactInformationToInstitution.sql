START TRANSACTION;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "MigrationId" = '20251124184659_AddContactInformationToInstitution') THEN
    ALTER TABLE metabase.institution RENAME COLUMN "WebsiteLocator" TO "Contact_WebsiteLocator";
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "MigrationId" = '20251124184659_AddContactInformationToInstitution') THEN
    ALTER TABLE metabase.institution RENAME COLUMN "PublicKey" TO "Contact_PostalAddress";
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "MigrationId" = '20251124184659_AddContactInformationToInstitution') THEN
    ALTER TABLE metabase.institution ADD "Contact_EmailAddress" text;
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "MigrationId" = '20251124184659_AddContactInformationToInstitution') THEN
    ALTER TABLE metabase.institution ADD "Contact_Exists" boolean;
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "MigrationId" = '20251124184659_AddContactInformationToInstitution') THEN
    ALTER TABLE metabase.institution ADD "Contact_PhoneNumber" text;
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "MigrationId" = '20251124184659_AddContactInformationToInstitution') THEN
    INSERT INTO "__EFMigrationsHistory" ("MigrationId", "ProductVersion")
    VALUES ('20251124184659_AddContactInformationToInstitution', '9.0.10');
    END IF;
END $EF$;
COMMIT;

