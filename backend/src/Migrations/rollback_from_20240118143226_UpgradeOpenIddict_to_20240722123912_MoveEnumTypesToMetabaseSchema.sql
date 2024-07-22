START TRANSACTION;

CREATE TYPE public.component_category AS ENUM ('material', 'layer', 'unit');
CREATE TYPE public.database_verification_state AS ENUM ('pending', 'verified');
CREATE TYPE public.institution_operating_state AS ENUM ('operating', 'not_operating');
CREATE TYPE public.institution_representative_role AS ENUM ('owner', 'assistant');
CREATE TYPE public.institution_state AS ENUM ('pending', 'verified');
CREATE TYPE public.method_category AS ENUM ('measurement', 'calculation');
CREATE TYPE public.prime_surface AS ENUM ('inside', 'outside');
CREATE TYPE public.standardizer AS ENUM ('aerc', 'agi', 'ashrae', 'breeam', 'bs', 'bsi', 'cen', 'cie', 'dgnb', 'din', 'dvwg', 'iec', 'ies', 'ift', 'iso', 'jis', 'leed', 'nfrc', 'riba', 'ul', 'unece', 'vdi', 'vff', 'well');

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM pg_namespace WHERE nspname = 'metabase') THEN
        CREATE SCHEMA metabase;
    END IF;
END $EF$;

CREATE EXTENSION IF NOT EXISTS pgcrypto;

ALTER TABLE metabase.method ALTER COLUMN "Standard_Standardizers" TYPE standardizer[] USING "Standard_Standardizers"::text::standardizer[];

ALTER TABLE metabase.method ALTER COLUMN "Categories" TYPE method_category[] USING "Categories"::text::method_category[];

ALTER TABLE metabase.institution_representative ALTER COLUMN "Role" TYPE institution_representative_role USING "Role"::text::institution_representative_role;

ALTER TABLE metabase.institution ALTER COLUMN "State" TYPE institution_state USING "State"::text::institution_state;

ALTER TABLE metabase.institution ALTER COLUMN "OperatingState" DROP DEFAULT;
ALTER TABLE metabase.institution ALTER COLUMN "OperatingState" TYPE institution_operating_state USING "OperatingState"::text::institution_operating_state;
ALTER TABLE metabase.institution ALTER COLUMN "OperatingState" SET DEFAULT 'operating'::institution_operating_state;

ALTER TABLE metabase.database ALTER COLUMN "VerificationState" TYPE database_verification_state USING "VerificationState"::text::database_verification_state;

ALTER TABLE metabase.data_format ALTER COLUMN "Standard_Standardizers" TYPE standardizer[] USING "Standard_Standardizers"::text::standardizer[];

ALTER TABLE metabase.component_assembly ALTER COLUMN "PrimeSurface" TYPE prime_surface USING "PrimeSurface"::text::prime_surface;

ALTER TABLE metabase.component ALTER COLUMN "Categories" TYPE component_category[] USING "Categories"::text::component_category[];

DROP TYPE metabase.component_category;
DROP TYPE metabase.database_verification_state;
DROP TYPE metabase.institution_operating_state;
DROP TYPE metabase.institution_representative_role;
DROP TYPE metabase.institution_state;
DROP TYPE metabase.method_category;
DROP TYPE metabase.prime_surface;
DROP TYPE metabase.standardizer;

DELETE FROM "__EFMigrationsHistory"
WHERE "MigrationId" = '20240722123912_MoveEnumTypesToMetabaseSchema';

COMMIT;

START TRANSACTION;

ALTER TABLE metabase.institution DROP COLUMN "OperatingState";

DROP TYPE public.institution_operating_state;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM pg_namespace WHERE nspname = 'metabase') THEN
        CREATE SCHEMA metabase;
    END IF;
END $EF$;

CREATE EXTENSION IF NOT EXISTS pgcrypto;

DELETE FROM "__EFMigrationsHistory"
WHERE "MigrationId" = '20240522093622_AddOperatingState';

COMMIT;

