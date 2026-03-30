import {
  DatabasesDocument,
  DatabaseNamesDocument,
  DatabasesQueryVariables,
} from "../../queries/databases.generated";
import paths from "../../paths";
import DatabaseList from "./DatabaseList";
import PaginatedEntities from "../entities/PaginatedEntities";

export default function PaginatedDatabases({
  where,
  showJump = false,
}: {
  where?: DatabasesQueryVariables["where"];
  showJump?: boolean;
}) {
  return (
    <PaginatedEntities
      entitiesQuery={DatabasesDocument}
      namesQuery={DatabaseNamesDocument}
      where={where}
      showJump={showJump}
      route={paths.database}
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
