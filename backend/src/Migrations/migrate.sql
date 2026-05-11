\set ON_ERROR_STOP on

CREATE TABLE IF NOT EXISTS "__EFMigrationsHistory" (
    "MigrationId" character varying(150) NOT NULL,
    "ProductVersion" character varying(32) NOT NULL,
    CONSTRAINT "PK___EFMigrationsHistory" PRIMARY KEY ("MigrationId")
);

START TRANSACTION;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "MigrationId" = '20210414155531_InitialCreate') THEN

CREATE SCHEMA IF NOT EXISTS metabase;

CREATE TYPE public.component_category AS ENUM ('material', 'layer', 'unit');
CREATE TYPE public.institution_representative_role AS ENUM ('owner', 'maintainer', 'assistant');
CREATE TYPE public.institution_state AS ENUM ('unknown', 'operative', 'inoperative');
CREATE TYPE public.method_category AS ENUM ('measurement', 'calculation');
CREATE TYPE public.standardizer AS ENUM ('aerc', 'agi', 'ashrae', 'breeam', 'bs', 'bsi', 'cen', 'cie', 'dgnb', 'din', 'dvwg', 'iec', 'ies', 'ift', 'iso', 'jis', 'leed', 'nfrc', 'riba', 'ul', 'unece', 'vdi', 'vff', 'well');
CREATE EXTENSION IF NOT EXISTS pgcrypto;

CREATE TABLE metabase.component (
    "Id" uuid NOT NULL DEFAULT (gen_random_uuid()),
    "Name" text NOT NULL,
    "Abbreviation" text NULL,
    "Description" text NOT NULL,
    "Availability" tsrange NULL,
    "Categories" component_category[] NOT NULL,
    CONSTRAINT "PK_component" PRIMARY KEY ("Id")
);

CREATE TABLE metabase.institution (
    "Id" uuid NOT NULL DEFAULT (gen_random_uuid()),
    "Name" text NOT NULL,
    "Abbreviation" text NULL,
    "Description" text NOT NULL,
    "WebsiteLocator" text NULL,
    "PublicKey" text NULL,
    "State" institution_state NOT NULL,
    CONSTRAINT "PK_institution" PRIMARY KEY ("Id")
);

CREATE TABLE metabase.method (
    "Id" uuid NOT NULL DEFAULT (gen_random_uuid()),
    "Name" text NOT NULL,
    "Description" text NOT NULL,
    "Standard_Title" text NULL,
    "Standard_Abstract" text NULL,
    "Standard_Section" text NULL,
    "Standard_Year" integer NULL,
    "Standard_Numeration_Prefix" text NULL,
    "Standard_Numeration_MainNumber" text NULL,
    "Standard_Numeration_Suffix" text NULL,
    "Standard_Standardizers" standardizer[] NULL,
    "Standard_Locator" text NULL,
    "Publication_Title" text NULL,
    "Publication_Abstract" text NULL,
    "Publication_Section" text NULL,
    "Publication_Authors" text[] NULL,
    "Publication_Doi" text NULL,
    "Publication_ArXiv" text NULL,
    "Publication_Urn" text NULL,
    "Publication_WebAddress" text NULL,
    "Validity" tsrange NULL,
    "Availability" tsrange NULL,
    "CalculationLocator" text NULL,
    "Categories" method_category[] NOT NULL,
    CONSTRAINT "PK_method" PRIMARY KEY ("Id")
);

CREATE TABLE metabase."OpenIddictApplications" (
    "Id" text NOT NULL,
    "ClientId" character varying(100) NULL,
    "ClientSecret" text NULL,
    "ConcurrencyToken" character varying(50) NULL,
    "ConsentType" character varying(50) NULL,
    "DisplayName" text NULL,
    "DisplayNames" text NULL,
    "Permissions" text NULL,
    "PostLogoutRedirectUris" text NULL,
    "Properties" text NULL,
    "RedirectUris" text NULL,
    "Requirements" text NULL,
    "Type" character varying(50) NULL,
    CONSTRAINT "PK_OpenIddictApplications" PRIMARY KEY ("Id")
);

CREATE TABLE metabase."OpenIddictScopes" (
    "Id" text NOT NULL,
    "ConcurrencyToken" character varying(50) NULL,
    "Description" text NULL,
    "Descriptions" text NULL,
    "DisplayName" text NULL,
    "DisplayNames" text NULL,
    "Name" character varying(200) NULL,
    "Properties" text NULL,
    "Resources" text NULL,
    CONSTRAINT "PK_OpenIddictScopes" PRIMARY KEY ("Id")
);

CREATE TABLE metabase.role (
    "Id" uuid NOT NULL,
    "Name" character varying(256) NULL,
    "NormalizedName" character varying(256) NULL,
    "ConcurrencyStamp" text NULL,
    CONSTRAINT "PK_role" PRIMARY KEY ("Id")
);

CREATE TABLE metabase."user" (
    "Id" uuid NOT NULL,
    "Name" text NOT NULL,
    "PostalAddress" text NULL,
    "WebsiteLocator" text NULL,
    "UserName" character varying(256) NULL,
    "NormalizedUserName" character varying(256) NULL,
    "Email" character varying(256) NULL,
    "NormalizedEmail" character varying(256) NULL,
    "EmailConfirmed" boolean NOT NULL,
    "PasswordHash" text NULL,
    "SecurityStamp" text NULL,
    "ConcurrencyStamp" text NULL,
    "PhoneNumber" text NULL,
    "PhoneNumberConfirmed" boolean NOT NULL,
    "TwoFactorEnabled" boolean NOT NULL,
    "LockoutEnd" timestamp with time zone NULL,
    "LockoutEnabled" boolean NOT NULL,
    "AccessFailedCount" integer NOT NULL,
    CONSTRAINT "PK_user" PRIMARY KEY ("Id")
);

CREATE TABLE metabase.component_concretization_and_generalization (
    "GeneralComponentId" uuid NOT NULL,
    "ConcreteComponentId" uuid NOT NULL,
    "Id" uuid NOT NULL,
    CONSTRAINT "PK_component_concretization_and_generalization" PRIMARY KEY ("GeneralComponentId", "ConcreteComponentId"),
    CONSTRAINT "FK_component_concretization_and_generalization_component_Concr~" FOREIGN KEY ("ConcreteComponentId") REFERENCES metabase.component ("Id") ON DELETE CASCADE,
    CONSTRAINT "FK_component_concretization_and_generalization_component_Gener~" FOREIGN KEY ("GeneralComponentId") REFERENCES metabase.component ("Id") ON DELETE CASCADE
);

CREATE TABLE metabase.component_manufacturer (
    "ComponentId" uuid NOT NULL,
    "InstitutionId" uuid NOT NULL,
    CONSTRAINT "PK_component_manufacturer" PRIMARY KEY ("ComponentId", "InstitutionId"),
    CONSTRAINT "FK_component_manufacturer_component_ComponentId" FOREIGN KEY ("ComponentId") REFERENCES metabase.component ("Id") ON DELETE CASCADE,
    CONSTRAINT "FK_component_manufacturer_institution_InstitutionId" FOREIGN KEY ("InstitutionId") REFERENCES metabase.institution ("Id") ON DELETE CASCADE
);

CREATE TABLE metabase.database (
    "Id" uuid NOT NULL DEFAULT (gen_random_uuid()),
    "Name" text NOT NULL,
    "Description" text NOT NULL,
    "Locator" text NOT NULL,
    "OperatorId" uuid NOT NULL,
    CONSTRAINT "PK_database" PRIMARY KEY ("Id"),
    CONSTRAINT "FK_database_institution_OperatorId" FOREIGN KEY ("OperatorId") REFERENCES metabase.institution ("Id") ON DELETE CASCADE
);

CREATE TABLE metabase."DataFormats" (
    "Id" uuid NOT NULL,
    "Name" text NOT NULL,
    "Extension" text NULL,
    "Description" text NOT NULL,
    "MediaType" text NOT NULL,
    "SchemaLocator" text NULL,
    "Standard_Title" text NULL,
    "Standard_Abstract" text NULL,
    "Standard_Section" text NULL,
    "Standard_Year" integer NULL,
    "Standard_Numeration_Prefix" text NULL,
    "Standard_Numeration_MainNumber" text NULL,
    "Standard_Numeration_Suffix" text NULL,
    "Standard_Standardizers" standardizer[] NULL,
    "Standard_Locator" text NULL,
    "Publication_Title" text NULL,
    "Publication_Abstract" text NULL,
    "Publication_Section" text NULL,
    "Publication_Authors" text[] NULL,
    "Publication_Doi" text NULL,
    "Publication_ArXiv" text NULL,
    "Publication_Urn" text NULL,
    "Publication_WebAddress" text NULL,
    "ManagerId" uuid NOT NULL,
    CONSTRAINT "PK_DataFormats" PRIMARY KEY ("Id"),
    CONSTRAINT "FK_DataFormats_institution_ManagerId" FOREIGN KEY ("ManagerId") REFERENCES metabase.institution ("Id") ON DELETE CASCADE
);

CREATE TABLE metabase.institution_method_developer (
    "MethodId" uuid NOT NULL,
    "InstitutionId" uuid NOT NULL,
    "Id" uuid NOT NULL,
    CONSTRAINT "PK_institution_method_developer" PRIMARY KEY ("InstitutionId", "MethodId"),
    CONSTRAINT "FK_institution_method_developer_institution_InstitutionId" FOREIGN KEY ("InstitutionId") REFERENCES metabase.institution ("Id") ON DELETE CASCADE,
    CONSTRAINT "FK_institution_method_developer_method_MethodId" FOREIGN KEY ("MethodId") REFERENCES metabase.method ("Id") ON DELETE CASCADE
);

CREATE TABLE metabase."OpenIddictAuthorizations" (
    "Id" text NOT NULL,
    "ApplicationId" text NULL,
    "ConcurrencyToken" character varying(50) NULL,
    "CreationDate" timestamp without time zone NULL,
    "Properties" text NULL,
    "Scopes" text NULL,
    "Status" character varying(50) NULL,
    "Subject" character varying(400) NULL,
    "Type" character varying(50) NULL,
    CONSTRAINT "PK_OpenIddictAuthorizations" PRIMARY KEY ("Id"),
    CONSTRAINT "FK_OpenIddictAuthorizations_OpenIddictApplications_Application~" FOREIGN KEY ("ApplicationId") REFERENCES metabase."OpenIddictApplications" ("Id") ON DELETE RESTRICT
);

CREATE TABLE metabase.role_claim (
    "Id" integer NOT NULL GENERATED BY DEFAULT AS IDENTITY,
    "RoleId" uuid NOT NULL,
    "ClaimType" text NULL,
    "ClaimValue" text NULL,
    CONSTRAINT "PK_role_claim" PRIMARY KEY ("Id"),
    CONSTRAINT "FK_role_claim_role_RoleId" FOREIGN KEY ("RoleId") REFERENCES metabase.role ("Id") ON DELETE CASCADE
);

CREATE TABLE metabase.institution_representative (
    "InstitutionId" uuid NOT NULL,
    "UserId" uuid NOT NULL,
    "Role" institution_representative_role NOT NULL,
    CONSTRAINT "PK_institution_representative" PRIMARY KEY ("InstitutionId", "UserId"),
    CONSTRAINT "FK_institution_representative_institution_InstitutionId" FOREIGN KEY ("InstitutionId") REFERENCES metabase.institution ("Id") ON DELETE CASCADE,
    CONSTRAINT "FK_institution_representative_user_UserId" FOREIGN KEY ("UserId") REFERENCES metabase."user" ("Id") ON DELETE CASCADE
);

CREATE TABLE metabase.user_claim (
    "Id" integer NOT NULL GENERATED BY DEFAULT AS IDENTITY,
    "UserId" uuid NOT NULL,
    "ClaimType" text NULL,
    "ClaimValue" text NULL,
    CONSTRAINT "PK_user_claim" PRIMARY KEY ("Id"),
    CONSTRAINT "FK_user_claim_user_UserId" FOREIGN KEY ("UserId") REFERENCES metabase."user" ("Id") ON DELETE CASCADE
);

CREATE TABLE metabase.user_login (
    "LoginProvider" text NOT NULL,
    "ProviderKey" text NOT NULL,
    "ProviderDisplayName" text NULL,
    "UserId" uuid NOT NULL,
    CONSTRAINT "PK_user_login" PRIMARY KEY ("LoginProvider", "ProviderKey"),
    CONSTRAINT "FK_user_login_user_UserId" FOREIGN KEY ("UserId") REFERENCES metabase."user" ("Id") ON DELETE CASCADE
);

CREATE TABLE metabase.user_method_developer (
    "MethodId" uuid NOT NULL,
    "UserId" uuid NOT NULL,
    "Id" uuid NOT NULL,
    CONSTRAINT "PK_user_method_developer" PRIMARY KEY ("UserId", "MethodId"),
    CONSTRAINT "FK_user_method_developer_method_MethodId" FOREIGN KEY ("MethodId") REFERENCES metabase.method ("Id") ON DELETE CASCADE,
    CONSTRAINT "FK_user_method_developer_user_UserId" FOREIGN KEY ("UserId") REFERENCES metabase."user" ("Id") ON DELETE CASCADE
);

CREATE TABLE metabase.user_role (
    "UserId" uuid NOT NULL,
    "RoleId" uuid NOT NULL,
    CONSTRAINT "PK_user_role" PRIMARY KEY ("UserId", "RoleId"),
    CONSTRAINT "FK_user_role_role_RoleId" FOREIGN KEY ("RoleId") REFERENCES metabase.role ("Id") ON DELETE CASCADE,
    CONSTRAINT "FK_user_role_user_UserId" FOREIGN KEY ("UserId") REFERENCES metabase."user" ("Id") ON DELETE CASCADE
);

CREATE TABLE metabase.user_token (
    "UserId" uuid NOT NULL,
    "LoginProvider" text NOT NULL,
    "Name" text NOT NULL,
    "Value" text NULL,
    CONSTRAINT "PK_user_token" PRIMARY KEY ("UserId", "LoginProvider", "Name"),
    CONSTRAINT "FK_user_token_user_UserId" FOREIGN KEY ("UserId") REFERENCES metabase."user" ("Id") ON DELETE CASCADE
);

CREATE TABLE metabase."OpenIddictTokens" (
    "Id" text NOT NULL,
    "ApplicationId" text NULL,
    "AuthorizationId" text NULL,
    "ConcurrencyToken" character varying(50) NULL,
    "CreationDate" timestamp without time zone NULL,
    "ExpirationDate" timestamp without time zone NULL,
    "Payload" text NULL,
    "Properties" text NULL,
    "RedemptionDate" timestamp without time zone NULL,
    "ReferenceId" character varying(100) NULL,
    "Status" character varying(50) NULL,
    "Subject" character varying(400) NULL,
    "Type" character varying(50) NULL,
    CONSTRAINT "PK_OpenIddictTokens" PRIMARY KEY ("Id"),
    CONSTRAINT "FK_OpenIddictTokens_OpenIddictApplications_ApplicationId" FOREIGN KEY ("ApplicationId") REFERENCES metabase."OpenIddictApplications" ("Id") ON DELETE RESTRICT,
    CONSTRAINT "FK_OpenIddictTokens_OpenIddictAuthorizations_AuthorizationId" FOREIGN KEY ("AuthorizationId") REFERENCES metabase."OpenIddictAuthorizations" ("Id") ON DELETE RESTRICT
);

