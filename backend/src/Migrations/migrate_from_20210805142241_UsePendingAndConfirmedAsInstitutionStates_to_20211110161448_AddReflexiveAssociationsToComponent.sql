START TRANSACTION;

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

COMMIT;

