START TRANSACTION;

ALTER TABLE metabase."DataFormats" DROP CONSTRAINT "FK_DataFormats_institution_ManagerId";

ALTER TABLE metabase."DataFormats" DROP CONSTRAINT "PK_DataFormats";

ALTER TABLE metabase.user_method_developer DROP COLUMN "Id";

ALTER TABLE metabase.institution_method_developer DROP COLUMN "Id";

ALTER TABLE metabase."DataFormats" RENAME TO data_format;

ALTER INDEX metabase."IX_DataFormats_ManagerId" RENAME TO "IX_data_format_ManagerId";

CREATE EXTENSION IF NOT EXISTS pgcrypto;

ALTER TABLE metabase.data_format ALTER COLUMN "Id" SET DEFAULT (gen_random_uuid());

ALTER TABLE metabase.data_format ADD CONSTRAINT "PK_data_format" PRIMARY KEY ("Id");

ALTER TABLE metabase.data_format ADD CONSTRAINT "FK_data_format_institution_ManagerId" FOREIGN KEY ("ManagerId") REFERENCES metabase.institution ("Id") ON DELETE CASCADE;

INSERT INTO "__EFMigrationsHistory" ("MigrationId", "ProductVersion")
VALUES ('20210708152541_RenameDataFormatsTableAndRemoveIdAndXminFromMethodDeveloperRelations', '5.0.7');

COMMIT;

