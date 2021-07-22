START TRANSACTION;

ALTER TABLE metabase.institution DROP CONSTRAINT "FK_institution_institution_ManagerId";

DROP INDEX metabase."IX_institution_ManagerId";

ALTER TABLE metabase.institution_representative DROP COLUMN "Pending";

ALTER TABLE metabase.institution DROP COLUMN "ManagerId";

CREATE SCHEMA IF NOT EXISTS metabase;

CREATE TYPE metabase.institution_representative_role AS ENUM ('owner', 'maintainer', 'assistant');
DROP TYPE public.institution_representative_role;

DELETE FROM "__EFMigrationsHistory"
WHERE "MigrationId" = '20210715150119_AddManagerRelationToInstitution';

COMMIT;

START TRANSACTION;

ALTER TABLE metabase.method DROP CONSTRAINT "FK_method_institution_ManagerId";

DROP INDEX metabase."IX_method_ManagerId";

ALTER TABLE metabase.user_method_developer DROP COLUMN "Pending";

ALTER TABLE metabase.method DROP COLUMN "ManagerId";

ALTER TABLE metabase.institution_method_developer DROP COLUMN "Pending";

DELETE FROM "__EFMigrationsHistory"
WHERE "MigrationId" = '20210712143023_AddMethodManagerOneToManyRelation';

COMMIT;

