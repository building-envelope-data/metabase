START TRANSACTION;

ALTER TABLE metabase.data_format DROP CONSTRAINT "FK_data_format_institution_ManagerId";

ALTER TABLE metabase.data_format DROP CONSTRAINT "PK_data_format";

ALTER TABLE metabase.data_format RENAME TO "DataFormats";

ALTER INDEX metabase."IX_data_format_ManagerId" RENAME TO "IX_DataFormats_ManagerId";

CREATE SCHEMA IF NOT EXISTS metabase;

CREATE TYPE metabase.component_category AS ENUM ('material', 'layer', 'unit');
CREATE SCHEMA IF NOT EXISTS metabase;

CREATE TYPE metabase.institution_representative_role AS ENUM ('owner', 'maintainer', 'assistant');
CREATE SCHEMA IF NOT EXISTS metabase;

CREATE TYPE metabase.institution_state AS ENUM ('unknown', 'operative', 'inoperative');
CREATE SCHEMA IF NOT EXISTS metabase;

CREATE TYPE metabase.method_category AS ENUM ('measurement', 'calculation');
CREATE SCHEMA IF NOT EXISTS metabase;

CREATE TYPE metabase.standardizer AS ENUM ('aerc', 'agi', 'ashrae', 'breeam', 'bs', 'bsi', 'cen', 'cie', 'dgnb', 'din', 'dvwg', 'iec', 'ies', 'ift', 'iso', 'jis', 'leed', 'nfrc', 'riba', 'ul', 'unece', 'vdi', 'vff', 'well');
DROP TYPE public.component_category;
DROP TYPE public.institution_representative_role;
DROP TYPE public.institution_state;
DROP TYPE public.method_category;
DROP TYPE public.standardizer;
CREATE EXTENSION IF NOT EXISTS pgcrypto;

ALTER TABLE metabase.user_method_developer ADD "Id" uuid NOT NULL DEFAULT '00000000-0000-0000-0000-000000000000';

ALTER TABLE metabase.institution_method_developer ADD "Id" uuid NOT NULL DEFAULT '00000000-0000-0000-0000-000000000000';

ALTER TABLE metabase."DataFormats" ALTER COLUMN "Id" DROP DEFAULT;

ALTER TABLE metabase."DataFormats" ADD CONSTRAINT "PK_DataFormats" PRIMARY KEY ("Id");

ALTER TABLE metabase."DataFormats" ADD CONSTRAINT "FK_DataFormats_institution_ManagerId" FOREIGN KEY ("ManagerId") REFERENCES metabase.institution ("Id") ON DELETE CASCADE;

DELETE FROM "__EFMigrationsHistory"
WHERE "MigrationId" = '20210708152541_RenameDataFormatsTableAndRemoveIdAndXminFromMethodDeveloperRelations';

COMMIT;

