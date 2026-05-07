import {
  InstitutionsDocument,
  InstitutionNamesDocument,
  InstitutionsQueryVariables,
  InstitutionsPartialFragment,
} from "../../queries/institutions.generated";
import paths from "../../paths";
import InstitutionList from "./InstitutionList";
import PaginatedEntities from "../entities/PaginatedEntities";
import {
  InstitutionFilterInput,
  InstitutionSortInput,
} from "../../__generated__/graphql";

export default function PaginatedInstitutions({
  where,
  order,
  showJump = false,
  extra,
}: {
  where?: InstitutionsQueryVariables["where"];
  order?: InstitutionsQueryVariables["order"];
  showJump?: boolean;
  extra?: React.ReactNode;
}) {
  return (
    <PaginatedEntities<
      InstitutionsPartialFragment,
      InstitutionFilterInput,
      InstitutionSortInput
    >
      entitiesQuery={InstitutionsDocument}
      namesQuery={InstitutionNamesDocument}
      where={where}
      order={order}
      showJump={showJump}
      route={paths.institution}
      extra={extra}
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
