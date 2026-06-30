import {
  ComponentsDocument,
  ComponentNamesDocument,
  ComponentsQueryVariables,
  ComponentsPartialFragment,
} from "../../queries/components.generated";
import paths from "../../paths";
import ComponentList from "./ComponentList";
import PaginatedEntities from "../entities/PaginatedEntities";
import {
  ComponentCategory,
  ComponentFilterInput,
  ComponentSortInput,
} from "../../__generated__/graphql";

export default function PaginatedComponents({
  where,
  order,
  showJump = false,
  extra,
}: {
  where?: ComponentsQueryVariables["where"];
  order?: ComponentsQueryVariables["order"];
  showJump?: boolean;
  extra?: React.ReactNode;
}) {
  return (
    <PaginatedEntities<
      ComponentsPartialFragment,
      ComponentFilterInput,
      ComponentSortInput
    >
      entitiesQuery={ComponentsDocument}
      namesQuery={ComponentNamesDocument}
      baseWhere={where}
      defaultOrder={order}
      showJump={showJump}
      route={paths.component}
      extra={extra}
      list={(props) => <ComponentList {...props} />}
      filterDefinitions={[
        {
          field: "name",
          type: "string",
        },
        {
          field: "abbreviation",
          type: "string",
        },
        {
          field: "description",
          type: "string",
        },
        {
          field: "categories",
          type: "list",
          item: {
            type: "enum",
            enumObject: ComponentCategory,
          },
        },
        {
          field: "manufacturers",
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
        { field: "abbreviation" },
        { field: "createdAt" },
        { field: "updatedAt" },
        { field: "id" },
      ]}
    />
  );
}
