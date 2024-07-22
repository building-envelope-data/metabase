START TRANSACTION;

CREATE TYPE public.institution_operating_state AS ENUM ('operating', 'not_operating');
DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM pg_namespace WHERE nspname = 'metabase') THEN
        CREATE SCHEMA metabase;
    END IF;
END $EF$;

CREATE EXTENSION IF NOT EXISTS pgcrypto;

ALTER TABLE metabase.institution ADD "OperatingState" institution_operating_state NOT NULL DEFAULT 'operating'::institution_operating_state;

INSERT INTO "__EFMigrationsHistory" ("MigrationId", "ProductVersion")
VALUES ('20240522093622_AddOperatingState', '8.0.6');

COMMIT;

START TRANSACTION;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM pg_namespace WHERE nspname = 'metabase') THEN
        CREATE SCHEMA metabase;
    END IF;
END $EF$;

CREATE TYPE metabase.component_category AS ENUM ('material', 'layer', 'unit');
DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM pg_namespace WHERE nspname = 'metabase') THEN
        CREATE SCHEMA metabase;
    END IF;
END $EF$;

CREATE TYPE metabase.database_verification_state AS ENUM ('pending', 'verified');
DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM pg_namespace WHERE nspname = 'metabase') THEN
        CREATE SCHEMA metabase;
    END IF;
END $EF$;

CREATE TYPE metabase.institution_operating_state AS ENUM ('operating', 'not_operating');
DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM pg_namespace WHERE nspname = 'metabase') THEN
        CREATE SCHEMA metabase;
    END IF;
END $EF$;

CREATE TYPE metabase.institution_representative_role AS ENUM ('owner', 'assistant');
DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM pg_namespace WHERE nspname = 'metabase') THEN
        CREATE SCHEMA metabase;
    END IF;
END $EF$;

CREATE TYPE metabase.institution_state AS ENUM ('pending', 'verified');
DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM pg_namespace WHERE nspname = 'metabase') THEN
        CREATE SCHEMA metabase;
    END IF;
END $EF$;

CREATE TYPE metabase.method_category AS ENUM ('measurement', 'calculation');
DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM pg_namespace WHERE nspname = 'metabase') THEN
        CREATE SCHEMA metabase;
    END IF;
END $EF$;

CREATE TYPE metabase.prime_surface AS ENUM ('inside', 'outside');
DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM pg_namespace WHERE nspname = 'metabase') THEN
        CREATE SCHEMA metabase;
    END IF;
END $EF$;

CREATE TYPE metabase.standardizer AS ENUM ('aerc', 'agi', 'ashrae', 'breeam', 'bs', 'bsi', 'cen', 'cie', 'dgnb', 'din', 'dvwg', 'iec', 'ies', 'ift', 'iso', 'jis', 'leed', 'nfrc', 'riba', 'ul', 'unece', 'vdi', 'vff', 'well');
DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM pg_namespace WHERE nspname = 'metabase') THEN
        CREATE SCHEMA metabase;
    END IF;
END $EF$;

CREATE EXTENSION IF NOT EXISTS pgcrypto;

ALTER TABLE metabase.method ALTER COLUMN "Standard_Standardizers" TYPE metabase.standardizer[] USING "Standard_Standardizers"::text::metabase.standardizer[];

ALTER TABLE metabase.method ALTER COLUMN "Categories" TYPE metabase.method_category[] USING "Categories"::text::metabase.method_category[];

ALTER TABLE metabase.institution_representative ALTER COLUMN "Role" TYPE metabase.institution_representative_role USING "Role"::text::metabase.institution_representative_role;

ALTER TABLE metabase.institution ALTER COLUMN "State" TYPE metabase.institution_state USING "State"::text::metabase.institution_state;

ALTER TABLE metabase.institution ALTER COLUMN "OperatingState" DROP DEFAULT;
ALTER TABLE metabase.institution ALTER COLUMN "OperatingState" TYPE metabase.institution_operating_state USING "OperatingState"::text::metabase.institution_operating_state;
ALTER TABLE metabase.institution ALTER COLUMN "OperatingState" SET DEFAULT 'operating'::metabase.institution_operating_state;

ALTER TABLE metabase.database ALTER COLUMN "VerificationState" DROP DEFAULT;
ALTER TABLE metabase.database ALTER COLUMN "VerificationState" TYPE metabase.database_verification_state USING "VerificationState"::text::metabase.database_verification_state;
ALTER TABLE metabase.database ALTER COLUMN "VerificationState" SET DEFAULT 'pending'::metabase.database_verification_state;

ALTER TABLE metabase.data_format ALTER COLUMN "Standard_Standardizers" TYPE metabase.standardizer[] USING "Standard_Standardizers"::text::metabase.standardizer[];

ALTER TABLE metabase.component_assembly ALTER COLUMN "PrimeSurface" TYPE metabase.prime_surface USING "PrimeSurface"::text::metabase.prime_surface;

ALTER TABLE metabase.component ALTER COLUMN "Categories" TYPE metabase.component_category[] USING "Categories"::text::metabase.component_category[];

DROP TYPE public.component_category;
DROP TYPE public.database_verification_state;
DROP TYPE public.institution_operating_state;
DROP TYPE public.institution_representative_role;
DROP TYPE public.institution_state;
DROP TYPE public.method_category;
DROP TYPE public.prime_surface;
DROP TYPE public.standardizer;

INSERT INTO "__EFMigrationsHistory" ("MigrationId", "ProductVersion")
VALUES ('20240722123912_MoveEnumTypesToMetabaseSchema', '8.0.6');

COMMIT;

