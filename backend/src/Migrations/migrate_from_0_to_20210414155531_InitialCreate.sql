CREATE TABLE IF NOT EXISTS "__EFMigrationsHistory" (
    "MigrationId" character varying(150) NOT NULL,
    "ProductVersion" character varying(32) NOT NULL,
    CONSTRAINT "PK___EFMigrationsHistory" PRIMARY KEY ("MigrationId")
);

START TRANSACTION;

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

COMMIT;

