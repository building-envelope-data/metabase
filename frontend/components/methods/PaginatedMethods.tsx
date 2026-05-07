import {
  MethodsDocument,
  MethodNamesDocument,
  MethodsQueryVariables,
  MethodsPartialFragment,
} from "../../queries/methods.generated";
import paths from "../../paths";
import MethodList from "./MethodList";
import PaginatedEntities from "../entities/PaginatedEntities";
import {
  MethodCategory,
  MethodFilterInput,
  MethodSortInput,
} from "../../__generated__/graphql";

export default function PaginatedMethods({
  where,
  order,
  showJump = false,
  extra,
}: {
  where?: MethodsQueryVariables["where"];
  order?: MethodsQueryVariables["order"];
  showJump?: boolean;
  extra?: React.ReactNode;
}) {
  return (
    <PaginatedEntities<
      MethodsPartialFragment,
      MethodFilterInput,
      MethodSortInput
    >
      entitiesQuery={MethodsDocument}
      namesQuery={MethodNamesDocument}
      where={where}
      order={order}
      showJump={showJump}
      route={paths.method}
      extra={extra}
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
