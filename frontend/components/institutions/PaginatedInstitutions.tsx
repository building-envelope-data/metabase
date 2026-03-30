import {
  InstitutionsDocument,
  InstitutionNamesDocument,
  InstitutionsQueryVariables,
} from "../../queries/institutions.generated";
import paths from "../../paths";
import InstitutionList from "./InstitutionList";
import PaginatedEntities from "../entities/PaginatedEntities";

export default function PaginatedInstitutions({
  where,
  showJump = false,
}: {
  where?: InstitutionsQueryVariables["where"];
  showJump?: boolean;
}) {
  return (
    <PaginatedEntities
      entitiesQuery={InstitutionsDocument}
      namesQuery={InstitutionNamesDocument}
      where={where}
      showJump={showJump}
      route={paths.institution}
      list={(props) => <InstitutionList {...props} />}
      filterDefinitions={[
        {
          field: "name",
          type: "string",
        },
        {
          field: "description",
          type: "string",
        },
        {
          field: "developedMethods",
          type: "list",
          item: {
            type: "object",
            items: [
              {
                field: "name",
                type: "string",
              },
              { field: "id", type: "uuid" },
            ],
          },
        },
        {
          field: "gnuPgKeyFingerprints",
          type: "list",
          item: {
            type: "object",
            items: [
              {
                field: "name",
                type: "string",
              },
              { field: "id", type: "uuid" },
            ],
          },
        },
        {
          field: "managedDataFormats",
          type: "list",
          item: {
            type: "object",
            items: [
              {
                field: "name",
                type: "string",
              },
              { field: "id", type: "uuid" },
            ],
          },
        },
        {
          field: "managedInstitutions",
          type: "list",
          item: {
            type: "object",
            items: [
              {
                field: "name",
                type: "string",
              },
              { field: "id", type: "uuid" },
            ],
          },
        },
        {
          field: "managedMethods",
          type: "list",
          item: {
            type: "object",
            items: [
              {
                field: "name",
                type: "string",
              },
              { field: "id", type: "uuid" },
            ],
          },
        },
        {
          field: "manufacturedComponents",
          type: "list",
          item: {
            type: "object",
            items: [
              {
                field: "name",
                type: "string",
              },
              { field: "id", type: "uuid" },
            ],
          },
        },
        {
          field: "operatedDatabases",
          type: "list",
          item: {
            type: "object",
            items: [
              {
                field: "name",
                type: "string",
              },
              { field: "id", type: "uuid" },
            ],
          },
        },
        {
          field: "representatives",
          type: "list",
          item: {
            type: "object",
            items: [
              {
                field: "name",
                type: "string",
              },
              { field: "id", type: "uuid" },
            ],
          },
        },
        {
          field: "representatives",
          type: "list",
          item: {
            type: "object",
            items: [
              {
                field: "name",
                type: "string",
              },
              { field: "id", type: "uuid" },
            ],
          },
        },
        {
          field: "manager",
          type: "object",
          items: [
            {
              field: "name",
              type: "string",
            },
            { field: "id", type: "uuid" },
          ],
        },
        {
          field: "id",
          type: "uuid",
        },
      ]}
      sortDefinitions={[
        { field: "name" },
        { field: "createdAt" },
        { field: "updatedAt" },
        { field: "id" },
      ]}
    />
  );
}
