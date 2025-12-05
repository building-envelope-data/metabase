START TRANSACTION;
DO $EF$
BEGIN
    IF EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "MigrationId" = '20251205154453_AddConfirmationBooleansToContactInformation') THEN
    ALTER TABLE metabase.institution DROP COLUMN "Contact_IsEmailAddressConfirmed";
    END IF;
END $EF$;
DO $EF$
BEGIN
    IF EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "MigrationId" = '20251205154453_AddConfirmationBooleansToContactInformation') THEN
    ALTER TABLE metabase.institution DROP COLUMN "Contact_IsPhoneNumberConfirmed";
    END IF;
END $EF$;
DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "MigrationId" = '20251205154453_AddConfirmationBooleansToContactInformation') THEN
    ALTER TABLE metabase.institution ADD "Contact_Exists" boolean;
    END IF;
END $EF$;
DO $EF$
BEGIN
    IF EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "MigrationId" = '20251205154453_AddConfirmationBooleansToContactInformation') THEN
    DELETE FROM "__EFMigrationsHistory"
    WHERE "MigrationId" = '20251205154453_AddConfirmationBooleansToContactInformation';
    END IF;
END $EF$;
COMMIT;