CREATE INDEX "IX_component_concretization_and_generalization_ConcreteCompone~" ON metabase.component_concretization_and_generalization ("ConcreteComponentId");

CREATE INDEX "IX_component_manufacturer_InstitutionId" ON metabase.component_manufacturer ("InstitutionId");

CREATE INDEX "IX_database_OperatorId" ON metabase.database ("OperatorId");

CREATE INDEX "IX_DataFormats_ManagerId" ON metabase."DataFormats" ("ManagerId");

CREATE INDEX "IX_institution_method_developer_MethodId" ON metabase.institution_method_developer ("MethodId");

CREATE INDEX "IX_institution_representative_UserId" ON metabase.institution_representative ("UserId");

CREATE UNIQUE INDEX "IX_OpenIddictApplications_ClientId" ON metabase."OpenIddictApplications" ("ClientId");

CREATE INDEX "IX_OpenIddictAuthorizations_ApplicationId_Status_Subject_Type" ON metabase."OpenIddictAuthorizations" ("ApplicationId", "Status", "Subject", "Type");

CREATE UNIQUE INDEX "IX_OpenIddictScopes_Name" ON metabase."OpenIddictScopes" ("Name");

CREATE INDEX "IX_OpenIddictTokens_ApplicationId_Status_Subject_Type" ON metabase."OpenIddictTokens" ("ApplicationId", "Status", "Subject", "Type");

CREATE INDEX "IX_OpenIddictTokens_AuthorizationId" ON metabase."OpenIddictTokens" ("AuthorizationId");

CREATE UNIQUE INDEX "IX_OpenIddictTokens_ReferenceId" ON metabase."OpenIddictTokens" ("ReferenceId");

CREATE UNIQUE INDEX "RoleNameIndex" ON metabase.role ("NormalizedName");

CREATE INDEX "IX_role_claim_RoleId" ON metabase.role_claim ("RoleId");

CREATE INDEX "EmailIndex" ON metabase."user" ("NormalizedEmail");

CREATE UNIQUE INDEX "UserNameIndex" ON metabase."user" ("NormalizedUserName");

CREATE INDEX "IX_user_claim_UserId" ON metabase.user_claim ("UserId");

CREATE INDEX "IX_user_login_UserId" ON metabase.user_login ("UserId");

CREATE INDEX "IX_user_method_developer_MethodId" ON metabase.user_method_developer ("MethodId");

CREATE INDEX "IX_user_role_RoleId" ON metabase.user_role ("RoleId");

INSERT INTO "__EFMigrationsHistory" ("MigrationId", "ProductVersion")
VALUES ('20210414155531_InitialCreate', '5.0.5');

    END IF;
END $EF$;

COMMIT;

START TRANSACTION;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "MigrationId" = '20210708152541_RenameDataFormatsTableAndRemoveIdAndXminFromMethodDeveloperRelations') THEN

ALTER TABLE metabase."DataFormats" DROP CONSTRAINT "FK_DataFormats_institution_ManagerId";

ALTER TABLE metabase."DataFormats" DROP CONSTRAINT "PK_DataFormats";

ALTER TABLE metabase.user_method_developer DROP COLUMN "Id";

ALTER TABLE metabase.institution_method_developer DROP COLUMN "Id";

ALTER TABLE metabase."DataFormats" RENAME TO data_format;

ALTER INDEX metabase."IX_DataFormats_ManagerId" RENAME TO "IX_data_format_ManagerId";

CREATE EXTENSION IF NOT EXISTS pgcrypto;

ALTER TABLE metabase.data_format ALTER COLUMN "Id" SET DEFAULT (gen_random_uuid());

ALTER TABLE metabase.data_format ADD CONSTRAINT "PK_data_format" PRIMARY KEY ("Id");

ALTER TABLE metabase.data_format ADD CONSTRAINT "FK_data_format_institution_ManagerId" FOREIGN KEY ("ManagerId") REFERENCES metabase.institution ("Id") ON DELETE CASCADE;

INSERT INTO "__EFMigrationsHistory" ("MigrationId", "ProductVersion")
VALUES ('20210708152541_RenameDataFormatsTableAndRemoveIdAndXminFromMethodDeveloperRelations', '5.0.7');

    END IF;
END $EF$;

COMMIT;

START TRANSACTION;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "MigrationId" = '20210712143023_AddMethodManagerOneToManyRelation') THEN

ALTER TABLE metabase.user_method_developer ADD "Pending" boolean NOT NULL DEFAULT TRUE;

ALTER TABLE metabase.method ADD "ManagerId" uuid NOT NULL DEFAULT '00000000-0000-0000-0000-000000000000';

ALTER TABLE metabase.institution_method_developer ADD "Pending" boolean NOT NULL DEFAULT TRUE;

CREATE INDEX "IX_method_ManagerId" ON metabase.method ("ManagerId");

ALTER TABLE metabase.method ADD CONSTRAINT "FK_method_institution_ManagerId" FOREIGN KEY ("ManagerId") REFERENCES metabase.institution ("Id") ON DELETE CASCADE;

INSERT INTO "__EFMigrationsHistory" ("MigrationId", "ProductVersion")
VALUES ('20210712143023_AddMethodManagerOneToManyRelation', '5.0.8');

    END IF;
END $EF$;

COMMIT;

START TRANSACTION;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "MigrationId" = '20210715150119_AddManagerRelationToInstitution') THEN

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

    END IF;
END $EF$;

COMMIT;

START TRANSACTION;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "MigrationId" = '20210722150010_AddPendingColumnToComponentManufacturer') THEN

ALTER TABLE metabase.component_manufacturer ADD "Pending" boolean NOT NULL DEFAULT FALSE;

INSERT INTO "__EFMigrationsHistory" ("MigrationId", "ProductVersion")
VALUES ('20210722150010_AddPendingColumnToComponentManufacturer', '5.0.8');

    END IF;
END $EF$;

COMMIT;

START TRANSACTION;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "MigrationId" = '20210805142241_UsePendingAndConfirmedAsInstitutionStates') THEN

ALTER TYPE institution_state RENAME TO institution_state_old;
CREATE TYPE institution_state AS ENUM ('pending', 'verified');
ALTER TABLE metabase.institution ALTER COLUMN "State" TYPE institution_state USING 'verified';
DROP TYPE institution_state_old;

INSERT INTO "__EFMigrationsHistory" ("MigrationId", "ProductVersion")
VALUES ('20210805142241_UsePendingAndConfirmedAsInstitutionStates', '5.0.9');

    END IF;
END $EF$;

COMMIT;

START TRANSACTION;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "MigrationId" = '20211110161448_AddReflexiveAssociationsToComponent') THEN

ALTER TABLE metabase.component_concretization_and_generalization DROP COLUMN "Id";

CREATE TABLE metabase.component_assembly (
    "AssembledComponentId" uuid NOT NULL,
    "PartComponentId" uuid NOT NULL,
    CONSTRAINT "PK_component_assembly" PRIMARY KEY ("AssembledComponentId", "PartComponentId"),
    CONSTRAINT "FK_component_assembly_component_AssembledComponentId" FOREIGN KEY ("AssembledComponentId") REFERENCES metabase.component ("Id") ON DELETE CASCADE,
    CONSTRAINT "FK_component_assembly_component_PartComponentId" FOREIGN KEY ("PartComponentId") REFERENCES metabase.component ("Id") ON DELETE CASCADE
);

CREATE TABLE metabase.component_variant (
    "OfComponentId" uuid NOT NULL,
    "ToComponentId" uuid NOT NULL,
    CONSTRAINT "PK_component_variant" PRIMARY KEY ("OfComponentId", "ToComponentId"),
    CONSTRAINT "FK_component_variant_component_OfComponentId" FOREIGN KEY ("OfComponentId") REFERENCES metabase.component ("Id") ON DELETE CASCADE,
    CONSTRAINT "FK_component_variant_component_ToComponentId" FOREIGN KEY ("ToComponentId") REFERENCES metabase.component ("Id") ON DELETE CASCADE
);

CREATE INDEX "IX_component_assembly_PartComponentId" ON metabase.component_assembly ("PartComponentId");

CREATE INDEX "IX_component_variant_ToComponentId" ON metabase.component_variant ("ToComponentId");

INSERT INTO "__EFMigrationsHistory" ("MigrationId", "ProductVersion")
VALUES ('20211110161448_AddReflexiveAssociationsToComponent', '5.0.11');

    END IF;
END $EF$;

COMMIT;

START TRANSACTION;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "MigrationId" = '20230126154403_UpgradeNpqsqlToVersion7') THEN

ALTER TABLE metabase."OpenIddictTokens" ALTER COLUMN "RedemptionDate" TYPE timestamp with time zone;

ALTER TABLE metabase."OpenIddictTokens" ALTER COLUMN "ExpirationDate" TYPE timestamp with time zone;

ALTER TABLE metabase."OpenIddictTokens" ALTER COLUMN "CreationDate" TYPE timestamp with time zone;

ALTER TABLE metabase."OpenIddictAuthorizations" ALTER COLUMN "CreationDate" TYPE timestamp with time zone;

ALTER TABLE metabase.method ALTER COLUMN "Validity" TYPE tstzrange USING tstzrange(
  lower("Validity"), upper("Validity"),
  concat(
    CASE WHEN lower_inc("Validity") THEN '[' else '(' END,
    CASE WHEN upper_inc("Validity") THEN ']' ELSE ')' END
  )
);

ALTER TABLE metabase.method ALTER COLUMN "Availability" TYPE tstzrange USING tstzrange(
  lower("Availability"), upper("Availability"),
  concat(
    CASE WHEN lower_inc("Availability") THEN '[' else '(' END,
    CASE WHEN upper_inc("Availability") THEN ']' ELSE ')' END
  )
);

ALTER TABLE metabase.component ALTER COLUMN "Availability" TYPE tstzrange USING tstzrange(
  lower("Availability"), upper("Availability"),
  concat(
    CASE WHEN lower_inc("Availability") THEN '[' else '(' END,
    CASE WHEN upper_inc("Availability") THEN ']' ELSE ')' END
  )
);

INSERT INTO "__EFMigrationsHistory" ("MigrationId", "ProductVersion")
VALUES ('20230126154403_UpgradeNpqsqlToVersion7', '7.0.2');

    END IF;
END $EF$;

COMMIT;

START TRANSACTION;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "MigrationId" = '20230210135411_AddDataProtectionKeys') THEN

CREATE TABLE metabase."DataProtectionKeys" (
    "Id" integer GENERATED BY DEFAULT AS IDENTITY,
    "FriendlyName" text NULL,
    "Xml" text NULL,
    CONSTRAINT "PK_DataProtectionKeys" PRIMARY KEY ("Id")
);

INSERT INTO "__EFMigrationsHistory" ("MigrationId", "ProductVersion")
VALUES ('20230210135411_AddDataProtectionKeys', '7.0.2');

    END IF;
END $EF$;

COMMIT;

START TRANSACTION;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "MigrationId" = '20230306150049_AddIndexAndPrimeSurfaceToAssembly') THEN

CREATE TYPE public.prime_surface AS ENUM ('inside', 'outside');

ALTER TABLE metabase.component_assembly ADD "Index" smallint;

ALTER TABLE metabase.component_assembly ADD "PrimeSurface" prime_surface;

INSERT INTO "__EFMigrationsHistory" ("MigrationId", "ProductVersion")
VALUES ('20230306150049_AddIndexAndPrimeSurfaceToAssembly', '8.0.6');

    END IF;
END $EF$;

COMMIT;

START TRANSACTION;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "MigrationId" = '20230412131346_ConfigureOnDeleteAction') THEN

ALTER TABLE metabase.data_format DROP CONSTRAINT "FK_data_format_institution_ManagerId";

ALTER TABLE metabase.database DROP CONSTRAINT "FK_database_institution_OperatorId";

ALTER TABLE metabase.institution DROP CONSTRAINT "FK_institution_institution_ManagerId";

ALTER TABLE metabase.method DROP CONSTRAINT "FK_method_institution_ManagerId";

ALTER TABLE metabase.data_format ADD CONSTRAINT "FK_data_format_institution_ManagerId" FOREIGN KEY ("ManagerId") REFERENCES metabase.institution ("Id") ON DELETE RESTRICT;

ALTER TABLE metabase.database ADD CONSTRAINT "FK_database_institution_OperatorId" FOREIGN KEY ("OperatorId") REFERENCES metabase.institution ("Id") ON DELETE RESTRICT;

ALTER TABLE metabase.institution ADD CONSTRAINT "FK_institution_institution_ManagerId" FOREIGN KEY ("ManagerId") REFERENCES metabase.institution ("Id") ON DELETE RESTRICT;

ALTER TABLE metabase.method ADD CONSTRAINT "FK_method_institution_ManagerId" FOREIGN KEY ("ManagerId") REFERENCES metabase.institution ("Id") ON DELETE RESTRICT;

INSERT INTO "__EFMigrationsHistory" ("MigrationId", "ProductVersion")
VALUES ('20230412131346_ConfigureOnDeleteAction', '8.0.6');

    END IF;
END $EF$;

COMMIT;

START TRANSACTION;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "MigrationId" = '20230419100526_AddVerificationStateAndCodeToDatabaseTable') THEN

CREATE TYPE public.database_verification_state AS ENUM ('pending', 'verified');

ALTER TABLE metabase.database ADD "VerificationCode" text NOT NULL DEFAULT '';

ALTER TABLE metabase.database ADD "VerificationState" database_verification_state NOT NULL DEFAULT 'pending'::database_verification_state;

INSERT INTO "__EFMigrationsHistory" ("MigrationId", "ProductVersion")
VALUES ('20230419100526_AddVerificationStateAndCodeToDatabaseTable', '8.0.6');

    END IF;
END $EF$;

COMMIT;

START TRANSACTION;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "MigrationId" = '20230419112023_InitializeDatabaseVerificationCodesAndSetVerificationStateToVerified') THEN

UPDATE metabase.database SET "VerificationState" = 'verified';

INSERT INTO "__EFMigrationsHistory" ("MigrationId", "ProductVersion")
VALUES ('20230419112023_InitializeDatabaseVerificationCodesAndSetVerificationStateToVerified', '8.0.6');

    END IF;
END $EF$;

COMMIT;

START TRANSACTION;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "MigrationId" = '20240118143226_UpgradeOpenIddict') THEN

ALTER TABLE metabase."OpenIddictApplications" RENAME COLUMN "Type" TO "ClientType";

ALTER TABLE metabase."OpenIddictApplications" ADD "ApplicationType" character varying(50);

