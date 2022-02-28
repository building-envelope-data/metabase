START TRANSACTION;

ALTER TABLE metabase.institution DROP CONSTRAINT "FK_institution_institution_ManagerId";

DROP INDEX metabase."IX_institution_ManagerId";

ALTER TABLE metabase.institution_representative DROP COLUMN "Pending";

ALTER TABLE metabase.institution DROP COLUMN "ManagerId";

CREATE SCHEMA IF NOT EXISTS metabase;

ALTER TYPE institution_representative_role RENAME TO institution_representative_role_old;
CREATE TYPE institution_representative_role AS ENUM ('owner', 'maintainer', 'assistant');;
ALTER TABLE metabase.institution_representative ALTER COLUMN "Role" TYPE institution_representative_role USING "Role"::text::institution_representative_role;
DROP TYPE institution_representative_role_old;

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

