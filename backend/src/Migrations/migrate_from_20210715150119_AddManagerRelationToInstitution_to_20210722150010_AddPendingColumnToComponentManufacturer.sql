START TRANSACTION;

ALTER TABLE metabase.component_manufacturer ADD "Pending" boolean NOT NULL DEFAULT FALSE;

INSERT INTO "__EFMigrationsHistory" ("MigrationId", "ProductVersion")
VALUES ('20210722150010_AddPendingColumnToComponentManufacturer', '5.0.8');

COMMIT;