ALTER TABLE metabase."OpenIddictApplications" ADD "JsonWebKeySet" text;

ALTER TABLE metabase."OpenIddictApplications" ADD "Settings" text;

INSERT INTO "__EFMigrationsHistory" ("MigrationId", "ProductVersion")
VALUES ('20240118143226_UpgradeOpenIddict', '8.0.6');

    END IF;
END $EF$;

COMMIT;

START TRANSACTION;

DO $XX$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "MigrationId" = '20240522093622_AddOperatingState') THEN

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

    END IF;
END $XX$;

COMMIT;

START TRANSACTION;

DO $XX$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "MigrationId" = '20240722123912_MoveEnumTypesToMetabaseSchema') THEN

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

    END IF;
END $XX$;

COMMIT;

START TRANSACTION;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "MigrationId" = '20241202180656_AddPrimeSurfaceAndDirectionAndSurfaceLayersToComponent') THEN
ALTER TABLE metabase.component ADD "PrimeDirection_Description" text;

ALTER TABLE metabase.component ADD "PrimeDirection_Exists" boolean;

ALTER TABLE metabase.component ADD "PrimeDirection_Reference_Exists" boolean;

ALTER TABLE metabase.component ADD "PrimeDirection_Reference_Publication_Abstract" text;

ALTER TABLE metabase.component ADD "PrimeDirection_Reference_Publication_ArXiv" text;

ALTER TABLE metabase.component ADD "PrimeDirection_Reference_Publication_Authors" text[];

ALTER TABLE metabase.component ADD "PrimeDirection_Reference_Publication_Doi" text;

ALTER TABLE metabase.component ADD "PrimeDirection_Reference_Publication_Section" text;

ALTER TABLE metabase.component ADD "PrimeDirection_Reference_Publication_Title" text;

ALTER TABLE metabase.component ADD "PrimeDirection_Reference_Publication_Urn" text;

ALTER TABLE metabase.component ADD "PrimeDirection_Reference_Publication_WebAddress" text;

ALTER TABLE metabase.component ADD "PrimeDirection_Reference_Standard_Abstract" text;

ALTER TABLE metabase.component ADD "PrimeDirection_Reference_Standard_Locator" text;

ALTER TABLE metabase.component ADD "PrimeDirection_Reference_Standard_Numeration_MainNumber" text;

ALTER TABLE metabase.component ADD "PrimeDirection_Reference_Standard_Numeration_Prefix" text;

ALTER TABLE metabase.component ADD "PrimeDirection_Reference_Standard_Numeration_Suffix" text;

ALTER TABLE metabase.component ADD "PrimeDirection_Reference_Standard_Section" text;

ALTER TABLE metabase.component ADD "PrimeDirection_Reference_Standard_Standardizers" metabase.standardizer[];

ALTER TABLE metabase.component ADD "PrimeDirection_Reference_Standard_Title" text;

ALTER TABLE metabase.component ADD "PrimeDirection_Reference_Standard_Year" integer;

ALTER TABLE metabase.component ADD "PrimeSurface_Description" text;

ALTER TABLE metabase.component ADD "PrimeSurface_Exists" boolean;

ALTER TABLE metabase.component ADD "PrimeSurface_Reference_Exists" boolean;

ALTER TABLE metabase.component ADD "PrimeSurface_Reference_Publication_Abstract" text;

ALTER TABLE metabase.component ADD "PrimeSurface_Reference_Publication_ArXiv" text;

ALTER TABLE metabase.component ADD "PrimeSurface_Reference_Publication_Authors" text[];

ALTER TABLE metabase.component ADD "PrimeSurface_Reference_Publication_Doi" text;

ALTER TABLE metabase.component ADD "PrimeSurface_Reference_Publication_Section" text;

ALTER TABLE metabase.component ADD "PrimeSurface_Reference_Publication_Title" text;

ALTER TABLE metabase.component ADD "PrimeSurface_Reference_Publication_Urn" text;

ALTER TABLE metabase.component ADD "PrimeSurface_Reference_Publication_WebAddress" text;

ALTER TABLE metabase.component ADD "PrimeSurface_Reference_Standard_Abstract" text;

ALTER TABLE metabase.component ADD "PrimeSurface_Reference_Standard_Locator" text;

ALTER TABLE metabase.component ADD "PrimeSurface_Reference_Standard_Numeration_MainNumber" text;

ALTER TABLE metabase.component ADD "PrimeSurface_Reference_Standard_Numeration_Prefix" text;

ALTER TABLE metabase.component ADD "PrimeSurface_Reference_Standard_Numeration_Suffix" text;

ALTER TABLE metabase.component ADD "PrimeSurface_Reference_Standard_Section" text;

ALTER TABLE metabase.component ADD "PrimeSurface_Reference_Standard_Standardizers" metabase.standardizer[];

ALTER TABLE metabase.component ADD "PrimeSurface_Reference_Standard_Title" text;

ALTER TABLE metabase.component ADD "PrimeSurface_Reference_Standard_Year" integer;

ALTER TABLE metabase.component ADD "SwitchableLayers_Description" text;

ALTER TABLE metabase.component ADD "SwitchableLayers_Exists" boolean;

ALTER TABLE metabase.component ADD "SwitchableLayers_Reference_Exists" boolean;

ALTER TABLE metabase.component ADD "SwitchableLayers_Reference_Publication_Abstract" text;

ALTER TABLE metabase.component ADD "SwitchableLayers_Reference_Publication_ArXiv" text;

ALTER TABLE metabase.component ADD "SwitchableLayers_Reference_Publication_Authors" text[];

ALTER TABLE metabase.component ADD "SwitchableLayers_Reference_Publication_Doi" text;

ALTER TABLE metabase.component ADD "SwitchableLayers_Reference_Publication_Section" text;

ALTER TABLE metabase.component ADD "SwitchableLayers_Reference_Publication_Title" text;

ALTER TABLE metabase.component ADD "SwitchableLayers_Reference_Publication_Urn" text;

ALTER TABLE metabase.component ADD "SwitchableLayers_Reference_Publication_WebAddress" text;

ALTER TABLE metabase.component ADD "SwitchableLayers_Reference_Standard_Abstract" text;

ALTER TABLE metabase.component ADD "SwitchableLayers_Reference_Standard_Locator" text;

ALTER TABLE metabase.component ADD "SwitchableLayers_Reference_Standard_Numeration_MainNumber" text;

ALTER TABLE metabase.component ADD "SwitchableLayers_Reference_Standard_Numeration_Prefix" text;

ALTER TABLE metabase.component ADD "SwitchableLayers_Reference_Standard_Numeration_Suffix" text;

ALTER TABLE metabase.component ADD "SwitchableLayers_Reference_Standard_Section" text;

ALTER TABLE metabase.component ADD "SwitchableLayers_Reference_Standard_Standardizers" metabase.standardizer[];

ALTER TABLE metabase.component ADD "SwitchableLayers_Reference_Standard_Title" text;

ALTER TABLE metabase.component ADD "SwitchableLayers_Reference_Standard_Year" integer;

INSERT INTO "__EFMigrationsHistory" ("MigrationId", "ProductVersion")
VALUES ('20241202180656_AddPrimeSurfaceAndDirectionAndSurfaceLayersToComponent', '9.0.1');

    END IF;
END $EF$;

COMMIT;

START TRANSACTION;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "MigrationId" = '20241213072103_AddRelationInstitutionToApplication') THEN

ALTER TABLE metabase."OpenIddictTokens" DROP CONSTRAINT "FK_OpenIddictTokens_OpenIddictApplications_ApplicationId";
ALTER TABLE metabase."OpenIddictTokens" DROP CONSTRAINT "FK_OpenIddictTokens_OpenIddictAuthorizations_AuthorizationId";
ALTER TABLE metabase."OpenIddictAuthorizations" DROP CONSTRAINT "FK_OpenIddictAuthorizations_OpenIddictApplications_Application~";

ALTER TABLE metabase."OpenIddictTokens" ALTER COLUMN "AuthorizationId" TYPE uuid USING "AuthorizationId"::text::uuid;

ALTER TABLE metabase."OpenIddictTokens" ALTER COLUMN "ApplicationId" TYPE uuid USING "ApplicationId"::text::uuid;

ALTER TABLE metabase."OpenIddictTokens" ALTER COLUMN "Id" TYPE uuid USING "Id"::text::uuid;

ALTER TABLE metabase."OpenIddictScopes" ALTER COLUMN "Id" TYPE uuid USING "Id"::text::uuid;

ALTER TABLE metabase."OpenIddictAuthorizations" ALTER COLUMN "ApplicationId" TYPE uuid USING "ApplicationId"::text::uuid;

ALTER TABLE metabase."OpenIddictAuthorizations" ALTER COLUMN "Id" TYPE uuid USING "Id"::text::uuid;

ALTER TABLE metabase."OpenIddictApplications" ALTER COLUMN "Id" TYPE uuid USING "Id"::text::uuid;

ALTER TABLE metabase."OpenIddictTokens" ADD CONSTRAINT "FK_OpenIddictTokens_OpenIddictApplications_ApplicationId" FOREIGN KEY ("ApplicationId") REFERENCES metabase."OpenIddictApplications" ("Id") ON DELETE RESTRICT;
ALTER TABLE metabase."OpenIddictTokens" ADD CONSTRAINT "FK_OpenIddictTokens_OpenIddictAuthorizations_AuthorizationId" FOREIGN KEY ("AuthorizationId") REFERENCES metabase."OpenIddictAuthorizations" ("Id") ON DELETE RESTRICT;
ALTER TABLE metabase."OpenIddictAuthorizations" ADD CONSTRAINT "FK_OpenIddictAuthorizations_OpenIddictApplications_Application~" FOREIGN KEY ("ApplicationId") REFERENCES metabase."OpenIddictApplications" ("Id") ON DELETE RESTRICT;

CREATE TABLE metabase.institution_application (
    "InstitutionId" uuid NOT NULL,
    "ApplicationId" uuid NOT NULL,
    CONSTRAINT "PK_institution_application" PRIMARY KEY ("InstitutionId", "ApplicationId"),
    CONSTRAINT "FK_institution_application_OpenIddictApplications_ApplicationId" FOREIGN KEY ("ApplicationId") REFERENCES metabase."OpenIddictApplications" ("Id") ON DELETE CASCADE,
    CONSTRAINT "FK_institution_application_institution_InstitutionId" FOREIGN KEY ("InstitutionId") REFERENCES metabase.institution ("Id") ON DELETE CASCADE
);

CREATE INDEX "IX_institution_application_ApplicationId" ON metabase.institution_application ("ApplicationId");

INSERT INTO "__EFMigrationsHistory" ("MigrationId", "ProductVersion")
VALUES ('20241213072103_AddRelationInstitutionToApplication', '9.0.1');


    END IF;
END $EF$;

COMMIT;

START TRANSACTION;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "MigrationId" = '20250129164308_UpgradeToNet9') THEN

INSERT INTO "__EFMigrationsHistory" ("MigrationId", "ProductVersion")
VALUES ('20250129164308_UpgradeToNet9', '9.0.1');

    END IF;
END $EF$;

COMMIT;

START TRANSACTION;

DO $XX$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "MigrationId" = '20250409132838_AddSigningPermissionAndFingerprint') THEN
DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM pg_namespace WHERE nspname = 'metabase') THEN
        CREATE SCHEMA metabase;
    END IF;
END $EF$;

CREATE TYPE metabase.data_signing_permission AS ENUM ('allowed', 'forbidden', 'never');

ALTER TABLE metabase.institution_representative ADD "DataSigningPermission" metabase.data_signing_permission NOT NULL DEFAULT 'never'::metabase.data_signing_permission;

ALTER TABLE metabase.institution_representative ADD "KeyFingerprints" text[] NOT NULL DEFAULT ARRAY[]::text[];

INSERT INTO "__EFMigrationsHistory" ("MigrationId", "ProductVersion")
VALUES ('20250409132838_AddSigningPermissionAndFingerprint', '9.0.5');

    END IF;
END $XX$;

COMMIT;

START TRANSACTION;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "MigrationId" = '20250604122410_AddExtrasToComponentAndInstitution') THEN
ALTER TABLE metabase.institution ADD "Extras" jsonb;

ALTER TABLE metabase.component ADD "Extras" jsonb;

INSERT INTO "__EFMigrationsHistory" ("MigrationId", "ProductVersion")
VALUES ('20250604122410_AddExtrasToComponentAndInstitution', '9.0.5');

    END IF;
END $EF$;

COMMIT;

START TRANSACTION;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "MigrationId" = '20250714160511_InstitutionOpenIdConnectApplicationAssociationProperly') THEN

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

    END IF;
END $EF$;

COMMIT;

START TRANSACTION;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "MigrationId" = '20250725171644_UpgradeOpenIddictToVersion7') THEN
ALTER TABLE metabase."OpenIddictTokens" ALTER COLUMN "Type" TYPE character varying(150);

INSERT INTO "__EFMigrationsHistory" ("MigrationId", "ProductVersion")
VALUES ('20250725171644_UpgradeOpenIddictToVersion7', '9.0.7');

    END IF;
END $EF$;

COMMIT;

START TRANSACTION;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "MigrationId" = '20250812162009_AddPrefixGnuPgToKeyFingerprints') THEN
ALTER TABLE metabase.institution_representative RENAME COLUMN "KeyFingerprints" TO "GnuPgKeyFingerprints";

INSERT INTO "__EFMigrationsHistory" ("MigrationId", "ProductVersion")
VALUES ('20250812162009_AddPrefixGnuPgToKeyFingerprints', '9.0.7');

    END IF;
END $EF$;

COMMIT;

START TRANSACTION;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "MigrationId" = '20250815155558_MakeOpenIddictApplicationsBelongToExactlyOneInstitution') THEN

ALTER TABLE metabase."OpenIddictApplications" ADD "OwnerId" uuid NOT NULL DEFAULT '00000000-0000-0000-0000-000000000000';

CREATE INDEX "IX_OpenIddictApplications_OwnerId" ON metabase."OpenIddictApplications" ("OwnerId");

UPDATE metabase."OpenIddictApplications" SET "OwnerId" = "InstitutionId" FROM metabase.institution_open_id_connect_application WHERE "ApplicationId" = "Id";

ALTER TABLE metabase."OpenIddictApplications" ADD CONSTRAINT "FK_OpenIddictApplications_institution_OwnerId" FOREIGN KEY ("OwnerId") REFERENCES metabase.institution ("Id") ON DELETE RESTRICT;

DROP TABLE metabase.institution_open_id_connect_application;

INSERT INTO "__EFMigrationsHistory" ("MigrationId", "ProductVersion")
VALUES ('20250815155558_MakeOpenIddictApplicationsBelongToExactlyOneInstitution', '9.0.7');

    END IF;
END $EF$;

COMMIT;

START TRANSACTION;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "MigrationId" = '20250826140321_MakeGnuPgFingerprintItsOwnEntity') THEN

