import {
  AnyDatabasesDocument,
  DatabaseNamesDocument,
  AnyDatabasesQueryVariables,
  DatabasesPartialFragment,
} from "../../queries/databases.generated";
import paths from "../../paths";
import DatabaseList from "./DatabaseList";
import PaginatedEntities from "../entities/PaginatedEntities";
import {
  DatabaseFilterInput,
  DatabaseSortInput,
} from "../../__generated__/graphql";

export default function PaginatedAnyDatabases({
  where,
  order,
  showJump = false,
  extra,
}: {
  where?: AnyDatabasesQueryVariables["where"];
  order?: AnyDatabasesQueryVariables["order"];
  showJump?: boolean;
  extra?: React.ReactNode;
}) {
  return (
    <PaginatedEntities<
      DatabasesPartialFragment,
      DatabaseFilterInput,
      DatabaseSortInput
    >
      entitiesQuery={AnyDatabasesDocument}
      namesQuery={DatabaseNamesDocument}
      where={where}
      order={order}
      showJump={showJump}
      route={paths.database}
      extra={extra}
      list={(props) => <DatabaseList {...props} />}
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
          field: "locator",
          type: "url",
        },
        {
          field: "operator",
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
