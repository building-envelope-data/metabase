START TRANSACTION;
DO $EF$
BEGIN
    IF EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "MigrationId" = '20251124184659_AddContactInformationToInstitution') THEN
    ALTER TABLE metabase.institution DROP COLUMN "Contact_EmailAddress";
    END IF;
END $EF$;
DO $EF$
BEGIN
    IF EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "MigrationId" = '20251124184659_AddContactInformationToInstitution') THEN
    ALTER TABLE metabase.institution DROP COLUMN "Contact_Exists";
    END IF;
END $EF$;
DO $EF$
BEGIN
    IF EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "MigrationId" = '20251124184659_AddContactInformationToInstitution') THEN
    ALTER TABLE metabase.institution DROP COLUMN "Contact_PhoneNumber";
    END IF;
END $EF$;
DO $EF$
BEGIN
    IF EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "MigrationId" = '20251124184659_AddContactInformationToInstitution') THEN
    ALTER TABLE metabase.institution RENAME COLUMN "Contact_WebsiteLocator" TO "WebsiteLocator";
    END IF;
END $EF$;
DO $EF$
BEGIN
    IF EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "MigrationId" = '20251124184659_AddContactInformationToInstitution') THEN
    ALTER TABLE metabase.institution RENAME COLUMN "Contact_PostalAddress" TO "PublicKey";
    END IF;
END $EF$;
DO $EF$
BEGIN
    IF EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "MigrationId" = '20251124184659_AddContactInformationToInstitution') THEN
    DELETE FROM "__EFMigrationsHistory"
    WHERE "MigrationId" = '20251124184659_AddContactInformationToInstitution';
    END IF;
END $EF$;
COMMIT;