ALTER TABLE metabase.institution_representative DROP COLUMN "DataSigningPermission";

ALTER TABLE metabase.institution_representative DROP COLUMN "GnuPgKeyFingerprints";

DROP TYPE metabase.data_signing_permission;

CREATE TABLE metabase.gnu_pg_fingerprint (
    "Id" uuid NOT NULL DEFAULT (gen_random_uuid()),
    "Fingerprint" text NOT NULL,
    "CreationDate" timestamp with time zone NOT NULL,
    "RevocationDate" timestamp with time zone,
    "UserId" uuid NOT NULL,
    "InstitutionId" uuid NOT NULL,
    CONSTRAINT "PK_gnu_pg_fingerprint" PRIMARY KEY ("Id"),
    CONSTRAINT "FK_gnu_pg_fingerprint_institution_InstitutionId" FOREIGN KEY ("InstitutionId") REFERENCES metabase.institution ("Id") ON DELETE CASCADE,
    CONSTRAINT "FK_gnu_pg_fingerprint_user_UserId" FOREIGN KEY ("UserId") REFERENCES metabase."user" ("Id") ON DELETE CASCADE
);

CREATE UNIQUE INDEX "IX_gnu_pg_fingerprint_Fingerprint" ON metabase.gnu_pg_fingerprint ("Fingerprint");

CREATE INDEX "IX_gnu_pg_fingerprint_InstitutionId" ON metabase.gnu_pg_fingerprint ("InstitutionId");

CREATE INDEX "IX_gnu_pg_fingerprint_UserId" ON metabase.gnu_pg_fingerprint ("UserId");

INSERT INTO "__EFMigrationsHistory" ("MigrationId", "ProductVersion")
VALUES ('20250826140321_MakeGnuPgFingerprintItsOwnEntity', '9.0.7');

    END IF;
END $EF$;

COMMIT;

START TRANSACTION;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "MigrationId" = '20250828103451_AlignFieldNamesOfFingerprint') THEN

ALTER TABLE metabase.gnu_pg_fingerprint RENAME COLUMN "RevocationDate" TO "RevokedAt";

ALTER TABLE metabase.gnu_pg_fingerprint RENAME COLUMN "CreationDate" TO "CreatedAt";

ALTER TABLE metabase.gnu_pg_fingerprint ADD "AllowedAt" timestamp with time zone;

INSERT INTO "__EFMigrationsHistory" ("MigrationId", "ProductVersion")
VALUES ('20250828103451_AlignFieldNamesOfFingerprint', '9.0.7');

    END IF;
END $EF$;

COMMIT;

START TRANSACTION;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "MigrationId" = '20250829124512_UseForbidInsteadOfRevokeForGnuPgKeyFingerprint') THEN

ALTER TABLE metabase.gnu_pg_fingerprint RENAME COLUMN "RevokedAt" TO "ForbiddenAt";

INSERT INTO "__EFMigrationsHistory" ("MigrationId", "ProductVersion")
VALUES ('20250829124512_UseForbidInsteadOfRevokeForGnuPgKeyFingerprint', '9.0.7');

    END IF;
END $EF$;

COMMIT;
START TRANSACTION;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "MigrationId" = '20251124184659_AddContactInformationToInstitution') THEN
    ALTER TABLE metabase.institution RENAME COLUMN "WebsiteLocator" TO "Contact_WebsiteLocator";
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "MigrationId" = '20251124184659_AddContactInformationToInstitution') THEN
    ALTER TABLE metabase.institution RENAME COLUMN "PublicKey" TO "Contact_PostalAddress";
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "MigrationId" = '20251124184659_AddContactInformationToInstitution') THEN
    ALTER TABLE metabase.institution ADD "Contact_EmailAddress" text;
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "MigrationId" = '20251124184659_AddContactInformationToInstitution') THEN
    ALTER TABLE metabase.institution ADD "Contact_Exists" boolean;
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "MigrationId" = '20251124184659_AddContactInformationToInstitution') THEN
    ALTER TABLE metabase.institution ADD "Contact_PhoneNumber" text;
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "MigrationId" = '20251124184659_AddContactInformationToInstitution') THEN
    INSERT INTO "__EFMigrationsHistory" ("MigrationId", "ProductVersion")
    VALUES ('20251124184659_AddContactInformationToInstitution', '9.0.10');
    END IF;
END $EF$;
COMMIT;

START TRANSACTION;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "MigrationId" = '20251128175152_AddParametersAndSourcesToMethods') THEN
    ALTER TABLE metabase.method RENAME COLUMN "Standard_Year" TO "Reference_Standard_Year";
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "MigrationId" = '20251128175152_AddParametersAndSourcesToMethods') THEN
    ALTER TABLE metabase.method RENAME COLUMN "Standard_Title" TO "Reference_Standard_Title";
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "MigrationId" = '20251128175152_AddParametersAndSourcesToMethods') THEN
    ALTER TABLE metabase.method RENAME COLUMN "Standard_Standardizers" TO "Reference_Standard_Standardizers";
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "MigrationId" = '20251128175152_AddParametersAndSourcesToMethods') THEN
    ALTER TABLE metabase.method RENAME COLUMN "Standard_Section" TO "Reference_Standard_Section";
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "MigrationId" = '20251128175152_AddParametersAndSourcesToMethods') THEN
    ALTER TABLE metabase.method RENAME COLUMN "Standard_Numeration_Suffix" TO "Reference_Standard_Numeration_Suffix";
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "MigrationId" = '20251128175152_AddParametersAndSourcesToMethods') THEN
    ALTER TABLE metabase.method RENAME COLUMN "Standard_Numeration_Prefix" TO "Reference_Standard_Numeration_Prefix";
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "MigrationId" = '20251128175152_AddParametersAndSourcesToMethods') THEN
    ALTER TABLE metabase.method RENAME COLUMN "Standard_Numeration_MainNumber" TO "Reference_Standard_Numeration_MainNumber";
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "MigrationId" = '20251128175152_AddParametersAndSourcesToMethods') THEN
    ALTER TABLE metabase.method RENAME COLUMN "Standard_Locator" TO "Reference_Standard_Locator";
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "MigrationId" = '20251128175152_AddParametersAndSourcesToMethods') THEN
    ALTER TABLE metabase.method RENAME COLUMN "Standard_Abstract" TO "Reference_Standard_Abstract";
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "MigrationId" = '20251128175152_AddParametersAndSourcesToMethods') THEN
    ALTER TABLE metabase.method RENAME COLUMN "Publication_WebAddress" TO "Reference_Publication_WebAddress";
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "MigrationId" = '20251128175152_AddParametersAndSourcesToMethods') THEN
    ALTER TABLE metabase.method RENAME COLUMN "Publication_Urn" TO "Reference_Publication_Urn";
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "MigrationId" = '20251128175152_AddParametersAndSourcesToMethods') THEN
    ALTER TABLE metabase.method RENAME COLUMN "Publication_Title" TO "Reference_Publication_Title";
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "MigrationId" = '20251128175152_AddParametersAndSourcesToMethods') THEN
    ALTER TABLE metabase.method RENAME COLUMN "Publication_Section" TO "Reference_Publication_Section";
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "MigrationId" = '20251128175152_AddParametersAndSourcesToMethods') THEN
    ALTER TABLE metabase.method RENAME COLUMN "Publication_Doi" TO "Reference_Publication_Doi";
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "MigrationId" = '20251128175152_AddParametersAndSourcesToMethods') THEN
    ALTER TABLE metabase.method RENAME COLUMN "Publication_Authors" TO "Reference_Publication_Authors";
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "MigrationId" = '20251128175152_AddParametersAndSourcesToMethods') THEN
    ALTER TABLE metabase.method RENAME COLUMN "Publication_ArXiv" TO "Reference_Publication_ArXiv";
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "MigrationId" = '20251128175152_AddParametersAndSourcesToMethods') THEN
    ALTER TABLE metabase.method RENAME COLUMN "Publication_Abstract" TO "Reference_Publication_Abstract";
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "MigrationId" = '20251128175152_AddParametersAndSourcesToMethods') THEN
    ALTER TABLE metabase.data_format RENAME COLUMN "Standard_Year" TO "Reference_Standard_Year";
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "MigrationId" = '20251128175152_AddParametersAndSourcesToMethods') THEN
    ALTER TABLE metabase.data_format RENAME COLUMN "Standard_Title" TO "Reference_Standard_Title";
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "MigrationId" = '20251128175152_AddParametersAndSourcesToMethods') THEN
    ALTER TABLE metabase.data_format RENAME COLUMN "Standard_Standardizers" TO "Reference_Standard_Standardizers";
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "MigrationId" = '20251128175152_AddParametersAndSourcesToMethods') THEN
    ALTER TABLE metabase.data_format RENAME COLUMN "Standard_Section" TO "Reference_Standard_Section";
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "MigrationId" = '20251128175152_AddParametersAndSourcesToMethods') THEN
    ALTER TABLE metabase.data_format RENAME COLUMN "Standard_Numeration_Suffix" TO "Reference_Standard_Numeration_Suffix";
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "MigrationId" = '20251128175152_AddParametersAndSourcesToMethods') THEN
    ALTER TABLE metabase.data_format RENAME COLUMN "Standard_Numeration_Prefix" TO "Reference_Standard_Numeration_Prefix";
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "MigrationId" = '20251128175152_AddParametersAndSourcesToMethods') THEN
    ALTER TABLE metabase.data_format RENAME COLUMN "Standard_Numeration_MainNumber" TO "Reference_Standard_Numeration_MainNumber";
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "MigrationId" = '20251128175152_AddParametersAndSourcesToMethods') THEN
    ALTER TABLE metabase.data_format RENAME COLUMN "Standard_Locator" TO "Reference_Standard_Locator";
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "MigrationId" = '20251128175152_AddParametersAndSourcesToMethods') THEN
    ALTER TABLE metabase.data_format RENAME COLUMN "Standard_Abstract" TO "Reference_Standard_Abstract";
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "MigrationId" = '20251128175152_AddParametersAndSourcesToMethods') THEN
    ALTER TABLE metabase.data_format RENAME COLUMN "Publication_WebAddress" TO "Reference_Publication_WebAddress";
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "MigrationId" = '20251128175152_AddParametersAndSourcesToMethods') THEN
    ALTER TABLE metabase.data_format RENAME COLUMN "Publication_Urn" TO "Reference_Publication_Urn";
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "MigrationId" = '20251128175152_AddParametersAndSourcesToMethods') THEN
    ALTER TABLE metabase.data_format RENAME COLUMN "Publication_Title" TO "Reference_Publication_Title";
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "MigrationId" = '20251128175152_AddParametersAndSourcesToMethods') THEN
    ALTER TABLE metabase.data_format RENAME COLUMN "Publication_Section" TO "Reference_Publication_Section";
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "MigrationId" = '20251128175152_AddParametersAndSourcesToMethods') THEN
    ALTER TABLE metabase.data_format RENAME COLUMN "Publication_Doi" TO "Reference_Publication_Doi";
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "MigrationId" = '20251128175152_AddParametersAndSourcesToMethods') THEN
    ALTER TABLE metabase.data_format RENAME COLUMN "Publication_Authors" TO "Reference_Publication_Authors";
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "MigrationId" = '20251128175152_AddParametersAndSourcesToMethods') THEN
    ALTER TABLE metabase.data_format RENAME COLUMN "Publication_ArXiv" TO "Reference_Publication_ArXiv";
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "MigrationId" = '20251128175152_AddParametersAndSourcesToMethods') THEN
    ALTER TABLE metabase.data_format RENAME COLUMN "Publication_Abstract" TO "Reference_Publication_Abstract";
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "MigrationId" = '20251128175152_AddParametersAndSourcesToMethods') THEN
    ALTER TABLE metabase.method ADD "Reference_Exists" boolean;
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "MigrationId" = '20251128175152_AddParametersAndSourcesToMethods') THEN
    ALTER TABLE metabase.data_format ADD "Reference_Exists" boolean;
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "MigrationId" = '20251128175152_AddParametersAndSourcesToMethods') THEN
    CREATE TABLE metabase."MethodParameter" (
        "MethodId" uuid NOT NULL,
        "Id" integer GENERATED BY DEFAULT AS IDENTITY,
        "Name" text NOT NULL,
        "Type" jsonb NOT NULL,
        CONSTRAINT "PK_MethodParameter" PRIMARY KEY ("MethodId", "Id"),
        CONSTRAINT "FK_MethodParameter_method_MethodId" FOREIGN KEY ("MethodId") REFERENCES metabase.method ("Id") ON DELETE CASCADE
    );
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "MigrationId" = '20251128175152_AddParametersAndSourcesToMethods') THEN
    CREATE TABLE metabase."MethodSource" (
        "MethodId" uuid NOT NULL,
        "Id" integer GENERATED BY DEFAULT AS IDENTITY,
        "Name" text NOT NULL,
        "Description" text NOT NULL,
        CONSTRAINT "PK_MethodSource" PRIMARY KEY ("MethodId", "Id"),
        CONSTRAINT "FK_MethodSource_method_MethodId" FOREIGN KEY ("MethodId") REFERENCES metabase.method ("Id") ON DELETE CASCADE
    );
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "MigrationId" = '20251128175152_AddParametersAndSourcesToMethods') THEN
    INSERT INTO "__EFMigrationsHistory" ("MigrationId", "ProductVersion")
    VALUES ('20251128175152_AddParametersAndSourcesToMethods', '9.0.10');
    END IF;
END $EF$;
COMMIT;

START TRANSACTION;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "MigrationId" = '20251205154453_AddConfirmationBooleansToContactInformation') THEN
    ALTER TABLE metabase.institution ADD "Contact_IsPhoneNumberConfirmed" boolean;
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "MigrationId" = '20251205154453_AddConfirmationBooleansToContactInformation') THEN
    ALTER TABLE metabase.institution ADD "Contact_IsEmailAddressConfirmed" boolean;
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "MigrationId" = '20251205154453_AddConfirmationBooleansToContactInformation') THEN
    INSERT INTO "__EFMigrationsHistory" ("MigrationId", "ProductVersion")
    VALUES ('20251205154453_AddConfirmationBooleansToContactInformation', '10.0.0');
    END IF;
END $EF$;
COMMIT;

