START TRANSACTION;

ALTER TABLE metabase."DataFormats" DROP CONSTRAINT "FK_DataFormats_institution_ManagerId";

ALTER TABLE metabase."DataFormats" DROP CONSTRAINT "PK_DataFormats";

ALTER TABLE metabase.user_method_developer DROP COLUMN "Id";

ALTER TABLE metabase.institution_method_developer DROP COLUMN "Id";

ALTER TABLE metabase."DataFormats" RENAME TO data_format;

ALTER INDEX metabase."IX_DataFormats_ManagerId" RENAME TO "IX_data_format_ManagerId";

CREATE TYPE public.component_category AS ENUM ('material', 'layer', 'unit');
CREATE TYPE public.institution_representative_role AS ENUM ('owner', 'maintainer', 'assistant');
CREATE TYPE public.institution_state AS ENUM ('unknown', 'operative', 'inoperative');
CREATE TYPE public.method_category AS ENUM ('measurement', 'calculation');
CREATE TYPE public.standardizer AS ENUM ('aerc', 'agi', 'ashrae', 'breeam', 'bs', 'bsi', 'cen', 'cie', 'dgnb', 'din', 'dvwg', 'iec', 'ies', 'ift', 'iso', 'jis', 'leed', 'nfrc', 'riba', 'ul', 'unece', 'vdi', 'vff', 'well');
DROP TYPE metabase.component_category;
DROP TYPE metabase.institution_representative_role;
DROP TYPE metabase.institution_state;
DROP TYPE metabase.method_category;
DROP TYPE metabase.standardizer;
CREATE EXTENSION IF NOT EXISTS pgcrypto;

ALTER TABLE metabase.data_format ALTER COLUMN "Id" SET DEFAULT (gen_random_uuid());

ALTER TABLE metabase.data_format ADD CONSTRAINT "PK_data_format" PRIMARY KEY ("Id");

ALTER TABLE metabase.data_format ADD CONSTRAINT "FK_data_format_institution_ManagerId" FOREIGN KEY ("ManagerId") REFERENCES metabase.institution ("Id") ON DELETE CASCADE;

INSERT INTO "__EFMigrationsHistory" ("MigrationId", "ProductVersion")
VALUES ('20210708152541_RenameDataFormatsTableAndRemoveIdAndXminFromMethodDeveloperRelations', '5.0.7');

COMMIT;

