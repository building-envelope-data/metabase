START TRANSACTION;

DROP TABLE metabase.component_concretization_and_generalization;

DROP TABLE metabase.component_manufacturer;

DROP TABLE metabase.database;

DROP TABLE metabase."DataFormats";

DROP TABLE metabase.institution_method_developer;

DROP TABLE metabase.institution_representative;

DROP TABLE metabase."OpenIddictScopes";

DROP TABLE metabase."OpenIddictTokens";

DROP TABLE metabase.role_claim;

DROP TABLE metabase.user_claim;

DROP TABLE metabase.user_login;

DROP TABLE metabase.user_method_developer;

DROP TABLE metabase.user_role;

DROP TABLE metabase.user_token;

DROP TABLE metabase.component;

DROP TABLE metabase.institution;

DROP TABLE metabase."OpenIddictAuthorizations";

DROP TABLE metabase.method;

DROP TABLE metabase.role;

DROP TABLE metabase."user";

DROP TABLE metabase."OpenIddictApplications";

DROP EXTENSION IF EXISTS pgcrypto;

DROP TYPE public.component_category;
DROP TYPE public.institution_representative_role;
DROP TYPE public.institution_state;
DROP TYPE public.method_category;
DROP TYPE public.standardizer;

DELETE FROM "__EFMigrationsHistory"
WHERE "MigrationId" = '20210414155531_InitialCreate';

COMMIT;