START TRANSACTION;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "MigrationId" = '20260210104058_AddManagerToComponent') THEN
    ALTER TABLE metabase.method ADD "Reference_Publication_Exists" boolean;
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "MigrationId" = '20260210104058_AddManagerToComponent') THEN
    ALTER TABLE metabase.method ADD "Reference_Standard_Exists" boolean;
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "MigrationId" = '20260210104058_AddManagerToComponent') THEN
    ALTER TABLE metabase.data_format ADD "Reference_Publication_Exists" boolean;
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "MigrationId" = '20260210104058_AddManagerToComponent') THEN
    ALTER TABLE metabase.data_format ADD "Reference_Standard_Exists" boolean;
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "MigrationId" = '20260210104058_AddManagerToComponent') THEN
    ALTER TABLE metabase.component ADD "ManagerId" uuid NOT NULL DEFAULT '00000000-0000-0000-0000-000000000000';
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "MigrationId" = '20260210104058_AddManagerToComponent') THEN
    ALTER TABLE metabase.component ADD "PrimeDirection_Reference_Publication_Exists" boolean;
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "MigrationId" = '20260210104058_AddManagerToComponent') THEN
    ALTER TABLE metabase.component ADD "PrimeDirection_Reference_Standard_Exists" boolean;
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "MigrationId" = '20260210104058_AddManagerToComponent') THEN
    ALTER TABLE metabase.component ADD "PrimeSurface_Reference_Publication_Exists" boolean;
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "MigrationId" = '20260210104058_AddManagerToComponent') THEN
    ALTER TABLE metabase.component ADD "PrimeSurface_Reference_Standard_Exists" boolean;
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "MigrationId" = '20260210104058_AddManagerToComponent') THEN
    ALTER TABLE metabase.component ADD "SwitchableLayers_Reference_Publication_Exists" boolean;
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "MigrationId" = '20260210104058_AddManagerToComponent') THEN
    ALTER TABLE metabase.component ADD "SwitchableLayers_Reference_Standard_Exists" boolean;
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "MigrationId" = '20260210104058_AddManagerToComponent') THEN
    CREATE INDEX "IX_component_ManagerId" ON metabase.component ("ManagerId");
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "MigrationId" = '20260210104058_AddManagerToComponent') THEN
    update metabase.component c set "ManagerId" = (
            select COALESCE(i1."ManagerId", m1."InstitutionId")
            from metabase.component_manufacturer m1
            join metabase.institution i1 on i1."Id" = m1."InstitutionId"
            where m1."ComponentId" = c."Id"
            limit 1
        )
        where "ManagerId" = '00000000-0000-0000-0000-000000000000';
    ALTER TABLE metabase.component ADD CONSTRAINT "FK_component_institution_ManagerId" FOREIGN KEY ("ManagerId") REFERENCES metabase.institution ("Id") ON DELETE RESTRICT;
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "MigrationId" = '20260210104058_AddManagerToComponent') THEN
    INSERT INTO "__EFMigrationsHistory" ("MigrationId", "ProductVersion")
    VALUES ('20260210104058_AddManagerToComponent', '10.0.2');
    END IF;
END $EF$;
COMMIT;

START TRANSACTION;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "MigrationId" = '20260318153447_CorrectExistsFlagsOfReferences') THEN

                    UPDATE metabase.method
                    SET 
                        "Reference_Standard_Exists" = (
                            "Reference_Standard_Title" IS NOT NULL OR
                            "Reference_Standard_Abstract" IS NOT NULL OR
                            "Reference_Standard_Section" IS NOT NULL OR
                            "Reference_Standard_Year" IS NOT NULL OR
                            "Reference_Standard_Numeration_Prefix" IS NOT NULL OR
                            "Reference_Standard_Numeration_MainNumber" IS NOT NULL OR
                            "Reference_Standard_Numeration_Suffix" IS NOT NULL OR
                            "Reference_Standard_Standardizers" IS NOT NULL OR
                            "Reference_Standard_Locator" IS NOT NULL
                        ),
                        "Reference_Publication_Exists" = (
                            "Reference_Publication_Title" IS NOT NULL OR
                            "Reference_Publication_Abstract" IS NOT NULL OR
                            "Reference_Publication_Section" IS NOT NULL OR
                            "Reference_Publication_Authors" IS NOT NULL OR
                            "Reference_Publication_Doi" IS NOT NULL OR
                            "Reference_Publication_ArXiv" IS NOT NULL OR
                            "Reference_Publication_Urn" IS NOT NULL OR
                            "Reference_Publication_WebAddress" IS NOT NULL
                        ),
                        "Reference_Exists" = (
                            "Reference_Standard_Title" IS NOT NULL OR
                            "Reference_Standard_Abstract" IS NOT NULL OR
                            "Reference_Standard_Section" IS NOT NULL OR
                            "Reference_Standard_Year" IS NOT NULL OR
                            "Reference_Standard_Numeration_Prefix" IS NOT NULL OR
                            "Reference_Standard_Numeration_MainNumber" IS NOT NULL OR
                            "Reference_Standard_Numeration_Suffix" IS NOT NULL OR
                            "Reference_Standard_Standardizers" IS NOT NULL OR
                            "Reference_Standard_Locator" IS NOT NULL OR
                            "Reference_Publication_Title" IS NOT NULL OR
                            "Reference_Publication_Abstract" IS NOT NULL OR
                            "Reference_Publication_Section" IS NOT NULL OR
                            "Reference_Publication_Authors" IS NOT NULL OR
                            "Reference_Publication_Doi" IS NOT NULL OR
                            "Reference_Publication_ArXiv" IS NOT NULL OR
                            "Reference_Publication_Urn" IS NOT NULL OR
                            "Reference_Publication_WebAddress" IS NOT NULL
                        );

                    UPDATE metabase.data_format
                    SET
                        "Reference_Standard_Exists" = (
                            "Reference_Standard_Title" IS NOT NULL OR
                            "Reference_Standard_Abstract" IS NOT NULL OR
                            "Reference_Standard_Section" IS NOT NULL OR
                            "Reference_Standard_Year" IS NOT NULL OR
                            "Reference_Standard_Numeration_Prefix" IS NOT NULL OR
                            "Reference_Standard_Numeration_MainNumber" IS NOT NULL OR
                            "Reference_Standard_Numeration_Suffix" IS NOT NULL OR
                            "Reference_Standard_Standardizers" IS NOT NULL OR
                            "Reference_Standard_Locator" IS NOT NULL
                        ),
                        "Reference_Publication_Exists" = (
                            "Reference_Publication_Title" IS NOT NULL OR
                            "Reference_Publication_Abstract" IS NOT NULL OR
                            "Reference_Publication_Section" IS NOT NULL OR
                            "Reference_Publication_Authors" IS NOT NULL OR
                            "Reference_Publication_Doi" IS NOT NULL OR
                            "Reference_Publication_ArXiv" IS NOT NULL OR
                            "Reference_Publication_Urn" IS NOT NULL OR
                            "Reference_Publication_WebAddress" IS NOT NULL
                        ),
                        "Reference_Exists" = (
                            "Reference_Standard_Title" IS NOT NULL OR
                            "Reference_Standard_Abstract" IS NOT NULL OR
                            "Reference_Standard_Section" IS NOT NULL OR
                            "Reference_Standard_Year" IS NOT NULL OR
                            "Reference_Standard_Numeration_Prefix" IS NOT NULL OR
                            "Reference_Standard_Numeration_MainNumber" IS NOT NULL OR
                            "Reference_Standard_Numeration_Suffix" IS NOT NULL OR
                            "Reference_Standard_Standardizers" IS NOT NULL OR
                            "Reference_Standard_Locator" IS NOT NULL OR
                            "Reference_Publication_Title" IS NOT NULL OR
                            "Reference_Publication_Abstract" IS NOT NULL OR
                            "Reference_Publication_Section" IS NOT NULL OR
                            "Reference_Publication_Authors" IS NOT NULL OR
                            "Reference_Publication_Doi" IS NOT NULL OR
                            "Reference_Publication_ArXiv" IS NOT NULL OR
                            "Reference_Publication_Urn" IS NOT NULL OR
                            "Reference_Publication_WebAddress" IS NOT NULL
                        );

                    UPDATE metabase.component
                    SET
                        "PrimeSurface_Reference_Standard_Exists" = (
                            "PrimeSurface_Reference_Standard_Title" IS NOT NULL OR
                            "PrimeSurface_Reference_Standard_Abstract" IS NOT NULL OR
                            "PrimeSurface_Reference_Standard_Section" IS NOT NULL OR
                            "PrimeSurface_Reference_Standard_Year" IS NOT NULL OR
                            "PrimeSurface_Reference_Standard_Numeration_Prefix" IS NOT NULL OR
                            "PrimeSurface_Reference_Standard_Numeration_MainNumber" IS NOT NULL OR
                            "PrimeSurface_Reference_Standard_Numeration_Suffix" IS NOT NULL OR
                            "PrimeSurface_Reference_Standard_Standardizers" IS NOT NULL OR
                            "PrimeSurface_Reference_Standard_Locator" IS NOT NULL
                        ),
                        "PrimeSurface_Reference_Publication_Exists" = (
                            "PrimeSurface_Reference_Publication_Title" IS NOT NULL OR
                            "PrimeSurface_Reference_Publication_Abstract" IS NOT NULL OR
                            "PrimeSurface_Reference_Publication_Section" IS NOT NULL OR
                            "PrimeSurface_Reference_Publication_Authors" IS NOT NULL OR
                            "PrimeSurface_Reference_Publication_Doi" IS NOT NULL OR
                            "PrimeSurface_Reference_Publication_ArXiv" IS NOT NULL OR
                            "PrimeSurface_Reference_Publication_Urn" IS NOT NULL OR
                            "PrimeSurface_Reference_Publication_WebAddress" IS NOT NULL
                        ),
                        "PrimeSurface_Reference_Exists" = (
                            "PrimeSurface_Reference_Standard_Title" IS NOT NULL OR
                            "PrimeSurface_Reference_Standard_Abstract" IS NOT NULL OR
                            "PrimeSurface_Reference_Standard_Section" IS NOT NULL OR
                            "PrimeSurface_Reference_Standard_Year" IS NOT NULL OR
                            "PrimeSurface_Reference_Standard_Numeration_Prefix" IS NOT NULL OR
                            "PrimeSurface_Reference_Standard_Numeration_MainNumber" IS NOT NULL OR
                            "PrimeSurface_Reference_Standard_Numeration_Suffix" IS NOT NULL OR
                            "PrimeSurface_Reference_Standard_Standardizers" IS NOT NULL OR
                            "PrimeSurface_Reference_Standard_Locator" IS NOT NULL OR
                            "PrimeSurface_Reference_Publication_Title" IS NOT NULL OR
                            "PrimeSurface_Reference_Publication_Abstract" IS NOT NULL OR
                            "PrimeSurface_Reference_Publication_Section" IS NOT NULL OR
                            "PrimeSurface_Reference_Publication_Authors" IS NOT NULL OR
                            "PrimeSurface_Reference_Publication_Doi" IS NOT NULL OR
                            "PrimeSurface_Reference_Publication_ArXiv" IS NOT NULL OR
                            "PrimeSurface_Reference_Publication_Urn" IS NOT NULL OR
                            "PrimeSurface_Reference_Publication_WebAddress" IS NOT NULL
                        ),
                        "PrimeSurface_Exists" = (
                            "PrimeSurface_Description" IS NOT NULL OR
                            "PrimeSurface_Reference_Standard_Title" IS NOT NULL OR
                            "PrimeSurface_Reference_Standard_Abstract" IS NOT NULL OR
                            "PrimeSurface_Reference_Standard_Section" IS NOT NULL OR
                            "PrimeSurface_Reference_Standard_Year" IS NOT NULL OR
                            "PrimeSurface_Reference_Standard_Numeration_Prefix" IS NOT NULL OR
                            "PrimeSurface_Reference_Standard_Numeration_MainNumber" IS NOT NULL OR
                            "PrimeSurface_Reference_Standard_Numeration_Suffix" IS NOT NULL OR
                            "PrimeSurface_Reference_Standard_Standardizers" IS NOT NULL OR
                            "PrimeSurface_Reference_Standard_Locator" IS NOT NULL OR
                            "PrimeSurface_Reference_Publication_Title" IS NOT NULL OR
                            "PrimeSurface_Reference_Publication_Abstract" IS NOT NULL OR
                            "PrimeSurface_Reference_Publication_Section" IS NOT NULL OR
                            "PrimeSurface_Reference_Publication_Authors" IS NOT NULL OR
                            "PrimeSurface_Reference_Publication_Doi" IS NOT NULL OR
                            "PrimeSurface_Reference_Publication_ArXiv" IS NOT NULL OR
                            "PrimeSurface_Reference_Publication_Urn" IS NOT NULL OR
                            "PrimeSurface_Reference_Publication_WebAddress" IS NOT NULL
                        ),

                        "PrimeDirection_Reference_Standard_Exists" = (
                            "PrimeDirection_Reference_Standard_Title" IS NOT NULL OR
                            "PrimeDirection_Reference_Standard_Abstract" IS NOT NULL OR
                            "PrimeDirection_Reference_Standard_Section" IS NOT NULL OR
                            "PrimeDirection_Reference_Standard_Year" IS NOT NULL OR
                            "PrimeDirection_Reference_Standard_Numeration_Prefix" IS NOT NULL OR
                            "PrimeDirection_Reference_Standard_Numeration_MainNumber" IS NOT NULL OR
                            "PrimeDirection_Reference_Standard_Numeration_Suffix" IS NOT NULL OR
                            "PrimeDirection_Reference_Standard_Standardizers" IS NOT NULL OR
                            "PrimeDirection_Reference_Standard_Locator" IS NOT NULL
                        ),
                        "PrimeDirection_Reference_Publication_Exists" = (
                            "PrimeDirection_Reference_Publication_Title" IS NOT NULL OR
                            "PrimeDirection_Reference_Publication_Abstract" IS NOT NULL OR
                            "PrimeDirection_Reference_Publication_Section" IS NOT NULL OR
                            "PrimeDirection_Reference_Publication_Authors" IS NOT NULL OR
                            "PrimeDirection_Reference_Publication_Doi" IS NOT NULL OR
                            "PrimeDirection_Reference_Publication_ArXiv" IS NOT NULL OR
                            "PrimeDirection_Reference_Publication_Urn" IS NOT NULL OR
                            "PrimeDirection_Reference_Publication_WebAddress" IS NOT NULL
                        ),
                        "PrimeDirection_Reference_Exists" = (
                            "PrimeDirection_Reference_Standard_Title" IS NOT NULL OR
                            "PrimeDirection_Reference_Standard_Abstract" IS NOT NULL OR
                            "PrimeDirection_Reference_Standard_Section" IS NOT NULL OR
                            "PrimeDirection_Reference_Standard_Year" IS NOT NULL OR
                            "PrimeDirection_Reference_Standard_Numeration_Prefix" IS NOT NULL OR
                            "PrimeDirection_Reference_Standard_Numeration_MainNumber" IS NOT NULL OR
                            "PrimeDirection_Reference_Standard_Numeration_Suffix" IS NOT NULL OR
                            "PrimeDirection_Reference_Standard_Standardizers" IS NOT NULL OR
                            "PrimeDirection_Reference_Standard_Locator" IS NOT NULL OR
                            "PrimeDirection_Reference_Publication_Title" IS NOT NULL OR
                            "PrimeDirection_Reference_Publication_Abstract" IS NOT NULL OR
                            "PrimeDirection_Reference_Publication_Section" IS NOT NULL OR
                            "PrimeDirection_Reference_Publication_Authors" IS NOT NULL OR
                            "PrimeDirection_Reference_Publication_Doi" IS NOT NULL OR
                            "PrimeDirection_Reference_Publication_ArXiv" IS NOT NULL OR
                            "PrimeDirection_Reference_Publication_Urn" IS NOT NULL OR
                            "PrimeDirection_Reference_Publication_WebAddress" IS NOT NULL
                        ),
                        "PrimeDirection_Exists" = (
                            "PrimeDirection_Description" IS NOT NULL OR
                            "PrimeDirection_Reference_Standard_Title" IS NOT NULL OR
                            "PrimeDirection_Reference_Standard_Abstract" IS NOT NULL OR
                            "PrimeDirection_Reference_Standard_Section" IS NOT NULL OR
                            "PrimeDirection_Reference_Standard_Year" IS NOT NULL OR
                            "PrimeDirection_Reference_Standard_Numeration_Prefix" IS NOT NULL OR
                            "PrimeDirection_Reference_Standard_Numeration_MainNumber" IS NOT NULL OR
                            "PrimeDirection_Reference_Standard_Numeration_Suffix" IS NOT NULL OR
                            "PrimeDirection_Reference_Standard_Standardizers" IS NOT NULL OR
                            "PrimeDirection_Reference_Standard_Locator" IS NOT NULL OR
                            "PrimeDirection_Reference_Publication_Title" IS NOT NULL OR
                            "PrimeDirection_Reference_Publication_Abstract" IS NOT NULL OR
                            "PrimeDirection_Reference_Publication_Section" IS NOT NULL OR
                            "PrimeDirection_Reference_Publication_Authors" IS NOT NULL OR
                            "PrimeDirection_Reference_Publication_Doi" IS NOT NULL OR
                            "PrimeDirection_Reference_Publication_ArXiv" IS NOT NULL OR
                            "PrimeDirection_Reference_Publication_Urn" IS NOT NULL OR
                            "PrimeDirection_Reference_Publication_WebAddress" IS NOT NULL
                        ),

                        "SwitchableLayers_Reference_Standard_Exists" = (
                            "SwitchableLayers_Reference_Standard_Title" IS NOT NULL OR
                            "SwitchableLayers_Reference_Standard_Abstract" IS NOT NULL OR
                            "SwitchableLayers_Reference_Standard_Section" IS NOT NULL OR
                            "SwitchableLayers_Reference_Standard_Year" IS NOT NULL OR
                            "SwitchableLayers_Reference_Standard_Numeration_Prefix" IS NOT NULL OR
                            "SwitchableLayers_Reference_Standard_Numeration_MainNumber" IS NOT NULL OR
                            "SwitchableLayers_Reference_Standard_Numeration_Suffix" IS NOT NULL OR
                            "SwitchableLayers_Reference_Standard_Standardizers" IS NOT NULL OR
                            "SwitchableLayers_Reference_Standard_Locator" IS NOT NULL
                        ),
                        "SwitchableLayers_Reference_Publication_Exists" = (
                            "SwitchableLayers_Reference_Publication_Title" IS NOT NULL OR
                            "SwitchableLayers_Reference_Publication_Abstract" IS NOT NULL OR
                            "SwitchableLayers_Reference_Publication_Section" IS NOT NULL OR
                            "SwitchableLayers_Reference_Publication_Authors" IS NOT NULL OR
                            "SwitchableLayers_Reference_Publication_Doi" IS NOT NULL OR
                            "SwitchableLayers_Reference_Publication_ArXiv" IS NOT NULL OR
                            "SwitchableLayers_Reference_Publication_Urn" IS NOT NULL OR
                            "SwitchableLayers_Reference_Publication_WebAddress" IS NOT NULL
                        ),
                        "SwitchableLayers_Reference_Exists" = (
                            "SwitchableLayers_Reference_Standard_Title" IS NOT NULL OR
                            "SwitchableLayers_Reference_Standard_Abstract" IS NOT NULL OR
                            "SwitchableLayers_Reference_Standard_Section" IS NOT NULL OR
                            "SwitchableLayers_Reference_Standard_Year" IS NOT NULL OR
                            "SwitchableLayers_Reference_Standard_Numeration_Prefix" IS NOT NULL OR
                            "SwitchableLayers_Reference_Standard_Numeration_MainNumber" IS NOT NULL OR
                            "SwitchableLayers_Reference_Standard_Numeration_Suffix" IS NOT NULL OR
                            "SwitchableLayers_Reference_Standard_Standardizers" IS NOT NULL OR
                            "SwitchableLayers_Reference_Standard_Locator" IS NOT NULL OR
                            "SwitchableLayers_Reference_Publication_Title" IS NOT NULL OR
                            "SwitchableLayers_Reference_Publication_Abstract" IS NOT NULL OR
                            "SwitchableLayers_Reference_Publication_Section" IS NOT NULL OR
                            "SwitchableLayers_Reference_Publication_Authors" IS NOT NULL OR
                            "SwitchableLayers_Reference_Publication_Doi" IS NOT NULL OR
                            "SwitchableLayers_Reference_Publication_ArXiv" IS NOT NULL OR
                            "SwitchableLayers_Reference_Publication_Urn" IS NOT NULL OR
                            "SwitchableLayers_Reference_Publication_WebAddress" IS NOT NULL
                        ),
                        "SwitchableLayers_Exists" = (
                            "SwitchableLayers_Description" IS NOT NULL OR
                            "SwitchableLayers_Reference_Standard_Title" IS NOT NULL OR
                            "SwitchableLayers_Reference_Standard_Abstract" IS NOT NULL OR
                            "SwitchableLayers_Reference_Standard_Section" IS NOT NULL OR
                            "SwitchableLayers_Reference_Standard_Year" IS NOT NULL OR
                            "SwitchableLayers_Reference_Standard_Numeration_Prefix" IS NOT NULL OR
                            "SwitchableLayers_Reference_Standard_Numeration_MainNumber" IS NOT NULL OR
                            "SwitchableLayers_Reference_Standard_Numeration_Suffix" IS NOT NULL OR
                            "SwitchableLayers_Reference_Standard_Standardizers" IS NOT NULL OR
                            "SwitchableLayers_Reference_Standard_Locator" IS NOT NULL OR
                            "SwitchableLayers_Reference_Publication_Title" IS NOT NULL OR
                            "SwitchableLayers_Reference_Publication_Abstract" IS NOT NULL OR
                            "SwitchableLayers_Reference_Publication_Section" IS NOT NULL OR
                            "SwitchableLayers_Reference_Publication_Authors" IS NOT NULL OR
                            "SwitchableLayers_Reference_Publication_Doi" IS NOT NULL OR
                            "SwitchableLayers_Reference_Publication_ArXiv" IS NOT NULL OR
                            "SwitchableLayers_Reference_Publication_Urn" IS NOT NULL OR
                            "SwitchableLayers_Reference_Publication_WebAddress" IS NOT NULL
                        );

    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "MigrationId" = '20260318153447_CorrectExistsFlagsOfReferences') THEN
    INSERT INTO "__EFMigrationsHistory" ("MigrationId", "ProductVersion")
    VALUES ('20260318153447_CorrectExistsFlagsOfReferences', '10.0.5');
    END IF;
