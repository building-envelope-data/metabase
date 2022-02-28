START TRANSACTION;

DROP TABLE metabase.component_assembly;

DROP TABLE metabase.component_variant;

ALTER TABLE metabase.component_concretization_and_generalization ADD "Id" uuid NOT NULL DEFAULT '00000000-0000-0000-0000-000000000000';

DELETE FROM "__EFMigrationsHistory"
WHERE "MigrationId" = '20211110161448_AddReflexiveAssociationsToComponent';

COMMIT;

