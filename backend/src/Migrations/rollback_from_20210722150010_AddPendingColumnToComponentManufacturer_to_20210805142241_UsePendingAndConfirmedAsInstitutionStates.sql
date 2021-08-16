START TRANSACTION;

CREATE SCHEMA IF NOT EXISTS metabase;

ALTER TYPE institution_state RENAME TO institution_state_old;
CREATE TYPE institution_state AS ENUM ('unknown', 'operative', 'inoperative');
ALTER TABLE metabase.institution ALTER COLUMN "State" TYPE institution_state USING 'operative';
DROP TYPE institution_state_old;

DELETE FROM "__EFMigrationsHistory"
WHERE "MigrationId" = '20210805142241_UsePendingAndConfirmedAsInstitutionStates';

COMMIT;