END $EF$;
COMMIT;

START TRANSACTION;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "MigrationId" = '20260328153447_CorrectExistsFlagsOfReferencesSecondAttempt') THEN

                    UPDATE metabase.method
                    SET 
                        "Reference_Standard_Exists" = CASE WHEN
                            "Reference_Standard_Title" IS NOT NULL OR
                            "Reference_Standard_Abstract" IS NOT NULL OR
                            "Reference_Standard_Section" IS NOT NULL OR
                            "Reference_Standard_Year" IS NOT NULL OR
                            "Reference_Standard_Numeration_Prefix" IS NOT NULL OR
                            "Reference_Standard_Numeration_MainNumber" IS NOT NULL OR
                            "Reference_Standard_Numeration_Suffix" IS NOT NULL OR
                            "Reference_Standard_Standardizers" IS NOT NULL OR
                            "Reference_Standard_Locator" IS NOT NULL
                            THEN TRUE
                          ELSE NULL
                        END,
                        "Reference_Publication_Exists" = CASE WHEN
                            "Reference_Publication_Title" IS NOT NULL OR
                            "Reference_Publication_Abstract" IS NOT NULL OR
                            "Reference_Publication_Section" IS NOT NULL OR
                            "Reference_Publication_Authors" IS NOT NULL OR
                            "Reference_Publication_Doi" IS NOT NULL OR
                            "Reference_Publication_ArXiv" IS NOT NULL OR
                            "Reference_Publication_Urn" IS NOT NULL OR
                            "Reference_Publication_WebAddress" IS NOT NULL
                            THEN TRUE
                          ELSE NULL
                        END,
                        "Reference_Exists" = CASE WHEN
                            "Reference_Standard_Title" IS NOT NULL OR
                            "Reference_Standard_Abstract" IS NOT NULL OR
                            "Reference_Standard_Section" IS NOT NULL OR
                            "Reference_Standard_Year" IS NOT NULL OR
                            "Reference_Standard_Numeration_Prefix" IS NOT NULL OR
                            "Reference_Standard_Numeration_MainNumber" IS NOT NULL OR
                            "Reference_Standard_Numeration_Suffix" IS NOT NULL OR
                            "Reference_Standard_Standardizers" IS NOT NULL OR
                            "Reference_Standard_Locator" IS NOT NULL OR
                            "Reference_Publication_Title" IS NOT NULL OR
                            "Reference_Publication_Abstract" IS NOT NULL OR
                            "Reference_Publication_Section" IS NOT NULL OR
                            "Reference_Publication_Authors" IS NOT NULL OR
                            "Reference_Publication_Doi" IS NOT NULL OR
                            "Reference_Publication_ArXiv" IS NOT NULL OR
                            "Reference_Publication_Urn" IS NOT NULL OR
                            "Reference_Publication_WebAddress" IS NOT NULL
                            THEN TRUE
                          ELSE NULL
                        END;

                    UPDATE metabase.data_format
                    SET
                        "Reference_Standard_Exists" = CASE WHEN
                            "Reference_Standard_Title" IS NOT NULL OR
                            "Reference_Standard_Abstract" IS NOT NULL OR
                            "Reference_Standard_Section" IS NOT NULL OR
                            "Reference_Standard_Year" IS NOT NULL OR
                            "Reference_Standard_Numeration_Prefix" IS NOT NULL OR
                            "Reference_Standard_Numeration_MainNumber" IS NOT NULL OR
                            "Reference_Standard_Numeration_Suffix" IS NOT NULL OR
                            "Reference_Standard_Standardizers" IS NOT NULL OR
                            "Reference_Standard_Locator" IS NOT NULL
                            THEN TRUE
                          ELSE NULL
                        END,
                        "Reference_Publication_Exists" = CASE WHEN
                            "Reference_Publication_Title" IS NOT NULL OR
                            "Reference_Publication_Abstract" IS NOT NULL OR
                            "Reference_Publication_Section" IS NOT NULL OR
                            "Reference_Publication_Authors" IS NOT NULL OR
                            "Reference_Publication_Doi" IS NOT NULL OR
                            "Reference_Publication_ArXiv" IS NOT NULL OR
                            "Reference_Publication_Urn" IS NOT NULL OR
                            "Reference_Publication_WebAddress" IS NOT NULL
                            THEN TRUE
                          ELSE NULL
                        END,
                        "Reference_Exists" = CASE WHEN
                            "Reference_Standard_Title" IS NOT NULL OR
                            "Reference_Standard_Abstract" IS NOT NULL OR
                            "Reference_Standard_Section" IS NOT NULL OR
                            "Reference_Standard_Year" IS NOT NULL OR
                            "Reference_Standard_Numeration_Prefix" IS NOT NULL OR
                            "Reference_Standard_Numeration_MainNumber" IS NOT NULL OR
                            "Reference_Standard_Numeration_Suffix" IS NOT NULL OR
                            "Reference_Standard_Standardizers" IS NOT NULL OR
                            "Reference_Standard_Locator" IS NOT NULL OR
                            "Reference_Publication_Title" IS NOT NULL OR
                            "Reference_Publication_Abstract" IS NOT NULL OR
                            "Reference_Publication_Section" IS NOT NULL OR
                            "Reference_Publication_Authors" IS NOT NULL OR
                            "Reference_Publication_Doi" IS NOT NULL OR
                            "Reference_Publication_ArXiv" IS NOT NULL OR
                            "Reference_Publication_Urn" IS NOT NULL OR
                            "Reference_Publication_WebAddress" IS NOT NULL
                            THEN TRUE
                          ELSE NULL
                        END;

                    UPDATE metabase.component
                    SET
                        "PrimeSurface_Reference_Standard_Exists" = CASE WHEN
                            "PrimeSurface_Reference_Standard_Title" IS NOT NULL OR
                            "PrimeSurface_Reference_Standard_Abstract" IS NOT NULL OR
                            "PrimeSurface_Reference_Standard_Section" IS NOT NULL OR
                            "PrimeSurface_Reference_Standard_Year" IS NOT NULL OR
                            "PrimeSurface_Reference_Standard_Numeration_Prefix" IS NOT NULL OR
                            "PrimeSurface_Reference_Standard_Numeration_MainNumber" IS NOT NULL OR
                            "PrimeSurface_Reference_Standard_Numeration_Suffix" IS NOT NULL OR
                            "PrimeSurface_Reference_Standard_Standardizers" IS NOT NULL OR
                            "PrimeSurface_Reference_Standard_Locator" IS NOT NULL
                            THEN TRUE
                          ELSE NULL
                        END,
                        "PrimeSurface_Reference_Publication_Exists" = CASE WHEN
                            "PrimeSurface_Reference_Publication_Title" IS NOT NULL OR
                            "PrimeSurface_Reference_Publication_Abstract" IS NOT NULL OR
                            "PrimeSurface_Reference_Publication_Section" IS NOT NULL OR
                            "PrimeSurface_Reference_Publication_Authors" IS NOT NULL OR
                            "PrimeSurface_Reference_Publication_Doi" IS NOT NULL OR
                            "PrimeSurface_Reference_Publication_ArXiv" IS NOT NULL OR
                            "PrimeSurface_Reference_Publication_Urn" IS NOT NULL OR
                            "PrimeSurface_Reference_Publication_WebAddress" IS NOT NULL
                            THEN TRUE
                          ELSE NULL
                        END,
                        "PrimeSurface_Reference_Exists" = CASE WHEN
                            "PrimeSurface_Reference_Standard_Title" IS NOT NULL OR
                            "PrimeSurface_Reference_Standard_Abstract" IS NOT NULL OR
                            "PrimeSurface_Reference_Standard_Section" IS NOT NULL OR
                            "PrimeSurface_Reference_Standard_Year" IS NOT NULL OR
                            "PrimeSurface_Reference_Standard_Numeration_Prefix" IS NOT NULL OR
                            "PrimeSurface_Reference_Standard_Numeration_MainNumber" IS NOT NULL OR
                            "PrimeSurface_Reference_Standard_Numeration_Suffix" IS NOT NULL OR
                            "PrimeSurface_Reference_Standard_Standardizers" IS NOT NULL OR
                            "PrimeSurface_Reference_Standard_Locator" IS NOT NULL OR
                            "PrimeSurface_Reference_Publication_Title" IS NOT NULL OR
                            "PrimeSurface_Reference_Publication_Abstract" IS NOT NULL OR
                            "PrimeSurface_Reference_Publication_Section" IS NOT NULL OR
                            "PrimeSurface_Reference_Publication_Authors" IS NOT NULL OR
                            "PrimeSurface_Reference_Publication_Doi" IS NOT NULL OR
                            "PrimeSurface_Reference_Publication_ArXiv" IS NOT NULL OR
                            "PrimeSurface_Reference_Publication_Urn" IS NOT NULL OR
                            "PrimeSurface_Reference_Publication_WebAddress" IS NOT NULL
                            THEN TRUE
                          ELSE NULL
                        END,
                        "PrimeSurface_Exists" = CASE WHEN
                            "PrimeSurface_Description" IS NOT NULL OR
                            "PrimeSurface_Reference_Standard_Title" IS NOT NULL OR
                            "PrimeSurface_Reference_Standard_Abstract" IS NOT NULL OR
                            "PrimeSurface_Reference_Standard_Section" IS NOT NULL OR
                            "PrimeSurface_Reference_Standard_Year" IS NOT NULL OR
                            "PrimeSurface_Reference_Standard_Numeration_Prefix" IS NOT NULL OR
                            "PrimeSurface_Reference_Standard_Numeration_MainNumber" IS NOT NULL OR
                            "PrimeSurface_Reference_Standard_Numeration_Suffix" IS NOT NULL OR
                            "PrimeSurface_Reference_Standard_Standardizers" IS NOT NULL OR
                            "PrimeSurface_Reference_Standard_Locator" IS NOT NULL OR
                            "PrimeSurface_Reference_Publication_Title" IS NOT NULL OR
                            "PrimeSurface_Reference_Publication_Abstract" IS NOT NULL OR
                            "PrimeSurface_Reference_Publication_Section" IS NOT NULL OR
                            "PrimeSurface_Reference_Publication_Authors" IS NOT NULL OR
                            "PrimeSurface_Reference_Publication_Doi" IS NOT NULL OR
                            "PrimeSurface_Reference_Publication_ArXiv" IS NOT NULL OR
                            "PrimeSurface_Reference_Publication_Urn" IS NOT NULL OR
                            "PrimeSurface_Reference_Publication_WebAddress" IS NOT NULL
                            THEN TRUE
                          ELSE NULL
                        END,

                        "PrimeDirection_Reference_Standard_Exists" = CASE WHEN
                            "PrimeDirection_Reference_Standard_Title" IS NOT NULL OR
                            "PrimeDirection_Reference_Standard_Abstract" IS NOT NULL OR
                            "PrimeDirection_Reference_Standard_Section" IS NOT NULL OR
                            "PrimeDirection_Reference_Standard_Year" IS NOT NULL OR
                            "PrimeDirection_Reference_Standard_Numeration_Prefix" IS NOT NULL OR
                            "PrimeDirection_Reference_Standard_Numeration_MainNumber" IS NOT NULL OR
                            "PrimeDirection_Reference_Standard_Numeration_Suffix" IS NOT NULL OR
                            "PrimeDirection_Reference_Standard_Standardizers" IS NOT NULL OR
                            "PrimeDirection_Reference_Standard_Locator" IS NOT NULL
                            THEN TRUE
                          ELSE NULL
                        END,
                        "PrimeDirection_Reference_Publication_Exists" = CASE WHEN
                            "PrimeDirection_Reference_Publication_Title" IS NOT NULL OR
                            "PrimeDirection_Reference_Publication_Abstract" IS NOT NULL OR
                            "PrimeDirection_Reference_Publication_Section" IS NOT NULL OR
                            "PrimeDirection_Reference_Publication_Authors" IS NOT NULL OR
                            "PrimeDirection_Reference_Publication_Doi" IS NOT NULL OR
                            "PrimeDirection_Reference_Publication_ArXiv" IS NOT NULL OR
                            "PrimeDirection_Reference_Publication_Urn" IS NOT NULL OR
                            "PrimeDirection_Reference_Publication_WebAddress" IS NOT NULL
                            THEN TRUE
                          ELSE NULL
                        END,
                        "PrimeDirection_Reference_Exists" = CASE WHEN
                            "PrimeDirection_Reference_Standard_Title" IS NOT NULL OR
                            "PrimeDirection_Reference_Standard_Abstract" IS NOT NULL OR
                            "PrimeDirection_Reference_Standard_Section" IS NOT NULL OR
                            "PrimeDirection_Reference_Standard_Year" IS NOT NULL OR
                            "PrimeDirection_Reference_Standard_Numeration_Prefix" IS NOT NULL OR
                            "PrimeDirection_Reference_Standard_Numeration_MainNumber" IS NOT NULL OR
                            "PrimeDirection_Reference_Standard_Numeration_Suffix" IS NOT NULL OR
                            "PrimeDirection_Reference_Standard_Standardizers" IS NOT NULL OR
                            "PrimeDirection_Reference_Standard_Locator" IS NOT NULL OR
                            "PrimeDirection_Reference_Publication_Title" IS NOT NULL OR
                            "PrimeDirection_Reference_Publication_Abstract" IS NOT NULL OR
                            "PrimeDirection_Reference_Publication_Section" IS NOT NULL OR
                            "PrimeDirection_Reference_Publication_Authors" IS NOT NULL OR
                            "PrimeDirection_Reference_Publication_Doi" IS NOT NULL OR
                            "PrimeDirection_Reference_Publication_ArXiv" IS NOT NULL OR
                            "PrimeDirection_Reference_Publication_Urn" IS NOT NULL OR
                            "PrimeDirection_Reference_Publication_WebAddress" IS NOT NULL
                            THEN TRUE
                          ELSE NULL
                        END,
                        "PrimeDirection_Exists" = CASE WHEN
                            "PrimeDirection_Description" IS NOT NULL OR
                            "PrimeDirection_Reference_Standard_Title" IS NOT NULL OR
                            "PrimeDirection_Reference_Standard_Abstract" IS NOT NULL OR
                            "PrimeDirection_Reference_Standard_Section" IS NOT NULL OR
                            "PrimeDirection_Reference_Standard_Year" IS NOT NULL OR
                            "PrimeDirection_Reference_Standard_Numeration_Prefix" IS NOT NULL OR
                            "PrimeDirection_Reference_Standard_Numeration_MainNumber" IS NOT NULL OR
                            "PrimeDirection_Reference_Standard_Numeration_Suffix" IS NOT NULL OR
                            "PrimeDirection_Reference_Standard_Standardizers" IS NOT NULL OR
                            "PrimeDirection_Reference_Standard_Locator" IS NOT NULL OR
                            "PrimeDirection_Reference_Publication_Title" IS NOT NULL OR
                            "PrimeDirection_Reference_Publication_Abstract" IS NOT NULL OR
                            "PrimeDirection_Reference_Publication_Section" IS NOT NULL OR
                            "PrimeDirection_Reference_Publication_Authors" IS NOT NULL OR
                            "PrimeDirection_Reference_Publication_Doi" IS NOT NULL OR
                            "PrimeDirection_Reference_Publication_ArXiv" IS NOT NULL OR
                            "PrimeDirection_Reference_Publication_Urn" IS NOT NULL OR
                            "PrimeDirection_Reference_Publication_WebAddress" IS NOT NULL
                            THEN TRUE
                          ELSE NULL
                        END,

                        "SwitchableLayers_Reference_Standard_Exists" = CASE WHEN
                            "SwitchableLayers_Reference_Standard_Title" IS NOT NULL OR
                            "SwitchableLayers_Reference_Standard_Abstract" IS NOT NULL OR
                            "SwitchableLayers_Reference_Standard_Section" IS NOT NULL OR
                            "SwitchableLayers_Reference_Standard_Year" IS NOT NULL OR
                            "SwitchableLayers_Reference_Standard_Numeration_Prefix" IS NOT NULL OR
                            "SwitchableLayers_Reference_Standard_Numeration_MainNumber" IS NOT NULL OR
                            "SwitchableLayers_Reference_Standard_Numeration_Suffix" IS NOT NULL OR
                            "SwitchableLayers_Reference_Standard_Standardizers" IS NOT NULL OR
                            "SwitchableLayers_Reference_Standard_Locator" IS NOT NULL
                            THEN TRUE
                          ELSE NULL
                        END,
                        "SwitchableLayers_Reference_Publication_Exists" = CASE WHEN
                            "SwitchableLayers_Reference_Publication_Title" IS NOT NULL OR
                            "SwitchableLayers_Reference_Publication_Abstract" IS NOT NULL OR
                            "SwitchableLayers_Reference_Publication_Section" IS NOT NULL OR
                            "SwitchableLayers_Reference_Publication_Authors" IS NOT NULL OR
                            "SwitchableLayers_Reference_Publication_Doi" IS NOT NULL OR
                            "SwitchableLayers_Reference_Publication_ArXiv" IS NOT NULL OR
                            "SwitchableLayers_Reference_Publication_Urn" IS NOT NULL OR
                            "SwitchableLayers_Reference_Publication_WebAddress" IS NOT NULL
                            THEN TRUE
                          ELSE NULL
                        END,
                        "SwitchableLayers_Reference_Exists" = CASE WHEN
                            "SwitchableLayers_Reference_Standard_Title" IS NOT NULL OR
                            "SwitchableLayers_Reference_Standard_Abstract" IS NOT NULL OR
                            "SwitchableLayers_Reference_Standard_Section" IS NOT NULL OR
                            "SwitchableLayers_Reference_Standard_Year" IS NOT NULL OR
                            "SwitchableLayers_Reference_Standard_Numeration_Prefix" IS NOT NULL OR
                            "SwitchableLayers_Reference_Standard_Numeration_MainNumber" IS NOT NULL OR
                            "SwitchableLayers_Reference_Standard_Numeration_Suffix" IS NOT NULL OR
                            "SwitchableLayers_Reference_Standard_Standardizers" IS NOT NULL OR
                            "SwitchableLayers_Reference_Standard_Locator" IS NOT NULL OR
                            "SwitchableLayers_Reference_Publication_Title" IS NOT NULL OR
                            "SwitchableLayers_Reference_Publication_Abstract" IS NOT NULL OR
                            "SwitchableLayers_Reference_Publication_Section" IS NOT NULL OR
                            "SwitchableLayers_Reference_Publication_Authors" IS NOT NULL OR
                            "SwitchableLayers_Reference_Publication_Doi" IS NOT NULL OR
                            "SwitchableLayers_Reference_Publication_ArXiv" IS NOT NULL OR
                            "SwitchableLayers_Reference_Publication_Urn" IS NOT NULL OR
                            "SwitchableLayers_Reference_Publication_WebAddress" IS NOT NULL
                            THEN TRUE
                          ELSE NULL
                        END,
                        "SwitchableLayers_Exists" = CASE WHEN
                            "SwitchableLayers_Description" IS NOT NULL OR
                            "SwitchableLayers_Reference_Standard_Title" IS NOT NULL OR
                            "SwitchableLayers_Reference_Standard_Abstract" IS NOT NULL OR
                            "SwitchableLayers_Reference_Standard_Section" IS NOT NULL OR
                            "SwitchableLayers_Reference_Standard_Year" IS NOT NULL OR
                            "SwitchableLayers_Reference_Standard_Numeration_Prefix" IS NOT NULL OR
                            "SwitchableLayers_Reference_Standard_Numeration_MainNumber" IS NOT NULL OR
                            "SwitchableLayers_Reference_Standard_Numeration_Suffix" IS NOT NULL OR
                            "SwitchableLayers_Reference_Standard_Standardizers" IS NOT NULL OR
                            "SwitchableLayers_Reference_Standard_Locator" IS NOT NULL OR
                            "SwitchableLayers_Reference_Publication_Title" IS NOT NULL OR
                            "SwitchableLayers_Reference_Publication_Abstract" IS NOT NULL OR
                            "SwitchableLayers_Reference_Publication_Section" IS NOT NULL OR
                            "SwitchableLayers_Reference_Publication_Authors" IS NOT NULL OR
                            "SwitchableLayers_Reference_Publication_Doi" IS NOT NULL OR
                            "SwitchableLayers_Reference_Publication_ArXiv" IS NOT NULL OR
                            "SwitchableLayers_Reference_Publication_Urn" IS NOT NULL OR
                            "SwitchableLayers_Reference_Publication_WebAddress" IS NOT NULL
                            THEN TRUE
                          ELSE NULL
                        END;

    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "MigrationId" = '20260328153447_CorrectExistsFlagsOfReferencesSecondAttempt') THEN
    INSERT INTO "__EFMigrationsHistory" ("MigrationId", "ProductVersion")
    VALUES ('20260328153447_CorrectExistsFlagsOfReferencesSecondAttempt', '10.0.5');
    END IF;
