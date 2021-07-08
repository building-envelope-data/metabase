START TRANSACTION;

ALTER TABLE metabase.data_format DROP CONSTRAINT "FK_data_format_institution_ManagerId";

ALTER TABLE metabase.data_format DROP CONSTRAINT "PK_data_format";

ALTER TABLE metabase.data_format RENAME TO "DataFormats";

ALTER INDEX metabase."IX_data_format_ManagerId" RENAME TO "IX_DataFormats_ManagerId";

CREATE SCHEMA IF NOT EXISTS metabase;

CREATE EXTENSION IF NOT EXISTS pgcrypto;

ALTER TABLE metabase.user_method_developer ADD "Id" uuid NOT NULL DEFAULT '00000000-0000-0000-0000-000000000000';

ALTER TABLE metabase.institution_method_developer ADD "Id" uuid NOT NULL DEFAULT '00000000-0000-0000-0000-000000000000';

ALTER TABLE metabase."DataFormats" ALTER COLUMN "Id" DROP DEFAULT;

ALTER TABLE metabase."DataFormats" ADD CONSTRAINT "PK_DataFormats" PRIMARY KEY ("Id");

ALTER TABLE metabase."DataFormats" ADD CONSTRAINT "FK_DataFormats_institution_ManagerId" FOREIGN KEY ("ManagerId") REFERENCES metabase.institution ("Id") ON DELETE CASCADE;

DELETE FROM "__EFMigrationsHistory"
WHERE "MigrationId" = '20210708152541_RenameDataFormatsTableAndRemoveIdAndXminFromMethodDeveloperRelations';

COMMIT;

