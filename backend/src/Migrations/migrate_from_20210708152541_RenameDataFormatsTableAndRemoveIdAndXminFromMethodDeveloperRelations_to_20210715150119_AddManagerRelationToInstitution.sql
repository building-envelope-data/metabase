START TRANSACTION;

ALTER TABLE metabase.user_method_developer ADD "Pending" boolean NOT NULL DEFAULT TRUE;

ALTER TABLE metabase.method ADD "ManagerId" uuid NOT NULL DEFAULT '00000000-0000-0000-0000-000000000000';

ALTER TABLE metabase.institution_method_developer ADD "Pending" boolean NOT NULL DEFAULT TRUE;

CREATE INDEX "IX_method_ManagerId" ON metabase.method ("ManagerId");

ALTER TABLE metabase.method ADD CONSTRAINT "FK_method_institution_ManagerId" FOREIGN KEY ("ManagerId") REFERENCES metabase.institution ("Id") ON DELETE CASCADE;

INSERT INTO "__EFMigrationsHistory" ("MigrationId", "ProductVersion")
VALUES ('20210712143023_AddMethodManagerOneToManyRelation', '5.0.8');

COMMIT;

START TRANSACTION;

ALTER TYPE institution_representative_role RENAME TO institution_representative_role_old;
CREATE TYPE institution_representative_role AS ENUM ('owner', 'assistant');
ALTER TABLE metabase.institution_representative ALTER COLUMN "Role" TYPE institution_representative_role USING "Role"::text::institution_representative_role;
DROP TYPE institution_representative_role_old;

ALTER TABLE metabase.institution_representative ADD "Pending" boolean NOT NULL DEFAULT TRUE;

ALTER TABLE metabase.institution ADD "ManagerId" uuid NULL;

CREATE INDEX "IX_institution_ManagerId" ON metabase.institution ("ManagerId");

ALTER TABLE metabase.institution ADD CONSTRAINT "FK_institution_institution_ManagerId" FOREIGN KEY ("ManagerId") REFERENCES metabase.institution ("Id") ON DELETE RESTRICT;

INSERT INTO "__EFMigrationsHistory" ("MigrationId", "ProductVersion")
VALUES ('20210715150119_AddManagerRelationToInstitution', '5.0.8');

COMMIT;

