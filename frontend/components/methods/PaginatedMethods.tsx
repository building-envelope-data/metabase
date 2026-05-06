import {
  MethodsDocument,
  MethodNamesDocument,
  MethodsQueryVariables,
} from "../../queries/methods.generated";
import paths from "../../paths";
import MethodList from "./MethodList";
import PaginatedEntities from "../entities/PaginatedEntities";
import { MethodCategory } from "../../__generated__/graphql";

export default function PaginatedMethods({
  where,
  showJump = false,
}: {
  where?: MethodsQueryVariables["where"];
  showJump?: boolean;
}) {
  return (
    <PaginatedEntities
      entitiesQuery={MethodsDocument}
      namesQuery={MethodNamesDocument}
      where={where}
      showJump={showJump}
      route={paths.method}
      list={(props) => <MethodList {...props} />}
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
          field: "categories",
          type: "list",
          item: { type: "enum", enumObject: MethodCategory },
        },
        {
          field: "calculationLocator",
          type: "url",
        },
        {
          field: "institutionDevelopers",
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
          field: "userDevelopers",
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
