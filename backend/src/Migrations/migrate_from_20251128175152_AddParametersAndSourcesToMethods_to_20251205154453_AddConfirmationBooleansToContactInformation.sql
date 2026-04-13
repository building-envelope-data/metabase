START TRANSACTION;

DO $EF$
BEGIN
    IF EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "MigrationId" = '20251205154453_AddConfirmationBooleansToContactInformation') THEN
    ALTER TABLE metabase.institution DROP COLUMN "Contact_Exists";
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "MigrationId" = '20251205154453_AddConfirmationBooleansToContactInformation') THEN
    ALTER TABLE metabase.institution ADD "Contact_IsPhoneNumberConfirmed" boolean;
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "MigrationId" = '20251205154453_AddConfirmationBooleansToContactInformation') THEN
    ALTER TABLE metabase.institution ADD "Contact_IsEmailAddressConfirmed" boolean;
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "MigrationId" = '20251205154453_AddConfirmationBooleansToContactInformation') THEN
    INSERT INTO "__EFMigrationsHistory" ("MigrationId", "ProductVersion")
    VALUES ('20251205154453_AddConfirmationBooleansToContactInformation', '10.0.0');
    END IF;
END $EF$;
COMMIT;

