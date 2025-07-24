START TRANSACTION;
ALTER TABLE metabase.institution_open_id_connect_application DROP CONSTRAINT "FK_institution_open_id_connect_application_OpenIddictApplicati~";

ALTER TABLE metabase.institution_open_id_connect_application DROP CONSTRAINT "FK_institution_open_id_connect_application_institution_Institu~";

ALTER TABLE metabase.institution_open_id_connect_application DROP CONSTRAINT "PK_institution_open_id_connect_application";

ALTER TABLE metabase.institution_open_id_connect_application RENAME TO institution_application;

ALTER INDEX metabase."IX_institution_open_id_connect_application_ApplicationId" RENAME TO "IX_institution_application_ApplicationId";

ALTER TABLE metabase.institution_application ADD CONSTRAINT "PK_institution_application" PRIMARY KEY ("InstitutionId", "ApplicationId");

ALTER TABLE metabase.institution_application ADD CONSTRAINT "FK_institution_application_OpenIddictApplications_ApplicationId" FOREIGN KEY ("ApplicationId") REFERENCES metabase."OpenIddictApplications" ("Id") ON DELETE CASCADE;

ALTER TABLE metabase.institution_application ADD CONSTRAINT "FK_institution_application_institution_InstitutionId" FOREIGN KEY ("InstitutionId") REFERENCES metabase.institution ("Id") ON DELETE CASCADE;

DELETE FROM "__EFMigrationsHistory"
WHERE "MigrationId" = '20250714160511_InstitutionOpenIdConnectApplicationAssociationProperly';

ALTER TABLE metabase.institution DROP COLUMN "Extras";

ALTER TABLE metabase.component DROP COLUMN "Extras";

DELETE FROM "__EFMigrationsHistory"
WHERE "MigrationId" = '20250604122410_AddExtrasToComponentAndInstitution';

COMMIT;

