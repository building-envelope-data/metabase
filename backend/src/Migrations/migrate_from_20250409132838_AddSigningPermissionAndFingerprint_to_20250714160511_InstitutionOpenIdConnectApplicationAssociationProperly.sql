START TRANSACTION;
ALTER TABLE metabase.institution ADD "Extras" jsonb;

ALTER TABLE metabase.component ADD "Extras" jsonb;

INSERT INTO "__EFMigrationsHistory" ("MigrationId", "ProductVersion")
VALUES ('20250604122410_AddExtrasToComponentAndInstitution', '9.0.5');

ALTER TABLE metabase.institution_application DROP CONSTRAINT "FK_institution_application_OpenIddictApplications_ApplicationId";

ALTER TABLE metabase.institution_application DROP CONSTRAINT "FK_institution_application_institution_InstitutionId";

ALTER TABLE metabase.institution_application DROP CONSTRAINT "PK_institution_application";

ALTER TABLE metabase.institution_application RENAME TO institution_open_id_connect_application;

ALTER INDEX metabase."IX_institution_application_ApplicationId" RENAME TO "IX_institution_open_id_connect_application_ApplicationId";

ALTER TABLE metabase.institution_open_id_connect_application ADD CONSTRAINT "PK_institution_open_id_connect_application" PRIMARY KEY ("InstitutionId", "ApplicationId");

ALTER TABLE metabase.institution_open_id_connect_application ADD CONSTRAINT "FK_institution_open_id_connect_application_OpenIddictApplicati~" FOREIGN KEY ("ApplicationId") REFERENCES metabase."OpenIddictApplications" ("Id") ON DELETE CASCADE;

ALTER TABLE metabase.institution_open_id_connect_application ADD CONSTRAINT "FK_institution_open_id_connect_application_institution_Institu~" FOREIGN KEY ("InstitutionId") REFERENCES metabase.institution ("Id") ON DELETE CASCADE;

INSERT INTO "__EFMigrationsHistory" ("MigrationId", "ProductVersion")
VALUES ('20250714160511_InstitutionOpenIdConnectApplicationAssociationProperly', '9.0.5');

COMMIT;

