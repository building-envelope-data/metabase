START TRANSACTION;

ALTER TYPE institution_state RENAME TO institution_state_old;
CREATE TYPE institution_state AS ENUM ('pending', 'verified');
ALTER TABLE metabase.institution ALTER COLUMN "State" TYPE institution_state USING 'verified';
DROP TYPE institution_state_old;

INSERT INTO "__EFMigrationsHistory" ("MigrationId", "ProductVersion")
VALUES ('20210805142241_UsePendingAndConfirmedAsInstitutionStates', '5.0.9');

COMMIT;