END $EF$;
COMMIT;

START TRANSACTION;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "MigrationId" = '20260402204629_AddUpdatedAndCreatedAtTimestamps') THEN
    ALTER TABLE metabase.user_method_developer ADD "CreatedAt" timestamp with time zone NOT NULL DEFAULT (now());
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "MigrationId" = '20260402204629_AddUpdatedAndCreatedAtTimestamps') THEN
    ALTER TABLE metabase.user_method_developer ADD "UpdatedAt" timestamp with time zone NOT NULL DEFAULT (now());
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "MigrationId" = '20260402204629_AddUpdatedAndCreatedAtTimestamps') THEN
    ALTER TABLE metabase."user" ALTER COLUMN "Id" SET DEFAULT (gen_random_uuid());
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "MigrationId" = '20260402204629_AddUpdatedAndCreatedAtTimestamps') THEN
    ALTER TABLE metabase."user" ADD "CreatedAt" timestamp with time zone NOT NULL DEFAULT (now());
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "MigrationId" = '20260402204629_AddUpdatedAndCreatedAtTimestamps') THEN
    ALTER TABLE metabase."user" ADD "UpdatedAt" timestamp with time zone NOT NULL DEFAULT (now());
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "MigrationId" = '20260402204629_AddUpdatedAndCreatedAtTimestamps') THEN
    ALTER TABLE metabase."OpenIddictTokens" ALTER COLUMN "Id" SET DEFAULT (gen_random_uuid());
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "MigrationId" = '20260402204629_AddUpdatedAndCreatedAtTimestamps') THEN
    ALTER TABLE metabase."OpenIddictTokens" ADD "CreatedAt" timestamp with time zone NOT NULL DEFAULT (now());
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "MigrationId" = '20260402204629_AddUpdatedAndCreatedAtTimestamps') THEN
    ALTER TABLE metabase."OpenIddictTokens" ADD "UpdatedAt" timestamp with time zone NOT NULL DEFAULT (now());
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "MigrationId" = '20260402204629_AddUpdatedAndCreatedAtTimestamps') THEN
    ALTER TABLE metabase."OpenIddictScopes" ALTER COLUMN "Id" SET DEFAULT (gen_random_uuid());
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "MigrationId" = '20260402204629_AddUpdatedAndCreatedAtTimestamps') THEN
    ALTER TABLE metabase."OpenIddictScopes" ADD "CreatedAt" timestamp with time zone NOT NULL DEFAULT TIMESTAMPTZ '1970-01-01T00:00:00Z';
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "MigrationId" = '20260402204629_AddUpdatedAndCreatedAtTimestamps') THEN
    ALTER TABLE metabase."OpenIddictScopes" ADD "UpdatedAt" timestamp with time zone NOT NULL DEFAULT TIMESTAMPTZ '1970-01-01T00:00:00Z';
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "MigrationId" = '20260402204629_AddUpdatedAndCreatedAtTimestamps') THEN
    ALTER TABLE metabase."OpenIddictAuthorizations" ALTER COLUMN "Id" SET DEFAULT (gen_random_uuid());
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "MigrationId" = '20260402204629_AddUpdatedAndCreatedAtTimestamps') THEN
    ALTER TABLE metabase."OpenIddictAuthorizations" ADD "CreatedAt" timestamp with time zone NOT NULL DEFAULT (now());
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "MigrationId" = '20260402204629_AddUpdatedAndCreatedAtTimestamps') THEN
    ALTER TABLE metabase."OpenIddictAuthorizations" ADD "UpdatedAt" timestamp with time zone NOT NULL DEFAULT (now());
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "MigrationId" = '20260402204629_AddUpdatedAndCreatedAtTimestamps') THEN
    ALTER TABLE metabase."OpenIddictApplications" ALTER COLUMN "Id" SET DEFAULT (gen_random_uuid());
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "MigrationId" = '20260402204629_AddUpdatedAndCreatedAtTimestamps') THEN
    ALTER TABLE metabase."OpenIddictApplications" ADD "CreatedAt" timestamp with time zone NOT NULL DEFAULT (now());
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "MigrationId" = '20260402204629_AddUpdatedAndCreatedAtTimestamps') THEN
    ALTER TABLE metabase."OpenIddictApplications" ADD "UpdatedAt" timestamp with time zone NOT NULL DEFAULT (now());
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "MigrationId" = '20260402204629_AddUpdatedAndCreatedAtTimestamps') THEN
    ALTER TABLE metabase.method ADD "CreatedAt" timestamp with time zone NOT NULL DEFAULT (now());
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "MigrationId" = '20260402204629_AddUpdatedAndCreatedAtTimestamps') THEN
    ALTER TABLE metabase.method ADD "UpdatedAt" timestamp with time zone NOT NULL DEFAULT (now());
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "MigrationId" = '20260402204629_AddUpdatedAndCreatedAtTimestamps') THEN
    ALTER TABLE metabase.institution_representative ADD "CreatedAt" timestamp with time zone NOT NULL DEFAULT (now());
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "MigrationId" = '20260402204629_AddUpdatedAndCreatedAtTimestamps') THEN
    ALTER TABLE metabase.institution_representative ADD "UpdatedAt" timestamp with time zone NOT NULL DEFAULT (now());
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "MigrationId" = '20260402204629_AddUpdatedAndCreatedAtTimestamps') THEN
    ALTER TABLE metabase.institution_method_developer ADD "CreatedAt" timestamp with time zone NOT NULL DEFAULT (now());
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "MigrationId" = '20260402204629_AddUpdatedAndCreatedAtTimestamps') THEN
    ALTER TABLE metabase.institution_method_developer ADD "UpdatedAt" timestamp with time zone NOT NULL DEFAULT (now());
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "MigrationId" = '20260402204629_AddUpdatedAndCreatedAtTimestamps') THEN
    ALTER TABLE metabase.institution ADD "CreatedAt" timestamp with time zone NOT NULL DEFAULT (now());
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "MigrationId" = '20260402204629_AddUpdatedAndCreatedAtTimestamps') THEN
    ALTER TABLE metabase.institution ADD "UpdatedAt" timestamp with time zone NOT NULL DEFAULT (now());
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "MigrationId" = '20260402204629_AddUpdatedAndCreatedAtTimestamps') THEN
    ALTER TABLE metabase.gnu_pg_fingerprint ALTER COLUMN "CreatedAt" SET DEFAULT (now());
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "MigrationId" = '20260402204629_AddUpdatedAndCreatedAtTimestamps') THEN
    ALTER TABLE metabase.gnu_pg_fingerprint ADD "UpdatedAt" timestamp with time zone NOT NULL DEFAULT (now());
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "MigrationId" = '20260402204629_AddUpdatedAndCreatedAtTimestamps') THEN
    ALTER TABLE metabase.database ADD "CreatedAt" timestamp with time zone NOT NULL DEFAULT (now());
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "MigrationId" = '20260402204629_AddUpdatedAndCreatedAtTimestamps') THEN
    ALTER TABLE metabase.database ADD "UpdatedAt" timestamp with time zone NOT NULL DEFAULT (now());
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "MigrationId" = '20260402204629_AddUpdatedAndCreatedAtTimestamps') THEN
    ALTER TABLE metabase.data_format ADD "CreatedAt" timestamp with time zone NOT NULL DEFAULT (now());
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "MigrationId" = '20260402204629_AddUpdatedAndCreatedAtTimestamps') THEN
    ALTER TABLE metabase.data_format ADD "UpdatedAt" timestamp with time zone NOT NULL DEFAULT (now());
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "MigrationId" = '20260402204629_AddUpdatedAndCreatedAtTimestamps') THEN
    ALTER TABLE metabase.component_variant ADD "CreatedAt" timestamp with time zone NOT NULL DEFAULT (now());
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "MigrationId" = '20260402204629_AddUpdatedAndCreatedAtTimestamps') THEN
    ALTER TABLE metabase.component_variant ADD "UpdatedAt" timestamp with time zone NOT NULL DEFAULT (now());
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "MigrationId" = '20260402204629_AddUpdatedAndCreatedAtTimestamps') THEN
    ALTER TABLE metabase.component_manufacturer ADD "CreatedAt" timestamp with time zone NOT NULL DEFAULT (now());
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "MigrationId" = '20260402204629_AddUpdatedAndCreatedAtTimestamps') THEN
    ALTER TABLE metabase.component_manufacturer ADD "UpdatedAt" timestamp with time zone NOT NULL DEFAULT (now());
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "MigrationId" = '20260402204629_AddUpdatedAndCreatedAtTimestamps') THEN
    ALTER TABLE metabase.component_concretization_and_generalization ADD "CreatedAt" timestamp with time zone NOT NULL DEFAULT (now());
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "MigrationId" = '20260402204629_AddUpdatedAndCreatedAtTimestamps') THEN
    ALTER TABLE metabase.component_concretization_and_generalization ADD "UpdatedAt" timestamp with time zone NOT NULL DEFAULT (now());
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "MigrationId" = '20260402204629_AddUpdatedAndCreatedAtTimestamps') THEN
    ALTER TABLE metabase.component_assembly ADD "CreatedAt" timestamp with time zone NOT NULL DEFAULT (now());
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "MigrationId" = '20260402204629_AddUpdatedAndCreatedAtTimestamps') THEN
    ALTER TABLE metabase.component_assembly ADD "UpdatedAt" timestamp with time zone NOT NULL DEFAULT (now());
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "MigrationId" = '20260402204629_AddUpdatedAndCreatedAtTimestamps') THEN
    ALTER TABLE metabase.component ADD "CreatedAt" timestamp with time zone NOT NULL DEFAULT (now());
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "MigrationId" = '20260402204629_AddUpdatedAndCreatedAtTimestamps') THEN
    ALTER TABLE metabase.component ADD "UpdatedAt" timestamp with time zone NOT NULL DEFAULT (now());
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "MigrationId" = '20260402204629_AddUpdatedAndCreatedAtTimestamps') THEN
    INSERT INTO "__EFMigrationsHistory" ("MigrationId", "ProductVersion")
    VALUES ('20260402204629_AddUpdatedAndCreatedAtTimestamps', '10.0.5');
    END IF;
