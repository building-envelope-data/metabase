import {
  ComponentsDocument,
  ComponentNamesDocument,
  ComponentsQueryVariables,
} from "../../queries/components.generated";
import paths from "../../paths";
import ComponentList from "./ComponentList";
import PaginatedEntities from "../entities/PaginatedEntities";
import { ComponentCategory } from "../../__generated__/graphql";

export default function PaginatedComponents({
  where,
  showJump = false,
}: {
  where?: ComponentsQueryVariables["where"];
  showJump?: boolean;
}) {
  return (
    <PaginatedEntities
      entitiesQuery={ComponentsDocument}
      namesQuery={ComponentNamesDocument}
      where={where}
      showJump={showJump}
      route={paths.component}
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