END $EF$;
COMMIT;

START TRANSACTION;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "MigrationId" = '20260508172623_AddIndicesForNameAndCreatedAtAndAtDefaultValuesForAuditableEntitiesAndAssociations') THEN
    ALTER TABLE metabase."OpenIddictScopes" ALTER COLUMN "UpdatedAt" SET DEFAULT (now());
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "MigrationId" = '20260508172623_AddIndicesForNameAndCreatedAtAndAtDefaultValuesForAuditableEntitiesAndAssociations') THEN
    ALTER TABLE metabase."OpenIddictScopes" ALTER COLUMN "CreatedAt" SET DEFAULT (now());
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "MigrationId" = '20260508172623_AddIndicesForNameAndCreatedAtAndAtDefaultValuesForAuditableEntitiesAndAssociations') THEN
    CREATE UNIQUE INDEX "IX_user_CreatedAt_Id" ON metabase."user" ("CreatedAt", "Id");
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "MigrationId" = '20260508172623_AddIndicesForNameAndCreatedAtAndAtDefaultValuesForAuditableEntitiesAndAssociations') THEN
    CREATE UNIQUE INDEX "IX_user_Name_Id" ON metabase."user" ("Name", "Id");
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "MigrationId" = '20260508172623_AddIndicesForNameAndCreatedAtAndAtDefaultValuesForAuditableEntitiesAndAssociations') THEN
    CREATE UNIQUE INDEX "IX_OpenIddictTokens_CreatedAt_Id" ON metabase."OpenIddictTokens" ("CreatedAt", "Id");
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "MigrationId" = '20260508172623_AddIndicesForNameAndCreatedAtAndAtDefaultValuesForAuditableEntitiesAndAssociations') THEN
    CREATE UNIQUE INDEX "IX_OpenIddictScopes_CreatedAt_Id" ON metabase."OpenIddictScopes" ("CreatedAt", "Id");
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "MigrationId" = '20260508172623_AddIndicesForNameAndCreatedAtAndAtDefaultValuesForAuditableEntitiesAndAssociations') THEN
    CREATE UNIQUE INDEX "IX_OpenIddictAuthorizations_CreatedAt_Id" ON metabase."OpenIddictAuthorizations" ("CreatedAt", "Id");
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "MigrationId" = '20260508172623_AddIndicesForNameAndCreatedAtAndAtDefaultValuesForAuditableEntitiesAndAssociations') THEN
    CREATE UNIQUE INDEX "IX_OpenIddictApplications_CreatedAt_Id" ON metabase."OpenIddictApplications" ("CreatedAt", "Id");
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "MigrationId" = '20260508172623_AddIndicesForNameAndCreatedAtAndAtDefaultValuesForAuditableEntitiesAndAssociations') THEN
    CREATE UNIQUE INDEX "IX_method_CreatedAt_Id" ON metabase.method ("CreatedAt", "Id");
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "MigrationId" = '20260508172623_AddIndicesForNameAndCreatedAtAndAtDefaultValuesForAuditableEntitiesAndAssociations') THEN
    CREATE UNIQUE INDEX "IX_method_Name_Id" ON metabase.method ("Name", "Id");
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "MigrationId" = '20260508172623_AddIndicesForNameAndCreatedAtAndAtDefaultValuesForAuditableEntitiesAndAssociations') THEN
    CREATE UNIQUE INDEX "IX_institution_CreatedAt_Id" ON metabase.institution ("CreatedAt", "Id");
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "MigrationId" = '20260508172623_AddIndicesForNameAndCreatedAtAndAtDefaultValuesForAuditableEntitiesAndAssociations') THEN
    CREATE UNIQUE INDEX "IX_institution_Name_Id" ON metabase.institution ("Name", "Id");
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "MigrationId" = '20260508172623_AddIndicesForNameAndCreatedAtAndAtDefaultValuesForAuditableEntitiesAndAssociations') THEN
    CREATE UNIQUE INDEX "IX_gnu_pg_fingerprint_CreatedAt_Id" ON metabase.gnu_pg_fingerprint ("CreatedAt", "Id");
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "MigrationId" = '20260508172623_AddIndicesForNameAndCreatedAtAndAtDefaultValuesForAuditableEntitiesAndAssociations') THEN
    CREATE UNIQUE INDEX "IX_database_CreatedAt_Id" ON metabase.database ("CreatedAt", "Id");
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "MigrationId" = '20260508172623_AddIndicesForNameAndCreatedAtAndAtDefaultValuesForAuditableEntitiesAndAssociations') THEN
    CREATE UNIQUE INDEX "IX_database_Name_Id" ON metabase.database ("Name", "Id");
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "MigrationId" = '20260508172623_AddIndicesForNameAndCreatedAtAndAtDefaultValuesForAuditableEntitiesAndAssociations') THEN
    CREATE UNIQUE INDEX "IX_data_format_CreatedAt_Id" ON metabase.data_format ("CreatedAt", "Id");
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "MigrationId" = '20260508172623_AddIndicesForNameAndCreatedAtAndAtDefaultValuesForAuditableEntitiesAndAssociations') THEN
    CREATE UNIQUE INDEX "IX_data_format_Name_Id" ON metabase.data_format ("Name", "Id");
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "MigrationId" = '20260508172623_AddIndicesForNameAndCreatedAtAndAtDefaultValuesForAuditableEntitiesAndAssociations') THEN
    CREATE UNIQUE INDEX "IX_component_CreatedAt_Id" ON metabase.component ("CreatedAt", "Id");
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "MigrationId" = '20260508172623_AddIndicesForNameAndCreatedAtAndAtDefaultValuesForAuditableEntitiesAndAssociations') THEN
    CREATE UNIQUE INDEX "IX_component_Name_Id" ON metabase.component ("Name", "Id");
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "MigrationId" = '20260508172623_AddIndicesForNameAndCreatedAtAndAtDefaultValuesForAuditableEntitiesAndAssociations') THEN
    INSERT INTO "__EFMigrationsHistory" ("MigrationId", "ProductVersion")
    VALUES ('20260508172623_AddIndicesForNameAndCreatedAtAndAtDefaultValuesForAuditableEntitiesAndAssociations', '10.0.7');
    END IF;
END $EF$;
COMMIT;

START TRANSACTION;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "MigrationId" = '20260511201930_AddCustomerSupportScopeToMetabaseOpenIdConnectClientApplication') THEN
                UPDATE metabase."OpenIddictApplications" 
                SET "Permissions" = ("Permissions"::jsonb || '["scp:api:support"]'::jsonb)::text
                WHERE "ClientId" = 'metabase';
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "MigrationId" = '20260511201930_AddCustomerSupportScopeToMetabaseOpenIdConnectClientApplication') THEN
    INSERT INTO "__EFMigrationsHistory" ("MigrationId", "ProductVersion")
    VALUES ('20260511201930_AddCustomerSupportScopeToMetabaseOpenIdConnectClientApplication', '10.0.7');
    END IF;
END $EF$;

COMMIT;

